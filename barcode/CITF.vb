﻿Public Class CITF
    Inherits CBarcode

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

    Private Const DPI As Integer = 72

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
            If CODE_PATTERNS(c) Is Nothing Then
                Throw New ArgumentException("illegal data: " & data)
            End If
        Next
    End Sub

    Protected Function _Encode(ByVal data As String) As String
        Dim _data As String = data
        If Not _data.Length Mod 2 = 0 Then
            _data = "0" & _data
        End If
        Return _data
    End Function

    Public Sub Render(ByVal g As Graphics, _
                      ByVal x As Single, ByVal y As Single, ByVal w As Single, ByVal h As Single, _
                      ByVal data As String)
        Render(g, x, y, w, h, DPI, data)
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
        Dim shortBarWidth As Single = mmToPixel(dpi, 1.016F)
        Dim longBarWidth = mmToPixel(dpi, 1.016F * 2.5F)

        Dim width = MarginX
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

        Dim h As Single = pointToPixel(dpi, r.Height - MarginY * 2)
        Dim barHeight As Single = h
        If WithText Then
            barHeight *= 0.7F
        End If
        Dim height = barHeight + MarginY

        Dim w As Single = pointToPixel(dpi, r.Width - MarginX * 2)
        If w <= 0 Or h <= 0 Then
            Exit Sub
        End If

        Dim xPos As Single = 0
        Dim scale As Single = w / width
        For Each code As Integer() In codes
            For i As Integer = 0 To code.Length - 1
                Dim c As Integer = code(i)
                Dim barWidth As Single = longBarWidth * scale
                If c = 0 Then
                    barWidth = shortBarWidth * scale
                End If
                Dim b As System.Drawing.Brush = Brushes.White
                If i Mod 2 = 0 Then
                    b = Brushes.Black
                End If
                g.FillRectangle(b, New RectangleF(r.X + xPos + MarginX, r.Y + MarginY, barWidth, barHeight))
                xPos += barWidth
            Next
        Next

        If WithText Then
            Dim _data As String = _Encode(data)

            Dim textHeight As Single = h * 0.2F
            Dim textWidth As Single = ((w * 0.9F) / _data.Length) * 2.0F
            Dim fs As Single = Math.Max(Math.Min(textHeight, textWidth), 6.0F)
            Dim f As New Font("Arial", fs)

            Dim format As StringFormat = New StringFormat()
            format.Alignment = StringAlignment.Center
            g.DrawString(_data, f, Brushes.Black, r.X + w / 2 + MarginX, r.Y + height, format)
        End If
    End Sub

    Private Function pointToPixel(ByVal dpi As Integer, ByVal point As Single) As Single
        Return dpi * (point / 72.0F)
    End Function

    Private Function mmToPixel(ByVal dpi As Integer, ByVal mm As Single) As Single
        Return dpi * (mm / 25.4F)
    End Function

End Class