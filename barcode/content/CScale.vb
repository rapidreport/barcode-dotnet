Namespace content

    Public MustInherit Class CScale

        Public Const DPI As Integer = 72

        Private _marginX As Single
        Private _marginY As Single
        Private _width As Single
        Private _heigth As Single
        Private _dpi As Integer

        Public Sub New(ByVal marginX As Single, ByVal marginY As Single, _
                       ByVal width As Single, ByVal height As Single, ByVal dpi As Integer)
            _marginX = marginX
            _marginY = marginY
            _width = width
            _heigth = height
            _dpi = dpi
        End Sub

        Public Sub New(ByVal marginX As Single, ByVal marginY As Single, _
                       ByVal width As Single, ByVal height As Single)
            Me.New(marginX, marginY, width, height, DPI)
        End Sub

        Public Shared Function pointToPixel(ByVal dpi As Integer, ByVal point As Single) As Single
            Return dpi * (point / 72.0F)
        End Function

        Public Shared Function mmToPixel(ByVal dpi As Integer, ByVal mm As Single) As Single
            Return dpi * (mm / 25.4F)
        End Function

        Public Function GetMarginX() As Single
            Return _marginX
        End Function

        Public Function GetMarginY() As Single
            Return _marginY
        End Function

        Public Function GetWidth() As Single
            Return _width
        End Function

        Public Function GetHeigth() As Single
            Return _heigth
        End Function

        Public Function GetDpi() As Integer
            Return _dpi
        End Function

        Public Sub SetMarginX(ByVal marginX As Single)
            _marginX = marginX
        End Sub

        Public Sub SetMarginY(ByVal marginY As Single)
            _marginY = marginY
        End Sub

        Public Sub SetWidth(ByVal width As Single)
            _width = width
        End Sub

        Public Sub SetHeight(ByVal heigth As Single)
            _heigth = heigth
        End Sub

        Public Sub SetDpi(ByVal dpi As Integer)
            _dpi = dpi
        End Sub

        Public MustOverride Function PixelMarginX() As Single

        Public MustOverride Function PixelMarginY() As Single

        Public MustOverride Function PixelWidth() As Single

        Public MustOverride Function PixelHeight() As Single

    End Class

End Namespace