Public Class CBarcode

    Public Shared BarWidth As Single = 1.0F

    Public MarginX As Single = 2.0F
    Public MarginY As Single = 2.0F

    Public WithText As Boolean = True

    Public Shared Function pointToPixel(ByVal dpi As Integer, ByVal point As Single) As Single
        Return dpi * (point / 72.0F)
    End Function

    Public Shared Function mmToPixel(ByVal dpi As Integer, ByVal mm As Single) As Single
        Return dpi * (mm / 25.4F)
    End Function

    Public Class BarContent

        Private _bars As New List(Of Bar)
        Private _text As Text = Nothing

        Public Sub New()
        End Sub

        Public Function GetBars() As List(Of Bar)
            Return _bars
        End Function

        Public Function GetText() As Text
            Return _text
        End Function

        Public Sub SetBars(ByVal bars As List(Of Bar))
            Me._bars = bars
        End Sub

        Public Sub SetText(ByVal text As Text)
            Me._text = text
        End Sub

        Public Sub Add(ByVal bar As Bar)
            _bars.Add(bar)
        End Sub

        Public Class Bar

            Private _x As Single = 0
            Private _y As Single = 0
            Private _width As Single = 0
            Private _height As Single = 0

            Public Sub New(ByVal x As Single, ByVal y As Single, ByVal width As Single, ByVal height As Single)
                Me._x = x
                Me._y = y
                Me._width = width
                Me._height = height
            End Sub

            Public Sub New(ByVal r As RectangleF)
                Me._x = r.X
                Me._y = r.Y
                Me._width = r.Height
                Me._height = r.Height
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
                Me._x = x
            End Sub

            Public Sub SetY(ByVal y As Single)
                Me._y = y
            End Sub

            Public Sub SetWidth(ByVal width As Single)
                Me._width = width
            End Sub

            Public Sub SetHeight(ByVal height As Single)
                Me._height = height
            End Sub

        End Class

        Public Class Text

            Private _code As String
            Private _font As Font
            Private _x As Single
            Private _y As Single
            Private _format As StringFormat

            Public Sub New(ByVal code As String, ByVal font As Font, ByVal x As Single, ByVal y As Single, ByVal format As StringFormat)
                Me._code = code
                Me._font = font
                Me._x = x
                Me._y = y
                Me._format = format
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
                Me._code = code
            End Sub

            Public Sub SetFont(ByVal font As Font)
                Me._font = font
            End Sub

            Public Sub SetX(ByVal x As Single)
                Me._x = x
            End Sub

            Public Sub SetY(ByVal y As Single)
                Me._y = y
            End Sub

            Public Sub SetFormat(ByVal format As StringFormat)
                Me._format = format
            End Sub

        End Class

    End Class

End Class
