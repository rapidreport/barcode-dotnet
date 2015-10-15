Public Class Yubin
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
            For Each p As Byte In cp
                ret.Add(CODE_PATTERNS(p, 0))
                ret.Add(CODE_PATTERNS(p, 1))
                ret.Add(CODE_PATTERNS(p, 2))
                sum_p += p
                l += 1
                If l = MAX_SIZE Then
                    Exit For
                End If
            Next
            If l = MAX_SIZE Then
                Exit For
            End If
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
                Throw New ArgumentException("(yubin)不正なデータです: " & data)
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
            g.FillRectangle(Brushes.Black, New RectangleF(x, by, uw * BarWidth, bh))
            x += uw * 2
        Next
    End Sub

End Class