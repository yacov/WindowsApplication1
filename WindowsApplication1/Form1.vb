Imports System.IO
Imports System.Net.Http
Imports System.Security.Cryptography
Imports System.Web
Imports System.Text

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
            data = url & HttpContext.Current.Server.UrlEncode(EncryptCustom(uc, key, 256, 128, CipherMode.CBC, System.Security.Cryptography.PaddingMode.PKCS7)) _
                    & "&AuthKey=" & HttpContext.Current.Server.UrlEncode(EncryptCustom(ak, key, 256, 128, System.Security.Cryptography.CipherMode.CBC, System.Security.Cryptography.PaddingMode.PKCS7))


            Console.WriteLine(data)
            Return data
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


    Public Function EncryptCustom(ByVal stringToEncrypt As String, ByVal sEncryptionKey As String, keySize As Integer, blockSize As Integer, cipherMode As CipherMode, paddingMode As PaddingMode) As String
        Dim inputByteArray(stringToEncrypt.Length) As Byte
        Using provider = New AesCryptoServiceProvider()
            With provider
                .KeySize = keySize
                .BlockSize = blockSize
                .Mode = cipherMode
                .Padding = paddingMode
                .Key = System.Text.Encoding.UTF8.GetBytes(sEncryptionKey)
            End With
            inputByteArray = Convert.FromBase64String(stringToEncrypt)
            Using ms = New MemoryStream(inputByteArray)
                ' Read the first 16 bytes which is the IV.
                Dim iv As Byte() = New Byte(15) {}
                ms.Read(iv, 0, 16)
                provider.IV = iv
                Using encryptor = provider.CreateEncryptor()

                    Using cs = New CryptoStream(ms, encryptor, CryptoStreamMode.Write)
                        Using sr = New StreamReader(cs)
                            Return sr.ReadToEnd()
                        End Using
                    End Using
                End Using
            End Using
        End Using
    End Function

End Class