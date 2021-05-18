Imports System
Imports System.IO
Imports System.Text
Imports Org.BouncyCastle.Crypto
Imports Org.BouncyCastle.Crypto.Engines
Imports Org.BouncyCastle.Crypto.Generators
Imports Org.BouncyCastle.Crypto.Modes
Imports Org.BouncyCastle.Crypto.Parameters
Imports Org.BouncyCastle.Security


Namespace Encryption
	Module AESGCM
		Public Class cBCEncrypt

			Private ReadOnly Random As SecureRandom = New SecureRandom()

			Public ReadOnly NonceBitSize As Integer = 128

			Public ReadOnly MacBitSize As Integer = 128

			Public ReadOnly KeyBitSize As Integer = 256

			Public ReadOnly SaltBitSize As Integer = 128

			Public ReadOnly Iterations As Integer = 10000

			Public ReadOnly MinPasswordLength As Integer = 8


			Function NewKey() As Byte()
				Dim key = New Byte(KeyBitSize / 8 - 1) {}
				Random.NextBytes(key)
				Return key
			End Function

			Function SimpleEncrypt(ByVal secretMessage As String, ByVal key As Byte(), ByVal Optional nonSecretPayload As Byte() = Nothing) As String
				If String.IsNullOrEmpty(secretMessage) Then Throw New ArgumentException("Secret Message Required!", "secretMessage")
				Dim plainText = Encoding.UTF8.GetBytes(secretMessage)
				Dim cipherText = SimpleEncrypt(plainText, key, nonSecretPayload)
				Return Convert.ToBase64String(cipherText)
			End Function

			Function SimpleDecrypt(ByVal encryptedMessage As String, ByVal key As Byte(), ByVal Optional nonSecretPayloadLength As Integer = 0) As String
				If String.IsNullOrEmpty(encryptedMessage) Then Return "" 'Throw New ArgumentException("Encrypted Message Required!", "encryptedMessage")
				Dim cipherText = Convert.FromBase64String(encryptedMessage)
				Dim plainText = SimpleDecrypt(cipherText, key, nonSecretPayloadLength)
				Return If(plainText Is Nothing, Nothing, Encoding.UTF8.GetString(plainText))
			End Function

			Function SimpleEncryptWithPassword(ByVal secretMessage As String, ByVal password As String, ByVal Optional nonSecretPayload As Byte() = Nothing) As String
				If String.IsNullOrEmpty(secretMessage) Then Throw New ArgumentException("Secret Message Required!", "secretMessage")
				Dim plainText = Encoding.UTF8.GetBytes(secretMessage)
				Dim cipherText = SimpleEncryptWithPassword(plainText, password, nonSecretPayload)
				Return Convert.ToBase64String(cipherText)
			End Function

			Function SimpleDecryptWithPassword(ByVal encryptedMessage As String, ByVal password As String, ByVal Optional nonSecretPayloadLength As Integer = 0) As String
				If String.IsNullOrWhiteSpace(encryptedMessage) Then Return ""  ' Throw New ArgumentException("Encrypted Message Required!", "encryptedMessage")
				Dim cipherText = Convert.FromBase64String(encryptedMessage)
				Dim plainText = SimpleDecryptWithPassword(cipherText, password, nonSecretPayloadLength)
				Return If(plainText Is Nothing, Nothing, Encoding.UTF8.GetString(plainText))
			End Function

			Function SimpleEncrypt(ByVal secretMessage As Byte(), ByVal key As Byte(), ByVal Optional nonSecretPayload As Byte() = Nothing) As Byte()
				If key Is Nothing OrElse key.Length <> KeyBitSize / 8 Then Throw New ArgumentException(String.Format("Key needs to be {0} bit!", KeyBitSize), "key")
				If secretMessage Is Nothing OrElse secretMessage.Length = 0 Then Throw New ArgumentException("Secret Message Required!", "secretMessage")
				nonSecretPayload = If(nonSecretPayload, New Byte() {})
				Dim nonce = New Byte(NonceBitSize / 8 - 1) {}
				Random.NextBytes(nonce, 0, nonce.Length)
				Dim cipher = New GcmBlockCipher(New AesFastEngine())
				Dim parameters = New AeadParameters(New KeyParameter(key), MacBitSize, nonce, nonSecretPayload)
				cipher.Init(True, parameters)
				Dim cipherText = New Byte(cipher.GetOutputSize(secretMessage.Length) - 1) {}
				Dim len = cipher.ProcessBytes(secretMessage, 0, secretMessage.Length, cipherText, 0)
				cipher.DoFinal(cipherText, len)
				Using combinedStream = New MemoryStream()
					Using binaryWriter = New BinaryWriter(combinedStream)
						binaryWriter.Write(nonSecretPayload)
						binaryWriter.Write(nonce)
						binaryWriter.Write(cipherText)
					End Using

					Return combinedStream.ToArray()
				End Using
			End Function

			Function SimpleDecrypt(ByVal encryptedMessage As Byte(), ByVal key As Byte(), ByVal Optional nonSecretPayloadLength As Integer = 0) As Byte()
				If key Is Nothing OrElse key.Length <> KeyBitSize / 8 Then Throw New ArgumentException(String.Format("Key needs to be {0} bit!", KeyBitSize), "key")
				If encryptedMessage Is Nothing OrElse encryptedMessage.Length = 0 Then Throw New ArgumentException("Encrypted Message Required!", "encryptedMessage")
				Using cipherStream = New MemoryStream(encryptedMessage)
					Using cipherReader = New BinaryReader(cipherStream)
						Dim nonSecretPayload = cipherReader.ReadBytes(nonSecretPayloadLength)
						Dim nonce = cipherReader.ReadBytes(NonceBitSize / 8)
						Dim cipher = New GcmBlockCipher(New AesFastEngine())
						Dim parameters = New AeadParameters(New KeyParameter(key), MacBitSize, nonce, nonSecretPayload)
						cipher.Init(False, parameters)
						Dim cipherText = cipherReader.ReadBytes(encryptedMessage.Length - nonSecretPayloadLength - nonce.Length)
						Dim plainText = New Byte(cipher.GetOutputSize(cipherText.Length) - 1) {}
						Try
							Dim len = cipher.ProcessBytes(cipherText, 0, cipherText.Length, plainText, 0)
							cipher.DoFinal(plainText, len)
						Catch __unusedInvalidCipherTextException1__ As InvalidCipherTextException
							Return Nothing
						End Try

						Return plainText
					End Using
				End Using
			End Function

			Function SimpleEncryptWithPassword(ByVal secretMessage As Byte(), ByVal password As String, ByVal Optional nonSecretPayload As Byte() = Nothing) As Byte()
				nonSecretPayload = If(nonSecretPayload, New Byte() {})
				If String.IsNullOrWhiteSpace(password) OrElse password.Length < MinPasswordLength Then Throw New ArgumentException(String.Format("Must have a password of at least {0} characters!", MinPasswordLength), "password")
				If secretMessage Is Nothing OrElse secretMessage.Length = 0 Then Throw New ArgumentException("Secret Message Required!", "secretMessage")
				Dim generator = New Pkcs5S2ParametersGenerator()
				Dim salt = New Byte(SaltBitSize / 8 - 1) {}
				Random.NextBytes(salt)
				generator.Init(PbeParametersGenerator.Pkcs5PasswordToBytes(password.ToCharArray()), salt, Iterations)
				Dim key = CType(generator.GenerateDerivedMacParameters(KeyBitSize), KeyParameter)
				Dim payload = New Byte(salt.Length + nonSecretPayload.Length - 1) {}
				Array.Copy(nonSecretPayload, payload, nonSecretPayload.Length)
				Array.Copy(salt, 0, payload, nonSecretPayload.Length, salt.Length)
				Return SimpleEncrypt(secretMessage, key.GetKey(), payload)
			End Function

			Function SimpleDecryptWithPassword(ByVal encryptedMessage As Byte(), ByVal password As String, ByVal Optional nonSecretPayloadLength As Integer = 0) As Byte()
				If String.IsNullOrWhiteSpace(password) OrElse password.Length < MinPasswordLength Then Throw New ArgumentException(String.Format("Must have a password of at least {0} characters!", MinPasswordLength), "password")
				If encryptedMessage Is Nothing OrElse encryptedMessage.Length = 0 Then Throw New ArgumentException("Encrypted Message Required!", "encryptedMessage")
				Dim generator = New Pkcs5S2ParametersGenerator()
				Dim salt = New Byte(SaltBitSize / 8 - 1) {}
				Array.Copy(encryptedMessage, nonSecretPayloadLength, salt, 0, salt.Length)
				generator.Init(PbeParametersGenerator.Pkcs5PasswordToBytes(password.ToCharArray()), salt, Iterations)
				Dim key = CType(generator.GenerateDerivedMacParameters(KeyBitSize), KeyParameter)
				Return SimpleDecrypt(encryptedMessage, key.GetKey(), salt.Length + nonSecretPayloadLength)
			End Function


		End Class
	End Module

End Namespace
