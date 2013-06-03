﻿Namespace content

    Public Class PointScale
        Inherits Scale

        Public Sub New(ByVal marginX As Single, ByVal marginY As Single, _
                       ByVal width As Single, ByVal height As Single, ByVal dpi As Integer)
            MyBase.New(marginX, marginY, width, height, dpi)
        End Sub

        Public Sub New(ByVal marginX As Single, ByVal marginY As Single, _
                       ByVal width As Single, ByVal height As Single)
            MyBase.New(marginX, marginY, width, height, DPI)
        End Sub

        Public Overrides Function PixelMarginX() As Single
            Return PointToPixel(GetDpi(), GetMarginX())
        End Function

        Public Overrides Function PixelMarginY() As Single
            Return PointToPixel(GetDpi(), GetMarginY())
        End Function

        Public Overrides Function PixelWidth() As Single
            Return PointToPixel(GetDpi(), GetWidth()) - (PixelMarginX() * 2)
        End Function

        Public Overrides Function PixelHeight() As Single
            Return PointToPixel(GetDpi(), GetHeigth()) - (PixelMarginY() * 2)
        End Function

    End Class

End Namespace