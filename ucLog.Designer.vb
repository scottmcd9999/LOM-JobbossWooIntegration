<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucLog
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
		Me.grLog = New DevExpress.XtraGrid.GridControl()
		Me.gvLogs = New DevExpress.XtraGrid.Views.Grid.GridView()
		CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SplitContainerControl1.SuspendLayout()
		CType(Me.grLog, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvLogs, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'SplitContainerControl1
		'
		Me.SplitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.SplitContainerControl1.Location = New System.Drawing.Point(0, 0)
		Me.SplitContainerControl1.Name = "SplitContainerControl1"
		Me.SplitContainerControl1.Panel1.Text = "Panel1"
		Me.SplitContainerControl1.Panel2.Controls.Add(Me.grLog)
		Me.SplitContainerControl1.Panel2.Text = "Panel2"
		Me.SplitContainerControl1.Size = New System.Drawing.Size(788, 490)
		Me.SplitContainerControl1.SplitterPosition = 226
		Me.SplitContainerControl1.TabIndex = 0
		'
		'grLog
		'
		Me.grLog.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grLog.Location = New System.Drawing.Point(0, 0)
		Me.grLog.MainView = Me.gvLogs
		Me.grLog.Name = "grLog"
		Me.grLog.Size = New System.Drawing.Size(550, 490)
		Me.grLog.TabIndex = 0
		Me.grLog.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvLogs})
		'
		'gvLogs
		'
		Me.gvLogs.GridControl = Me.grLog
		Me.gvLogs.Name = "gvLogs"
		'
		'ucLog
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.Controls.Add(Me.SplitContainerControl1)
		Me.Name = "ucLog"
		Me.Size = New System.Drawing.Size(788, 490)
		CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.SplitContainerControl1.ResumeLayout(False)
		CType(Me.grLog, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvLogs, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub

	Friend WithEvents SplitContainerControl1 As DevExpress.XtraEditors.SplitContainerControl
	Friend WithEvents grLog As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvLogs As DevExpress.XtraGrid.Views.Grid.GridView
End Class
