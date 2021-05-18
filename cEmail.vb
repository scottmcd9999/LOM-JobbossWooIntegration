Imports System.Net.Mail
Imports System.Data.SqlClient

Public Class cEmail
	' MIMEKIT and MAILKIT Licensing
	'	Permission Is hereby granted, free of charge, to any person obtaining a copy
	'of this software And associated documentation files (the "Software"), to deal
	'in the Software without restriction, including without limitation the rights
	'to use, copy, modify, merge, publish, distribute, sublicense, And/Or sell
	'copies of the Software, And to permit persons to whom the Software Is
	'furnished to do so, subject to the following conditions:

	'The above copyright notice And this permission notice shall be included In
	'all copies Or substantial portions Of the Software.

	'THE SOFTWARE Is PROVIDED "AS IS", WITHOUT WARRANTY Of ANY KIND, EXPRESS Or
	'IMPLIED, INCLUDING BUT Not LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	'FITNESS FOR A PARTICULAR PURPOSE And NONINFRINGEMENT. IN NO EVENT SHALL THE
	'AUTHORS Or COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES Or OTHER
	'LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT Or OTHERWISE, ARISING FROM,
	'OUT OF Or IN CONNECTION WITH THE SOFTWARE Or THE USE Or OTHER DEALINGS IN
	'THE SOFTWARE.

	' MIMEKIT and MAILKIT are available from http://www.mimekit.net/#, or via NuGet in Visual Studio

	Private logger As NLog.Logger = NLog.LogManager.GetCurrentClassLogger

	Public Sub New()
		SendTo = New List(Of String)
	End Sub

	'Property client As MailKit.Net.Smtp.SmtpClient
	'Property EmailMessage As MimeKit.MimeMessage
	'Property SendToAddr As MimeKit.InternetAddress
	'Property SendFromAddr As MimeKit.InternetAddress
	'Property SendCCAddr As MimeKit.InternetAddress
	Property Errors As String
	'Property SendTo As String
	Property SendFrom As String
	Property CC As String
	Property Subject As String
	Property Body As String
	Property RequiresLogin As Boolean
	Property Login As String
	Property Password As String
	Property IsHTML As Boolean
	Property Port As String
	Property SMTPServerName As String
	Property EnableSSL As Boolean
	Property IsAuto As Boolean
	Property SendTo As List(Of String)
	Property cmd As SqlCommand
	Function AddSendToAddress(EmailAddress As String) As Boolean
		SendTo.add(EmailAddress)

	End Function
	Function SendEmail() As Boolean
		Try
			'/
			Dim DocPath As String = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) & "\ConcurIntegration"
			Dim DocFile As String = System.IO.Path.Combine(DocPath, "smpt_err.log")

			Using lgr As New MailKit.ProtocolLogger(DocFile)
				Using client As New MailKit.Net.Smtp.SmtpClient(lgr)

					If SMTPServerName.Length = 0 Or Port.Length = 0 Then
						Errors = "No SMTP Servername or Port"
						Return False
					End If

					client.Connect(SMTPServerName, Convert.ToInt32(Port), MailKit.Security.SecureSocketOptions.Auto)

					If RequiresLogin Then
						'/ must have the login creds:
						If Login.Length = 0 And Password.Length = 0 Then
							Errors = "Authentication required, but no Login or Password"
							Return False
						End If
						client.AuthenticationMechanisms.Remove("XOAUTH2")
						client.Authenticate(Login, Password)
					End If

					If SendFrom.Length = 0 Then
						Errors = "No Send From account"
						Return False
					End If

					Dim msg As New MimeKit.MimeMessage
					Dim addr As New MimeKit.MailboxAddress(SendFrom)
					msg.From.Add(addr)

					If SendTo.Count = 0 Then
						Errors = "No Send To addresses provided"
						Return False
					Else
						For i As Integer = 0 To SendTo.Count - 1
							If Not String.IsNullOrEmpty(SendTo(i)) Then
								msg.To.Add(New MimeKit.MailboxAddress(SendTo(i)))
							End If
						Next
					End If

					'If SendTo.IndexOf(";") <> -1 Then
					'	Dim adds As String() = SendTo.Split(";")
					'	For i As Integer = 0 To adds.GetUpperBound(0)
					'		msg.To.Add(New MimeKit.MailboxAddress(adds(i)))
					'		'logger.Info("SENDTO: " & adds(i))
					'	Next i
					'Else
					'	msg.To.Add(New MimeKit.MailboxAddress(SendTo))
					'End If

					If CC.Length > 0 Then
						If CC.IndexOf(";") <> -1 Then
							Dim adds As String() = CC.Split(";")
							For i As Integer = 0 To adds.GetUpperBound(0)
								If Not String.IsNullOrEmpty(adds(i)) Then
									msg.Cc.Add(New MimeKit.MailboxAddress(adds(i)))
								End If
							Next i
						Else
							If Not String.IsNullOrEmpty(CC) Then
								msg.Cc.Add(New MimeKit.MailboxAddress(CC))
							End If
						End If
					End If

					msg.Subject = Subject

					Dim msgbb As New MimeKit.BodyBuilder
					msgbb.HtmlBody = Body
					msg.Body = msgbb.ToMessageBody

					Try
						client.Send(msg, Nothing)
					Catch ex As MailKit.Net.Smtp.SmtpCommandException
						If Not IsAuto Then
							MsgBox(ex.ToString)
						Else
							logger.Info(ex.ToString)
						End If

					End Try

					client.Disconnect(True)
				End Using
			End Using
			Return True
		Catch ex As Exception
			logger.Error(ex.ToString, "cEmail.SendEmail Error")
			If Not IsAuto Then
				MessageBox.Show(String.Format("Error in SendEmail: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			End If
			Return False
		Finally

		End Try
	End Function

	Function LoadSettings(cmd As SqlCommand) As Boolean
		Try
			'Dim ce As New cEncrypt("rfq119$")
			'Dim enc As New Encryption.cBCEncrypt
			'Dim cs As New cSettings_DB With {.IsAuto = IsAuto, .cmd = cmd}
			'If cs.ReadSetting("SMTP").ToString.Length > 0 Then
			'	SMTPServerName = enc.SimpleDecrypt(cs.ReadSetting("SMTP"), enc.NewKey)
			'Else
			'	Return False
			'End If
			'If cs.ReadSetting("SMTPPort").ToString.Length > 0 Then
			'	Port = ce.DecryptData(cs.ReadSetting("SMTPPort"))
			'Else
			'	Return False
			'End If
			'If cs.ReadSetting("SMTPSendFrom").ToString.Length > 0 Then
			'	SendFrom = ce.DecryptData(cs.ReadSetting("SMTPSendFrom"))
			'Else
			'	Return False
			'End If
			'If cs.ReadSetting("SMTPCC").ToString.Length > 0 Then
			'	CC = ce.DecryptData(cs.ReadSetting("SMTPCC"))
			'End If

			'If cs.ReadSetting("SMTPRequiresLogin") = "True" Then
			'	RequiresLogin = True
			'	If cs.ReadSetting("SMTPLogin").ToString.Length > 0 Then
			'		Login = ce.DecryptData(cs.ReadSetting("SMTPLogin"))
			'	Else
			'		Return False
			'	End If

			'	If cs.ReadSetting("SMTPPassword").ToString.Length > 0 Then
			'		Password = ce.DecryptData(cs.ReadSetting("SMTPPassword"))
			'	Else
			'		Return False
			'	End If
			'End If

			'EnableSSL = cs.ReadSetting("EnableSSL") = "True"

			'logger.Info("LOADSETTINGS: Server " & SMTPServerName & "  Port " & Port & "  SendFrom " & SendFrom & "  CC " & CC & "  Login " & Login & "  Password  " & Password & "   EnableSSL " & EnableSSL.ToString)

			'logger.Info(SMTPServerName & " " & Port & " " & SendFrom & " " & CC & " " & Login & " " & Password)

			Return True
		Catch ex As Exception
			logger.Error(ex.ToString, "MAM.cEmail.LoadSettings Error")
			If Not IsAuto Then
				MessageBox.Show(String.Format("Error in LoadSettings: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			End If
			Return False
		Finally
		End Try
	End Function

	Function LoadSettings() As Boolean
		Try

			SendTo = New List(Of String)
			'Dim ce As New cEncrypt("rfq119$")
			Dim enc As New Encryption.cBCEncrypt

			Dim cs As New cSettings 'With {.cmd = cmd}
			If cs.ReadSetting("SMTP").ToString.Length > 0 Then
				SMTPServerName = enc.SimpleDecryptWithPassword(cs.ReadSetting("SMTP"), "&rfq119!")
			Else
				Return False
			End If
			If cs.ReadSetting("SMTPPort").ToString.Length > 0 Then
				Port = enc.SimpleDecryptWithPassword(cs.ReadSetting("SMTPPort"), "&rfq119!")
			Else
				Return False
			End If
			If cs.ReadSetting("SMTPSendFrom").ToString.Length > 0 Then
				SendFrom = enc.SimpleDecryptWithPassword(cs.ReadSetting("SMTPSendFrom"), "&rfq119!")
			Else
				Return False
			End If
			If cs.ReadSetting("SMTPCC").ToString.Length > 0 Then
				CC = enc.SimpleDecryptWithPassword(cs.ReadSetting("SMTPCC"), "&rfq119!")
			End If

			If cs.ReadSetting("SMTPRequiresLogin") = "True" Then
				RequiresLogin = True
				If cs.ReadSetting("SMTPLogin").ToString.Length > 0 Then
					Login = enc.SimpleDecryptWithPassword(cs.ReadSetting("SMTPLogin"), "&rfq119!")
				Else
					Return False
				End If

				If cs.ReadSetting("SMTPPassword").ToString.Length > 0 Then
					Password = enc.SimpleDecryptWithPassword(cs.ReadSetting("SMTPPassword"), "&rfq119!")
				Else
					Return False
				End If
			End If

			EnableSSL = cs.ReadSetting("EnableSSL") = "True"

			'logger.Info("Server " & SMTPServerName & "  Port " & Port & "  SendFrom " & SendFrom & "  CC " & CC & "  Login " & Login & "  Password  " & Password & "   EnableSSL " & EnableSSL.ToString)

			Return True
		Catch ex As Exception
			logger.Error(ex.ToString, "MAM.cEmail.LoadSettings Error")
			If Not IsAuto Then
				MessageBox.Show(String.Format("Error in LoadSettings: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			End If
			Return False
		Finally

		End Try

	End Function
End Class
