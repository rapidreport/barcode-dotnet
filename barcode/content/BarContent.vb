Namespace content

    Public Class BarContent

        Private _bars As New List(Of CBar)
        Private _text As New List(Of CText)

        Public Function GetBars() As List(Of CBar)
            Return _bars
        End Function

        Public Function GetText() As List(Of CText)
            Return _text
        End Function

        Public Sub SetBars(ByVal bars As List(Of CBar))
            _bars = bars
        End Sub

        Public Sub SetText(ByVal text As List(Of CText))
            _text = text
        End Sub

        Public Sub Add(ByVal bar As CBar)
            _bars.Add(bar)
        End Sub

        Public Sub Add(ByVal text As CText)
            _text.Add(text)
        End Sub

        Public Sub Draw(ByVal g As Graphics)
            DrawBars(g)
            DrawText(g)
        End Sub

        Public Sub DrawBars(ByVal g As Graphics)
            If GetBars() Is Nothing OrElse GetBars.Count = 0 Then
                Exit Sub
            End If
            For Each b As CBar In GetBars()
                g.FillRectangle(Brushes.Black, b.GetX, b.GetY, b.GetWidth, b.GetHeight)
            Next
        End Sub

        Public Sub DrawText(ByVal g As Graphics)
            If GetBars() Is Nothing OrElse GetBars.Count = 0 Then
                Exit Sub
            End If
            If GetText() Is Nothing OrElse GetText.Count = 0 Then
                Exit Sub
            End If
            For Each t As CText In GetText()
                g.DrawString(t.GetCode, t.GetFont, Brushes.Black, t.GetX, t.GetY, t.GetFormat)
            Next
        End Sub

        Public Class CBar

            Private _x As Single
            Private _y As Single
            Private _width As Single
            Private _height As Single

            Public Sub New(ByVal x As Single, ByVal y As Single, ByVal width As Single, ByVal height As Single)
                _x = x
                _y = y
                _width = width
                _height = height
            End Sub

            Public Sub New(ByVal r As RectangleF)
                _x = r.X
                _y = r.Y
                _width = r.Height
                _height = r.Height
            End Sub

            Public Function GetX() As Single
                Return _x
            End Function

            Public Function GetY() As Single
                Return _y
            End Function

            Public Function GetWidth() As Single
                Return _width
            End Function

            Public Function GetHeight() As Single
                Return _height
            End Function

            Public Sub SetX(ByVal x As Single)
                _x = x
            End Sub

            Public Sub SetY(ByVal y As Single)
                _y = y
            End Sub

            Public Sub SetWidth(ByVal width As Single)
                _width = width
            End Sub

            Public Sub SetHeight(ByVal height As Single)
                _height = height
            End Sub

        End Class

        Public Class CText

            Private _code As String
            Private _font As Font
            Private _x As Single
            Private _y As Single
            Private _format As StringFormat

            Public Sub New(ByVal code As String, ByVal font As Font, ByVal x As Single, ByVal y As Single, ByVal format As StringFormat)
                _code = code
                _font = font
                _x = x
                _y = y
                _format = format
            End Sub

            Public Function GetCode() As String
                Return _code
            End Function

            Public Function GetFont() As Font
                Return _font
            End Function

            Public Function GetX() As Single
                Return _x
            End Function

            Public Function GetY() As Single
                Return _y
            End Function

            Public Function GetFormat() As StringFormat
                Return _format
            End Function

            Public Sub SetCode(ByVal code As String)
                _code = code
            End Sub

            Public Sub SetFont(ByVal font As Font)
                _font = font
            End Sub

            Public Sub SetX(ByVal x As Single)
                _x = x
            End Sub

            Public Sub SetY(ByVal y As Single)
                _y = y
            End Sub

            Public Sub SetFormat(ByVal format As StringFormat)
                _format = format
            End Sub

        End Class
    End Class

End Namespace
