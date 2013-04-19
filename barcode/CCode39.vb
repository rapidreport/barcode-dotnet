Public Class CCode39
    Inherits CBarcode

    Private Shared CODE_PATTERNS(,) As Byte = _
      {{0, 0, 0, 1, 1, 0, 1, 0, 0}, _
       {1, 0, 0, 1, 0, 0, 0, 0, 1}, _
       {0, 0, 1, 1, 0, 0, 0, 0, 1}, _
       {1, 0, 1, 1, 0, 0, 0, 0, 0}, _
       {0, 0, 0, 1, 1, 0, 0, 0, 1}, _
       {1, 0, 0, 1, 1, 0, 0, 0, 0}, _
       {0, 0, 1, 1, 1, 0, 0, 0, 0}, _
       {0, 0, 0, 1, 0, 0, 1, 0, 1}, _
       {1, 0, 0, 1, 0, 0, 1, 0, 0}, _
       {0, 0, 1, 1, 0, 0, 1, 0, 0}, _
       {1, 0, 0, 0, 0, 1, 0, 0, 1}, _
       {0, 0, 1, 0, 0, 1, 0, 0, 1}, _
       {1, 0, 1, 0, 0, 1, 0, 0, 0}, _
       {0, 0, 0, 0, 1, 1, 0, 0, 1}, _
       {1, 0, 0, 0, 1, 1, 0, 0, 0}, _
       {0, 0, 1, 0, 1, 1, 0, 0, 0}, _
       {0, 0, 0, 0, 0, 1, 1, 0, 1}, _
       {1, 0, 0, 0, 0, 1, 1, 0, 0}, _
       {0, 0, 1, 0, 0, 1, 1, 0, 0}, _
       {0, 0, 0, 0, 1, 1, 1, 0, 0}, _
       {1, 0, 0, 0, 0, 0, 0, 1, 1}, _
       {0, 0, 1, 0, 0, 0, 0, 1, 1}, _
       {1, 0, 1, 0, 0, 0, 0, 1, 0}, _
       {0, 0, 0, 0, 1, 0, 0, 1, 1}, _
       {1, 0, 0, 0, 1, 0, 0, 1, 0}, _
       {0, 0, 1, 0, 1, 0, 0, 1, 0}, _
       {0, 0, 0, 0, 0, 0, 1, 1, 1}, _
       {1, 0, 0, 0, 0, 0, 1, 1, 0}, _
       {0, 0, 1, 0, 0, 0, 1, 1, 0}, _
       {0, 0, 0, 0, 1, 0, 1, 1, 0}, _
       {1, 1, 0, 0, 0, 0, 0, 0, 1}, _
       {0, 1, 1, 0, 0, 0, 0, 0, 1}, _
       {1, 1, 1, 0, 0, 0, 0, 0, 0}, _
       {0, 1, 0, 0, 1, 0, 0, 0, 1}, _
       {1, 1, 0, 0, 1, 0, 0, 0, 0}, _
       {0, 1, 1, 0, 1, 0, 0, 0, 0}, _
       {0, 1, 0, 0, 0, 0, 1, 0, 1}, _
       {1, 1, 0, 0, 0, 0, 1, 0, 0}, _
       {0, 1, 1, 0, 0, 0, 1, 0, 0}, _
       {0, 1, 0, 1, 0, 1, 0, 0, 0}, _
       {0, 1, 0, 1, 0, 0, 0, 1, 0}, _
       {0, 1, 0, 0, 0, 1, 0, 1, 0}, _
       {0, 0, 0, 1, 0, 1, 0, 1, 0}, _
       {0, 1, 0, 0, 1, 0, 1, 0, 0}}

    Private Const CHARS As String = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%*"
    Private Const START_STOP_POINT As Integer = 43

    Public GenerateCheckSum As Boolean = False
    Public WithCheckSumText As Boolean = False

    Public Function Encode(ByVal data As String) As Byte()
        If data Is Nothing OrElse data.Length = 0 Then
            Return Nothing
        End If
        Dim ps As New List(Of Integer)
        With Nothing
            ps.Add(START_STOP_POINT)
            Dim _ps As List(Of Integer) = getCodePoints(data)
            ps.AddRange(_ps)
            If Me.GenerateCheckSum Then
                ps.Add(Me.calcCheckDigit(_ps))
            End If
            ps.Add(START_STOP_POINT)
        End With
        Return _Encode(ps)
    End Function

    Protected Function _Encode(ByVal ps As List(Of Integer)) As Byte()
        Dim ret As New List(Of Byte)
        For Each p As Integer In ps
            Me.addCodes(ret, p)
        Next
        Return ret.ToArray
    End Function

    Protected Sub addCodes(ByVal l As List(Of Byte), ByVal p As Integer)
        If l.Count > 0 Then
            l.Add(0)
        End If
        l.Add(CODE_PATTERNS(p, 0))
        l.Add(CODE_PATTERNS(p, 1))
        l.Add(CODE_PATTERNS(p, 2))
        l.Add(CODE_PATTERNS(p, 3))
        l.Add(CODE_PATTERNS(p, 4))
        l.Add(CODE_PATTERNS(p, 5))
        l.Add(CODE_PATTERNS(p, 6))
        l.Add(CODE_PATTERNS(p, 7))
        l.Add(CODE_PATTERNS(p, 8))
    End Sub

    Private Function getCodePoints(ByVal data As String) As List(Of Integer)
        Dim ret As New List(Of Integer)
        For Each c As Char In data
            Dim p As Integer = CHARS.IndexOf(c)
            If p >= 0 Then
                ret.Add(p)
            Else
                Throw New ArgumentException("illegal data: " & data)
            End If
        Next
        Return ret
    End Function

    Private Function calcCheckDigit(ByVal ps As List(Of Integer)) As Integer
        Dim s As Integer = 0
        For Each p As Integer In ps
            s += p
        Next
        Return s Mod 43
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
        If Me.WithText Then
            _h *= 0.7F
        End If
        If w <= 0 Or h <= 0 Then
            Exit Sub
        End If
        Dim ps As New List(Of Integer)
        Dim _data As String = data
        With Nothing
            ps.Add(START_STOP_POINT)
            Dim _ps As List(Of Integer) = getCodePoints(data)
            ps.AddRange(_ps)
            If Me.GenerateCheckSum Then
                Dim cd As Integer = Me.calcCheckDigit(_ps)
                ps.Add(cd)
                If Me.WithCheckSumText Then
                    _data &= CHARS(cd)
                End If
            End If
            ps.Add(START_STOP_POINT)
            _data = "*" & _data & "*"
        End With
        Dim cs As Byte() = Me._Encode(ps)
        Dim mw As Single = w / (ps.Count * 13 - 1)
        Dim draw As Boolean = True
        Dim x As Single = MarginX
        For i As Integer = 0 To cs.Length - 1
            Dim dw As Single = (cs(i) + 1) * mw
            If draw Then
                g.FillRectangle(Brushes.Black, _
                                New RectangleF(r.X + x, r.Y + MarginY, dw * BarWidth, _h))
            End If
            draw = Not draw
            x += dw
        Next
        If Me.WithText Then
            Dim fs As Single = h * 0.2F
            fs = Math.Min(fs, ((w * 0.9F) / _data.Length) * 2.0F)
            fs = Math.Max(fs, 6.0F)
            Dim f As New Font("Arial", fs)
            Dim format As StringFormat = New StringFormat()
            format.Alignment = StringAlignment.Center
            g.DrawString(_data, f, Brushes.Black, r.X + w / 2 + MarginX, r.Y + _h + MarginY, format)
        End If
    End Sub

End Class
