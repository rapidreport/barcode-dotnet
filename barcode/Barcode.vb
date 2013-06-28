Imports System.Math

Public Class Barcode

    Public Shared BarWidth As Single = 1.0F

    Public MarginX As Single = 2.0F
    Public MarginY As Single = 2.0F

    Public WithText As Boolean = True

    Public Function FontSize(ByVal width As Single, ByVal heigth As Single, ByVal data As String) As Single
        Dim _width As Single = ((width * 0.9F) / data.Length) * 2.0F
        Dim _height As Single = heigth * 0.2F
        Dim fs As Single = Max(Min(_width, _height), 6.0F)
        Return fs
    End Function

    Public Function CenterAlign( _
      ByVal font As Font, _
      ByVal g As Graphics, _
      ByVal width As Single, _
      ByVal data As String, _
      ByVal unit As GraphicsUnit)
        Dim _unit As GraphicsUnit = g.PageUnit
        g.PageUnit = unit
        Dim sf As New StringFormat
        Dim s As SizeF = g.MeasureString(data, font, Integer.MaxValue, sf)
        Dim x As Single = width - s.Width
        g.PageUnit = _unit
        If x <= 0 Then
            Return 0
        End If
        x /= 2
        Const margin As Single = 3.0F
        Return x + margin
    End Function

    Public Function GetFont(ByVal txt As String, ByVal w As Single, ByVal h As Single) As Font
        Dim fs As Single = h * 0.2F
        fs = Math.Min(fs, ((w * 0.9F) / txt.Length) * 2.0F)
        fs = Math.Max(fs, 6.0F)
        Return New Font("Arial", fs)
    End Function

End Class
