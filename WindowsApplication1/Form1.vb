Imports System.IO
Imports System.Net.Http
Imports System.Security.Cryptography
Imports System.Web
Imports System.Text

Public Class Form1
    'Function for creating URL using custom encrypt
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

    'custom encrypt function based on custom decript
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

    'Alternative function to ecrypt
    Public Function AES_Encrypt(ByVal input As String, ByVal pass As String) As String
        Dim AES As New System.Security.Cryptography.RijndaelManaged
        Dim Hash_AES As New System.Security.Cryptography.MD5CryptoServiceProvider
        Dim encrypted As String = ""
        Try
            Dim hash(31) As Byte
            Dim temp As Byte() = Hash_AES.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(pass))
            Array.Copy(temp, 0, hash, 0, 16)
            Array.Copy(temp, 0, hash, 15, 16)
            AES.Key = hash
            AES.Mode = CipherMode.ECB
            Dim DESEncrypter As System.Security.Cryptography.ICryptoTransform = AES.CreateEncryptor
            Dim Buffer As Byte() = System.Text.ASCIIEncoding.ASCII.GetBytes(input)
            encrypted = Convert.ToBase64String(DESEncrypter.TransformFinalBlock(Buffer, 0, Buffer.Length))
            Return encrypted
        Catch ex As Exception
        End Try
    End Function


    Private Sub GoToUrl()
        Dim url As String
        Dim browser As String
        Dim name As String
        browser = ""
        name = ComboBox1.SelectedText

        Select Case name
            Case "Chrome"
                browser = "C:\Program Files (x86)\Mozilla Firefox\firefox.exe"
            Case "Firefox"
                browser = "C:\Program Files (x86)\Mozilla Firefox\firefox.exe"
            Case "Ie"
                browser = "ieexplore.exe"
        End Select


        url = LinkLabel1.Text
        'url = "https://dade.acceliplan.com/app/api/public/login?UserCode=TURVQVQwMDEAAAAAAAAAAMkImQ8qQFCwcYD72DxBHPA%3d&AuthKey=MDcvMTIvMjAxNiAwNDo0M3ajLdeDlIF49nWymwTiEufcub%2fydKSz2V63%2fOi03e%2bq"
        ' Process.Start(browser, url)
        Process.Start(browser, url)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim urlCreated As String = SSOPCG()
        LinkLabel1.Text = urlCreated



    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

    End Sub






    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        GoToUrl()
    End Sub
End Class