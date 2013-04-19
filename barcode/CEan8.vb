﻿Public Class CEan8
    Inherits CEan

    Private Shared GUARDS() As Integer = {0, 2, 20, 22, 40, 42}
    Private Shared CHARPOS() As Single = {7, 14, 21, 28, 39, 46, 53, 60}

    Public Function Encode(ByVal data As String) As Byte()
        If data Is Nothing OrElse data.Length = 0 Then
            Return Nothing
        End If
        Dim _data As List(Of Byte) = pack(data)
        If _data.Count = 7 Then
            _data.Add(Me.calcCheckDigit(_data))
        End If
        If _data.Count <> 8 Then
            Throw New ArgumentException("illegal data: " & data)
        End If
        Return _Encode(_data)
    End Function

    Private Function _Encode(ByVal data As List(Of Byte)) As Byte()
        Dim cs As New List(Of Byte)
        cs.AddRange(START_PATTERN)
        For i As Integer = 0 To 3
            addCodes(cs, data(i))
        Next
        cs.AddRange(CENTER_PATTERN)
        For i As Integer = 4 To 7
            addCodes(cs, data(i))
        Next
        cs.AddRange(STOP_PATTERN)
        Return cs.ToArray
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
        Dim h As Single = r.Height - Me.MarginY * 2
        Dim _h As Single = h
        Dim fs As Single = 0.0F
        If Me.WithText Then
            _h *= 0.7F
            fs = h * 0.2F
            fs = Math.Min(fs, ((w * 0.9F) / data.Length) * 2.0F)
            fs = Math.Max(fs, 6.0F)
        End If
        If w <= 0 Or h <= 0 Then
            Exit Sub
        End If
        Dim _data As List(Of Byte) = pack(data)
        If _data.Count = 7 Then
            _data.Add(Me.calcCheckDigit(_data))
        End If
        If _data.Count <> 8 Then
            Throw New ArgumentException("illegal data: " & data)
        End If
        Dim cs() As Byte = _Encode(_data)
        Dim mw As Single = w / (8 * 7 + 11)
        Dim draw As Boolean = True
        Dim x As Single = MarginX
        For i As Integer = 0 To cs.Length - 1
            Dim dw As Single = cs(i) * mw
            If draw Then
                Dim __h As Single = _h
                If Array.IndexOf(GUARDS, i) >= 0 Then
                    __h += fs / 2
                End If
                g.FillRectangle(Brushes.Black, _
                                New RectangleF(r.X + x, r.Y + MarginY, dw * BarWidth, __h))
            End If
            draw = Not draw
            x += dw
        Next
        If Me.WithText Then
            Dim f As New Font("Arial", fs)
            Dim format As StringFormat = New StringFormat()
            format.Alignment = StringAlignment.Center
            For i As Integer = 0 To 7
                g.DrawString(_data(i), f, Brushes.Black, r.X + CHARPOS(i) * mw + MarginX, r.Y + _h + MarginY, format)
            Next
        End If
    End Sub

End Class
