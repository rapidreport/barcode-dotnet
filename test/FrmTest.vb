﻿Imports jp.co.systembase.barcode

Public Class FrmTest

    Private WithEvents pd As New System.Drawing.Printing.PrintDocument

    Private page As Integer = 1

    Private Sub FrmTest_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim ppd As New PrintPreviewDialog
        ppd.Document = pd
        ppd.ShowDialog()
    End Sub

    Private Sub pd_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles pd.PrintPage
        Dim g As Graphics = e.Graphics
        Select Case page
            Case 1
                With Nothing
                    Dim b As New CCode128
                    b.Render(g, 20, 50, 200, 50, "0123456789CODE")
                    b.Render(g, 20, 150, 200, 50, "!@#$%^&*()-")
                    b.Render(g, 20, 250, 200, 50, "[]{};':""")
                    b.WithText = False
                    b.Render(g, 20, 350, 200, 50, "abcd12!")
                End With
                With Nothing
                    Dim b As New CEan8
                    b.Render(g, 300, 50, 100, 50, "4901234")
                    b.Render(g, 300, 150, 100, 50, "5678901")
                    b.Render(g, 300, 250, 100, 50, "5678901")
                    b.WithText = False
                    b.Render(g, 300, 350, 100, 50, "8888888")
                End With
                With Nothing
                    Dim b As New CEan13
                    b.Render(g, 500, 50, 150, 50, "490123456789")
                    b.Render(g, 500, 150, 150, 50, "192205502800")
                    b.Render(g, 500, 250, 150, 50, "978488337649")
                    b.WithText = False
                    b.Render(g, 500, 350, 150, 50, "390123456789")
                End With
                With Nothing
                    Dim b As New CCode39
                    b.Render(g, 20, 500, 200, 50, "0123456789")
                    b.Render(g, 20, 600, 200, 50, "ABCDEFGHIJ")
                    b.GenerateCheckSum = True
                    b.Render(g, 20, 700, 200, 50, "KLMNOPQRSTU")
                    b.WithText = False
                    b.Render(g, 20, 800, 200, 50, "VWXYZ-. $/+%")
                End With
                With Nothing
                    Dim b As New CCodabar
                    b.Render(g, 300, 500, 150, 50, "A123456A")
                    b.Render(g, 300, 600, 150, 50, "B987653B")
                    b.GenerateCheckSum = True
                    b.Render(g, 300, 700, 150, 50, "C82-$:/.+34C")
                    b.WithText = False
                    b.Render(g, 300, 800, 150, 50, "D98-$:/.+21D")
                End With
                e.HasMorePages = True
                With Nothing
                    Dim b As New CITF
                    b.Render(g, 500, 500, 200, 50, "12345678901231")
                    b.Render(g, 500, 600, 200, 50, "14901234567891")
                    b.Render(g, 500, 700, 200, 50, "104901234567893")
                    b.WithText = False
                    b.Render(g, 500, 800, 200, 50, "12345678901231")
                End With
            Case 2
                With Nothing
                    Dim b As New CYubinCustomer
                    b.Render(g, 20, 50, 300, 50, 8.0F, 100, "1234567890-")
                    b.Render(g, 20, 150, 350, 50, 9.0F, 100, "ABCDEFGHIJKLMNOPQRST")
                    b.Render(g, 20, 250, 400, 50, 10.0F, 100, "UVWXYZ")
                    b.Render(g, 20, 350, 450, 50, 11.5F, 100, "024007315-10-3")
                End With
                e.HasMorePages = False
            Case Else
        End Select

        If e.HasMorePages Then
            page += 1
        Else
            page = 1
        End If
    End Sub

End Class
