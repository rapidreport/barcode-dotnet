Imports jp.co.systembase.barcode.CBarcode.BarContent

Public Class CYubinCustomer
    Inherits CBarcode

    Private Shared CODE_CHARS() As String = _
        {"144", _
         "114", _
         "132", _
         "312", _
         "123", _
         "141", _
         "321", _
         "213", _
         "231", _
         "411", _
         "414", _
         "324", _
         "342", _
         "234", _
         "432", _
         "243", _
         "423", _
         "441", _
         "111"}

    Private Shared CODE_PATTERNS As New Dictionary(Of Char, String()) From _
        {{"1", {CODE_CHARS(1), ""}}, _
         {"2", {CODE_CHARS(2), ""}}, _
         {"3", {CODE_CHARS(3), ""}}, _
         {"4", {CODE_CHARS(4), ""}}, _
         {"5", {CODE_CHARS(5), ""}}, _
         {"6", {CODE_CHARS(6), ""}}, _
         {"7", {CODE_CHARS(7), ""}}, _
         {"8", {CODE_CHARS(8), ""}}, _
         {"9", {CODE_CHARS(9), ""}}, _
         {"0", {CODE_CHARS(0), ""}}, _
         {"-", {CODE_CHARS(10), ""}}, _
         {"A", {CODE_CHARS(11), CODE_CHARS(0)}}, _
         {"B", {CODE_CHARS(11), CODE_CHARS(1)}}, _
         {"C", {CODE_CHARS(11), CODE_CHARS(2)}}, _
         {"D", {CODE_CHARS(11), CODE_CHARS(3)}}, _
         {"E", {CODE_CHARS(11), CODE_CHARS(4)}}, _
         {"F", {CODE_CHARS(11), CODE_CHARS(5)}}, _
         {"G", {CODE_CHARS(11), CODE_CHARS(6)}}, _
         {"H", {CODE_CHARS(11), CODE_CHARS(7)}}, _
         {"I", {CODE_CHARS(11), CODE_CHARS(8)}}, _
         {"J", {CODE_CHARS(11), CODE_CHARS(9)}}, _
         {"K", {CODE_CHARS(12), CODE_CHARS(0)}}, _
         {"L", {CODE_CHARS(12), CODE_CHARS(1)}}, _
         {"M", {CODE_CHARS(12), CODE_CHARS(2)}}, _
         {"N", {CODE_CHARS(12), CODE_CHARS(3)}}, _
         {"O", {CODE_CHARS(12), CODE_CHARS(4)}}, _
         {"P", {CODE_CHARS(12), CODE_CHARS(5)}}, _
         {"Q", {CODE_CHARS(12), CODE_CHARS(6)}}, _
         {"R", {CODE_CHARS(12), CODE_CHARS(7)}}, _
         {"S", {CODE_CHARS(12), CODE_CHARS(8)}}, _
         {"T", {CODE_CHARS(12), CODE_CHARS(9)}}, _
         {"U", {CODE_CHARS(13), CODE_CHARS(0)}}, _
         {"V", {CODE_CHARS(13), CODE_CHARS(1)}}, _
         {"W", {CODE_CHARS(13), CODE_CHARS(2)}}, _
         {"X", {CODE_CHARS(13), CODE_CHARS(3)}}, _
         {"Y", {CODE_CHARS(13), CODE_CHARS(4)}}, _
         {"Z", {CODE_CHARS(13), CODE_CHARS(5)}}}

    Private Const START_PATTERN As String = "13"
    Private Const STOP_PATTERN As String = "31"

    Private Const CODE_MAX_SIZE As Integer = 20

    Private Shared CHECK_DIGIT_PATTERNS As New Dictionary(Of String, Integer)

    Private Const DPI As Integer = 72

    Shared Sub New()
        For i As Integer = 0 To CODE_CHARS.Length - 1
            CHECK_DIGIT_PATTERNS.Add(CODE_CHARS(i), i)
        Next
    End Sub

    Public Function Encode(ByVal data As String) As List(Of String)
        If data Is Nothing OrElse data.Length = 0 Then
            Return Nothing
        End If

        validate(data)

        Dim codes As New List(Of String)
        For Each c As Char In data
            Dim s As String() = CODE_PATTERNS(c)
            codes.Add(s(0))
            If Not s(1) Is String.Empty Then
                codes.Add(s(1))
            End If
        Next

        For i As Integer = codes.Count To CODE_MAX_SIZE
            codes.Add(CODE_CHARS(14)) ' CC4
        Next

        Dim ret = codes.GetRange(0, CODE_MAX_SIZE)
        ret.Add(calcCheckDigit(ret))
        ret.Insert(0, START_PATTERN)
        ret.Add(STOP_PATTERN)

        Return ret
    End Function

    Private Sub validate(ByVal data As String)
        For Each c As Char In data
            If CODE_PATTERNS(c) Is Nothing Then
                Throw New ArgumentException("illegal data: " & data)
            End If
        Next
    End Sub

    Private Function calcCheckDigit(ByVal codes As List(Of String)) As String
        Dim sum As Integer = 0
        For Each s As String In codes
            sum += CHECK_DIGIT_PATTERNS(s)
        Next

        Const checkNum As Integer = 19
        Dim pos As Integer = checkNum - (sum Mod checkNum)
        If pos = checkNum Then
            pos = 0
        End If

        Return CODE_CHARS(pos)
    End Function

    Public Function CreateContent(ByVal x As Single, ByVal y As Single, ByVal w As Single, ByVal h As Single, _
                             ByVal point As Single, ByVal data As String)
        Return CreateContent(x, y, w, h, point, DPI, data)
    End Function

    Public Function CreateContent(ByVal x As Single, ByVal y As Single, ByVal w As Single, ByVal h As Single, _
                             ByVal point As Single, ByVal dpi As Integer, ByVal data As String)
        Return CreateContent(New RectangleF(x, y, w, h), point, dpi, data)
    End Function

    Public Function CreateContent(ByVal r As RectangleF, ByVal point As Single, _
                             ByVal data As String)
        Return CreateContent(r, point, DPI, data)
    End Function

    Public Function CreateContent(ByVal r As RectangleF, ByVal point As Single, ByVal dpi As Integer, _
                                  ByVal data As String) As BarContent
        If point < 8.0F OrElse 11.5F < point Then
            Throw New ArgumentException("illegal data: " & data)
        End If

        Dim marginX As Single = pointToPixel(dpi, Me.MarginX)
        Dim marginY As Single = pointToPixel(dpi, Me.MarginY)

        Dim longBarHeight As Single = mmToPixel(dpi, 3.6F * point / 10.0F)
        Dim timingBarHeight As Single = mmToPixel(dpi, 1.2F * point / 10.0F)
        Dim semilongBarHeight As Single = longBarHeight / 2.0F + timingBarHeight / 2.0F
        Dim barWidth As Single = mmToPixel(dpi, 0.6F * point / 10.0F)
        Dim barSpace As Single = mmToPixel(dpi, 0.6F * point / 10.0F)

        Dim xPos As Single = r.X
        Dim yTop As Single = r.Y
        Dim xMax As Single = r.X + pointToPixel(dpi, r.Width) - marginX * 2
        Dim yMax As Single = r.Y + pointToPixel(dpi, r.Height) - marginY * 2

        Dim ret As New BarContent
        For Each code As String In Encode(data)
            For Each c As Char In code
                Dim yPos As Single = yTop
                Dim barHeight As Single = 0.0F
                Select Case c
                    Case "1"
                        barHeight = longBarHeight
                    Case "2"
                        barHeight = semilongBarHeight
                    Case "3"
                        yPos = yTop + longBarHeight - semilongBarHeight
                        barHeight = semilongBarHeight
                    Case "4"
                        yPos = yTop + longBarHeight - semilongBarHeight
                        barHeight = timingBarHeight
                    Case Else
                        Throw New ArgumentException("illegal switch case: " & c)
                End Select

                Dim x As Single = xPos + marginX
                Dim y As Single = yPos + marginY
                If x > xMax Or y > yMax Then
                    Exit For
                End If

                If y + barHeight > yMax Then
                    barHeight = yMax - y
                End If

                Dim b As BarContent.Bar = New BarContent.Bar(x, y, barWidth, barHeight)
                ret.Add(b)

                xPos = xPos + barWidth + barSpace
            Next
        Next

        Return ret
    End Function

    Public Sub Render(ByVal g As Graphics, _
                      ByVal x As Single, ByVal y As Single, ByVal w As Single, ByVal h As Single, _
                      ByVal point As Single, ByVal data As String)
        Render(g, x, y, w, h, point, DPI, data)
    End Sub

    Public Sub Render(ByVal g As Graphics, _
                      ByVal x As Single, ByVal y As Single, ByVal w As Single, ByVal h As Single, _
                      ByVal point As Single, ByVal dpi As Integer, ByVal data As String)
        Render(g, New RectangleF(x, y, w, h), point, dpi, data)
    End Sub

    Public Sub Render(ByVal g As Graphics, _
                      ByVal r As RectangleF, ByVal point As Single, _
                      ByVal data As String)
        Render(g, r, point, DPI, data)
    End Sub

    Public Sub Render(ByVal g As Graphics, _
                      ByVal r As RectangleF, ByVal point As Single, ByVal dpi As Integer, _
                      ByVal data As String)
        Dim c As BarContent = CreateContent(r, point, dpi, data)
        If c Is Nothing Then
            Exit Sub
        End If

        For Each b As Bar In c.GetBars
            g.FillRectangle(Brushes.Black, b.GetX, b.GetY, b.GetWidth, b.GetHeight)
        Next
    End Sub

End Class
