Imports jp.co.systembase.barcode.content
Imports jp.co.systembase.barcode.content.CBarContent
Imports jp.co.systembase.barcode.content.CScale

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

        For i As Integer = codes.Count + 1 To CODE_MAX_SIZE
            codes.Add(CODE_CHARS(14)) ' CC4
        Next

        Dim ret = codes.GetRange(0, CODE_MAX_SIZE)
        Dim pos As Integer = calcCheckDigit(ret)
        ret.Add(CODE_CHARS(pos))
        ret.Insert(0, START_PATTERN)
        ret.Add(STOP_PATTERN)

        Return ret
    End Function

    Private Sub validate(ByVal data As String)
        For Each c As Char In data
            If Not CODE_PATTERNS.ContainsKey(c) Then
                Throw New ArgumentException("illegal char: " & c & " of data: " & data)
            End If
        Next
    End Sub

    Private Function calcCheckDigit(ByVal codes As List(Of String)) As Integer
        Dim sum As Integer = 0
        For Each s As String In codes
            sum += CHECK_DIGIT_PATTERNS(s)
        Next

        Const checkNum As Integer = 19
        Dim pos As Integer = checkNum - (sum Mod checkNum)
        If pos = checkNum Then
            pos = 0
        End If

        Return pos
    End Function

    Public Function CreateContent(ByVal x As Single, ByVal y As Single, ByVal w As Single, ByVal h As Single, _
                                  ByVal point As Single, ByVal data As String)
        Return CreateContent(New RectangleF(x, y, w, h), point, data)
    End Function

    Public Function CreateContent(ByVal x As Single, ByVal y As Single, ByVal w As Single, ByVal h As Single, _
                                  ByVal point As Single, ByVal dpi As Integer, ByVal data As String)
        Return CreateContent(New RectangleF(x, y, w, h), point, dpi, data)
    End Function

    Public Function CreateContent(ByVal r As RectangleF, _
                                  ByVal point As Single, ByVal data As String)
        Return CreateContent(r, point, DPI, data)
    End Function

    Public Function CreateContent(ByVal r As RectangleF, _
                                  ByVal point As Single, ByVal dpi As Integer, ByVal data As String) As CBarContent
        If point < 8.0F OrElse 11.5F < point Then
            Throw New ArgumentException("illegal data: " & data & ", point is 8.0 to 11.5")
        End If

        Dim longBarHeight As Single = MmToPixel(dpi, 3.6F * point / 10.0F)
        Dim timingBarHeight As Single = MmToPixel(dpi, 1.2F * point / 10.0F)
        Dim semilongBarHeight As Single = longBarHeight / 2.0F + timingBarHeight / 2.0F
        Dim barWidth As Single = MmToPixel(dpi, 0.6F * point / 10.0F)
        Dim barSpace As Single = MmToPixel(dpi, 0.6F * point / 10.0F)

        Dim scale As CScale = New CPointScale(MarginX, MarginY, r.Width, r.Height, dpi)
        Dim xPos As Single = r.X
        Dim yTop As Single = r.Y
        Dim xMax As Single = r.X + scale.PixelWidth
        Dim yMax As Single = r.Y + scale.PixelHeight

        Dim ret As New CBarContent
        For Each code As String In Encode(data)
            For Each c As Char In code
                Dim yPos As Single = yTop
                Dim barHeight As Single = 0
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

                Dim x As Single = xPos + scale.PixelMarginX
                Dim y As Single = yPos + scale.PixelMarginY
                If x > xMax OrElse y > yMax Then
                    Exit For
                End If
                If y + barHeight > yMax Then
                    barHeight = yMax - y
                End If

                Dim b As New CBarContent.CBar(x, y, barWidth, barHeight)
                ret.Add(b)

                xPos = xPos + barWidth + barSpace
            Next
        Next

        Return ret
    End Function

    Public Sub Render(ByVal g As Graphics, _
                      ByVal x As Single, ByVal y As Single, ByVal w As Single, ByVal h As Single, _
                      ByVal point As Single, ByVal data As String)
        Render(g, New RectangleF(x, y, w, h), point, data)
    End Sub

    Public Sub Render(ByVal g As Graphics, _
                      ByVal x As Single, ByVal y As Single, ByVal w As Single, ByVal h As Single, _
                      ByVal point As Single, ByVal dpi As Integer, ByVal data As String)
        Render(g, New RectangleF(x, y, w, h), point, dpi, data)
    End Sub

    Public Sub Render(ByVal g As Graphics, _
                      ByVal r As RectangleF, _
                      ByVal point As Single, ByVal data As String)
        Render(g, r, point, DPI, data)
    End Sub

    Public Sub Render(ByVal g As Graphics, _
                      ByVal r As RectangleF, _
                      ByVal point As Single, ByVal dpi As Integer, ByVal data As String)
        Dim c As CBarContent = CreateContent(r, point, dpi, data)
        c.Draw(g)
    End Sub

End Class
