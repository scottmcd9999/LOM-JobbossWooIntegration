<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fMain
	Inherits DevExpress.XtraBars.Ribbon.RibbonForm

	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> _
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		If disposing AndAlso components IsNot Nothing Then
			components.Dispose()
		End If
		MyBase.Dispose(disposing)
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer

	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.  
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim DockingContainer2 As DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer = New DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer()
		Me.DocumentGroup1 = New DevExpress.XtraBars.Docking2010.Views.Tabbed.DocumentGroup(Me.components)
		Me.ucSplashDocument = New DevExpress.XtraBars.Docking2010.Views.Tabbed.Document(Me.components)
		Me.ucFilesDocument = New DevExpress.XtraBars.Docking2010.Views.Tabbed.Document(Me.components)
		Me.ucHelpDocument = New DevExpress.XtraBars.Docking2010.Views.Tabbed.Document(Me.components)
		Me.ucSetupDocument = New DevExpress.XtraBars.Docking2010.Views.Tabbed.Document(Me.components)
		Me.ucLogDocument = New DevExpress.XtraBars.Docking2010.Views.Tabbed.Document(Me.components)
		Me.rcMain = New DevExpress.XtraBars.Ribbon.RibbonControl()
		Me.bbExit = New DevExpress.XtraBars.BarButtonItem()
		Me.bbShowFiles = New DevExpress.XtraBars.BarButtonItem()
		Me.bbImportFiles = New DevExpress.XtraBars.BarButtonItem()
		Me.bbImportSalesOrders = New DevExpress.XtraBars.BarButtonItem()
		Me.bbFTP_GetFiles = New DevExpress.XtraBars.BarButtonItem()
		Me.bcAddSalesOrders = New DevExpress.XtraBars.BarCheckItem()
		Me.bbSalesOrders = New DevExpress.XtraBars.BarButtonItem()
		Me.bcSO_ShowAll = New DevExpress.XtraBars.BarCheckItem()
		Me.bcSO_ShowUnProcessed = New DevExpress.XtraBars.BarCheckItem()
		Me.siCopyright = New DevExpress.XtraBars.BarStaticItem()
		Me.siVersion = New DevExpress.XtraBars.BarStaticItem()
		Me.siConnected = New DevExpress.XtraBars.BarStaticItem()
		Me.bbHelp = New DevExpress.XtraBars.BarButtonItem()
		Me.bbSetup = New DevExpress.XtraBars.BarButtonItem()
		Me.bbLogs = New DevExpress.XtraBars.BarButtonItem()
		Me.bbFTPTest = New DevExpress.XtraBars.BarButtonItem()
		Me.bbFTPSave = New DevExpress.XtraBars.BarButtonItem()
		Me.bbEmailTest = New DevExpress.XtraBars.BarButtonItem()
		Me.bbEmailSave = New DevExpress.XtraBars.BarButtonItem()
		Me.rpFile = New DevExpress.XtraBars.Ribbon.RibbonPage()
		Me.rpgProgram = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
		Me.rpImport = New DevExpress.XtraBars.Ribbon.RibbonPage()
		Me.rpgFTP = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
		Me.rpgImport = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
		Me.rpgSalesOrders = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
		Me.rpLog = New DevExpress.XtraBars.Ribbon.RibbonPage()
		Me.rpgLogs = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
		Me.rpSetup = New DevExpress.XtraBars.Ribbon.RibbonPage()
		Me.rpgSetup = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
		Me.rpHelp = New DevExpress.XtraBars.Ribbon.RibbonPage()
		Me.rpgHelp = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
		Me.RibbonStatusBar = New DevExpress.XtraBars.Ribbon.RibbonStatusBar()
		Me.dmMainDocManager = New DevExpress.XtraBars.Docking2010.DocumentManager(Me.components)
		Me.tvMain = New DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(Me.components)
		CType(Me.DocumentGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ucSplashDocument, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ucFilesDocument, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ucHelpDocument, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ucSetupDocument, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ucLogDocument, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.rcMain, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.dmMainDocManager, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.tvMain, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'DocumentGroup1
		'
		Me.DocumentGroup1.Items.AddRange(New DevExpress.XtraBars.Docking2010.Views.Tabbed.Document() {Me.ucSplashDocument, Me.ucFilesDocument, Me.ucHelpDocument, Me.ucSetupDocument, Me.ucLogDocument})
		'
		'ucSplashDocument
		'
		Me.ucSplashDocument.Caption = "Splash"
		Me.ucSplashDocument.ControlName = "ucSplash"
		Me.ucSplashDocument.ControlTypeName = "ucSplace"
		Me.ucSplashDocument.FloatLocation = New System.Drawing.Point(261, 310)
		Me.ucSplashDocument.FloatSize = New System.Drawing.Size(1218, 513)
		'
		'ucFilesDocument
		'
		Me.ucFilesDocument.Caption = "Files"
		Me.ucFilesDocument.ControlName = "ucFiles"
		Me.ucFilesDocument.ControlTypeName = "ucFiles"
		Me.ucFilesDocument.FloatLocation = New System.Drawing.Point(333, 310)
		Me.ucFilesDocument.FloatSize = New System.Drawing.Size(1218, 513)
		'
		'ucHelpDocument
		'
		Me.ucHelpDocument.Caption = "Help"
		Me.ucHelpDocument.ControlName = "ucHelp"
		Me.ucHelpDocument.ControlTypeName = "ucHelp"
		'
		'ucSetupDocument
		'
		Me.ucSetupDocument.Caption = "Setting"
		Me.ucSetupDocument.ControlName = "ucSetup"
		Me.ucSetupDocument.ControlTypeName = "ucSetup"
		'
		'ucLogDocument
		'
		Me.ucLogDocument.Caption = "Log"
		Me.ucLogDocument.ControlName = "ucLog"
		Me.ucLogDocument.ControlTypeName = "ucLog"
		'
		'rcMain
		'
		Me.rcMain.ExpandCollapseItem.Id = 0
		Me.rcMain.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.rcMain.ExpandCollapseItem, Me.rcMain.SearchEditItem, Me.bbExit, Me.bbShowFiles, Me.bbImportFiles, Me.bbImportSalesOrders, Me.bbFTP_GetFiles, Me.bcAddSalesOrders, Me.bbSalesOrders, Me.bcSO_ShowAll, Me.bcSO_ShowUnProcessed, Me.siCopyright, Me.siVersion, Me.siConnected, Me.bbHelp, Me.bbSetup, Me.bbLogs, Me.bbFTPTest, Me.bbFTPSave, Me.bbEmailTest, Me.bbEmailSave})
		Me.rcMain.Location = New System.Drawing.Point(0, 0)
		Me.rcMain.MaxItemId = 21
		Me.rcMain.Name = "rcMain"
		Me.rcMain.Pages.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPage() {Me.rpFile, Me.rpImport, Me.rpLog, Me.rpSetup, Me.rpHelp})
		Me.rcMain.Size = New System.Drawing.Size(1224, 151)
		Me.rcMain.StatusBar = Me.RibbonStatusBar
		'
		'bbExit
		'
		Me.bbExit.Caption = "Exit"
		Me.bbExit.Id = 1
		Me.bbExit.ImageOptions.Image = Global.Jobboss_Woo_Integration.My.Resources.Resources._exit
		Me.bbExit.ImageOptions.LargeImage = Global.Jobboss_Woo_Integration.My.Resources.Resources._exit
		Me.bbExit.Name = "bbExit"
		'
		'bbShowFiles
		'
		Me.bbShowFiles.Caption = "View Web Order Files"
		Me.bbShowFiles.Id = 2
		Me.bbShowFiles.ImageOptions.Image = Global.Jobboss_Woo_Integration.My.Resources.Resources.order_find
		Me.bbShowFiles.ImageOptions.LargeImage = Global.Jobboss_Woo_Integration.My.Resources.Resources.order_find
		Me.bbShowFiles.Name = "bbShowFiles"
		'
		'bbImportFiles
		'
		Me.bbImportFiles.Caption = "Import Web Order Files"
		Me.bbImportFiles.Id = 3
		Me.bbImportFiles.ImageOptions.Image = Global.Jobboss_Woo_Integration.My.Resources.Resources.order_add
		Me.bbImportFiles.ImageOptions.LargeImage = Global.Jobboss_Woo_Integration.My.Resources.Resources.order_add
		Me.bbImportFiles.Name = "bbImportFiles"
		'
		'bbImportSalesOrders
		'
		Me.bbImportSalesOrders.Caption = "Import Sales Orders"
		Me.bbImportSalesOrders.Id = 4
		Me.bbImportSalesOrders.ImageOptions.Image = Global.Jobboss_Woo_Integration.My.Resources.Resources.money_button_arrow_up
		Me.bbImportSalesOrders.ImageOptions.LargeImage = Global.Jobboss_Woo_Integration.My.Resources.Resources.money_button_arrow_up
		Me.bbImportSalesOrders.Name = "bbImportSalesOrders"
		'
		'bbFTP_GetFiles
		'
		Me.bbFTP_GetFiles.Caption = "Get Files From Website"
		Me.bbFTP_GetFiles.Id = 5
		Me.bbFTP_GetFiles.ImageOptions.Image = Global.Jobboss_Woo_Integration.My.Resources.Resources.cloud_download_ok_2
		Me.bbFTP_GetFiles.ImageOptions.LargeImage = Global.Jobboss_Woo_Integration.My.Resources.Resources.cloud_download_ok_2
		Me.bbFTP_GetFiles.Name = "bbFTP_GetFiles"
		'
		'bcAddSalesOrders
		'
		Me.bcAddSalesOrders.BindableChecked = True
		Me.bcAddSalesOrders.Caption = "Add Sales Orders During Import"
		Me.bcAddSalesOrders.Checked = True
		Me.bcAddSalesOrders.Id = 6
		Me.bcAddSalesOrders.ImageOptions.Image = Global.Jobboss_Woo_Integration.My.Resources.Resources.order_ok_2
		Me.bcAddSalesOrders.ImageOptions.LargeImage = Global.Jobboss_Woo_Integration.My.Resources.Resources.order_ok_2
		Me.bcAddSalesOrders.Name = "bcAddSalesOrders"
		'
		'bbSalesOrders
		'
		Me.bbSalesOrders.Caption = "View Sales Orders"
		Me.bbSalesOrders.Id = 7
		Me.bbSalesOrders.ImageOptions.Image = Global.Jobboss_Woo_Integration.My.Resources.Resources.money_find
		Me.bbSalesOrders.ImageOptions.LargeImage = Global.Jobboss_Woo_Integration.My.Resources.Resources.money_find
		Me.bbSalesOrders.Name = "bbSalesOrders"
		'
		'bcSO_ShowAll
		'
		Me.bcSO_ShowAll.Caption = "Show All Orders"
		Me.bcSO_ShowAll.CheckStyle = DevExpress.XtraBars.BarCheckStyles.Radio
		Me.bcSO_ShowAll.GroupIndex = 1
		Me.bcSO_ShowAll.Id = 8
		Me.bcSO_ShowAll.ImageOptions.DisabledImage = Global.Jobboss_Woo_Integration.My.Resources.Resources.money_filter
		Me.bcSO_ShowAll.ImageOptions.DisabledLargeImage = Global.Jobboss_Woo_Integration.My.Resources.Resources.money_filter
		Me.bcSO_ShowAll.ImageOptions.Image = Global.Jobboss_Woo_Integration.My.Resources.Resources.money
		Me.bcSO_ShowAll.ImageOptions.LargeImage = Global.Jobboss_Woo_Integration.My.Resources.Resources.money
		Me.bcSO_ShowAll.ItemAppearance.Pressed.Options.UseImage = True
		Me.bcSO_ShowAll.ItemInMenuAppearance.Pressed.Options.UseImage = True
		Me.bcSO_ShowAll.Name = "bcSO_ShowAll"
		'
		'bcSO_ShowUnProcessed
		'
		Me.bcSO_ShowUnProcessed.BindableChecked = True
		Me.bcSO_ShowUnProcessed.Caption = "Show Orders Not Imported to Jobboss"
		Me.bcSO_ShowUnProcessed.Checked = True
		Me.bcSO_ShowUnProcessed.CheckStyle = DevExpress.XtraBars.BarCheckStyles.Radio
		Me.bcSO_ShowUnProcessed.GroupIndex = 1
		Me.bcSO_ShowUnProcessed.Id = 10
		Me.bcSO_ShowUnProcessed.ImageOptions.Image = Global.Jobboss_Woo_Integration.My.Resources.Resources.money_filter
		Me.bcSO_ShowUnProcessed.ImageOptions.LargeImage = Global.Jobboss_Woo_Integration.My.Resources.Resources.money_filter
		Me.bcSO_ShowUnProcessed.Name = "bcSO_ShowUnProcessed"
		'
		'siCopyright
		'
		Me.siCopyright.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring
		Me.siCopyright.Caption = "Copyright"
		Me.siCopyright.Id = 11
		Me.siCopyright.Name = "siCopyright"
		'
		'siVersion
		'
		Me.siVersion.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring
		Me.siVersion.Caption = "Version"
		Me.siVersion.Id = 12
		Me.siVersion.Name = "siVersion"
		Me.siVersion.TextAlignment = System.Drawing.StringAlignment.Center
		'
		'siConnected
		'
		Me.siConnected.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring
		Me.siConnected.Caption = "Connected"
		Me.siConnected.Id = 13
		Me.siConnected.Name = "siConnected"
		Me.siConnected.TextAlignment = System.Drawing.StringAlignment.Far
		'
		'bbHelp
		'
		Me.bbHelp.Caption = "Help"
		Me.bbHelp.Id = 14
		Me.bbHelp.ImageOptions.Image = Global.Jobboss_Woo_Integration.My.Resources.Resources.button_help
		Me.bbHelp.ImageOptions.LargeImage = Global.Jobboss_Woo_Integration.My.Resources.Resources.button_help
		Me.bbHelp.Name = "bbHelp"
		'
		'bbSetup
		'
		Me.bbSetup.Caption = "Setup"
		Me.bbSetup.Id = 15
		Me.bbSetup.ImageOptions.Image = Global.Jobboss_Woo_Integration.My.Resources.Resources.settings_options1
		Me.bbSetup.ImageOptions.LargeImage = Global.Jobboss_Woo_Integration.My.Resources.Resources.settings_options1
		Me.bbSetup.Name = "bbSetup"
		'
		'bbLogs
		'
		Me.bbLogs.Caption = "Logs"
		Me.bbLogs.Id = 16
		Me.bbLogs.ImageOptions.Image = Global.Jobboss_Woo_Integration.My.Resources.Resources.script
		Me.bbLogs.ImageOptions.LargeImage = Global.Jobboss_Woo_Integration.My.Resources.Resources.script
		Me.bbLogs.Name = "bbLogs"
		'
		'bbFTPTest
		'
		Me.bbFTPTest.Caption = "Test FTP Connection"
		Me.bbFTPTest.Id = 17
		Me.bbFTPTest.ImageOptions.Image = Global.Jobboss_Woo_Integration.My.Resources.Resources.folder_network_refresh
		Me.bbFTPTest.ImageOptions.LargeImage = Global.Jobboss_Woo_Integration.My.Resources.Resources.folder_network_refresh
		Me.bbFTPTest.Name = "bbFTPTest"
		'
		'bbFTPSave
		'
		Me.bbFTPSave.Caption = "Save FTP Settings"
		Me.bbFTPSave.Id = 18
		Me.bbFTPSave.ImageOptions.Image = Global.Jobboss_Woo_Integration.My.Resources.Resources.folder_network_save
		Me.bbFTPSave.ImageOptions.LargeImage = Global.Jobboss_Woo_Integration.My.Resources.Resources.folder_network_save
		Me.bbFTPSave.Name = "bbFTPSave"
		'
		'bbEmailTest
		'
		Me.bbEmailTest.Caption = "Test Email Settings"
		Me.bbEmailTest.Id = 19
		Me.bbEmailTest.ImageOptions.Image = Global.Jobboss_Woo_Integration.My.Resources.Resources.send_receive_mail_front_refresh
		Me.bbEmailTest.ImageOptions.LargeImage = Global.Jobboss_Woo_Integration.My.Resources.Resources.send_receive_mail_front_refresh
		Me.bbEmailTest.Name = "bbEmailTest"
		'
		'bbEmailSave
		'
		Me.bbEmailSave.Caption = "Save Email Settings"
		Me.bbEmailSave.Id = 20
		Me.bbEmailSave.ImageOptions.Image = Global.Jobboss_Woo_Integration.My.Resources.Resources.send_receive_mail_front_save
		Me.bbEmailSave.ImageOptions.LargeImage = Global.Jobboss_Woo_Integration.My.Resources.Resources.send_receive_mail_front_save
		Me.bbEmailSave.Name = "bbEmailSave"
		'
		'rpFile
		'
		Me.rpFile.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {Me.rpgProgram})
		Me.rpFile.Name = "rpFile"
		Me.rpFile.Text = "File"
		'
		'rpgProgram
		'
		Me.rpgProgram.ItemLinks.Add(Me.bbExit)
		Me.rpgProgram.Name = "rpgProgram"
		Me.rpgProgram.Text = "Program"
		'
		'rpImport
		'
		Me.rpImport.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {Me.rpgFTP, Me.rpgImport, Me.rpgSalesOrders})
		Me.rpImport.Name = "rpImport"
		Me.rpImport.Text = "Import"
		'
		'rpgFTP
		'
		Me.rpgFTP.ItemLinks.Add(Me.bbFTP_GetFiles)
		Me.rpgFTP.Name = "rpgFTP"
		Me.rpgFTP.Text = "FTP"
		'
		'rpgImport
		'
		Me.rpgImport.ItemLinks.Add(Me.bbShowFiles)
		Me.rpgImport.ItemLinks.Add(Me.bbImportFiles)
		Me.rpgImport.ItemLinks.Add(Me.bcAddSalesOrders)
		Me.rpgImport.Name = "rpgImport"
		Me.rpgImport.Text = "Web Order Files"
		'
		'rpgSalesOrders
		'
		Me.rpgSalesOrders.ItemLinks.Add(Me.bcSO_ShowAll)
		Me.rpgSalesOrders.ItemLinks.Add(Me.bcSO_ShowUnProcessed)
		Me.rpgSalesOrders.ItemLinks.Add(Me.bbImportSalesOrders)
		Me.rpgSalesOrders.Name = "rpgSalesOrders"
		Me.rpgSalesOrders.Text = "Sales Orders"
		'
		'rpLog
		'
		Me.rpLog.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {Me.rpgLogs})
		Me.rpLog.Name = "rpLog"
		Me.rpLog.Text = "Logs"
		'
		'rpgLogs
		'
		Me.rpgLogs.ItemLinks.Add(Me.bbLogs)
		Me.rpgLogs.Name = "rpgLogs"
		Me.rpgLogs.Text = "Logs"
		'
		'rpSetup
		'
		Me.rpSetup.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {Me.rpgSetup})
		Me.rpSetup.Name = "rpSetup"
		Me.rpSetup.Text = "Setup"
		'
		'rpgSetup
		'
		Me.rpgSetup.ItemLinks.Add(Me.bbFTPTest)
		Me.rpgSetup.ItemLinks.Add(Me.bbFTPSave)
		Me.rpgSetup.ItemLinks.Add(Me.bbEmailTest, True)
		Me.rpgSetup.ItemLinks.Add(Me.bbEmailSave)
		Me.rpgSetup.Name = "rpgSetup"
		Me.rpgSetup.Text = "Setup"
		'
		'rpHelp
		'
		Me.rpHelp.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {Me.rpgHelp})
		Me.rpHelp.Name = "rpHelp"
		Me.rpHelp.Text = "Help"
		'
		'rpgHelp
		'
		Me.rpgHelp.ItemLinks.Add(Me.bbHelp)
		Me.rpgHelp.Name = "rpgHelp"
		Me.rpgHelp.Text = "Help"
		'
		'RibbonStatusBar
		'
		Me.RibbonStatusBar.ItemLinks.Add(Me.siCopyright)
		Me.RibbonStatusBar.ItemLinks.Add(Me.siVersion)
		Me.RibbonStatusBar.ItemLinks.Add(Me.siConnected)
		Me.RibbonStatusBar.Location = New System.Drawing.Point(0, 694)
		Me.RibbonStatusBar.Name = "RibbonStatusBar"
		Me.RibbonStatusBar.Ribbon = Me.rcMain
		Me.RibbonStatusBar.Size = New System.Drawing.Size(1224, 21)
		'
		'dmMainDocManager
		'
		Me.dmMainDocManager.ContainerControl = Me
		Me.dmMainDocManager.MenuManager = Me.rcMain
		Me.dmMainDocManager.View = Me.tvMain
		Me.dmMainDocManager.ViewCollection.AddRange(New DevExpress.XtraBars.Docking2010.Views.BaseView() {Me.tvMain})
		'
		'tvMain
		'
		Me.tvMain.DocumentGroupProperties.ShowTabHeader = False
		Me.tvMain.DocumentGroups.AddRange(New DevExpress.XtraBars.Docking2010.Views.Tabbed.DocumentGroup() {Me.DocumentGroup1})
		Me.tvMain.Documents.AddRange(New DevExpress.XtraBars.Docking2010.Views.BaseDocument() {Me.ucHelpDocument, Me.ucSetupDocument, Me.ucLogDocument, Me.ucSplashDocument, Me.ucFilesDocument})
		DockingContainer2.Element = Me.DocumentGroup1
		Me.tvMain.RootContainer.Nodes.AddRange(New DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer() {DockingContainer2})
		'
		'fMain
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(1224, 715)
		Me.Controls.Add(Me.RibbonStatusBar)
		Me.Controls.Add(Me.rcMain)
		Me.Name = "fMain"
		Me.Ribbon = Me.rcMain
		Me.StatusBar = Me.RibbonStatusBar
		Me.Text = "Woo Commerce - Jobboss Integration"
		Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
		CType(Me.DocumentGroup1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ucSplashDocument, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ucFilesDocument, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ucHelpDocument, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ucSetupDocument, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ucLogDocument, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.rcMain, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.dmMainDocManager, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.tvMain, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub

	Friend WithEvents rcMain As DevExpress.XtraBars.Ribbon.RibbonControl
	Friend WithEvents rpFile As DevExpress.XtraBars.Ribbon.RibbonPage
	Friend WithEvents rpgProgram As DevExpress.XtraBars.Ribbon.RibbonPageGroup
	Friend WithEvents RibbonStatusBar As DevExpress.XtraBars.Ribbon.RibbonStatusBar
	Friend WithEvents dmMainDocManager As DevExpress.XtraBars.Docking2010.DocumentManager
	Friend WithEvents tvMain As DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView
	Friend WithEvents DocumentGroup1 As DevExpress.XtraBars.Docking2010.Views.Tabbed.DocumentGroup
	Friend WithEvents ucFilesDocument As DevExpress.XtraBars.Docking2010.Views.Tabbed.Document
	Friend WithEvents ucHelpDocument As DevExpress.XtraBars.Docking2010.Views.Tabbed.Document
	Friend WithEvents ucSetupDocument As DevExpress.XtraBars.Docking2010.Views.Tabbed.Document
	Friend WithEvents ucLogDocument As DevExpress.XtraBars.Docking2010.Views.Tabbed.Document
	Friend WithEvents rpImport As DevExpress.XtraBars.Ribbon.RibbonPage
	Friend WithEvents rpgImport As DevExpress.XtraBars.Ribbon.RibbonPageGroup
	Friend WithEvents rpLog As DevExpress.XtraBars.Ribbon.RibbonPage
	Friend WithEvents rpgLogs As DevExpress.XtraBars.Ribbon.RibbonPageGroup
	Friend WithEvents rpSetup As DevExpress.XtraBars.Ribbon.RibbonPage
	Friend WithEvents rpgSetup As DevExpress.XtraBars.Ribbon.RibbonPageGroup
	Friend WithEvents rpHelp As DevExpress.XtraBars.Ribbon.RibbonPage
	Friend WithEvents rpgHelp As DevExpress.XtraBars.Ribbon.RibbonPageGroup
	Friend WithEvents ucSplashDocument As DevExpress.XtraBars.Docking2010.Views.Tabbed.Document
	Friend WithEvents bbExit As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbShowFiles As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbImportFiles As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbImportSalesOrders As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbFTP_GetFiles As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents rpgFTP As DevExpress.XtraBars.Ribbon.RibbonPageGroup
	Friend WithEvents bcAddSalesOrders As DevExpress.XtraBars.BarCheckItem
	Friend WithEvents bbSalesOrders As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bcSO_ShowAll As DevExpress.XtraBars.BarCheckItem
	Friend WithEvents rpgSalesOrders As DevExpress.XtraBars.Ribbon.RibbonPageGroup
	Friend WithEvents bcSO_ShowUnProcessed As DevExpress.XtraBars.BarCheckItem
	Friend WithEvents siCopyright As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents siVersion As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents siConnected As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents bbHelp As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbSetup As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbLogs As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbFTPTest As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbFTPSave As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbEmailTest As DevExpress.XtraBars.BarButtonItem
	Friend WithEvents bbEmailSave As DevExpress.XtraBars.BarButtonItem
End Class
