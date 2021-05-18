<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucSetup
	Inherits DevExpress.XtraEditors.XtraUserControl

	'UserControl overrides dispose to clean up the component list.
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
		Me.GroupControl1 = New DevExpress.XtraEditors.GroupControl()
		Me.LabelControl3 = New DevExpress.XtraEditors.LabelControl()
		Me.XtraFolderBrowserDialog1 = New DevExpress.XtraEditors.XtraFolderBrowserDialog(Me.components)
		Me.GroupControl4 = New DevExpress.XtraEditors.GroupControl()
		Me.SplitContainerControl1 = New DevExpress.XtraEditors.SplitContainerControl()
		Me.GroupControl5 = New DevExpress.XtraEditors.GroupControl()
		Me.ckShowMessages = New DevExpress.XtraEditors.CheckEdit()
		Me.ckRequireLogin = New DevExpress.XtraEditors.CheckEdit()
		Me.ckTLSOverRegPort = New DevExpress.XtraEditors.CheckEdit()
		Me.cmTestEmail = New DevExpress.XtraEditors.SimpleButton()
		Me.txSMTP = New System.Windows.Forms.TextBox()
		Me.Label34 = New System.Windows.Forms.Label()
		Me.Label13 = New System.Windows.Forms.Label()
		Me.txSMTPPass = New System.Windows.Forms.TextBox()
		Me.txPort = New System.Windows.Forms.TextBox()
		Me.Label8 = New System.Windows.Forms.Label()
		Me.txEmailTo = New System.Windows.Forms.TextBox()
		Me.txSMTPFrom = New System.Windows.Forms.TextBox()
		Me.Label5 = New System.Windows.Forms.Label()
		Me.Label14 = New System.Windows.Forms.Label()
		Me.Label6 = New System.Windows.Forms.Label()
		Me.txSMTPLogin = New System.Windows.Forms.TextBox()
		Me.Label7 = New System.Windows.Forms.Label()
		Me.GroupControl7 = New DevExpress.XtraEditors.GroupControl()
		Me.grAlerts = New DevExpress.XtraGrid.GridControl()
		Me.gvAlerts = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.GroupControl8 = New DevExpress.XtraEditors.GroupControl()
		Me.txFTPFolder = New DevExpress.XtraEditors.TextEdit()
		Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
		Me.LabelControl14 = New DevExpress.XtraEditors.LabelControl()
		Me.txSSHPass = New DevExpress.XtraEditors.ButtonEdit()
		Me.txFTPUser = New DevExpress.XtraEditors.TextEdit()
		Me.LabelControl9 = New DevExpress.XtraEditors.LabelControl()
		Me.txFTPPort = New DevExpress.XtraEditors.TextEdit()
		Me.LabelControl8 = New DevExpress.XtraEditors.LabelControl()
		Me.txFTPServer = New DevExpress.XtraEditors.TextEdit()
		Me.LabelControl5 = New DevExpress.XtraEditors.LabelControl()
		Me.txLocalFolder = New DevExpress.XtraEditors.ButtonEdit()
		Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl()
		Me.cbJobbossCustomer = New DevExpress.XtraEditors.LookUpEdit()
		CType(Me.GroupControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupControl1.SuspendLayout()
		CType(Me.GroupControl4, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupControl4.SuspendLayout()
		CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.SplitContainerControl1.Panel1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SplitContainerControl1.Panel1.SuspendLayout()
		CType(Me.SplitContainerControl1.Panel2, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SplitContainerControl1.Panel2.SuspendLayout()
		Me.SplitContainerControl1.SuspendLayout()
		CType(Me.GroupControl5, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupControl5.SuspendLayout()
		CType(Me.ckShowMessages.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ckRequireLogin.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ckTLSOverRegPort.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.GroupControl7, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupControl7.SuspendLayout()
		CType(Me.grAlerts, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvAlerts, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.GroupControl8, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupControl8.SuspendLayout()
		CType(Me.txFTPFolder.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txSSHPass.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txFTPUser.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txFTPPort.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txFTPServer.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txLocalFolder.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.cbJobbossCustomer.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'GroupControl1
		'
		Me.GroupControl1.Controls.Add(Me.LabelControl3)
		Me.GroupControl1.Controls.Add(Me.cbJobbossCustomer)
		Me.GroupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Card
		Me.GroupControl1.Location = New System.Drawing.Point(5, 5)
		Me.GroupControl1.Name = "GroupControl1"
		Me.GroupControl1.Size = New System.Drawing.Size(414, 142)
		Me.GroupControl1.TabIndex = 0
		Me.GroupControl1.Text = "General Settings"
		'
		'LabelControl3
		'
		Me.LabelControl3.Location = New System.Drawing.Point(11, 33)
		Me.LabelControl3.Name = "LabelControl3"
		Me.LabelControl3.Size = New System.Drawing.Size(92, 13)
		Me.LabelControl3.TabIndex = 0
		Me.LabelControl3.Text = "Jobboss Customer:"
		'
		'XtraFolderBrowserDialog1
		'
		Me.XtraFolderBrowserDialog1.SelectedPath = "XtraFolderBrowserDialog1"
		'
		'GroupControl4
		'
		Me.GroupControl4.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.GroupControl4.Controls.Add(Me.SplitContainerControl1)
		Me.GroupControl4.GroupStyle = DevExpress.Utils.GroupStyle.Card
		Me.GroupControl4.Location = New System.Drawing.Point(425, 5)
		Me.GroupControl4.Name = "GroupControl4"
		Me.GroupControl4.Size = New System.Drawing.Size(618, 415)
		Me.GroupControl4.TabIndex = 2
		Me.GroupControl4.Text = "Email Setup"
		'
		'SplitContainerControl1
		'
		Me.SplitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.SplitContainerControl1.Location = New System.Drawing.Point(2, 21)
		Me.SplitContainerControl1.Name = "SplitContainerControl1"
		'
		'SplitContainerControl1.Panel1
		'
		Me.SplitContainerControl1.Panel1.Controls.Add(Me.GroupControl5)
		Me.SplitContainerControl1.Panel1.Text = "Panel1"
		'
		'SplitContainerControl1.Panel2
		'
		Me.SplitContainerControl1.Panel2.Controls.Add(Me.GroupControl7)
		Me.SplitContainerControl1.Panel2.Text = "Panel2"
		Me.SplitContainerControl1.Size = New System.Drawing.Size(614, 392)
		Me.SplitContainerControl1.SplitterPosition = 309
		Me.SplitContainerControl1.TabIndex = 0
		'
		'GroupControl5
		'
		Me.GroupControl5.Controls.Add(Me.ckShowMessages)
		Me.GroupControl5.Controls.Add(Me.ckRequireLogin)
		Me.GroupControl5.Controls.Add(Me.ckTLSOverRegPort)
		Me.GroupControl5.Controls.Add(Me.cmTestEmail)
		Me.GroupControl5.Controls.Add(Me.txSMTP)
		Me.GroupControl5.Controls.Add(Me.Label34)
		Me.GroupControl5.Controls.Add(Me.Label13)
		Me.GroupControl5.Controls.Add(Me.txSMTPPass)
		Me.GroupControl5.Controls.Add(Me.txPort)
		Me.GroupControl5.Controls.Add(Me.Label8)
		Me.GroupControl5.Controls.Add(Me.txEmailTo)
		Me.GroupControl5.Controls.Add(Me.txSMTPFrom)
		Me.GroupControl5.Controls.Add(Me.Label5)
		Me.GroupControl5.Controls.Add(Me.Label14)
		Me.GroupControl5.Controls.Add(Me.Label6)
		Me.GroupControl5.Controls.Add(Me.txSMTPLogin)
		Me.GroupControl5.Controls.Add(Me.Label7)
		Me.GroupControl5.Dock = System.Windows.Forms.DockStyle.Fill
		Me.GroupControl5.GroupStyle = DevExpress.Utils.GroupStyle.Light
		Me.GroupControl5.Location = New System.Drawing.Point(0, 0)
		Me.GroupControl5.Name = "GroupControl5"
		Me.GroupControl5.Size = New System.Drawing.Size(309, 392)
		Me.GroupControl5.TabIndex = 0
		Me.GroupControl5.Text = "SMTP Information"
		'
		'ckShowMessages
		'
		Me.ckShowMessages.Location = New System.Drawing.Point(103, 231)
		Me.ckShowMessages.Name = "ckShowMessages"
		Me.ckShowMessages.Properties.Caption = "Show Messages (Manual Mode)"
		Me.ckShowMessages.Size = New System.Drawing.Size(181, 19)
		Me.ckShowMessages.TabIndex = 15
		'
		'ckRequireLogin
		'
		Me.ckRequireLogin.Location = New System.Drawing.Point(103, 102)
		Me.ckRequireLogin.Name = "ckRequireLogin"
		Me.ckRequireLogin.Properties.Caption = "Require Login"
		Me.ckRequireLogin.Size = New System.Drawing.Size(140, 19)
		Me.ckRequireLogin.TabIndex = 6
		'
		'ckTLSOverRegPort
		'
		Me.ckTLSOverRegPort.Location = New System.Drawing.Point(103, 77)
		Me.ckTLSOverRegPort.Name = "ckTLSOverRegPort"
		Me.ckTLSOverRegPort.Properties.Caption = "TLS Over Regular Port"
		Me.ckTLSOverRegPort.Size = New System.Drawing.Size(140, 19)
		Me.ckTLSOverRegPort.TabIndex = 5
		'
		'cmTestEmail
		'
		Me.cmTestEmail.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.cmTestEmail.Location = New System.Drawing.Point(103, 256)
		Me.cmTestEmail.Name = "cmTestEmail"
		Me.cmTestEmail.Size = New System.Drawing.Size(181, 43)
		Me.cmTestEmail.TabIndex = 16
		Me.cmTestEmail.Text = "Test Email Settings"
		'
		'txSMTP
		'
		Me.txSMTP.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txSMTP.BackColor = System.Drawing.SystemColors.Window
		Me.txSMTP.Location = New System.Drawing.Point(103, 24)
		Me.txSMTP.Name = "txSMTP"
		Me.txSMTP.Size = New System.Drawing.Size(181, 21)
		Me.txSMTP.TabIndex = 1
		'
		'Label34
		'
		Me.Label34.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.Label34.AutoSize = True
		Me.Label34.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label34.ForeColor = System.Drawing.Color.Red
		Me.Label34.Location = New System.Drawing.Point(290, 24)
		Me.Label34.Name = "Label34"
		Me.Label34.Size = New System.Drawing.Size(15, 20)
		Me.Label34.TabIndex = 2
		Me.Label34.Text = "*"
		'
		'Label13
		'
		Me.Label13.AutoSize = True
		Me.Label13.Location = New System.Drawing.Point(68, 52)
		Me.Label13.Name = "Label13"
		Me.Label13.Size = New System.Drawing.Size(31, 13)
		Me.Label13.TabIndex = 3
		Me.Label13.Text = "Port:"
		'
		'txSMTPPass
		'
		Me.txSMTPPass.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txSMTPPass.Location = New System.Drawing.Point(103, 152)
		Me.txSMTPPass.Name = "txSMTPPass"
		Me.txSMTPPass.Size = New System.Drawing.Size(181, 21)
		Me.txSMTPPass.TabIndex = 10
		'
		'txPort
		'
		Me.txPort.Location = New System.Drawing.Point(103, 50)
		Me.txPort.Name = "txPort"
		Me.txPort.Size = New System.Drawing.Size(88, 21)
		Me.txPort.TabIndex = 4
		'
		'Label8
		'
		Me.Label8.AutoSize = True
		Me.Label8.Location = New System.Drawing.Point(26, 27)
		Me.Label8.Name = "Label8"
		Me.Label8.Size = New System.Drawing.Size(68, 13)
		Me.Label8.TabIndex = 0
		Me.Label8.Text = "SMTP Server"
		'
		'txEmailTo
		'
		Me.txEmailTo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txEmailTo.Location = New System.Drawing.Point(103, 204)
		Me.txEmailTo.Name = "txEmailTo"
		Me.txEmailTo.Size = New System.Drawing.Size(181, 21)
		Me.txEmailTo.TabIndex = 14
		'
		'txSMTPFrom
		'
		Me.txSMTPFrom.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txSMTPFrom.Location = New System.Drawing.Point(103, 179)
		Me.txSMTPFrom.Name = "txSMTPFrom"
		Me.txSMTPFrom.Size = New System.Drawing.Size(181, 21)
		Me.txSMTPFrom.TabIndex = 12
		'
		'Label5
		'
		Me.Label5.AutoSize = True
		Me.Label5.Location = New System.Drawing.Point(31, 130)
		Me.Label5.Name = "Label5"
		Me.Label5.Size = New System.Drawing.Size(61, 13)
		Me.Label5.TabIndex = 7
		Me.Label5.Text = "SMTP Login"
		'
		'Label14
		'
		Me.Label14.AutoSize = True
		Me.Label14.Location = New System.Drawing.Point(32, 207)
		Me.Label14.Name = "Label14"
		Me.Label14.Size = New System.Drawing.Size(63, 13)
		Me.Label14.TabIndex = 13
		Me.Label14.Text = "Send CC To"
		'
		'Label6
		'
		Me.Label6.AutoSize = True
		Me.Label6.Location = New System.Drawing.Point(11, 155)
		Me.Label6.Name = "Label6"
		Me.Label6.Size = New System.Drawing.Size(82, 13)
		Me.Label6.TabIndex = 9
		Me.Label6.Text = "SMTP Password"
		'
		'txSMTPLogin
		'
		Me.txSMTPLogin.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txSMTPLogin.Location = New System.Drawing.Point(103, 127)
		Me.txSMTPLogin.Name = "txSMTPLogin"
		Me.txSMTPLogin.Size = New System.Drawing.Size(181, 21)
		Me.txSMTPLogin.TabIndex = 8
		'
		'Label7
		'
		Me.Label7.AutoSize = True
		Me.Label7.Location = New System.Drawing.Point(39, 182)
		Me.Label7.Name = "Label7"
		Me.Label7.Size = New System.Drawing.Size(58, 13)
		Me.Label7.TabIndex = 11
		Me.Label7.Text = "Email From"
		'
		'GroupControl7
		'
		Me.GroupControl7.Controls.Add(Me.grAlerts)
		Me.GroupControl7.Dock = System.Windows.Forms.DockStyle.Fill
		Me.GroupControl7.GroupStyle = DevExpress.Utils.GroupStyle.Light
		Me.GroupControl7.Location = New System.Drawing.Point(0, 0)
		Me.GroupControl7.Name = "GroupControl7"
		Me.GroupControl7.Size = New System.Drawing.Size(293, 392)
		Me.GroupControl7.TabIndex = 0
		Me.GroupControl7.Text = "Alert Emails"
		'
		'grAlerts
		'
		Me.grAlerts.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grAlerts.Location = New System.Drawing.Point(2, 21)
		Me.grAlerts.MainView = Me.gvAlerts
		Me.grAlerts.Name = "grAlerts"
		Me.grAlerts.Size = New System.Drawing.Size(289, 369)
		Me.grAlerts.TabIndex = 0
		Me.grAlerts.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvAlerts})
		'
		'gvAlerts
		'
		Me.gvAlerts.GridControl = Me.grAlerts
		Me.gvAlerts.Name = "gvAlerts"
		Me.gvAlerts.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.[True]
		Me.gvAlerts.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.[True]
		Me.gvAlerts.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top
		Me.gvAlerts.OptionsView.ShowColumnHeaders = False
		Me.gvAlerts.OptionsView.ShowGroupPanel = False
		Me.gvAlerts.OptionsView.ShowIndicator = False
		'
		'GroupControl8
		'
		Me.GroupControl8.Controls.Add(Me.txFTPFolder)
		Me.GroupControl8.Controls.Add(Me.LabelControl1)
		Me.GroupControl8.Controls.Add(Me.LabelControl14)
		Me.GroupControl8.Controls.Add(Me.txSSHPass)
		Me.GroupControl8.Controls.Add(Me.txFTPUser)
		Me.GroupControl8.Controls.Add(Me.LabelControl9)
		Me.GroupControl8.Controls.Add(Me.txFTPPort)
		Me.GroupControl8.Controls.Add(Me.LabelControl8)
		Me.GroupControl8.Controls.Add(Me.txFTPServer)
		Me.GroupControl8.Controls.Add(Me.LabelControl5)
		Me.GroupControl8.Controls.Add(Me.txLocalFolder)
		Me.GroupControl8.Controls.Add(Me.LabelControl2)
		Me.GroupControl8.GroupStyle = DevExpress.Utils.GroupStyle.Card
		Me.GroupControl8.Location = New System.Drawing.Point(5, 153)
		Me.GroupControl8.Name = "GroupControl8"
		Me.GroupControl8.Size = New System.Drawing.Size(414, 185)
		Me.GroupControl8.TabIndex = 1
		Me.GroupControl8.Text = "FTP Setup"
		'
		'txFTPFolder
		'
		Me.txFTPFolder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txFTPFolder.Location = New System.Drawing.Point(70, 50)
		Me.txFTPFolder.Name = "txFTPFolder"
		Me.txFTPFolder.Size = New System.Drawing.Size(334, 20)
		Me.txFTPFolder.TabIndex = 3
		'
		'LabelControl1
		'
		Me.LabelControl1.Location = New System.Drawing.Point(10, 54)
		Me.LabelControl1.Name = "LabelControl1"
		Me.LabelControl1.Size = New System.Drawing.Size(55, 13)
		Me.LabelControl1.TabIndex = 2
		Me.LabelControl1.Text = "FTP Folder:"
		'
		'LabelControl14
		'
		Me.LabelControl14.Location = New System.Drawing.Point(15, 158)
		Me.LabelControl14.Name = "LabelControl14"
		Me.LabelControl14.Size = New System.Drawing.Size(50, 13)
		Me.LabelControl14.TabIndex = 10
		Me.LabelControl14.Text = "Password:"
		'
		'txSSHPass
		'
		Me.txSSHPass.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txSSHPass.Location = New System.Drawing.Point(70, 154)
		Me.txSSHPass.Name = "txSSHPass"
		Me.txSSHPass.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Search)})
		Me.txSSHPass.Properties.UseSystemPasswordChar = True
		Me.txSSHPass.Size = New System.Drawing.Size(334, 20)
		Me.txSSHPass.TabIndex = 11
		'
		'txFTPUser
		'
		Me.txFTPUser.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txFTPUser.Location = New System.Drawing.Point(70, 128)
		Me.txFTPUser.Name = "txFTPUser"
		Me.txFTPUser.Size = New System.Drawing.Size(334, 20)
		Me.txFTPUser.TabIndex = 9
		'
		'LabelControl9
		'
		Me.LabelControl9.Location = New System.Drawing.Point(39, 132)
		Me.LabelControl9.Name = "LabelControl9"
		Me.LabelControl9.Size = New System.Drawing.Size(26, 13)
		Me.LabelControl9.TabIndex = 8
		Me.LabelControl9.Text = "User:"
		'
		'txFTPPort
		'
		Me.txFTPPort.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txFTPPort.Location = New System.Drawing.Point(70, 102)
		Me.txFTPPort.Name = "txFTPPort"
		Me.txFTPPort.Size = New System.Drawing.Size(334, 20)
		Me.txFTPPort.TabIndex = 7
		'
		'LabelControl8
		'
		Me.LabelControl8.Location = New System.Drawing.Point(20, 106)
		Me.LabelControl8.Name = "LabelControl8"
		Me.LabelControl8.Size = New System.Drawing.Size(45, 13)
		Me.LabelControl8.TabIndex = 6
		Me.LabelControl8.Text = "FTP Port:"
		'
		'txFTPServer
		'
		Me.txFTPServer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txFTPServer.Location = New System.Drawing.Point(70, 76)
		Me.txFTPServer.Name = "txFTPServer"
		Me.txFTPServer.Size = New System.Drawing.Size(334, 20)
		Me.txFTPServer.TabIndex = 5
		'
		'LabelControl5
		'
		Me.LabelControl5.Location = New System.Drawing.Point(8, 80)
		Me.LabelControl5.Name = "LabelControl5"
		Me.LabelControl5.Size = New System.Drawing.Size(57, 13)
		Me.LabelControl5.TabIndex = 4
		Me.LabelControl5.Text = "FTP Server:"
		'
		'txLocalFolder
		'
		Me.txLocalFolder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txLocalFolder.Location = New System.Drawing.Point(70, 24)
		Me.txLocalFolder.Name = "txLocalFolder"
		Me.txLocalFolder.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.txLocalFolder.Size = New System.Drawing.Size(334, 20)
		Me.txLocalFolder.TabIndex = 1
		'
		'LabelControl2
		'
		Me.LabelControl2.Location = New System.Drawing.Point(37, 27)
		Me.LabelControl2.Name = "LabelControl2"
		Me.LabelControl2.Size = New System.Drawing.Size(28, 13)
		Me.LabelControl2.TabIndex = 0
		Me.LabelControl2.Text = "Local:"
		'
		'cbJobbossCustomer
		'
		Me.cbJobbossCustomer.Location = New System.Drawing.Point(109, 30)
		Me.cbJobbossCustomer.Name = "cbJobbossCustomer"
		Me.cbJobbossCustomer.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.cbJobbossCustomer.Properties.NullText = ""
		Me.cbJobbossCustomer.Size = New System.Drawing.Size(295, 20)
		Me.cbJobbossCustomer.TabIndex = 1
		'
		'ucSetup
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.Controls.Add(Me.GroupControl8)
		Me.Controls.Add(Me.GroupControl4)
		Me.Controls.Add(Me.GroupControl1)
		Me.Name = "ucSetup"
		Me.Size = New System.Drawing.Size(1046, 432)
		CType(Me.GroupControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupControl1.ResumeLayout(False)
		Me.GroupControl1.PerformLayout()
		CType(Me.GroupControl4, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupControl4.ResumeLayout(False)
		CType(Me.SplitContainerControl1.Panel1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.SplitContainerControl1.Panel1.ResumeLayout(False)
		CType(Me.SplitContainerControl1.Panel2, System.ComponentModel.ISupportInitialize).EndInit()
		Me.SplitContainerControl1.Panel2.ResumeLayout(False)
		CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.SplitContainerControl1.ResumeLayout(False)
		CType(Me.GroupControl5, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupControl5.ResumeLayout(False)
		Me.GroupControl5.PerformLayout()
		CType(Me.ckShowMessages.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ckRequireLogin.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ckTLSOverRegPort.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.GroupControl7, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupControl7.ResumeLayout(False)
		CType(Me.grAlerts, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvAlerts, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.GroupControl8, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupControl8.ResumeLayout(False)
		Me.GroupControl8.PerformLayout()
		CType(Me.txFTPFolder.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txSSHPass.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txFTPUser.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txFTPPort.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txFTPServer.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txLocalFolder.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.cbJobbossCustomer.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub

	Friend WithEvents GroupControl1 As DevExpress.XtraEditors.GroupControl
	Friend WithEvents XtraFolderBrowserDialog1 As DevExpress.XtraEditors.XtraFolderBrowserDialog
	Friend WithEvents GroupControl4 As DevExpress.XtraEditors.GroupControl
	Friend WithEvents SplitContainerControl1 As DevExpress.XtraEditors.SplitContainerControl
	Friend WithEvents GroupControl5 As DevExpress.XtraEditors.GroupControl
	Friend WithEvents ckShowMessages As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents ckRequireLogin As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents ckTLSOverRegPort As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents cmTestEmail As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents txSMTP As TextBox
	Friend WithEvents Label34 As Label
	Friend WithEvents Label13 As Label
	Friend WithEvents txSMTPPass As TextBox
	Friend WithEvents txPort As TextBox
	Friend WithEvents Label8 As Label
	Friend WithEvents txEmailTo As TextBox
	Friend WithEvents txSMTPFrom As TextBox
	Friend WithEvents Label5 As Label
	Friend WithEvents Label14 As Label
	Friend WithEvents Label6 As Label
	Friend WithEvents txSMTPLogin As TextBox
	Friend WithEvents Label7 As Label
	Friend WithEvents GroupControl7 As DevExpress.XtraEditors.GroupControl
	Friend WithEvents grAlerts As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvAlerts As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents GroupControl8 As DevExpress.XtraEditors.GroupControl
	Friend WithEvents LabelControl14 As DevExpress.XtraEditors.LabelControl
	Friend WithEvents txSSHPass As DevExpress.XtraEditors.ButtonEdit
	Friend WithEvents txFTPUser As DevExpress.XtraEditors.TextEdit
	Friend WithEvents LabelControl9 As DevExpress.XtraEditors.LabelControl
	Friend WithEvents txFTPPort As DevExpress.XtraEditors.TextEdit
	Friend WithEvents LabelControl8 As DevExpress.XtraEditors.LabelControl
	Friend WithEvents txFTPServer As DevExpress.XtraEditors.TextEdit
	Friend WithEvents LabelControl5 As DevExpress.XtraEditors.LabelControl
	Friend WithEvents txLocalFolder As DevExpress.XtraEditors.ButtonEdit
	Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
	Friend WithEvents txFTPFolder As DevExpress.XtraEditors.TextEdit
	Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
	Friend WithEvents LabelControl3 As DevExpress.XtraEditors.LabelControl
	Friend WithEvents cbJobbossCustomer As DevExpress.XtraEditors.LookUpEdit
End Class
