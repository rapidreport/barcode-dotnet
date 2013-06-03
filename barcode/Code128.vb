Public Class Code128
    Inherits Barcode

    Private Shared CODE_PATTERNS(,) As Byte = _
      {{2, 1, 2, 2, 2, 2}, _
       {2, 2, 2, 1, 2, 2}, _
       {2, 2, 2, 2, 2, 1}, _
       {1, 2, 1, 2, 2, 3}, _
       {1, 2, 1, 3, 2, 2}, _
       {1, 3, 1, 2, 2, 2}, _
       {1, 2, 2, 2, 1, 3}, _
       {1, 2, 2, 3, 1, 2}, _
       {1, 3, 2, 2, 1, 2}, _
       {2, 2, 1, 2, 1, 3}, _
       {2, 2, 1, 3, 1, 2}, _
       {2, 3, 1, 2, 1, 2}, _
       {1, 1, 2, 2, 3, 2}, _
       {1, 2, 2, 1, 3, 2}, _
       {1, 2, 2, 2, 3, 1}, _
       {1, 1, 3, 2, 2, 2}, _
       {1, 2, 3, 1, 2, 2}, _
       {1, 2, 3, 2, 2, 1}, _
       {2, 2, 3, 2, 1, 1}, _
       {2, 2, 1, 1, 3, 2}, _
       {2, 2, 1, 2, 3, 1}, _
       {2, 1, 3, 2, 1, 2}, _
       {2, 2, 3, 1, 1, 2}, _
       {3, 1, 2, 1, 3, 1}, _
       {3, 1, 1, 2, 2, 2}, _
       {3, 2, 1, 1, 2, 2}, _
       {3, 2, 1, 2, 2, 1}, _
       {3, 1, 2, 2, 1, 2}, _
       {3, 2, 2, 1, 1, 2}, _
       {3, 2, 2, 2, 1, 1}, _
       {2, 1, 2, 1, 2, 3}, _
       {2, 1, 2, 3, 2, 1}, _
       {2, 3, 2, 1, 2, 1}, _
       {1, 1, 1, 3, 2, 3}, _
       {1, 3, 1, 1, 2, 3}, _
       {1, 3, 1, 3, 2, 1}, _
       {1, 1, 2, 3, 1, 3}, _
       {1, 3, 2, 1, 1, 3}, _
       {1, 3, 2, 3, 1, 1}, _
       {2, 1, 1, 3, 1, 3}, _
       {2, 3, 1, 1, 1, 3}, _
       {2, 3, 1, 3, 1, 1}, _
       {1, 1, 2, 1, 3, 3}, _
       {1, 1, 2, 3, 3, 1}, _
       {1, 3, 2, 1, 3, 1}, _
       {1, 1, 3, 1, 2, 3}, _
       {1, 1, 3, 3, 2, 1}, _
       {1, 3, 3, 1, 2, 1}, _
       {3, 1, 3, 1, 2, 1}, _
       {2, 1, 1, 3, 3, 1}, _
       {2, 3, 1, 1, 3, 1}, _
       {2, 1, 3, 1, 1, 3}, _
       {2, 1, 3, 3, 1, 1}, _
       {2, 1, 3, 1, 3, 1}, _
       {3, 1, 1, 1, 2, 3}, _
       {3, 1, 1, 3, 2, 1}, _
       {3, 3, 1, 1, 2, 1}, _
       {3, 1, 2, 1, 1, 3}, _
       {3, 1, 2, 3, 1, 1}, _
       {3, 3, 2, 1, 1, 1}, _
       {3, 1, 4, 1, 1, 1}, _
       {2, 2, 1, 4, 1, 1}, _
       {4, 3, 1, 1, 1, 1}, _
       {1, 1, 1, 2, 2, 4}, _
       {1, 1, 1, 4, 2, 2}, _
       {1, 2, 1, 1, 2, 4}, _
       {1, 2, 1, 4, 2, 1}, _
       {1, 4, 1, 1, 2, 2}, _
       {1, 4, 1, 2, 2, 1}, _
       {1, 1, 2, 2, 1, 4}, _
       {1, 1, 2, 4, 1, 2}, _
       {1, 2, 2, 1, 1, 4}, _
       {1, 2, 2, 4, 1, 1}, _
       {1, 4, 2, 1, 1, 2}, _
       {1, 4, 2, 2, 1, 1}, _
       {2, 4, 1, 2, 1, 1}, _
       {2, 2, 1, 1, 1, 4}, _
       {4, 1, 3, 1, 1, 1}, _
       {2, 4, 1, 1, 1, 2}, _
       {1, 3, 4, 1, 1, 1}, _
       {1, 1, 1, 2, 4, 2}, _
       {1, 2, 1, 1, 4, 2}, _
       {1, 2, 1, 2, 4, 1}, _
       {1, 1, 4, 2, 1, 2}, _
       {1, 2, 4, 1, 1, 2}, _
       {1, 2, 4, 2, 1, 1}, _
       {4, 1, 1, 2, 1, 2}, _
       {4, 2, 1, 1, 1, 2}, _
       {4, 2, 1, 2, 1, 1}, _
       {2, 1, 2, 1, 4, 1}, _
       {2, 1, 4, 1, 2, 1}, _
       {4, 1, 2, 1, 2, 1}, _
       {1, 1, 1, 1, 4, 3}, _
       {1, 1, 1, 3, 4, 1}, _
       {1, 3, 1, 1, 4, 1}, _
       {1, 1, 4, 1, 1, 3}, _
       {1, 1, 4, 3, 1, 1}, _
       {4, 1, 1, 1, 1, 3}, _
       {4, 1, 1, 3, 1, 1}, _
       {1, 1, 3, 1, 4, 1}, _
       {1, 1, 4, 1, 3, 1}, _
       {3, 1, 1, 1, 4, 1}, _
       {4, 1, 1, 1, 3, 1}, _
       {2, 1, 1, 4, 1, 2}, _
       {2, 1, 1, 2, 1, 4}, _
       {2, 1, 1, 2, 3, 2}}
    Private Shared STOP_PATTERN() As Byte = {2, 3, 3, 1, 1, 1, 2}

    Private Const TO_C As Integer = 99
    Private Const TO_B As Integer = 100
    Private Const TO_A As Integer = 101
    Private Const START_A As Integer = 103
    Private Const START_B As Integer = 104
    Private Const START_C As Integer = 105

    Private Enum ECodeType
        NO_CHANGE
        A
        B
        C
    End Enum

    Public Function Encode(ByVal data As String) As Byte()
        If data Is Nothing OrElse data.Length = 0 Then
            Return Nothing
        End If
        Me.validation(data)
        Dim ps As List(Of Integer) = Me.getCodePoints(data)
        Dim cs As New List(Of Byte)
        For Each p As Integer In ps
            Me.addCodes(cs, p)
        Next
        addCodes(cs, Me.calcCheckDigit(ps))
        cs.AddRange(STOP_PATTERN)
        Return cs.ToArray
    End Function

    Private Sub validation(ByVal data As String)
        For Each c As Char In data
            If Asc(c) > &H7F Then
                Throw New ArgumentException("illegal data: " & data)
            End If
        Next
    End Sub

    Private Sub addCodes(ByVal l As List(Of Byte), ByVal p As Integer)
        l.Add(CODE_PATTERNS(p, 0))
        l.Add(CODE_PATTERNS(p, 1))
        l.Add(CODE_PATTERNS(p, 2))
        l.Add(CODE_PATTERNS(p, 3))
        l.Add(CODE_PATTERNS(p, 4))
        l.Add(CODE_PATTERNS(p, 5))
    End Sub

    Private Function getCodePoints(ByVal data As String) As List(Of Integer)
        Dim ret As New List(Of Integer)
        Dim _data As String = data
        Dim codeType As ECodeType = Me.getStartCodeType(data)
        Select Case codeType
            Case ECodeType.A
                ret.Add(START_A)
            Case ECodeType.B
                ret.Add(START_B)
            Case ECodeType.C
                ret.Add(START_C)
        End Select
        Do While (_data.Length > 0)
            Select Case Me.getNextCodeType(_data, codeType)
                Case ECodeType.A
                    ret.Add(TO_A)
                    codeType = ECodeType.A
                Case ECodeType.B
                    ret.Add(TO_B)
                    codeType = ECodeType.B
                Case ECodeType.C
                    ret.Add(TO_C)
                    codeType = ECodeType.C
            End Select
            Select Case codeType
                Case ECodeType.A
                    ret.Add(Me.getCodePointA(_data))
                    _data = _data.Substring(1)
                Case ECodeType.B
                    ret.Add(Me.getCodePointB(_data))
                    _data = _data.Substring(1)
                Case ECodeType.C
                    ret.Add(Me.getCodePointC(_data))
                    _data = _data.Substring(2)
            End Select
        Loop
        Return ret
    End Function

    Private Function getStartCodeType(ByVal data As String) As ECodeType
        If data.Length >= 2 Then
            If Char.IsDigit(data(0)) And Char.IsDigit(data(1)) Then
                Return ECodeType.C
            End If
        End If
        If Asc(data(0)) <= &H1F Then
            Return ECodeType.A
        End If
        Return ECodeType.B
    End Function

    Private Function getNextCodeType(ByVal data As String, ByVal codeType As ECodeType) As ECodeType
        If codeType <> ECodeType.C Then
            If data.Length >= 4 Then
                If Char.IsDigit(data(0)) And Char.IsDigit(data(1)) And Char.IsDigit(data(2)) And Char.IsDigit(data(3)) Then
                    Return ECodeType.C
                End If
            End If
        End If
        If codeType <> ECodeType.A Then
            If Asc(data(0)) <= &H1F Then
                Return ECodeType.A
            End If
        End If
        If codeType <> ECodeType.B Then
            If Asc(data(0)) >= &H60 Then
                Return ECodeType.B
            End If
        End If
        If codeType = ECodeType.C Then
            If data.Length < 2 OrElse (Not Char.IsDigit(data(0)) Or Not Char.IsDigit(data(1))) Then
                Return ECodeType.B
            End If
        End If
        Return ECodeType.NO_CHANGE
    End Function

    Private Function getCodePointA(ByVal data As String) As Integer
        Dim c As Integer = Asc(data(0))
        If c <= &H1F Then
            Return c + &H40
        Else
            Return c - &H20
        End If
    End Function

    Private Function getCodePointB(ByVal data As String) As Integer
        Return Asc(data(0)) - &H20
    End Function

    Private Function getCodePointC(ByVal data As String) As Integer
        Return Integer.Parse(data.Substring(0, 2))
    End Function

    Private Function calcCheckDigit(ByVal ps As List(Of Integer)) As Byte
        Dim t As Integer = ps(0)
        For i As Integer = 1 To ps.Count - 1
            t += i * ps(i)
        Next
        Return t Mod 103
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
        If (Me.WithText) Then
            _h *= 0.7F
        End If
        If w <= 0 Or h <= 0 Then
            Exit Sub
        End If
        With Nothing
            Me.validation(data)
            Dim ps As List(Of Integer) = Me.getCodePoints(data)
            Dim cs As New List(Of Byte)
            For Each p As Integer In ps
                addCodes(cs, p)
            Next
            addCodes(cs, Me.calcCheckDigit(ps))
            cs.AddRange(STOP_PATTERN)
            Dim mw As Single = w / ((ps.Count + 1) * 11 + 13)
            Dim draw As Boolean = True
            Dim x As Single = MarginX
            For Each c As Byte In cs
                Dim dw As Single = c * mw
                If draw Then
                    g.FillRectangle(Brushes.Black, _
                                    New RectangleF(r.X + x, r.Y + MarginY, dw * BarWidth, _h))
                End If
                draw = Not draw
                x += dw
            Next
        End With
        If Me.WithText Then
            Dim fs As Single = h * 0.2F
            fs = Math.Min(fs, ((w * 0.9F) / data.Length) * 2.0F)
            fs = Math.Max(fs, 6.0F)
            Dim f As New Font("Arial", fs)
            Dim format As StringFormat = New StringFormat()
            format.Alignment = StringAlignment.Center
            g.DrawString(data, f, Brushes.Black, r.X + w / 2 + MarginX, r.Y + _h + MarginY, format)
        End If
    End Sub

End Class
