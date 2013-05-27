Namespace content

    Public Class CPointScale
        Inherits CScale

        Public Sub New(ByVal marginX As Single, ByVal marginY As Single, _
                       ByVal width As Single, ByVal height As Single, ByVal dpi As Integer)
            MyBase.New(marginX, marginY, width, height, dpi)
        End Sub

        Public Sub New(ByVal marginX As Single, ByVal marginY As Single, _
                       ByVal width As Single, ByVal height As Single)
            MyBase.New(marginX, marginY, width, height, DPI)
        End Sub

        Public Overrides Function PixelMarginX() As Single
            Return pointToPixel(GetDpi(), GetMarginX())
        End Function

        Public Overrides Function PixelMarginY() As Single
            Return pointToPixel(GetDpi(), GetMarginY())
        End Function

        Public Overrides Function PixelWidth() As Single
            Return pointToPixel(GetDpi(), GetWidth()) - (PixelMarginX() * 2)
        End Function

        Public Overrides Function PixelHeight() As Single
            Return pointToPixel(GetDpi(), GetHeigth()) - (PixelMarginY() * 2)
        End Function

    End Class

End Namespace