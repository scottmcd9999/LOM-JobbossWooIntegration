Imports FluentFTP
Imports System
Imports System.Net

Public Class cFTP
	Private logger As NLog.Logger = NLog.LogManager.GetCurrentClassLogger

	Property FTPServer As String
	Property FTPPort As Integer
	Property FTPUser As String
	Property FTPPass As String
	Property FTPFolder As String
	Property LocalFolder As String

	'Copyright (c) 2015 Robin Rodricks And FluentFTP Contributors

	'Permission Is hereby granted, free of charge, to any person obtaining a copy of this software And associated documentation files 
	'(the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, 
	'publish, distribute, sublicense, And/Or sell copies of the Software, And to permit persons to whom the Software Is 
	'furnished to do so, subject to the following conditions:

	'The above copyright notice And this permission notice shall be included In all copies Or substantial portions Of the Software.

	'THE SOFTWARE Is PROVIDED "AS IS", WITHOUT WARRANTY Of ANY KIND, EXPRESS Or IMPLIED, INCLUDING BUT Not LIMITED To THE WARRANTIES Of 
	'MERCHANTABILITY, FITNESS For A PARTICULAR PURPOSE And NONINFRINGEMENT. In NO Event SHALL THE AUTHORS Or COPYRIGHT HOLDERS BE LIABLE 
	'For ANY CLAIM, DAMAGES Or OTHER LIABILITY, WHETHER In AN ACTION Of CONTRACT, TORT Or OTHERWISE, ARISING FROM, 
	'OUT Of Or In CONNECTION With THE SOFTWARE Or THE USE Or OTHER DEALINGS In THE SOFTWARE.

	Function DownloadFile() As Boolean
		Try
			Using ftp = New FtpClient(FTPServer, FTPPort, FTPUser, FTPPass)
				ftp.Connect()
				ftp.SetWorkingDirectory(FTPFolder)
				ftp.DownloadDirectory(LocalFolder, FTPFolder, FtpFolderSyncMode.Mirror)
				'' download a file and ensure the local directory is created
				'ftp.DownloadFile("D:\Github\FluentFTP\README.md", "/public_html/temp/README.md")

				'' download a file and ensure the local directory is created, verify the file after download
				'ftp.DownloadFile("D:\Github\FluentFTP\README.md", "/public_html/temp/README.md", FtpLocalExists.Overwrite, FtpVerify.Retry)

				Return True

			End Using
		Catch ex As Exception
			logger.Error(ex.ToString, "Jobboss_Woo_Integration.cFTP.DownloadFile Error")
			'MessageBox.Show(String.Format("Error in Jobboss_Woo_Integration.cFTP.DownloadFile: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			Return False
		Finally

		End Try

	End Function

End Class
