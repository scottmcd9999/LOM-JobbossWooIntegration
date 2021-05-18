Imports DevExpress.XtraBars
Imports DevExpress.XtraBars.Docking2010.Views
Imports DevExpress.XtraBars.Ribbon
Imports DevExpress.XtraEditors

Public Class fMain
	Private logger As NLog.Logger = NLog.LogManager.GetCurrentClassLogger
	Private Sub tvMain_DocumentActivated(sender As Object, e As DocumentEventArgs) Handles tvMain.DocumentActivated
		Try
			'/ sync the ribbon with the active document
			Select Case e.Document.ControlName
				Case "ucSetup"
					rcMain.SelectedPage = rpSetup
				Case "ucHelp"
					rcMain.SelectedPage = rpHelp
				Case "ucLog"
					rcMain.SelectedPage = rpLog
				Case "ucFiles"
					rcMain.SelectedPage = rpImport
				Case Else
					rcMain.SelectedPage = rpFile
			End Select
		Catch ex As Exception
			logger.Error(ex.ToString, "Jobboss_Woo_Integration.fMain.tvMain_DocumentActivated Error")
			XtraMessageBox.Show(String.Format("Error in Jobboss_Woo_Integration.fMain.tvMain_DocumentActivated: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
		Finally

		End Try
	End Sub

	Private Sub tvMain_QueryControl(sender As Object, e As QueryControlEventArgs) Handles tvMain.QueryControl
		Try
			If e.Document Is ucFilesDocument Then
				e.Control = New ucFiles
				gucFile = TryCast(e.Control, ucFiles)
			ElseIf e.Document Is ucHelpDocument Then
				e.Control = New ucHelp
				gucHelp = TryCast(e.Control, ucHelp)
			ElseIf e.Document Is ucLogDocument Then
				e.Control = New ucLog
				gucLog = TryCast(e.Control, ucLog)
			ElseIf e.Document Is ucSetupDocument Then
				e.Control = New ucSetup
				gucSetup = TryCast(e.Control, ucSetup)
			Else
				e.Control = New System.Windows.Forms.Control
			End If
		Catch ex As Exception
			logger.Error(ex.ToString, "Jobboss_Woo_Integration.fMain.tvMain_QueryControl Error")
			XtraMessageBox.Show(String.Format("Error in Jobboss_Woo_Integration.fMain.tvMain_QueryControl: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
		Finally

		End Try
	End Sub

	Private Sub rcMain_SelectedPageChanged(sender As Object, e As EventArgs) Handles rcMain.SelectedPageChanged
		'Try
		'	Select Case rcMain.SelectedPage.Name
		'		Case "rpSetup"
		'			tvMain.Controller.Activate(ucSetupDocument)
		'			'tvMain.Controller.Select(ucSetupDocument)
		'		Case "rpImport", "rpFiles", "rpFile"
		'			tvMain.Controller.Activate(ucFilesDocument)
		'			'tvMain.Controller.Select(ucFilesDocument)
		'		Case "rpHelp"
		'			tvMain.Controller.Activate(ucHelpDocument)
		'			'tvMain.Controller.Select(ucHelpDocument)
		'		Case "rpLog"
		'			tvMain.Controller.Activate(ucLogDocument)
		'			'tvMain.Controller.Select(ucLogDocument)
		'	End Select
		'Catch ex As Exception
		'	logger.Error(ex.ToString, "Jobboss_Woo_Integration.fMain.rcMain_SelectedPageChanged Error")
		'	XtraMessageBox.Show(String.Format("Error in Jobboss_Woo_Integration.fMain.rcMain_SelectedPageChanged: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
		'Finally

		'End Try
	End Sub

	Private Sub bbShowFiles_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbShowFiles.ItemClick
		tvMain.Controller.Activate(ucFilesDocument)
		tvMain.Controller.Select(ucFilesDocument)
		gucFile.LoadFiles(False)
	End Sub

	Private Sub bbImportFiles_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbImportFiles.ItemClick
		If gucFile.FileCount = 0 Then
			MessageBox.Show("There are no Web Order files to process", "No Files", MessageBoxButtons.OK, MessageBoxIcon.Information)
			Return
		End If

		If ProcessFiles(False) Then
			If bcAddSalesOrders.Checked Then
				If ImportOrders(False) Then
					MessageBox.Show("Order files were added to the integration system, and Sales Order have been created in Jobboss.", "Import Complete - Success", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
				Else
					MessageBox.Show("Order files have been imported into the integration system, but the system was not able to create one or more Sales Order.", "Import Complete - Errors", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
				End If
			Else
				MessageBox.Show("Order files have been imported into the integration system, but Sales Orders were NOT created in Jobboss.", "Import Complete", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
			End If
		Else
			MessageBox.Show("The integration system was not able to import the Order files, or there were no Order files to process.. Make sure you're connected to the correct Jobboss database and try this again.", "Unable to add Files to Integration System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
		End If
		gucFile.LoadFiles(False)
	End Sub

	Private Sub bbImportSalesOrders_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbImportSalesOrders.ItemClick
		If ImportOrders(False) Then
			MessageBox.Show("Web Orders were imported into Jobboss Sales Orders.", "Import Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
		Else
			MessageBox.Show("One or more Web Orders could not be imported into Jobboss Sales Orders. Review the orders, make any necessary changes, and try this again", "Unable to Complete", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
		End If
	End Sub

	Private Sub bbSalesOrders_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbSalesOrders.ItemClick
		If bcSO_ShowUnProcessed.Checked Then
			gucFile.LoadSalesOrders(False)
		Else
			gucFile.LoadSalesOrders(True)
		End If
	End Sub

	Private Sub bcSO_ShowAll_ItemClick(sender As Object, e As ItemClickEventArgs) Handles bcSO_ShowAll.ItemClick
		gucFile.LoadSalesOrders(True)
	End Sub

	Private Sub bcSO_ShowUnProcessed_ItemClick(sender As Object, e As ItemClickEventArgs) Handles bcSO_ShowUnProcessed.ItemClick
		gucFile.LoadSalesOrders(False)
	End Sub

	Private Sub fMain_Load(sender As Object, e As EventArgs) Handles Me.Load
		'/ set the Status items:
		siConnected.Caption = "Connected to " & JBServer() & "\" & JBDatabase()
		siCopyright.Caption = "Copyright 2021 ECi Jobboss, LLC. All Rights Reserved"

		Dim ver As FileVersionInfo = FileVersionInfo.GetVersionInfo(Application.ExecutablePath)
		Dim jbver As String = JobbossVersion()
		siVersion.Caption = "Woo-Jobboss Integration Version " & ver.FileMajorPart & "." & ver.FileMinorPart & "." & ver.FileBuildPart

		If jbver.Length > 0 Then
			siVersion.Caption = siVersion.Caption & " - Jobboss " & jbver
		End If

		'rcMain.SelectedPage = rpFile

		tvMain.Controller.Activate(ucFilesDocument)
		XtraMessageBox.SmartTextWrap = True

	End Sub

	Private Sub rcMain_SelectedPageChanging(sender As Object, e As RibbonPageChangingEventArgs) Handles rcMain.SelectedPageChanging
		Try
			Select Case e.Page.Name ' rcMain.SelectedPage.Name
				Case "rpSetup"
					tvMain.Controller.Activate(ucSetupDocument)
					'tvMain.Controller.Select(ucSetupDocument)
				Case "rpImport", "rpFiles", "rpFile"
					tvMain.Controller.Activate(ucFilesDocument)
					'tvMain.Controller.Select(ucFilesDocument)
				Case "rpHelp"
					tvMain.Controller.Activate(ucHelpDocument)
					'tvMain.Controller.Select(ucHelpDocument)
				Case "rpLog"
					tvMain.Controller.Activate(ucLogDocument)
					'tvMain.Controller.Select(ucLogDocument)
			End Select
		Catch ex As Exception
			logger.Error(ex.ToString, "Jobboss_Woo_Integration.fMain.rcMain_SelectedPageChanged Error")
			XtraMessageBox.Show(String.Format("Error in Jobboss_Woo_Integration.fMain.rcMain_SelectedPageChanged: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
		Finally

		End Try
	End Sub

	Private Sub bbFTPTest_ItemClick(sender As Object, e As ItemClickEventArgs) Handles bbFTPTest.ItemClick
		If gucSetup.TestFTP() Then
			XtraMessageBox.Show("FTP Connection was successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
		Else
			XtraMessageBox.Show("The system was not able to connect to your FTP site. Verify your settings and try this again.", "Unable to Connect", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End If
	End Sub

	Private Sub bbFTP_GetFiles_ItemClick(sender As Object, e As ItemClickEventArgs) Handles bbFTP_GetFiles.ItemClick
		Dim ftp As New cFTP
		If ftp.DownloadFile Then
			XtraMessageBox.Show("All Order files were downloaded from the website.", "Files Downloaded", MessageBoxButtons.OK, MessageBoxIcon.Information)
		Else
			XtraMessageBox.Show("The system was not able to download files from the website. Verify your FTP settings and try this again.", "Unable to Download", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
		End If
	End Sub
End Class