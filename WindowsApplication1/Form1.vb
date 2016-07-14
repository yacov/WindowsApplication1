Public Class Form1
    Private Function SSOPCG() As String
        Try
            Dim url As String = "https://dade.acceliplan.com/app/api/public/login?UserCode"


            Dim USERID As String = TextBox1.Text
            Dim DATETIME As String = System.DateTime.Now().ToString("MM/dd/yyyy hh:mm:ss")
            Dim data As String
            Dim key As String
            key = "TmUEYNGNE84GCT2m245MZ9qU2SY2o10R"

            '  Dim uc As String = Utilities.base64Encode(USERID.Trim)
            '  Dim ak As String = Utilities.base64Encode(DATETIME.Trim)

            Dim uc As String = Convert.ToBase64String(New System.Text.ASCIIEncoding().GetBytes(USERID.Trim))
            Dim ak As String = Convert.ToBase64String(New System.Text.ASCIIEncoding().GetBytes(DATETIME.Trim))

            'PCG is using Perl on their side so both userCode and authKey values had to be Base64-encoded to avoid errors related to character sets.
            'Also, PCG is only URL-decoding both userCode and authKey values and not the whole URL
            data = url & HttpContext.Current.Server.UrlEncode(myAES.EncryptCustom(uc, key, 256, 128, System.Security.Cryptography.CipherMode.CBC, System.Security.Cryptography.PaddingMode.PKCS7)) _
                    & "&AuthKey=" & HttpContext.Current.Server.UrlEncode(myAES.EncryptCustom(ak, key, 256, 128, System.Security.Cryptography.CipherMode.CBC, System.Security.Cryptography.PaddingMode.PKCS7))
            RedirectFullURL = data
            Return RedirectFullURL
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function


    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub GoToUrl(url As String, browser As String)
        browser =
        url = LinkLabel1.Text
        Process.Start(browser, url)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim urlCreated As String = SSOPCG()
        LinkLabel1.Text = urlCreated



    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

    End Sub
End Class
