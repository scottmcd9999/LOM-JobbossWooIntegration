Imports System.Data.SqlClient
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraTab
Imports ExcelDataReader
Public Class ucFiles
	Private logger As NLog.Logger = NLog.LogManager.GetCurrentClassLogger

	Function ImportFiles()

	End Function

	Function FileCount() As Integer
		If gvFiles.DataSource Is Nothing Then
			Return 0
		End If
		Dim dt As New DataTable
		dt = DirectCast(gvFiles.DataSource, DataTable)
		Return dt.Rows.Count
	End Function

	Function LoadFiles(IsAuto As Boolean) As Boolean
		tcFiles.SelectedTabPage = tpFiles
		Dim filepath As String = GetFolder("LocalFolder", IsAuto)
		If String.IsNullOrEmpty(filepath) Then
			If IsAuto Then
				SendAlert("", IsAuto, "The Inbound File Path has not been set")
			Else
				MessageBox.Show("The Inbound File Path has not been set", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
			End If
			Return False
		End If
		'filepath = IO.Path.Combine(filepath, "Inbound")
		'If Not IO.Directory.Exists(filepath) Then
		'	If IsAuto Then
		'		SendAlert("", IsAuto, "The Inbound File Path could not be located")
		'	Else
		'		MessageBox.Show("The Inbound File Path could not be located", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
		'	End If
		'	Return False
		'End If

		Dim dt As New DataTable
		dt.Columns.Add("FilePath", GetType(String))
		dt.Columns.Add("FileName", GetType(String))

		Dim di As New IO.DirectoryInfo(filepath)
		Dim fiarr As IO.FileInfo() = di.GetFiles("*")
		Dim errs As New cErrors
		For Each fi As IO.FileInfo In fiarr
			Dim dtr As DataRow = dt.NewRow
			dtr("FilePath") = fi.FullName
			dtr("FileName") = fi.Name
			dt.Rows.Add(dtr)
		Next
		grFiles.DataSource = dt

		With gvFiles
			.Columns("FilePath").Visible = False
		End With

		grFiles.ForceInitialize()

		If gvFiles.RowCount > 0 Then
			'Select Case FileType
			'	Case "INV"
			'		GetInvoiceInfo(gvFiles.GetRowCellValue(0, "FullPath"))
			'	Case "EXP"
			GetFileInfo(gvFiles.GetRowCellValue(0, "FilePath"))
			'	Case Else
			'End Select

			gvFiles.SelectRow(0)
			gvFiles.FocusedRowHandle = 0
		End If

	End Function
	Function LoadSalesOrders(ShowAll As Boolean) As Boolean
		tcFiles.SelectedTabPage = tpOrders
		Using con As New SqlConnection(JBConnection)
			con.Open()
			Using cmd As New SqlCommand
				cmd.Connection = con
				cmd.CommandText = "SELECT * FROM usr_Woo_Data "
				If Not ShowAll Then
					cmd.CommandText = cmd.CommandText & " WHERE (Sales_Order IS NULL OR LEN(Sales_Order)=0)"
				End If

				Using dt As New DataTable
					dt.Load(cmd.ExecuteReader)
					grSalesOrders.DataSource = dt
				End Using
			End Using
		End Using
	End Function

	Function GetFileInfo(FilePath As String) As Boolean

		If String.IsNullOrEmpty(FilePath) Then
			Return True
		End If

		Dim extn As String = IO.Path.GetExtension(FilePath)
		Dim dt As New DataTable
		Dim res As DataSet

		Dim xlconfig As New ExcelDataSetConfiguration
		xlconfig.ConfigureDataTable = Function(tablereader) New ExcelDataTableConfiguration With {.UseHeaderRow = True}

		Using stm As New IO.FileStream(FilePath, IO.FileMode.Open, IO.FileAccess.Read)
			If extn = ".xlsx" Then
				Using excelreader As IExcelDataReader = ExcelReaderFactory.CreateReader(stm)
					res = excelreader.AsDataSet(xlconfig)
				End Using
			ElseIf extn = ".csv" Or extn = ".txt" Then
				Using excelreader As IExcelDataReader = ExcelReaderFactory.CreateCsvReader(stm)
					res = excelreader.AsDataSet(xlconfig)
				End Using
			Else
				Using excelreader As IExcelDataReader = ExcelReaderFactory.CreateBinaryReader(stm)
					res = excelreader.AsDataSet(xlconfig)
				End Using
			End If
		End Using
		'/ get the Datatable from the res dataset - there will only be one:
		dt = res.Tables(0)
		grData.DataSource = dt

	End Function



	Private Sub gvFiles_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles gvFiles.FocusedRowChanged
		GetFileInfo(gvFiles.GetFocusedRowCellValue("FilePath"))
	End Sub

	Private Sub tcFiles_SelectedPageChanged(sender As Object, e As TabPageChangedEventArgs) Handles tcFiles.SelectedPageChanged
		If e.Page.Name = "tpOrders" Then
			'LoadSalesOrders()
		End If
	End Sub
End Class
