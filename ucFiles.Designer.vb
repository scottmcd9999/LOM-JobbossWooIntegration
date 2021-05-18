<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucFiles
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
		Me.SplitContainerControl1 = New DevExpress.XtraEditors.SplitContainerControl()
		Me.grFiles = New DevExpress.XtraGrid.GridControl()
		Me.gvFiles = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.grData = New DevExpress.XtraGrid.GridControl()
		Me.gvData = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.tcFiles = New DevExpress.XtraTab.XtraTabControl()
		Me.tpFiles = New DevExpress.XtraTab.XtraTabPage()
		Me.tpOrders = New DevExpress.XtraTab.XtraTabPage()
		Me.grSalesOrders = New DevExpress.XtraGrid.GridControl()
		Me.gvSalesOrders = New DevExpress.XtraGrid.Views.Grid.GridView()
		CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SplitContainerControl1.SuspendLayout()
		CType(Me.grFiles, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvFiles, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.grData, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvData, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.tcFiles, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.tcFiles.SuspendLayout()
		Me.tpFiles.SuspendLayout()
		Me.tpOrders.SuspendLayout()
		CType(Me.grSalesOrders, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvSalesOrders, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'SplitContainerControl1
		'
		Me.SplitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.SplitContainerControl1.Location = New System.Drawing.Point(0, 0)
		Me.SplitContainerControl1.Name = "SplitContainerControl1"
		Me.SplitContainerControl1.Panel1.Controls.Add(Me.grFiles)
		Me.SplitContainerControl1.Panel1.Text = "Panel1"
		Me.SplitContainerControl1.Panel2.Controls.Add(Me.grData)
		Me.SplitContainerControl1.Panel2.Text = "Panel2"
		Me.SplitContainerControl1.Size = New System.Drawing.Size(886, 555)
		Me.SplitContainerControl1.SplitterPosition = 246
		Me.SplitContainerControl1.TabIndex = 0
		'
		'grFiles
		'
		Me.grFiles.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grFiles.Location = New System.Drawing.Point(0, 0)
		Me.grFiles.MainView = Me.gvFiles
		Me.grFiles.Name = "grFiles"
		Me.grFiles.Size = New System.Drawing.Size(246, 555)
		Me.grFiles.TabIndex = 0
		Me.grFiles.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvFiles})
		'
		'gvFiles
		'
		Me.gvFiles.GridControl = Me.grFiles
		Me.gvFiles.Name = "gvFiles"
		Me.gvFiles.OptionsView.ShowColumnHeaders = False
		Me.gvFiles.OptionsView.ShowGroupPanel = False
		Me.gvFiles.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.[False]
		Me.gvFiles.OptionsView.ShowIndicator = False
		Me.gvFiles.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.[False]
		'
		'grData
		'
		Me.grData.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grData.Location = New System.Drawing.Point(0, 0)
		Me.grData.MainView = Me.gvData
		Me.grData.Name = "grData"
		Me.grData.Size = New System.Drawing.Size(628, 555)
		Me.grData.TabIndex = 0
		Me.grData.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvData})
		'
		'gvData
		'
		Me.gvData.GridControl = Me.grData
		Me.gvData.Name = "gvData"
		Me.gvData.OptionsView.ColumnAutoWidth = False
		Me.gvData.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.[True]
		Me.gvData.OptionsView.ShowAutoFilterRow = True
		'
		'tcFiles
		'
		Me.tcFiles.Dock = System.Windows.Forms.DockStyle.Fill
		Me.tcFiles.Location = New System.Drawing.Point(0, 0)
		Me.tcFiles.Name = "tcFiles"
		Me.tcFiles.SelectedTabPage = Me.tpFiles
		Me.tcFiles.Size = New System.Drawing.Size(888, 580)
		Me.tcFiles.TabIndex = 0
		Me.tcFiles.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.tpFiles, Me.tpOrders})
		'
		'tpFiles
		'
		Me.tpFiles.Controls.Add(Me.SplitContainerControl1)
		Me.tpFiles.Name = "tpFiles"
		Me.tpFiles.Size = New System.Drawing.Size(886, 555)
		Me.tpFiles.Text = "Files"
		'
		'tpOrders
		'
		Me.tpOrders.Controls.Add(Me.grSalesOrders)
		Me.tpOrders.Name = "tpOrders"
		Me.tpOrders.Size = New System.Drawing.Size(886, 555)
		Me.tpOrders.Text = "Sales Orders"
		'
		'grSalesOrders
		'
		Me.grSalesOrders.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grSalesOrders.Location = New System.Drawing.Point(0, 0)
		Me.grSalesOrders.MainView = Me.gvSalesOrders
		Me.grSalesOrders.Name = "grSalesOrders"
		Me.grSalesOrders.Size = New System.Drawing.Size(886, 555)
		Me.grSalesOrders.TabIndex = 0
		Me.grSalesOrders.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvSalesOrders})
		'
		'gvSalesOrders
		'
		Me.gvSalesOrders.GridControl = Me.grSalesOrders
		Me.gvSalesOrders.Name = "gvSalesOrders"
		Me.gvSalesOrders.OptionsView.ColumnAutoWidth = False
		Me.gvSalesOrders.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.[True]
		Me.gvSalesOrders.OptionsView.ShowAutoFilterRow = True
		'
		'ucFiles
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.Controls.Add(Me.tcFiles)
		Me.Name = "ucFiles"
		Me.Size = New System.Drawing.Size(888, 580)
		CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.SplitContainerControl1.ResumeLayout(False)
		CType(Me.grFiles, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvFiles, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.grData, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvData, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.tcFiles, System.ComponentModel.ISupportInitialize).EndInit()
		Me.tcFiles.ResumeLayout(False)
		Me.tpFiles.ResumeLayout(False)
		Me.tpOrders.ResumeLayout(False)
		CType(Me.grSalesOrders, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvSalesOrders, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub

	Friend WithEvents SplitContainerControl1 As DevExpress.XtraEditors.SplitContainerControl
	Friend WithEvents grFiles As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvFiles As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents grData As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvData As DevExpress.XtraGrid.Views.Grid.GridView
	Friend WithEvents tcFiles As DevExpress.XtraTab.XtraTabControl
	Friend WithEvents tpFiles As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents tpOrders As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents grSalesOrders As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvSalesOrders As DevExpress.XtraGrid.Views.Grid.GridView
End Class
