Imports System.Math

Public Class CBarcode

    Public Shared BarWidth As Single = 1.0F

    Public MarginX As Single = 2.0F
    Public MarginY As Single = 2.0F

    Public WithText As Boolean = True

    Public Function FontSize(ByVal width As Single, ByVal heigth As Single, ByVal data As String) As Single
        Dim _height As Single = heigth * 0.2F
        Dim _width As Single = ((width * 0.9F) / data.Length) * 2.0F
        Dim fs As Single = Max(Min(_height, _width), 6.0F)
        Return fs
    End Function
End Class
