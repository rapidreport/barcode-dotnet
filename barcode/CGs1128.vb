Imports System.Text
Imports System.Text.RegularExpressions
Imports jp.co.systembase.barcode.CBarcode.BarContent

Public Class CGs1128
    Inherits CBarcode

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
    Private Const SET_C As Integer = 99
    Private Const SET_B As Integer = 100
    Private Const SET_A As Integer = 101
    Private Const FNC1 As Integer = 102
    Private Const START_A As Integer = 103
    Private Const START_B As Integer = 104
    Private Const START_C As Integer = 105

    Private Const CHARS As String = "!""%&'()*+,-./0123456789:;<=>?ABCDEFGHIJKLMNOPQRSTUVWXYZ_abcdefghijklmnopqrstuvwxyz"

    Private Shared TYPE_A As New CodeTypeA
    Private Shared TYPE_B As New CodeTypeB
    Private Shared TYPE_C As New CodeTypeC

    Private MustInherit Class CodeType

        Dim _startCodePoint As Integer = 0
        Dim _codeSetPoint As Integer = 0

        Public MustOverride Function GetCodePoint(ByVal data As String) As Integer

        Public Sub New(ByVal startCodePoint As Integer, ByVal codeSetPoint As Integer)
            Me._startCodePoint = startCodePoint
            Me._codeSetPoint = codeSetPoint
        End Sub

        Public Overridable Function GetNextData(ByVal data As String) As String
            Return data.Substring(1)
        End Function

        Public Function GetStartCodePoint() As Integer
            Return _startCodePoint
        End Function

        Public Function GetCodeSetPoint() As Integer
            Return _codeSetPoint
        End Function

        Public Shared Function GetCodeType(ByVal data As String) As CodeType
            If data.Length >= 2 Then
                If Char.IsDigit(data(0)) And Char.IsDigit(data(1)) Then
                    Return TYPE_C
                End If
            End If
            If Asc(data(0)) <= &H1F Then ' <= 'US'
                Return TYPE_A
            End If
            Return TYPE_B
        End Function

    End Class

    Private Class CodeTypeA
        Inherits CodeType

        Public Sub New()
            MyBase.New(START_A, SET_A)
        End Sub

        Public Overrides Function GetCodePoint(ByVal data As String) As Integer
            Dim c As Integer = Asc(data(0))
            If c <= &H1F Then ' <= 'US'
                Return c + &H40 ' + '@'
            Else
                Return c - &H20 ' - 'SP'
            End If
        End Function

    End Class

    Private Class CodeTypeB
        Inherits CodeType

        Public Sub New()
            MyBase.New(START_B, SET_B)
        End Sub

        Public Overrides Function GetCodePoint(ByVal data As String) As Integer
            Return Asc(data(0)) - &H20 ' - 'SP'
        End Function

    End Class

    Private Class CodeTypeC
        Inherits CodeType

        Public Sub New()
            MyBase.New(START_C, SET_C)
        End Sub

        Public Overrides Function GetCodePoint(ByVal data As String) As Integer
            Return Integer.Parse(data.Substring(0, 2))
        End Function

        Public Overrides Function GetNextData(ByVal data As String) As String
            Return data.Substring(2)
        End Function

    End Class

    Private Const DPI As Integer = 72

    Private Const AI_START_PATTERN As String = "#{"
    Private Const AI_NUMBER_PATTERN As String = "[0-9]{2,4}"
    Private Const AI_END_PATTERN As String = "}"
    Private Const AI_PATTERN As String = AI_START_PATTERN + AI_NUMBER_PATTERN + AI_END_PATTERN

    Private Shared FIXED_AI As New Dictionary(Of String, Integer) From _
        {{"00", 20}, _
         {"01", 16}, _
         {"02", 16}, _
         {"03", 16}, _
         {"04", 18}, _
         {"11", 8}, _
         {"12", 8}, _
         {"13", 8}, _
         {"14", 8}, _
         {"15", 8}, _
         {"16", 8}, _
         {"17", 8}, _
         {"18", 8}, _
         {"19", 8}, _
         {"20", 4}, _
         {"31", 10}, _
         {"32", 10}, _
         {"33", 10}, _
         {"34", 10}, _
         {"35", 10}, _
         {"36", 10}, _
         {"41", 16}}

    Private _encodeCache As New Dictionary(Of String, List(Of String()))
    Private Const CACHE_MAX_SIZE As Integer = 10

    Public Function encode(ByVal data) As List(Of Byte())
        If data Is Nothing OrElse data.Length = 0 Then
            Return Nothing
        End If

        validate(data)

        Dim points As New List(Of Integer)
        Dim codes As List(Of String()) = createCodeMap(data)
        Dim type As CodeType = TYPE_C
        points.Add(type.GetStartCodePoint)
        points.Add(FNC1)
        For Each map As String() In codes
            Dim _data = map(0) & map(1)
            While _data.Length > 0
                Dim _type As CodeType = CodeType.GetCodeType(_data)
                If type.GetStartCodePoint <> _type.GetStartCodePoint Then
                    points.Add(_type.GetCodeSetPoint)
                    type = _type
                End If
                points.Add(_type.GetCodePoint(_data))
                _data = _type.GetNextData(_data)
            End While
            Dim ai_2 As String = map(0).Substring(0, 2)
            If Not FIXED_AI.ContainsKey(ai_2) Then
                points.Add(FNC1)
            End If
        Next
        If points(points.Count - 1) = FNC1 Then
            points.RemoveAt(points.Count - 1)
        End If
        points.Add(calcCheckDigit(points))

        Dim ret As New List(Of Byte())
        For Each point As Integer In points
            Dim code As New List(Of Byte)
            For i As Integer = 0 To 5
                code.Add(CODE_PATTERNS(point, i))
            Next
            ret.Add(code.ToArray)
        Next
        ret.Add(STOP_PATTERN)

        Return ret
    End Function

    Protected Function _Encode(ByVal data) As String
        Dim codes As List(Of String()) = createCodeMap(data)
        Dim sb As New StringBuilder()
        For Each map As String() In codes
            sb.Append("(" & map(0) & ")" & map(1))
        Next
        Return sb.ToString
    End Function

    Private Sub validate(ByVal data As String)
        Dim r As New Regex(AI_PATTERN)
        Dim _data As String = r.Replace(data, "")
        For Each c As Char In _data
            If CHARS.IndexOf(c) < 0 Then
                Throw New ArgumentException("illegal data: " & data)
            End If
        Next
        If Not data.StartsWith("#{") Or data.EndsWith("}") Then
            Throw New ArgumentException("illegal data: " & data)
        End If
        Dim codes As List(Of String()) = createCodeMap(data)
        For Each map As String() In codes
            Dim ai_2 As String = map(0).Substring(0, 2)
            If FIXED_AI.ContainsKey(ai_2) Then
                If FIXED_AI(ai_2) <> (map(0).Length + map(1).Length) Then
                    Throw New ArgumentException("illegal ai length: (" & map(0) & ")")
                End If
            End If
        Next

    End Sub

    Private Function createCodeMap(ByVal data As String) As List(Of String())
        If _encodeCache.ContainsKey(data) Then
            Return _encodeCache(data)
        End If
        Dim ret As New List(Of String())
        Dim r As New Regex(AI_PATTERN)
        Dim codes As String() = r.Split(data)
        r = New Regex(AI_START_PATTERN & "(?<ai>" & AI_NUMBER_PATTERN & ")" & AI_END_PATTERN)
        Dim m As Match = r.Match(data)
        For i As Integer = 1 To codes.Length - 1
            ret.Add({m.Groups("ai").Value, codes(i)})
            m = m.NextMatch()
        Next
        If _encodeCache.Count = CACHE_MAX_SIZE Then
            Dim key As String = String.Empty
            For Each _key As String In _encodeCache.Keys
                key = _key
                Exit For
            Next
            _encodeCache.Remove(key)
        End If
        _encodeCache.Add(data, ret)
        Return ret
    End Function

    Private Function calcCheckDigit(ByVal points As List(Of Integer)) As Byte
        Dim sum As Integer = points(0)
        For i As Integer = 1 To points.Count - 1
            sum += i * points(i)
        Next
        Const checkNum As Integer = 103
        Return sum Mod checkNum
    End Function

    Public Function CreateContent(ByVal x As Single, ByVal y As Single, ByVal w As Single, ByVal h As Single, _
                                  ByVal data As String) As BarContent
        Return CreateContent(x, y, w, h, DPI, data)
    End Function

    Public Function CreateContent(ByVal x As Single, ByVal y As Single, ByVal w As Single, ByVal h As Single, _
                                  ByVal dpi As Integer, ByVal data As String) As BarContent
        Return CreateContent(New RectangleF(x, y, w, h), dpi, data)
    End Function

    Public Function CreateContent(ByVal r As RectangleF, ByVal data As String) As BarContent
        Return CreateContent(r, DPI, data)
    End Function

    Public Function CreateContent(ByVal r As RectangleF, ByVal dpi As Integer, ByVal data As String) As BarContent
        Dim marginX As Single = pointToPixel(dpi, Me.MarginX)
        Dim marginY As Single = pointToPixel(dpi, Me.MarginY)

        Dim barWidth As Single = mmToPixel(dpi, 0.191F)

        Dim width As Single = 0.0F
        Dim codes As List(Of Byte()) = encode(data)
        For Each code As Byte() In codes
            For Each c As Integer In code
                width += barWidth * c
            Next
        Next

        Dim h As Single = pointToPixel(dpi, r.Height) - marginY * 2
        Dim barHeight As Single = h
        If WithText Then
            barHeight *= 0.7F
        End If
        Dim height = barHeight + marginY


        Dim w As Single = pointToPixel(dpi, r.Width) - marginX * 2
        If w <= 0 Or h <= 0 Then
            Return Nothing
        End If

        Dim ret As New BarContent
        Dim xPos As Single = 0.0F
        Dim scale As Single = (w - marginX) / width
        For Each code As Byte() In codes
            For i As Integer = 0 To code.Length - 1
                Dim c As Integer = code(i)
                Dim _barWidth As Single = barWidth * c * scale
                If i Mod 2 = 0 Then
                    Dim b As New BarContent.Bar(r.X + xPos + marginX, r.Y + marginY, _barWidth, barHeight)
                    ret.Add(b)
                End If
                xPos += _barWidth
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
            Dim t As New BarContent.Text(_data, f, r.X + w / 2 + marginX, r.Y + height, format)
            ret.SetText(t)
        End If

        Return ret
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
        Dim c As BarContent = CreateContent(r, dpi, data)
        If c Is Nothing Then
            Exit Sub
        End If

        For Each b As Bar In c.GetBars
            g.FillRectangle(Brushes.Black, b.GetX, b.GetY, b.GetWidth, b.GetHeight)
        Next

        Dim t As Text = c.GetText
        If Not t Is Nothing Then
            g.DrawString(t.GetCode, t.GetFont, Brushes.Black, t.GetX, t.GetY, t.GetFormat)
        End If
    End Sub

End Class
