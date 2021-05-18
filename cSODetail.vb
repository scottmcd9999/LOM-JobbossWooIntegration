Imports System.Data.SqlClient
Public Class cSODetail

	Private logger As NLog.Logger = NLog.LogManager.GetCurrentClassLogger
	'Property Config As cConfiguration
	Property Customer As String
	Property SalesOrder As String
	Property Status As String
	Property Material As String
	Property Rev As String
	Property Quantity As Double
	Property PickedQuantity As Double
	Property ShippedQuantity As Double
	Property UnitPrice As Double
	Property ShipTo As Integer
	'Property DockCode As String
	Property PromisedDate As Date
	Property OrderDate As Date
	Property SalesCode As String
	Property LineNumber As String
	Property SODetail As Integer
	Property Job As String
	Property TaxCode As String
	Property IsActive As Boolean
	Property TotalLineAmount As Double
	Property Notes As String
	Property ShippingInstructions As String
	Property PriceUoM As String
	Property StockUoM As String
	Property Delivery As cDelivery
	Property Matl As cMaterial
	Property Errors As String
	Property IsAuto As Boolean

	Private Sub AddError(ErrorMsg As String)
		If Errors Is Nothing Then
			Errors = ErrorMsg
		Else
			Errors = Errors & Environment.NewLine & ErrorMsg
		End If
	End Sub

	Private Function LoadClass(dtr As DataRow, cmd As SqlCommand, Optional IgnoreJobLink As Boolean = False) As Boolean
		Try
			Me.Status = dtr("Status")
			LineNumber = dtr("SO_Line")
			PromisedDate = dtr("Promised_Date")
			Material = dtr("Material")
			Matl = New cMaterial
			With Matl
				.cmd = cmd
				If Not .Load(Material, cmd) Then
					Return False
				End If
			End With

			SalesOrder = dtr("Sales_Order")
			Quantity = dtr("Order_Qty")
			SODetail = dtr("SO_Detail")
			Job = dtr("Job") & ""
			PickedQuantity = dtr("Picked_Qty")
			ShippedQuantity = dtr("Shipped_Qty")
			UnitPrice = dtr("Unit_Price")
			PriceUoM = dtr("Price_UofM")
			StockUoM = dtr("Stock_UofM")
			If IgnoreJobLink Then
				IsActive = PickedQuantity > 0 Or ShippedQuantity > 0 Or dtr("Status") = "Backorder" Or dtr("Status") = "Closed" _
					Or Not IsDBNull(dtr("PO"))
			Else
				IsActive = PickedQuantity > 0 Or ShippedQuantity > 0 Or dtr("Status") = "Backorder" Or dtr("Status") = "Closed" Or Not IsDBNull(dtr("Job")) _
					Or Not IsDBNull(dtr("PO"))
			End If

			'/ Internal Shipping Notes on the SO Detail are stored in the Delivery comment:
			cmd.CommandText = "SELECT Comment FROM Delivery WHERE SO_Detail=" & SODetail & " AND Job IS NULL AND Shipped_Date IS NULL"
			Dim note As Object = cmd.ExecuteScalar
			If note Is Nothing Then
				ShippingInstructions = ""
			Else
				ShippingInstructions = note.ToString
			End If

			Dim deliv As New cDelivery
			With deliv
				If Not .Load(SODetail, cmd) Then
					Return False
				End If
			End With

			Return True
		Catch ex As Exception
			logger.Error(ex.ToString, "SalesOrderImport.cSODetail.LoadClass Error")
			AddError(ex.ToString)
			If Not IsAuto Then
				MessageBox.Show(String.Format("Error in SalesOrderImport.cSODetail.LoadClass: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			End If
			Return False
		Finally

		End Try
	End Function

	Function AddSalesOrderDetailLine() As Boolean
		Try
			Using con As New SqlConnection(JBConnection)
				con.Open()
				Using cmd As New SqlCommand
					cmd.Connection = con
					Return AddSODetail(cmd)
				End Using
			End Using
		Catch ex As Exception
			logger.Error(ex.ToString, "AddSalesOrderDetailLine Error")
			If Not IsAuto Then
				MessageBox.Show(String.Format("Error in AddSalesOrderDetailLine: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			End If
			Return False
		Finally
		End Try
	End Function
	Function AddSalesOrderDetailLine(cmd As SqlCommand) As Boolean
		Try
			Return AddSODetail(cmd)
		Catch ex As Exception
			logger.Error(ex.ToString, "AddSalesOrderDetailLine Error")
			AddError(ex.ToString)
			If Not IsAuto Then
				MessageBox.Show(String.Format("Error in AddSalesOrderDetailLine: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			End If
			Return False
		Finally
		End Try
	End Function

	Public Function UpdateSODetail(cmd As SqlCommand, ModNotes As String) As Boolean
		Try
			'Dim ModNotes As String
			cmd.CommandText = "UPDATE SO_Detail SET Order_Qty=" & Quantity & ", Promised_Date=" & AddString(PromisedDate, DbType.Date) _
				& ",SO_Line=" & AddString(LineNumber) & ", Last_Updated=" & AddString(Now, DbType.DateTime) & " WHERE SO_Detail=" & SODetail
			cmd.ExecuteNonQuery()

			'If Config.UpdateDetailLineNotes Then
			'	ModNotes = Notes.Trim & vbCr & "[" & Now.Year & "/" & Now.Month.ToString("00") & "/" & Now.Day.ToString("00") & "]: " & ModNotes
			'	'/ Detail Line Notes are in the SO Detail table:
			'	cmd.CommandText = "UPDATE SO_Detail SET Note_Text=" & AddString(Notes) & ",Last_Updated=" & AddString(Now, DbType.DateTime) _
			'		& " WHERE SO_Detail=" & SODetail
			'End If

			'If Config.UpdateShippingInstructions Then
			'	ModNotes = ShippingInstructions.Trim & vbCr & "[" & Now.Year & "/" & Now.Month.ToString("00") & "/" & Now.Day.ToString("00") & "]: " & ModNotes
			'	'/ shipping instructions are in the Delivery table:
			'	If Not String.IsNullOrEmpty(Job) Then
			'		cmd.CommandText = "UPDATE Delivery SET Comment=" & AddString(ModNotes) & ", Last_Updated=" & AddString(Now, DbType.DateTime) _
			'			& " WHERE SO_Detail=" & SODetail & " AND Job=" & AddString(Job) & " AND Shipped_Date IS NULL"
			'	Else
			'		cmd.CommandText = "UPDATE Delivery SET Comment=" & AddString(ModNotes) & ", Last_Updated=" & AddString(Now, DbType.DateTime) _
			'		& " WHERE SO_Detail=" & SODetail & " AND Job IS NULL AND Shipped_Date IS NULL"
			'	End If
			'End If

			cmd.ExecuteNonQuery()
			Return True
		Catch ex As Exception
			logger.Error(ex.ToString, "SalesOrderImport.cSODetail.UpdateSODetail Error")
			AddError(ex.ToString)
			If Not IsAuto Then
				MessageBox.Show(String.Format("Error in SalesOrderImport.cSODetail.UpdateSODetail: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			End If
			Return False
		Finally

		End Try
	End Function

	Private Function AddSODetail(cmd As SqlCommand) As Boolean
		Try
			'Dim log As New cImportLog
			Dim vals As String
			Dim hdr As String = "INSERT INTO SO_Detail([Sales_Order],[SO_Line],[PO],[Line],[Material],[Ship_To],[Drop_Ship],[Quote],[Job],[Status],[Make_Buy],[Unit_Price]," _
				& "[Discount_Pct],[Price_UofM],[Total_Price],[Deferred_Qty],[Prepaid_Amt],[Unit_Cost],[Order_Qty],[Stock_UofM],[Backorder_Qty],[Picked_Qty]," _
				& "[Shipped_Qty],[Returned_Qty],[Certs_Required],[Taxable],[Commissionable],[Commission_Pct],[Sales_Code],[Note_Text],[Promised_Date],[Last_Updated]," _
				& "[Description],[Price_Unit_Conv],[Rev],[Tax_Code],[Ext_Description],[Cost_UofM],[Cost_Unit_Conv],[Res_Type],[Res_ID],[Res_Qty],[Partial_Res]," _
				& "[Prepaid_Trade_Amt],[ObjectID],[CommissionIncluded]) "

			Dim cust As New cCustomer
			If Not cust.Load(Customer, cmd) Then
				AddError("Could Not load Customer " & Customer)
				logger.Info("Could Not load Customer " & Customer)
				Return False
			End If

			If Not String.IsNullOrEmpty(cust.TaxCode) Then
				TaxCode = cust.TaxCode
			End If

			Matl = New cMaterial With {.cmd = cmd}
			If Not Matl.Load(Material, cmd) Then
				AddError("Could Not load Material: " & Material)
				logger.Info("Could not load Material " & Material)
				Return False
			End If
			'/ the 'original' line amount:
			Dim lineamt As Double = UnitPrice * Quantity

			Dim qtyprice As Double = Matl.GetQuantityPriceForMaterial(Quantity, cmd)

			If qtyprice <> 0 Then
				If qtyprice <> UnitPrice Then
					UnitPrice = qtyprice
				End If
			End If

			Dim lineprice As Double = (Quantity * UnitPrice)
			'/ 1.0.4 - modified to check DiscountLevel to determine if customer uses discounts:
			If cust.DiscountLevel > 0 Then
				lineprice = lineprice - (lineprice * (cust.DiscountPct / 100))
			End If

			TotalLineAmount = lineprice
			If String.IsNullOrEmpty(Status) Then
				Status = "Open"
			End If

			vals = AddString(SalesOrder, DbType.String, "NULL")
			If String.IsNullOrEmpty(LineNumber) Then
				LineNumber = NextSOLineNumber(SalesOrder, cmd)
				If String.IsNullOrEmpty(LineNumber) Then
					AddError("No Sales Order Line Number available")
					Return False
				End If
			End If

			vals = vals & "," & AddString(LineNumber, DbType.String, "001")
			vals = vals & ",NULL" '& AddString(dtr("PO"), DbType.String, "NULL")
			vals = vals & ",NULL" ' AddString(dtr("Line"), DbType.String, "NULL")
			vals = vals & "," & AddString(Material, DbType.String, "NULL")
			If ShipTo = 0 Then
				If cust.DefaultShipTo <> 0 Then
					ShipTo = cust.DefaultShipTo
				Else
					logger.Info("No Ship to")
					'AddError("No Ship To")
				End If
			End If
			vals = vals & "," & AddString(ShipTo, DbType.Double, "NULL")
			vals = vals & ",0" '& AddString(Convert.ToInt32(dtr("Drop_Ship")), DbType.Double)
			vals = vals & ",NULL" '& AddString(dtr("Quote"), DbType.String, "NULL")
			vals = vals & ",NULL" '& AddString(dtr("Job"), DbType.String, "NULL")
			vals = vals & "," & AddString(Status, DbType.String, "NULL")
			If Matl.PickBuy = "P" Then
				vals = vals & "," & AddString("M")
			Else
				vals = vals & "," & AddString("B")
			End If

			vals = vals & "," & AddString(UnitPrice, DbType.Double) 'unit PRICE
			vals = vals & "," & AddString(cust.DiscountPct, DbType.Double)
			vals = vals & "," & AddString(Matl.PriceUofM, DbType.String, "NULL")
			vals = vals & "," & AddString(lineprice, DbType.Double) 'total price
			vals = vals & "," & AddString(Quantity, DbType.Double) 'deferred qty
			vals = vals & "," & AddString(0, DbType.Double) 'prepaid amt
			vals = vals & "," & AddString(UnitPrice, DbType.Double) 'unit COST
			vals = vals & "," & AddString(Quantity, DbType.Double)
			vals = vals & "," & AddString(Matl.StockUofM, DbType.String, "NULL")
			vals = vals & "," & AddString(0, DbType.Double)
			vals = vals & ",0" '& AddString(dtr("Picked_Qty"), DbType.Double)
			vals = vals & ",0" '& AddString(dtr("Shipped_Qty"), DbType.Double)
			vals = vals & ",0" '& AddString(dtr("Returned_Qty"), DbType.Double)
			vals = vals & ",0" '& AddString(Convert.ToInt32(dtr("Certs_Required")), DbType.Double)
			vals = vals & "," & AddString(Convert.ToInt32(Matl.Taxable), DbType.Double)
			If Not String.IsNullOrEmpty(cust.SalesRep) Then
				Dim emp As New cEmployee
				If Not emp.Load(cust.SalesRep, cmd) Then
					logger.Info("Could not load Sales Rep " & cust.SalesRep)
					AddError("Could not load Sales Rep " & cust.SalesRep)
					Return False
				End If
				If emp.CommissionPct > 0 Then
					vals = vals & ",1" '& AddString(Convert.ToInt32(dtr("Commissionable")), DbType.Double)
					vals = vals & "," & AddString(emp.CommissionPct, DbType.Double)
				Else
					vals = vals & ",0" '& AddString(Convert.ToInt32(dtr("Commissionable")), DbType.Double)
					vals = vals & ",0" '& AddString(dtr("Commission_Pct"), DbType.Double)
				End If

			Else
				vals = vals & ",0" '& AddString(Convert.ToInt32(dtr("Commissionable")), DbType.Double)
				vals = vals & ",0" '& AddString(dtr("Commission_Pct"), DbType.Double)
			End If
			If Not String.IsNullOrEmpty(Matl.Sales_Code) Then
				SalesCode = Matl.Sales_Code
			End If
			vals = vals & "," & AddString(SalesCode, DbType.String, "NULL")
			vals = vals & "," & AddString(Notes, DbType.String, "NULL")
			vals = vals & "," & AddString(PromisedDate, DbType.DateTime, "NULL")
			vals = vals & "," & AddString(Now, DbType.DateTime)
			vals = vals & "," & AddString(Matl.Description, DbType.String, "NULL")
			vals = vals & "," & AddString(Matl.PriceUnitConversion, DbType.Double, "NULL")
			If String.IsNullOrEmpty(Rev) Then
				vals = vals & "," & AddString(Matl.Rev, DbType.String, "NULL")
			Else
				vals = vals & "," & AddString(Rev, DbType.String, "NULL")
			End If

			vals = vals & "," & AddString(cust.TaxCode, DbType.String, "NULL")
			vals = vals & "," & AddString(Matl.ExtendedDesc, DbType.String, "NULL")
			vals = vals & "," & AddString(Matl.CostUofM, DbType.String, "NULL")
			'/ cost unit conv does not seem to change regardless of the UofM set on the line:
			vals = vals & ",1" '& AddString(dtr("Cost_Unit_Conv"), DbType.Double, "NULL")
			vals = vals & ",NULL" '& AddString(dtr("Res_Type"), DbType.Double, "NULL")
			vals = vals & ",NULL" '& AddString(dtr("Res_ID"), DbType.String, "NULL")
			vals = vals & ",NULL" '& AddString(dtr("Res_Qty"), DbType.Double, "NULL")
			vals = vals & ",0" '& AddString(Convert.ToInt32(dtr("Partial_Res")), DbType.Double, "NULL")
			vals = vals & ",0" '& AddString(dtr("Prepaid_Trade_Amt"), DbType.Double, "NULL")
			Dim newGUID As String = System.Guid.NewGuid.ToString.ToUpper
			vals = vals & "," & AddString(newGUID, DbType.String)
			vals = vals & ",0" '& AddString(Convert.ToInt32(dtr("CommissionIncluded")), DbType.Double)

			cmd.CommandText = hdr & " VALUES(" & vals & ")"
			cmd.ExecuteNonQuery()

			cmd.CommandText = "SELECT SO_Detail FROM SO_Detail WHERE SO_DetailKey=(SELECT SCOPE_IDENTITY())"
			SODetail = cmd.ExecuteScalar

			cmd.CommandText = "UPDATE tblJB_Keys SET K_KeyValue=" & SODetail & " WHERE K_Table='SO_Detail'"
			cmd.ExecuteNonQuery()

			Return True
		Catch ex As Exception
			logger.Error(ex.ToString, "AddSODetail Error")
			AddError(ex.ToString)
			If Not IsAuto Then
				MessageBox.Show(String.Format("Error in AddSODetail: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			End If
			Return False
		Finally
		End Try
	End Function

	Function GetNextSOLineNumber(SalesOrder As String, cmd As SqlCommand) As String
		Try
			Return NextSOLineNumber(SalesOrder, cmd)
		Catch ex As Exception
			logger.Error(ex.ToString, "GetNextSOLineNumber Error")
			MessageBox.Show(String.Format("Error in GetNextSOLineNumber: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			Return String.Empty
		Finally
		End Try


	End Function
	Function GetNextSOLineNumber(SalesOrder As String) As String
		Try
			Using con As New SqlConnection(JBConnection)
				con.Open()
				Using cmd As New SqlCommand
					cmd.Connection = con
					Return NextSOLineNumber(SalesOrder, cmd)
				End Using
			End Using
		Catch ex As Exception
			logger.Error(ex.ToString, "GetNextSOLineNumber Error")
			MessageBox.Show(String.Format("Error in GetNextSOLineNumber: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			Return String.Empty
		Finally
		End Try
	End Function
	Private Function NextSOLineNumber(SalesOrder As String, cmd As SqlCommand) As String
		Try
			Dim soline As Integer = 1
			Dim ok As Boolean
			Do Until ok = True
				cmd.CommandText = "SELECT COUNT(*) FROM SO_Detail WHERE Sales_Order=" & AddString(SalesOrder) & " AND SO_Line=" & AddString(soline.ToString("000"))
				If cmd.ExecuteScalar = 0 Then
					'/ didn't find the line, so we're good:
					ok = True
				Else
					soline += 1
				End If
			Loop

			Return soline.ToString("000")

		Catch ex As Exception
			logger.Error(ex.ToString, "NextSOLineNumber Error")
			MessageBox.Show(String.Format("Error in NextSOLineNumber: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			Return String.Empty
		Finally
		End Try
	End Function

	Function LoadSODetail(SODetail As Integer, cmd As SqlCommand, Optional IgnoreJobLink As Boolean = False) As Boolean
		cmd.CommandText = "SELECT * FROM SO_Detail WHERE SO_Detail=" & SODetail
		Using dt As New DataTable
			dt.Load(cmd.ExecuteReader)
			If dt.Rows.Count = 0 Then
				Return False
			Else
				Return LoadClass(dt.Rows(0), cmd)
			End If
		End Using
	End Function


End Class
