Imports System.IO
Imports System.Security.Cryptography
Imports System.Web
Imports System.Text

Public Class Form1
    'Function for creating URL using custom encrypt
    Private Function SSOACCELIFY() As String
        Try
            Dim url As String = ""
            Dim key As String = ""
            url = "https://dade.acceliplan.com/app/api/public/login" & "?UserCode="
            key = "TmUEYNGNE84GCT2m245MZ9qU2SY2o10R"


            Dim USERID As String = TextBox1.Text
            Dim DATETIME As String = System.DateTime.Now().ToString("MM/dd/yyyy hh:mm:ss")
            Dim data As String

            Dim uc As String = Convert.ToBase64String(New System.Text.UTF8Encoding().GetBytes(USERID.Trim))
            Dim ak As String = Convert.ToBase64String(New System.Text.UTF8Encoding().GetBytes(DATETIME.Trim))

            ' Dim uc As String = Utilities.base64Encode(USERID.Trim)
            ' Dim ak As String = Utilities.base64Encode(DATETIME.Trim)
            Dim myAES As New System.Security.Cryptography.RijndaelManaged
            'Identical to PCG
            data = url & HttpUtility.UrlEncode(EncryptCustom(uc, key, 256, 128, CipherMode.CBC, PaddingMode.PKCS7)) _
                    & "&AuthKey=" & HttpUtility.UrlEncode(EncryptCustom(ak, key, 256, 128, CipherMode.CBC, System.Security.Cryptography.PaddingMode.PKCS7))
            Return data
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function


    Public Function EncryptCustom(ByVal stringToEncrypt As String, ByVal sEncryptionKey As String, keySize As Integer, blockSize As Integer, cipherMode As CipherMode, paddingMode As PaddingMode) As String
        Using aes = New AesCryptoServiceProvider() With {
                    .KeySize = keySize,
                    .BlockSize = blockSize,
                    .Mode = cipherMode,
                    .Padding = paddingMode,
                    .Key = System.Text.Encoding.UTF8.GetBytes(sEncryptionKey)
                }
            Dim input = Encoding.UTF8.GetBytes(stringToEncrypt)
            aes.GenerateIV()
            Using encrypter = aes.CreateEncryptor(aes.Key, aes.IV)
                Using cipherStream = New MemoryStream()
                    Using tCryptoStream = New CryptoStream(cipherStream, encrypter, CryptoStreamMode.Write)
                        Using tBinaryWriter = New BinaryWriter(tCryptoStream)
                            'Pre-pending IV to data
                            tBinaryWriter.Write(aes.IV)
                            tBinaryWriter.Write(input)
                            tCryptoStream.FlushFinalBlock()
                        End Using
                    End Using
                    Return Convert.ToBase64String(cipherStream.ToArray())
                End Using
            End Using
        End Using
    End Function


    Public Shared Function base64Encode(data As String) As String
        Try
            Dim encData_byte As Byte() = New Byte(data.Length - 1) {}
            encData_byte = System.Text.Encoding.UTF8.GetBytes(data)
            Dim encodedData As String = Convert.ToBase64String(encData_byte)
            Return encodedData
        Catch e As Exception
            Throw New Exception("Error in base64Encode" + e.Message)
        End Try
    End Function


    ' Dim myAES As New AESEncryption

    Private Sub GoToUrl()
        Dim url As String
        Dim browser As String
        Dim name As String
        browser = ""


        url = LinkLabel1.Text
        'url = "https://dade.acceliplan.com/app/api/public/login?UserCode=TURVQVQwMDEAAAAAAAAAAMkImQ8qQFCwcYD72DxBHPA%3d&AuthKey=MDcvMTIvMjAxNiAwNDo0M3ajLdeDlIF49nWymwTiEufcub%2fydKSz2V63%2fOi03e%2bq"
        'Process.Start(browser, url)
        Process.Start(url)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim urlCreated As String = SSOACCELIFY()
        URLGen.Text = urlCreated

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub






    Private Sub Button2_Click(sender As Object, e As EventArgs)
        GoToUrl()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles URLGen.TextChanged
        My.Computer.Clipboard.SetText(URLGen.Text)
    End Sub
End Class