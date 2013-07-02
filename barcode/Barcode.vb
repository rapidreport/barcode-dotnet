Imports System.Math

Public Class Barcode

    Public Shared BarWidth As Single = 1.0F

    Public MarginX As Single = 2.0F
    Public MarginY As Single = 2.0F

    Public WithText As Boolean = True

    Public Function FontSize(ByVal txt As String, ByVal w As Single, ByVal h As Single) As Single
        Dim ret As Single = h * 0.2F
        ret = Math.Min(ret, ((w * 0.9F) / txt.Length) * 2.0F)
        ret = Math.Max(ret, 6.0F)
        Return ret
    End Function

    Public Function GetFont(ByVal txt As String, ByVal w As Single, ByVal h As Single) As Font
        Return New Font("Arial", FontSize(txt, w, h))
    End Function

End Class
