Imports System.Data.SqlClient
Public Class cInvoiceDetail
	Private logger As NLog.Logger = NLog.LogManager.GetCurrentClassLogger

	Property InvoiceDetail As Integer
	Property inv As cInvoiceHeader
	Property pld As cPacklistDetail
	Property sod As cSODetail
	Property SalesCode As String
	Property ShippedDate As Date
	Property PriceUofM As String
	Property PurchaseUofM As String
	Property Quantity As Double
	Property UnitPrice As Double
	Property TaxCode As String
	Property DocLine As Integer
	Property IsAuto As Boolean
	Property cmd As SqlCommand
	Property ClassErrors As String
	Private Sub AddError(ErrorMsg As String)
		If ClassErrors Is Nothing Then
			ClassErrors = ErrorMsg
		Else
			ClassErrors = ClassErrors & crlf() & ErrorMsg
		End If
	End Sub
	Function AddDetail() As Boolean
		Try
			Dim hdr As String = "Document,Document_Line,AR_Code,Ship_Date,Quantity,Unit_Price,Price_UofM,Amount,Sales_Rep,Commission_Pct,Commissionable," _
						& "Prepaid_Amt,Prepaid_Code,Reference,Note_Text,Last_Updated,Order_Unit,Price_Unit_Conv,Tax_Code,Prepaid_Tax_Amount,CommissionIncluded)"

			Dim vals As String = AddString(inv.Document)
			vals = vals & "," & DocLine
			vals = vals & "," & AddString(pld.SODClass.SalesCode,, "NULL")
			vals = vals & "," & AddString(ShippedDate, DbType.Date, "NULL")
			vals = vals & "," & pld.Quantity
			vals = vals & "," & pld.SODClass.UnitPrice
			vals = vals & "," & AddString(pld.SODClass.PriceUoM)
			vals = vals & "," & pld.Quantity * pld.SODClass.UnitPrice
			vals = vals & ",NULL,0,0,0,NULL,NULL,'Added by Woo Integration'"
			vals = vals & "," & AddString(Now, DbType.DateTime)
			vals = vals & "," & AddString(pld.SODClass.Matl.PurchaseUofM)
			vals = vals & ",1"
			vals = vals & "," & AddString(pld.SODClass.SalesCode)
			vals = vals & ",0,0"

			cmd.CommandText = hdr & " VALUES(" & vals & ")"
			cmd.ExecuteNonQuery()

			cmd.CommandText = "SELECT Invoice_Detail FROM Invoice_Detail WHERE Invoice_DetailKey=(SELECT SCOPE_IDENTITY())"
			InvoiceDetail = cmd.ExecuteScalar
			Return True
		Catch ex As Exception
			logger.Error(ex.ToString, "Jobboss_Woo_Integration.cInvoiceDetail.AddDetail Error")
			AddError(ex.ToString)
			If Not IsAuto Then
				MessageBox.Show(String.Format("Error in Jobboss_Woo_Integration.cInvoiceDetail.AddDetail: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			End If
			Return False
		Finally

		End Try

	End Function

End Class
