Imports jp.co.systembase.barcode.content
Imports jp.co.systembase.barcode.content.BarContent
Imports jp.co.systembase.barcode.content.Scale

Public Class Itf
    Inherits Barcode

    Private Shared CODE_PATTERNS As New Dictionary(Of Char, Integer()) From _
        {{"0", {0, 0, 1, 1, 0}}, _
         {"1", {1, 0, 0, 0, 1}}, _
         {"2", {0, 1, 0, 0, 1}}, _
         {"3", {1, 1, 0, 0, 0}}, _
         {"4", {0, 0, 1, 0, 1}}, _
         {"5", {1, 0, 1, 0, 0}}, _
         {"6", {0, 1, 1, 0, 0}}, _
         {"7", {0, 0, 0, 1, 1}}, _
         {"8", {1, 0, 0, 1, 0}}, _
         {"9", {0, 1, 0, 1, 0}}}

    Private Shared START_PATTERN As Integer() = {0, 0, 0, 0}
    Private Shared STOP_PATTERN As Integer() = {1, 0, 0}

    Public GenerateCheckSum As Boolean = False

    Public Function Encode(ByVal data As String) As List(Of Integer())
        If data Is Nothing OrElse data.Length = 0 Then
            Return Nothing
        End If

        validate(data)

        Dim _data As String = _Encode(data)

        Dim ret As New List(Of Integer())
        ret.Add(START_PATTERN)
        For i As Integer = 0 To _data.Length - 1 Step 2
            Dim c1 As Char = _data(i)
            Dim c2 As Char = _data(i + 1)
            Dim code1 As Integer() = CODE_PATTERNS(c1)
            Dim code2 As Integer() = CODE_PATTERNS(c2)

            If Not code1.Length = code2.Length Then
                Throw New ArgumentException("illegal pattern length: " & c1 & " != " & c2)
            End If

            Dim code As New List(Of Integer)
            For t As Integer = 0 To code1.Length - 1
                code.Add(code1(t))
                code.Add(code2(t))
            Next
            ret.Add(code.ToArray)
        Next
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

    Private Function calcCheckDigit(ByVal data As String) As Integer
        If data.Length Mod 2 = 0 Then
            Throw New ArgumentException("illegal data length: " & data & ", must odd number")
        End If

        Dim sum As Integer = 0
        For i As Integer = data.Length - 1 To 0 Step -1
            Dim c As Integer = Asc(data(i))
            Dim n As Integer = c - &H30 ' - '0'
            If i Mod 2 <> 0 Then
                sum += n
            Else
                sum += n * 3
            End If
        Next

        Const checkNum = 10
        Dim cd As Integer = checkNum - (sum Mod checkNum)
        If cd = checkNum Then
            cd = 0
        End If

        Return cd
    End Function

    Protected Function _Encode(ByVal data As String) As String
        Dim _data As String = data
        If GenerateCheckSum Then
            If _data.Length Mod 2 = 0 Then
                _data = "0" & _data
            End If
            Dim cd As Integer = calcCheckDigit(_data)
            _data &= cd.ToString
        Else
            If _data.Length Mod 2 <> 0 Then
                _data = "0" & _data
            End If
        End If
        Return _data
    End Function

    Public Function CreateContent(ByVal x As Single, ByVal y As Single, ByVal w As Single, ByVal h As Single, _
                                  ByVal data As String) As BarContent
        Return CreateContent(New RectangleF(x, y, w, h), data)
    End Function

    Public Function CreateContent(ByVal x As Single, ByVal y As Single, ByVal w As Single, ByVal h As Single, _
                                  ByVal dpi As Integer, ByVal data As String) As BarContent
        Return CreateContent(New RectangleF(x, y, w, h), dpi, data)
    End Function

    Public Function CreateContent(ByVal r As RectangleF, ByVal data As String) As BarContent
        Return CreateContent(r, DPI, data)
    End Function

    Public Function CreateContent(ByVal r As RectangleF, ByVal dpi As Integer, ByVal data As String) As BarContent
        Dim shortBarWidth As Single = MmToPixel(dpi, 1.016F)
        Dim longBarWidth As Single = MmToPixel(dpi, 1.016F * 2.5F)

        Dim width = 0
        Dim codes As List(Of Integer()) = Encode(data)
        For Each code As Integer() In codes
            For Each c As Integer In code
                If c = 0 Then
                    width += shortBarWidth
                Else
                    width += longBarWidth
                End If
            Next
        Next

        Dim scale As Scale = New PointScale(marginX, marginY, r.Width, r.Height, dpi)
        Dim h As Single = scale.PixelHeight
        Dim barHeight As Single = h
        If WithText Then
            barHeight *= 0.7F
        End If

        Dim w As Single = scale.PixelWidth
        If w <= 0 OrElse h <= 0 Then
            Return Nothing
        End If

        Dim ret As New BarContent
        Dim xPos As Single = 0
        Dim _scale As Single = w / width
        For Each code As Integer() In codes
            For i As Integer = 0 To code.Length - 1
                Dim c As Integer = code(i)
                Dim barWidth As Single = 0.0F
                If c = 0 Then
                    barWidth = shortBarWidth
                Else
                    barWidth = longBarWidth
                End If
                barWidth *= _scale
                If i Mod 2 = 0 Then
                    Dim x As Single = r.X + xPos + scale.PixelMarginX
                    Dim y As Single = r.Y + scale.PixelMarginY
                    Dim b As New BarContent.Bar(x, y, barWidth, barHeight)
                    ret.Add(b)
                End If
                xPos += barWidth
            Next
        Next

        If WithText Then
            Dim _data As String = _Encode(data)

            Dim fs As Single = FontSize(w, h, _data)
            Dim f As New Font("Arial", fs)
            Dim format As StringFormat = New StringFormat()
            format.Alignment = StringAlignment.Center
            Dim x As Single = r.X + (w / 2) + scale.PixelMarginX
            Dim y As Single = r.Y + barHeight + scale.PixelMarginY

            Dim t As New BarContent.Text(_data, f, x, y, format)
            ret.Add(t)
        End If

        Return ret
    End Function

    Public Sub Render(ByVal g As Graphics, _
                      ByVal x As Single, ByVal y As Single, ByVal w As Single, ByVal h As Single, _
                      ByVal data As String)
        Render(g, New RectangleF(x, y, w, h), data)
    End Sub

    Public Sub Render(ByVal g As Graphics, _
                      ByVal x As Single, ByVal y As Single, ByVal w As Single, ByVal h As Single, _
                      ByVal dpi As Integer, ByVal data As String)
        Render(g, New RectangleF(x, y, w, h), dpi, data)
    End Sub

    Public Sub Render(ByVal g As Graphics, ByVal r As RectangleF, ByVal data As String)
        Render(g, r, DPI, data)
    End Sub

    Public Sub Render(ByVal g As Graphics, ByVal r As RectangleF, ByVal dpi As Integer, ByVal data As String)
        Dim c As BarContent = CreateContent(r, dpi, data)
        c.Draw(g)
    End Sub

End Class

