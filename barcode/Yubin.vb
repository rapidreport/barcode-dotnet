Imports jp.co.systembase.barcode.content
Imports jp.co.systembase.barcode.content.BarContent
Imports jp.co.systembase.barcode.content.Scale

Public Class YubinCustomer
    Inherits Barcode

    Private Shared CODE_PATTERNS(,) As Byte = _
        {{1, 4, 4}, _
         {1, 1, 4}, _
         {1, 3, 2}, _
         {3, 1, 2}, _
         {1, 2, 3}, _
         {1, 4, 1}, _
         {3, 2, 1}, _
         {2, 1, 3}, _
         {2, 3, 1}, _
         {4, 1, 1}, _
         {4, 1, 4}, _
         {3, 2, 4}, _
         {3, 4, 2}, _
         {2, 3, 4}, _
         {4, 3, 2}, _
         {2, 4, 3}, _
         {4, 2, 3}, _
         {4, 4, 1}, _
         {1, 1, 1}}

    Private Shared CODE_POINTS As New Dictionary(Of Char, Byte()) From _
        {{"0", {0}}, _
         {"1", {1}}, _
         {"2", {2}}, _
         {"3", {3}}, _
         {"4", {4}}, _
         {"5", {5}}, _
         {"6", {6}}, _
         {"7", {7}}, _
         {"8", {8}}, _
         {"9", {9}}, _
         {"-", {10}}, _
         {"A", {11, 0}}, _
         {"B", {11, 1}}, _
         {"C", {11, 2}}, _
         {"D", {11, 3}}, _
         {"E", {11, 4}}, _
         {"F", {11, 5}}, _
         {"G", {11, 6}}, _
         {"H", {11, 7}}, _
         {"I", {11, 8}}, _
         {"J", {11, 9}}, _
         {"K", {12, 0}}, _
         {"L", {12, 1}}, _
         {"M", {12, 2}}, _
         {"N", {12, 3}}, _
         {"O", {12, 4}}, _
         {"P", {12, 5}}, _
         {"Q", {12, 6}}, _
         {"R", {12, 7}}, _
         {"S", {12, 8}}, _
         {"T", {12, 9}}, _
         {"U", {13, 0}}, _
         {"V", {13, 1}}, _
         {"W", {13, 2}}, _
         {"X", {13, 3}}, _
         {"Y", {13, 4}}, _
         {"Z", {13, 5}}}

    Private Shared START_PATTERN() As Byte = {1, 3}
    Private Shared STOP_PATTERN() As Byte = {3, 1}

    Private Const MAX_SIZE As Integer = 20

    Public Function Encode(ByVal data As String) As List(Of Byte)
        If data Is Nothing OrElse data.Length = 0 Then
            Return Nothing
        End If
        validate(data)
        Dim ret As New List(Of Byte)
        Dim sum_p As Integer = 0
        Dim l As Integer = 0
        ret.AddRange(START_PATTERN)
        For Each c As Char In data
            Dim cp As Byte() = CODE_POINTS(c)
            If l + cp.Length > MAX_SIZE Then
                Exit For
            End If
            For Each p As Byte In cp
                sum_p += p
                ret.Add(CODE_PATTERNS(p, 0))
                ret.Add(CODE_PATTERNS(p, 1))
                ret.Add(CODE_PATTERNS(p, 2))
            Next
            l += cp.Length
        Next
        Do While l < MAX_SIZE
            ret.Add(CODE_PATTERNS(14, 0)) ' CC4
            ret.Add(CODE_PATTERNS(14, 1))
            ret.Add(CODE_PATTERNS(14, 2))
            sum_p += 14
            l += 1
        Loop
        With Nothing
            Dim cd As Integer = calcCheckDigit(sum_p)
            ret.Add(CODE_PATTERNS(cd, 0))
            ret.Add(CODE_PATTERNS(cd, 1))
            ret.Add(CODE_PATTERNS(cd, 2))
        End With
        ret.AddRange(STOP_PATTERN)
        Return ret
    End Function

    Private Sub validate(ByVal data As String)
        For Each c As Char In data
            If Not CODE_POINTS.ContainsKey(c) Then
                Throw New ArgumentException("illegal char: " & c & " of data: " & data)
            End If
        Next
    End Sub

    Private Function calcCheckDigit(ByVal p As Integer) As Byte
        Const checkNum As Integer = 19
        Dim pos As Integer = checkNum - (p Mod checkNum)
        If pos = checkNum Then
            pos = 0
        End If
        Return pos
    End Function

    Public Sub Render(ByVal g As Graphics, _
              ByVal x As Single, ByVal y As Single, ByVal w As Single, ByVal h As Single, _
              ByVal data As String)
        Me.Render(g, New RectangleF(x, y, w, h), data)
    End Sub

    Public Sub Render(ByVal g As Graphics, ByVal r As RectangleF, ByVal data As String)
        If data Is Nothing OrElse data.Length = 0 Then
            Exit Sub
        End If
        Dim w As Single = r.Width - Me.MarginX * 2
        If w <= 0 Then
            Exit Sub
        End If
        Dim codes As List(Of Byte) = Me.Encode(data)
        Dim uw As Single = w / (codes.Count * 2)
        Dim x As Single = r.Left + Me.MarginX
        Dim y As Single = r.Top + r.Height / 2
        For Each c As Byte In codes
            Dim by As Single = 0
            Dim bh As Single = 0
            Select Case c
                Case 1
                    by = y - uw * 3
                    bh = uw * 6
                Case 2
                    by = y - uw * 3
                    bh = uw * 4
                Case 3
                    by = y - uw
                    bh = uw * 4
                Case 4
                    by = y - uw
                    bh = uw * 2
            End Select
            g.FillRectangle(Brushes.Black, New RectangleF(x, by, uw, bh))
            x += uw * 2
        Next
    End Sub

    'Public Function CreateContent(ByVal x As Single, ByVal y As Single, ByVal w As Single, ByVal h As Single, _
    '                              ByVal point As Single, ByVal data As String)
    '    Return CreateContent(New RectangleF(x, y, w, h), point, data)
    'End Function

    'Public Function CreateContent(ByVal x As Single, ByVal y As Single, ByVal w As Single, ByVal h As Single, _
    '                              ByVal point As Single, ByVal dpi As Integer, ByVal data As String)
    '    Return CreateContent(New RectangleF(x, y, w, h), point, dpi, data)
    'End Function

    'Public Function CreateContent(ByVal r As RectangleF, _
    '                              ByVal point As Single, ByVal data As String)
    '    Return CreateContent(r, point, DPI, data)
    'End Function

    'Public Function CreateContent(ByVal r As RectangleF, _
    '                              ByVal point As Single, ByVal dpi As Integer, ByVal data As String) As BarContent
    '    If point < 8.0F OrElse 11.5F < point Then
    '        Throw New ArgumentException("illegal point: " & point & ", point is 8.0 to 11.5")
    '    End If

    '    Dim longBarHeight As Single = MmToPixel(dpi, 3.6F * point / 10.0F)
    '    Dim timingBarHeight As Single = MmToPixel(dpi, 1.2F * point / 10.0F)
    '    Dim semilongBarHeight As Single = longBarHeight / 2.0F + timingBarHeight / 2.0F
    '    Dim barWidth As Single = MmToPixel(dpi, 0.6F * point / 10.0F)
    '    Dim barSpace As Single = MmToPixel(dpi, 0.6F * point / 10.0F)

    '    Dim scale As Scale = New PointScale(MarginX, MarginY, r.Width, r.Height, dpi)
    '    Dim xPos As Single = r.X
    '    Dim yTop As Single = r.Y
    '    Dim xMax As Single = r.X + scale.PixelWidth
    '    Dim yMax As Single = r.Y + scale.PixelHeight

    '    Dim ret As New BarContent
    '    For Each code As String In Encode(data)
    '        For Each c As Char In code
    '            Dim yPos As Single = yTop
    '            Dim barHeight As Single = 0
    '            Select Case c
    '                Case "1"
    '                    barHeight = longBarHeight
    '                Case "2"
    '                    barHeight = semilongBarHeight
    '                Case "3"
    '                    yPos = yTop + longBarHeight - semilongBarHeight
    '                    barHeight = semilongBarHeight
    '                Case "4"
    '                    yPos = yTop + longBarHeight - semilongBarHeight
    '                    barHeight = timingBarHeight
    '                Case Else
    '                    Throw New ArgumentException("illegal switch case: " & c)
    '            End Select

    '            Dim x As Single = xPos + scale.PixelMarginX
    '            Dim y As Single = yPos + scale.PixelMarginY
    '            If x > xMax OrElse y > yMax Then
    '                Exit For
    '            End If
    '            If y + barHeight > yMax Then
    '                barHeight = yMax - y
    '            End If

    '            Dim b As New BarContent.Bar(x, y, barWidth, barHeight)
    '            ret.Add(b)

    '            xPos = xPos + barWidth + barSpace
    '        Next
    '    Next

    '    Return ret
    'End Function

    'Public Sub Render(ByVal g As Graphics, _
    '                  ByVal x As Single, ByVal y As Single, ByVal w As Single, ByVal h As Single, _
    '                  ByVal point As Single, ByVal data As String)
    '    Render(g, New RectangleF(x, y, w, h), point, data)
    'End Sub

    'Public Sub Render(ByVal g As Graphics, _
    '                  ByVal x As Single, ByVal y As Single, ByVal w As Single, ByVal h As Single, _
    '                  ByVal point As Single, ByVal dpi As Integer, ByVal data As String)
    '    Render(g, New RectangleF(x, y, w, h), point, dpi, data)
    'End Sub

    'Public Sub Render(ByVal g As Graphics, _
    '                  ByVal r As RectangleF, _
    '                  ByVal point As Single, ByVal data As String)
    '    Render(g, r, point, DPI, data)
    'End Sub

    'Public Sub Render(ByVal g As Graphics, _
    '                  ByVal r As RectangleF, _
    '                  ByVal point As Single, ByVal dpi As Integer, ByVal data As String)
    '    Dim c As BarContent = CreateContent(r, point, dpi, data)
    '    c.Draw(g)
    'End Sub

End Class
