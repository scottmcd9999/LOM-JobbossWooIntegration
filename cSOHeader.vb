Imports System.Data.SqlClient
Public Class cSOHeader
	Private logger As NLog.Logger = NLog.LogManager.GetCurrentClassLogger

	Public Sub New()
		SalesOrderDetails = New List(Of cSODetail)
	End Sub

	Property SODets As List(Of Integer)
	'Public Sub New()
	'	ImportLog = New cImportLog
	'End Sub

	Property Customer As String
	Property CustomerPO As String
	Property ShipTo As Integer
	Property ShipVia As String
	Property SalesOrder As String
	Property OrderDate As Date
	Property PromisedDate As Date
	Property TotalPrice As Double
	Property Terms As String
	Property TaxCode As String
	Property SOCustomer As cCustomer
	Property SOStatus As String
	Property SalesRep As String
	Property Note As String
	Property Comment As String
	Property ShipSchedule As String
	Property DockCode As String
	Property DeliveryNumber As String
	Property ReferenceID As String
	Property cmd As SqlCommand
	Property Errors As String
	Property Remove As Boolean
	Property Overwrite As Boolean
	Property LinkedJobs As Boolean
	Property RemoveIfOnlyLinked As Boolean
	Property IsActive As Boolean
	Property IsAuto As Boolean
	Property SalesOrderDetails As List(Of cSODetail)
	'Property ImportLog As cImportLog
	Private Sub AddError(msg As String)
		If String.IsNullOrEmpty(Errors) = 0 Then
			Errors = "-- " & msg
		Else
			Errors = Errors & Environment.NewLine & "-- " & msg
		End If
	End Sub
	Function SetUserValue(UVID As Integer) As Boolean
		Try
			cmd.CommandText = "UPDATE SO_Header SET User_Values=" & UVID & " WHERE Sales_Order=" & AddString(SalesOrder)
			cmd.ExecuteNonQuery()
			Return True
		Catch ex As Exception
			logger.Error(ex.ToString, "SetUserValue Error")
			If Not IsAuto Then
				MessageBox.Show(String.Format("Error in SetUserValue: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			End If

			Return False
		Finally
		End Try
	End Function

	Private Function AddHeader(cmd As SqlCommand) As Boolean
		Try
			Dim vals As String
			Dim hdr As String = "INSERT INTO SO_Header([Sales_Order],[Customer],[Ship_To],[Contact],[Sales_Rep],[Order_Taken_By],[Ship_Via],[Tax_Code],[Terms]," _
				& "[Sales_Tax_Amt],[Sales_Tax_Rate],[Order_Date],[Promised_Date],[Customer_PO],[Status],[Total_Price],[Currency_Conv_Rate],[Trade_Currency],[Fixed_Rate]" _
				& ",[Trade_Date],[Note_Text],[Comment],[Last_Updated],[User_Values],[Source],[Prepaid_Tax_Amount]) "

			If String.IsNullOrEmpty(SalesOrder) Then
				SalesOrder = NextSONumber(cmd)
			End If

			Dim cust As New cCustomer
			If Not cust.Load(Customer, cmd) Then
				AddError("Could not load Customer")
				Return False
			End If

			Dim pref As New cPreferences With {.cmd = cmd}
			If Not pref.Load() Then
				AddError("Could not load Preferences")
				Return False
			End If

			If SOStatus Is Nothing Then
				SOStatus = "Open"
			End If

			vals = AddString(SalesOrder, DbType.String, "NULL")
			vals = vals & "," & AddString(Customer, DbType.String, "NULL")

			If ShipTo = 0 Then
				If cust.DefaultShipTo <> 0 Then
					ShipTo = cust.DefaultShipTo
				Else
					AddError("No Ship To found")
					Return False
				End If
			End If

			vals = vals & "," & AddString(ShipTo, DbType.Double, "NULL")
			vals = vals & ",NULL" '& AddString(dtr("Contact"), DbType.Double, "NULL")
			vals = vals & "," & AddString(SalesRep, DbType.String, "NULL")
			vals = vals & ",NULL" '& AddString(dtr("Order_Taken_By"), DbType.String, "NULL")
			If String.IsNullOrEmpty(ShipVia) Then
				If Not String.IsNullOrEmpty(cust.DefaultShipVia) Then
					ShipVia = cust.DefaultShipVia
				End If
			End If
			vals = vals & "," & AddString(ShipVia, DbType.String, "NULL")
			vals = vals & "," & AddString(cust.TaxCode, DbType.String, "NULL")
			vals = vals & "," & AddString(cust.Terms, DbType.String, "NULL")
			vals = vals & ",0" '& AddString(dtr("Sales_Tax_Amt"), DbType.Double)
			vals = vals & ",0" '& AddString(dtr("Sales_Tax_Rate"), DbType.Double)
			If Not OrderDate = Date.MinValue Then
				vals = vals & "," & AddString(OrderDate.ToShortDateString, DbType.Date)
			Else
				vals = vals & "," & AddString(Now.ToShortDateString, DbType.DateTime) 'order date
				OrderDate = Now.ToShortDateString
			End If
			vals = vals & "," & AddString(PromisedDate, DbType.DateTime, "NULL")
			vals = vals & "," & AddString(CustomerPO, DbType.String, "NULL")
			vals = vals & "," & AddString(SOStatus, DbType.String, "Open")
			vals = vals & ",0" '& AddString(dtr("Total_Price"), DbType.Double)
			If pref.BaseCurrency = cust.CurrencyDef Then
				vals = vals & ",1"
			Else
				Dim currdef As New cCurrencyDef
				Dim convrate As Double = currdef.ConversionRate(pref.BaseCurrency, cust.CurrencyDef, cmd)
				vals = vals & "," & AddString(convrate)
			End If
			'vals = vals & "," & AddString(dtr("Currency_Conv_Rate"), DbType.Double)
			vals = vals & "," & AddString(cust.CurrencyDef, DbType.Double, "NULL") 'dtr("Trade_Currency"), DbType.Double, "NULL")
			vals = vals & ",0" '& AddString(Convert.ToInt32(dtr("Fixed_Rate")), DbType.Double)
			vals = vals & "," & AddString(Now.ToShortDateString, DbType.DateTime, "NULL")
			vals = vals & "," & AddString(Note, DbType.String, "NULL")
			vals = vals & "," & AddString(Comment, DbType.String, "NULL")
			vals = vals & "," & AddString(Now, DbType.DateTime)
			vals = vals & ",NULL" '& uv
			vals = vals & "," & AddString("System", DbType.String, "NULL") 'Source
			vals = vals & ",0" '& AddString(dtr("Prepaid_Tax_Amount"), DbType.Double, "NULL")

			cmd.CommandText = hdr & " VALUES(" & vals & ")"
			cmd.ExecuteNonQuery()

			cmd.CommandText = "UPDATE Auto_Number SET Last_Nbr=" & AddString(SalesOrder) & " WHERE Type='SalesOrder'"
			cmd.ExecuteNonQuery()

			Return True
		Catch ex As Exception
			logger.Error(ex.ToString, "AddHeader Error")
			AddError(ex.ToString)
			If Not IsAuto Then
				MessageBox.Show(String.Format("Error in AddHeader: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			End If

			Return False
		Finally
		End Try
	End Function
	Function AddSalesOrderHeader(cmd As SqlCommand) As Boolean
		Try
			Return AddHeader(cmd)
		Catch ex As Exception
			logger.Error(ex.ToString, "AddSOHeader Error")
			AddError(ex.ToString)
			If Not IsAuto Then
				MessageBox.Show(String.Format("Error in AddSOHeader: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			End If

			Return False
		Finally
		End Try
	End Function
	Function AddSalesOrderHeader() As Boolean
		Try
			Using con As New SqlConnection(JBConnection)
				con.Open()
				Using trn As SqlTransaction = con.BeginTransaction
					Using cmd As New SqlCommand
						cmd.Connection = con
						cmd.Transaction = trn
						Return AddHeader(cmd)
					End Using
				End Using
			End Using
		Catch ex As Exception
			logger.Error(ex.ToString, "AddSOHeader Error")
			AddError(ex.ToString)
			If Not IsAuto Then
				MessageBox.Show(String.Format("Error in AddSOHeader: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			End If

			Return False
		Finally
		End Try
	End Function

	'Private Function FindSO(Customer As String, PO As String, Material As String, Status As String, ShipTo As Integer, cmd As SqlCommand) As String
	'	Try
	'		'cmd.CommandText = "SELECT soh.Sales_Order FROM SO_Header soh INNER JOIN SO_Detail sod ON soh.Sales_Order=sod.Sales_Order" _
	'		'	& " WHERE soh.Customer=" & AddString(Customer) & " And Customer_PO=" & AddString(PO) & " AND soh.Status='Open'" _
	'		'	& " AND sod.Material=" & AddString(Material)

	'		If String.IsNullOrEmpty(Material) Then
	'			cmd.CommandText = "SELECT DISTINCT Sales_Order FROM SO_Header AS soh" _
	'				& " WHERE Customer =" & AddString(Customer) & " AND " _
	'				& " Customer_PO = " & AddString(PO) & " AND Status =" & AddString(Status) ' 'Open'"
	'		Else
	'			cmd.CommandText = "SELECT DISTINCT Sales_Order FROM SO_Header AS soh" _
	'						& " INNER JOIN (SELECT Sales_Order AS SODSO FROM SO_Detail WHERE Material=" & AddString(Material) & ") AS sod" _
	'						& " ON soh.Sales_Order=sod.SODSO WHERE Customer =" & AddString(Customer) & " AND " _
	'						& " Customer_PO = " & AddString(PO) & " AND Status =" & AddString(Status) ' 'Open'"
	'		End If

	'		Using dt As New DataTable
	'			dt.Load(cmd.ExecuteReader)
	'			If dt.Rows.Count > 0 Then
	'				Me.SalesOrder = dt.Rows(0).Item("Sales_Order")
	'				Me.Customer = Customer
	'				Me.CustomerPO = PO
	'				Return SalesOrder
	'			Else
	'				Return String.Empty
	'			End If
	'		End Using
	'	Catch ex As Exception
	'		logger.Error(ex.ToString, "FindSO Error")
	'		AddError(ex.ToString)
	'		If Not IsAuto Then
	'			MessageBox.Show(String.Format("Error in FindSO: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
	'		End If

	'		Return "ERROR"
	'	Finally
	'	End Try
	'End Function

	'Function FindForecastSalesOrderMatch(Customer As String, PO As String, Status As String, cmd As SqlCommand) As String
	'	Try
	'		Return FindSO(Customer, PO, "", Status, 0, cmd)

	'	Catch ex As Exception
	'		logger.Error(ex.ToString, "FindSalesOrderMatch Error")
	'		AddError(ex.ToString)
	'		If Not IsAuto Then
	'			MessageBox.Show(String.Format("Error in FindSalesOrderMatch: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
	'		End If

	'		Return String.Empty
	'	Finally
	'	End Try
	'End Function
	'Function FindSalesOrderMatch(Customer As String, PO As String, Material As String, Status As String, ShipTo As Integer, cmd As SqlCommand) As String
	'	Try
	'		Return FindSO(Customer, PO, Material, Status, ShipTo, cmd)

	'	Catch ex As Exception
	'		logger.Error(ex.ToString, "FindSalesOrderMatch Error")
	'		If Not IsAuto Then
	'			MessageBox.Show(String.Format("Error in FindSalesOrderMatch: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
	'		End If

	'		Return String.Empty
	'	Finally
	'	End Try
	'End Function
	'Function FindSalesOrderMatch(Customer As String, PO As String, Material As String, ShipTo As Integer, cmd As SqlCommand) As String
	'	Try
	'		Return FindSO(Customer, PO, Material, "OPEN", ShipTo, cmd)

	'	Catch ex As Exception
	'		logger.Error(ex.ToString, "FindSalesOrderMatch Error")
	'		MessageBox.Show(String.Format("Error in FindSalesOrderMatch: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
	'		Return String.Empty
	'	Finally
	'	End Try
	'End Function
	'Function FindSalesOrderMatch(Customer As String, PO As String, Material As String, ShipTo As Integer) As String
	'	Try
	'		Using con As New SqlConnection(JBConnection)
	'			con.Open()
	'			Using cmd As New SqlCommand
	'				cmd.Connection = con
	'				Return FindSO(Customer, PO, Material, "OPEN", ShipTo, cmd)
	'			End Using
	'		End Using
	'	Catch ex As Exception
	'		logger.Error(ex.ToString, "FindSalesOrderMatch Error")
	'		MessageBox.Show(String.Format("Error in FindSalesOrderMatch: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
	'		Return String.Empty
	'	Finally
	'	End Try
	'End Function
	'Function CleanForecastSO(cmd As SqlCommand, Material As String) As Boolean
	'	Try
	'		cmd.CommandText = "DELETE FROM Delivery WHERE SO_Detail IN (SELECT SO_Detail FROM SO_Detail WHERE Sales_Order=" & AddString(SalesOrder) & ")"
	'		cmd.ExecuteNonQuery()
	'		cmd.CommandText = "DELETE FROM Tax_Detail WHERE Owner_Type IN (0,2,3,4) AND Owner_ID IN (SELECT SO_Detail FROM SO_Detail WHERE Sales_Order=" & AddString(SalesOrder) & ")"
	'		cmd.ExecuteNonQuery()
	'		cmd.CommandText = "DELETE FROM SO_Detail WHERE SO_Detail IN (SELECT SO_Detail FROM SO_Detail WHERE Sales_Order=" & AddString(SalesOrder) & ")"
	'		cmd.ExecuteNonQuery()
	'		Return True
	'	Catch ex As Exception
	'		logger.Error(ex.ToString, "CleanForecastSO Error")
	'		MessageBox.Show(String.Format("Error in CleanForecastSO: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
	'	Finally
	'	End Try
	'End Function
	'Private Function CleanSO(cmd As SqlCommand, mats As List(Of String), RemoveInactive As Boolean, CloseActive As Boolean) As Boolean
	'	Try
	'		SODets = New List(Of Integer)
	'		If CloseActive Then
	'			With ImportLog
	'				.Action = "Active Line Closed"
	'			End With
	'			If Not mats Is Nothing Then
	'				If mats.Count > 0 Then
	'					'/ we only have certain materials to remove/update:
	'					'/ below are ACTIVE LINES:
	'					For i As Integer = 0 To mats.Count - 1
	'						If RemoveIfOnlyLinked Then
	'							cmd.CommandText = "SELECT * FROM SO_Detail WHERE (Picked_Qty<>0 OR Returned_Qty<>0 OR PO IS NOT NULL)" _
	'								 & " AND Sales_Order=" & AddString(SalesOrder) & " AND Material=" & AddString(mats(i))
	'						Else
	'							cmd.CommandText = "SELECT * FROM SO_Detail WHERE (Picked_Qty<>0 OR Returned_Qty<>0 OR Job IS NOT NULL OR Quote IS NOT NULL OR PO IS NOT NULL)" _
	'								 & " AND Sales_Order=" & AddString(SalesOrder) & " AND Material=" & AddString(mats(i))
	'						End If

	'						Using dt As New DataTable
	'							dt.Load(cmd.ExecuteReader)
	'							For Each dtr As DataRow In dt.Rows
	'								logger.Info("Updating " & dtr("SO_Detail"))
	'								With ImportLog
	'									.SODetail = dtr("SO_Detail")
	'									.SalesOrder = dtr("Sales_Order")
	'									.OrderDate = OrderDate
	'									.ShipTo = dtr("Ship_To")
	'									.Material = dtr("Material")
	'									.Quantity = dtr("Order_Qty")
	'									.PromisedDate = dtr("Promised_Date")
	'									.Rev = dtr("Rev") & ""
	'									.Status = dtr("Status") & ""
	'									.UnitPrice = dtr("Unit_Price")
	'									.LineNumber = dtr("SO_Line") & ""
	'									If Not IsDBNull(dtr("Job")) Then
	'										.AdditionalInfo = dtr("Job") & ""
	'										LinkedJobs = True
	'									End If
	'									.AddImportLog(cmd)
	'								End With
	'								'cmd.CommandText = "UPDATE SO_Detail SET Status='Closed', Deferred_Qty=0, Backorder_Qty=0, Last_Updated=" & AddString(Now, DbType.DateTime) _
	'								'												& " WHERE (Picked_Qty<>0 OR Returned_Qty<>0 OR Job IS NOT NULL OR Quote IS NOT NULL OR PO IS NOT NULL)" _
	'								'												& " AND Sales_Order=" & AddString(SalesOrder) & " AND Material=" & AddString(mats(i))
	'								cmd.CommandText = "UPDATE SO_Detail SET Status='Closed', Deferred_Qty=0, Last_Updated=" & AddString(Now, DbType.DateTime) _
	'									& " WHERE SO_Detail=" & dtr("SO_Detail")
	'								cmd.ExecuteNonQuery()
	'								cmd.CommandText = "UPDATE Delivery SET Remaining_Qty=0,Last_Updated=" & AddString(Now, DbType.DateTime) _
	'									& " WHERE SO_Detail=" & dtr("SO_Detail")
	'								cmd.ExecuteNonQuery()
	'							Next
	'						End Using
	'					Next
	'				Else
	'					If RemoveIfOnlyLinked Then
	'						cmd.CommandText = "SELECT * FROM SO_Detail WHERE (Picked_Qty<>0 OR Returned_Qty<>0 OR PO IS NOT NULL)" _
	'							& " AND Sales_Order=" & AddString(SalesOrder)
	'					Else
	'						cmd.CommandText = "SELECT * FROM SO_Detail WHERE (Picked_Qty<>0 OR Returned_Qty<>0 OR Job IS NOT NULL OR Quote IS NOT NULL OR PO IS NOT NULL)" _
	'							& " AND Sales_Order=" & AddString(SalesOrder)
	'					End If

	'					Using dt As New DataTable
	'						dt.Load(cmd.ExecuteReader)
	'						For Each dtr As DataRow In dt.Rows
	'							logger.Info("Updating " & dtr("SO_Detail"))
	'							With ImportLog
	'								.SODetail = dtr("SO_Detail")
	'								.SalesOrder = dtr("Sales_Order")
	'								.OrderDate = OrderDate
	'								.ShipTo = dtr("Ship_To")
	'								.Material = dtr("Material")
	'								.Quantity = dtr("Order_Qty")
	'								.PromisedDate = dtr("Promised_Date")
	'								.Rev = dtr("Rev") & ""
	'								.Status = dtr("Status") & ""
	'								.UnitPrice = dtr("Unit_Price")
	'								.LineNumber = dtr("SO_Line") & ""
	'								If Not IsDBNull(dtr("Job")) Then
	'									.AdditionalInfo = dtr("Job") & ""
	'									LinkedJobs = True
	'								End If
	'								.AddImportLog(cmd)
	'							End With
	'							'cmd.CommandText = "UPDATE SO_Detail SET Status='Closed', Deferred_Qty=0, Backorder_Qty=0, Last_Updated=" & AddString(Now, DbType.DateTime) _
	'							'												& " WHERE (Picked_Qty<>0 OR Returned_Qty<>0 OR Job IS NOT NULL OR Quote IS NOT NULL OR PO IS NOT NULL)" _
	'							'												& " AND Sales_Order=" & AddString(SalesOrder)
	'							cmd.CommandText = "UPDATE SO_Detail SET Status='Closed', Deferred_Qty=0, Backorder_Qty=0, Last_Updated=" & AddString(Now, DbType.DateTime) _
	'								& " WHERE SO_Detail=" & dtr("SO_Detail")

	'							cmd.ExecuteNonQuery()
	'							cmd.CommandText = "UPDATE Delivery SET Remaining_Qty=0,Last_Updated=" & AddString(Now, DbType.DateTime) _
	'								& " WHERE SO_Detail=" & dtr("SO_Detail")
	'							cmd.ExecuteNonQuery()
	'						Next
	'					End Using
	'				End If
	'			Else
	'				If RemoveIfOnlyLinked Then
	'					cmd.CommandText = "SELECT * FROM SO_Detail" _
	'						& " WHERE (Picked_Qty<>0 OR Returned_Qty<>0 OR PO IS NOT NULL)" _
	'						& " AND Sales_Order=" & AddString(SalesOrder)
	'				Else
	'					cmd.CommandText = "SELECT * FROM SO_Detail" _
	'						& " WHERE (Picked_Qty<>0 OR Returned_Qty<>0 OR Job IS NOT NULL OR Quote IS NOT NULL OR PO IS NOT NULL)" _
	'						& " AND Sales_Order=" & AddString(SalesOrder)
	'				End If

	'				Using dt As New DataTable
	'					dt.Load(cmd.ExecuteReader)

	'					For Each dtr As DataRow In dt.Rows
	'						logger.Info("Updating " & dtr("SO_Detail"))
	'						With ImportLog
	'							.SODetail = dtr("SO_Detail")
	'							.SalesOrder = dtr("Sales_Order")
	'							.OrderDate = OrderDate
	'							.ShipTo = dtr("Ship_To")
	'							.Material = dtr("Material")
	'							.Quantity = dtr("Order_Qty")
	'							.PromisedDate = dtr("Promised_Date")
	'							.Rev = dtr("Rev") & ""
	'							.Status = dtr("Status") & ""
	'							.UnitPrice = dtr("Unit_Price")
	'							.LineNumber = dtr("SO_Line") & ""
	'							If Not IsDBNull(dtr("Job")) Then
	'								.AdditionalInfo = dtr("Job") & ""
	'								LinkedJobs = True
	'							End If
	'							.AddImportLog(cmd)
	'						End With
	'						cmd.CommandText = "UPDATE SO_Detail SET Status='Closed', Deferred_Qty=0, Backorder_Qty=0, Last_Updated=" & AddString(Now, DbType.DateTime) _
	'							& " WHERE SO_Detail=" & dtr("SO_Detail")
	'						'(Picked_Qty<>0 Or Returned_Qty<>0 Or Job Is Not NULL Or Quote Is Not NULL Or PO Is Not NULL)" _
	'						'& " And Sales_Order=" & AddString(SalesOrder)
	'						cmd.ExecuteNonQuery()
	'					Next
	'				End Using
	'			End If
	'		End If
	'		'/ below are INACTIVE lines:
	'		If RemoveInactive Then
	'			ImportLog.Action = "Inactive Line Removed"
	'			If Not mats Is Nothing Then
	'				If mats.Count > 0 Then
	'					'logger.Info("Removing Material " & mats(0).ToString)
	'					For i As Integer = 0 To mats.Count - 1
	'						If RemoveIfOnlyLinked Then
	'							cmd.CommandText = "SELECT * FROM SO_Detail WHERE Sales_Order=" & AddString(SalesOrder) _
	'								& " AND Status IN ('Open','Hold') AND Picked_Qty=0 AND Returned_Qty=0 AND PO IS NULL) AND Material=" & AddString(mats(i))
	'						Else
	'							cmd.CommandText = "SELECT * FROM SO_Detail WHERE Sales_Order=" & AddString(SalesOrder) _
	'								& " AND Status IN ('Open','Hold') AND Picked_Qty=0 AND Returned_Qty=0 AND Job IS NULL AND Quote IS NULL AND PO IS NULL) AND Material=" & AddString(mats(i))
	'						End If

	'						Using dt As New DataTable
	'							dt.Load(cmd.ExecuteReader)
	'							For Each dtr As DataRow In dt.Rows
	'								logger.Info("Removing " & dtr("SO_Detail"))
	'								With ImportLog
	'									.SODetail = dtr("SO_Detail")
	'									.SalesOrder = dtr("Sales_Order")
	'									.Customer = Customer
	'									.OrderDate = OrderDate
	'									If Not IsDBNull(dtr("Ship_To")) Then
	'										.ShipTo = dtr("Ship_To")
	'									End If

	'									.Material = dtr("Material")
	'									.Quantity = dtr("Order_Qty")
	'									If Not IsDBNull(dtr("Promised_Date")) Then
	'										.PromisedDate = dtr("Promised_Date")
	'									End If

	'									.Rev = dtr("Rev") & ""
	'									.Status = dtr("Status") & "'"
	'									.UnitPrice = dtr("Unit_Price")
	'									.LineNumber = dtr("SO_Line") & ""
	'									If Not IsDBNull(dtr("Job")) Then
	'										.AdditionalInfo = dtr("Job") & ""
	'										LinkedJobs = True
	'									End If
	'									.AddImportLog(cmd)
	'								End With
	'								'		cmd.CommandText = "DELETE FROM SO_Detail WHERE SO_Detail IN (SELECT SO_Detail FROM SO_Detail WHERE Sales_Order=" & AddString(SalesOrder) _
	'								'& " AND Status IN ('Open','Hold') AND Picked_Qty=0 AND Returned_Qty=0 AND Job IS NULL AND Quote IS NULL AND PO IS NULL) AND Material=" & AddString(mats(i))
	'								'logger.Info(cmd.CommandText)
	'								cmd.CommandText = "DELETE FROM SO_Detail WHERE SO_Detail=" & dtr("SO_Detail")
	'								cmd.ExecuteNonQuery()
	'								cmd.CommandText = "DELETE FROM Delivery WHERE SO_Detail =" & dtr("SO_Detail")
	'								cmd.ExecuteNonQuery()
	'								SODets.Add(dtr("SO_Detail"))
	'								'cmd.CommandText = "DELETE FROM Tax_Detail WHERE Tax_Detail IN (SELECT Tax_Detail FROM (SELECT CASE WHEN ISNUMERIC(Owner_ID)=1 THEN Owner_ID" _
	'								'	& " ELSE 0 END AS OwnerID FROM Tax_Detail) AS TD WHERE Tax_Detail.Owner_Type IN (0,2,3,4) And TD.OwnerID=" & dtr("SO_Detail") & ")"
	'								'cmd.ExecuteNonQuery()
	'							Next
	'						End Using
	'					Next
	'				End If
	'			Else
	'				'/ no materials were passed in, so we just delete all inactive lines:
	'				If RemoveIfOnlyLinked Then
	'					cmd.CommandText = "SELECT * FROM SO_Detail WHERE Sales_Order=" & AddString(SalesOrder) _
	'						& " AND Status IN ('Open','Hold') AND Picked_Qty=0 AND Returned_Qty=0 AND PO IS NULL"
	'				Else
	'					cmd.CommandText = "SELECT * FROM SO_Detail WHERE Sales_Order=" & AddString(SalesOrder) _
	'						& " AND Status IN ('Open','Hold') AND Picked_Qty=0 AND Returned_Qty=0 AND Job IS NULL AND Quote IS NULL AND PO IS NULL"
	'				End If

	'				Using dt As New DataTable
	'					dt.Load(cmd.ExecuteReader)
	'					For Each dtr As DataRow In dt.Rows
	'						logger.Info("Removing " & dtr("SO_Detail"))
	'						With ImportLog
	'							.SODetail = dtr("SO_Detail")
	'							.SalesOrder = dtr("Sales_Order")
	'							.OrderDate = OrderDate
	'							If Not IsDBNull(dtr("Ship_To")) Then
	'								.ShipTo = dtr("Ship_To")
	'							End If
	'							'.ShipTo = dtr("Ship_To")
	'							.Material = dtr("Material")
	'							.Quantity = dtr("Order_Qty")
	'							If Not IsDBNull(dtr("Promised_Date")) Then
	'								.PromisedDate = dtr("Promised_Date")
	'							End If
	'							.Rev = dtr("Rev") & ""
	'							.Status = dtr("Status") & ""
	'							If Not IsDBNull(dtr("Unit_Price")) Then
	'								.UnitPrice = dtr("Unit_Price")
	'							End If

	'							.LineNumber = dtr("SO_Line") & ""
	'							If Not IsDBNull(dtr("Job")) Then
	'								.AdditionalInfo = dtr("Job") & ""
	'								LinkedJobs = True
	'							End If
	'							.AddImportLog(cmd)
	'						End With

	'						cmd.CommandText = "DELETE FROM SO_Detail WHERE SO_Detail=" & dtr("SO_Detail")
	'						'logger.Info(cmd.CommandText)
	'						cmd.ExecuteNonQuery()
	'						cmd.CommandText = "DELETE FROM Delivery WHERE SO_Detail =" & dtr("SO_Detail")
	'						cmd.ExecuteNonQuery()
	'						SODets.Add(dtr("SO_Detail"))

	'						'cmd.CommandText = "DELETE FROM Tax_Detail WHERE Tax_Detail IN (SELECT Tax_Detail FROM (SELECT CASE WHEN ISNUMERIC(Owner_ID)=1 THEN Owner_ID" _
	'						'	& " ELSE 0 END AS OwnerID FROM Tax_Detail) AS TD WHERE Tax_Detail.Owner_Type IN (0,2,3,4) And TD.OwnerID=" & dtr("SO_Detail") & ")"
	'						'cmd.ExecuteNonQuery()
	'					Next
	'				End Using
	'			End If
	'			'cmd.CommandText = "DELETE FROM SO_Detail WHERE SO_Detail IN (SELECT SO_Detail FROM SO_Detail WHERE Sales_Order=" & AddString(SalesOrder) _
	'			'							& " And Status IN ('Open','Hold') AND Picked_Qty=0 AND Returned_Qty=0 AND Job IS NULL AND Quote IS NULL AND PO IS NULL)"
	'			'		'logger.Info(cmd.CommandText)
	'			'		cmd.ExecuteNonQuery()
	'			'	End If
	'			'End If

	'			'/ clean up deliveries and tax lines. Note in above code where Detail lines are removed, we also remove Delivery and Tax Detail records.
	'			'/ this section serves to remove any extraneous lines as needed.
	'			cmd.CommandText = "DELETE FROM Delivery WHERE SO_Detail IN (SELECT SO_Detail FROM SO_Detail WHERE Sales_Order=" & AddString(SalesOrder) _
	'				& " And Status IN ('Open','Hold') AND Picked_Qty=0 AND Returned_Qty=0 AND Job IS NULL AND Quote IS NULL AND PO IS NULL)"
	'			cmd.ExecuteNonQuery()

	'			cmd.CommandText = "DELETE FROM Tax_Detail WHERE Owner_Type IN (0,2,3,4) AND Owner_ID IN (SELECT SO_Detail FROM SO_Detail WHERE Sales_Order=" & AddString(SalesOrder) _
	'					& " AND Status IN ('Open','Hold') AND Picked_Qty=0 AND Returned_Qty=0 AND Job IS NULL AND Quote IS NULL AND PO IS NULL)"
	'			cmd.ExecuteNonQuery()
	'		End If
	'		Return True
	'	Catch ex As Exception
	'		logger.Error(ex.ToString, "CleanSO Error")
	'		MessageBox.Show(String.Format("Error in CleanSO: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
	'		Return False
	'	Finally
	'	End Try
	'End Function

	'Function CleanSalesOrder(Optional Mats As List(Of String) = Nothing, Optional RemoveInactive As Boolean = False, Optional CloseActive As Boolean = False) As Boolean
	'	Try
	'		Using con As New SqlConnection(JBConnection)
	'			con.Open()
	'			Using cmd As New SqlCommand
	'				cmd.Connection = con
	'				Return CleanSO(cmd, Mats, RemoveInactive, CloseActive)
	'			End Using
	'		End Using
	'	Catch ex As Exception
	'		logger.Error(ex.ToString, "CleanSalesOrder Error")
	'		MessageBox.Show(String.Format("Error in CleanSalesOrder: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
	'		Return False
	'	Finally
	'	End Try

	'End Function

	'Function CleanSalesOrder(cmd As SqlCommand, Optional Mats As List(Of String) = Nothing, Optional RemoveInactive As Boolean = False, Optional CloseActive As Boolean = False) As Boolean
	'	Try
	'		Return CleanSO(cmd, Mats, RemoveInactive, CloseActive)
	'	Catch ex As Exception
	'		logger.Error(ex.ToString, "CleanSalesOrder Error")
	'		MessageBox.Show(String.Format("Error in CleanSalesOrder: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
	'		Return False
	'	Finally
	'	End Try
	'End Function

	'Private Function CloseSalesOrder(SalesORder As String, cmd As SqlCommand) As Boolean
	'	Try
	'		cmd.CommandText = "UPDATE SO_Detail SET Status='Closed', Deferred_Qty=0, Backorder_Qty=0, Last_Updated=" & AddString(Now, DbType.DateTime) _
	'					& " WHERE Sales_Order=" & AddString(SalesORder)
	'		cmd.ExecuteNonQuery()
	'		cmd.CommandText = "UPDATE Delivery SET Remaining_Quantity=0, Last_Updated=" & AddString(Now, DbType.DateTime) _
	'			& " WHERE SO_Detail IN (SELECT SO_Detail FROM SO_Detail WHERE Sales_Order=" & AddString(SalesORder) & ")"
	'		cmd.ExecuteNonQuery()
	'		cmd.CommandText = "UPDATE SO_Header SET Status='Closed',Last_Updated=" & AddString(Now, DbType.DateTime) & " WHERE Sales_Order=" & AddString(SalesORder)
	'		cmd.ExecuteNonQuery()
	'		Dim log As New cImportLog
	'		With log
	'			.SalesOrder = SalesORder
	'			.Action = "Closed"
	'			.AddImportLog(cmd)
	'		End With
	'		Return True
	'	Catch ex As Exception
	'		logger.Error(ex.ToString, "CloseSalesOrder Error")
	'		MessageBox.Show(String.Format("Error in CloseSalesOrder: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
	'		Return False
	'	Finally
	'	End Try
	'End Function
	'Function CloseOrder(SalesOrder As String, cmd As SqlCommand) As Boolean
	'	Return CloseSalesOrder(SalesOrder, cmd)
	'End Function
	'Function CloseOrder(SalesOrder As String) As Boolean
	'	Using con As New SqlConnection(JBConnection)
	'		con.Open()
	'		Using cmd As New SqlCommand
	'			cmd.Connection = con
	'			Return CloseSalesOrder(SalesOrder, cmd)
	'		End Using
	'	End Using
	'End Function
	'Function RemoveOrder(SalesOrder As String, cmd As SqlCommand) As Boolean
	'	Return RemoveSalesOrder(SalesOrder, cmd)
	'End Function
	'Function RemoveOrder(SalesOrder As String) As Boolean
	'	Using con As New SqlConnection(JBConnection)
	'		con.Open()
	'		Using cmd As New SqlCommand
	'			cmd.Connection = con
	'			Return RemoveSalesOrder(SalesOrder, cmd)
	'		End Using
	'	End Using
	'End Function
	'Private Function RemoveSalesOrder(SalesOrder As String, cmd As SqlCommand) As Boolean
	'	Try
	'		'/ close out any deliveries associated with the Sales Order
	'		cmd.CommandText = "UPDATE Delivery SET Remaining_Quantity=0, Last_Updated=" & AddString(Now, DbType.DateTime) _
	'				& " WHERE SO_Detail IN (SELECT SO_Detail FROM SO_Detail WHERE Sales_Order=" & AddString(SalesOrder) & ")"
	'		cmd.ExecuteNonQuery()
	'		cmd.CommandText = "DELETE FROM SO_Detail WHERE Sales_Order=" & AddString(SalesOrder)
	'		cmd.ExecuteNonQuery()
	'		cmd.CommandText = "DELETE FROM SO_Header WHERE Sales_Order=" & AddString(SalesOrder)
	'		cmd.ExecuteNonQuery()
	'		Return True
	'	Catch ex As Exception
	'		logger.Error(ex.ToString, "SalesOrderImport.cSOHeader.RemoveSalesOrder Error")
	'		MessageBox.Show(String.Format("Error in SalesOrderImport.cSOHeader.RemoveSalesOrder: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
	'		Return False
	'	Finally

	'	End Try
	'End Function

	'Private Function SetOrderDate(OrderDate As Date, cmd As SqlCommand) As Boolean
	'	Try
	'		cmd.CommandText = "UPDATE SO_Header SET Order_Date=" & AddString(OrderDate, DbType.Date) & " WHERE Sales_Order=" & AddString(SalesOrder)
	'		cmd.ExecuteNonQuery()
	'		Return True
	'	Catch ex As Exception
	'		logger.Error(ex.ToString, "SetOrderDate Error")
	'		MessageBox.Show(String.Format("Error in SetOrderDate: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
	'		Return False
	'	Finally
	'	End Try
	'End Function

	'Function SetSOOrderDate(OrderDate As Date) As Boolean
	'	Try
	'		Using con As New SqlConnection(JBConnection)
	'			con.Open()
	'			Using cmd As New SqlCommand
	'				cmd.Connection = con
	'				Return SetOrderDate(OrderDate, cmd)
	'			End Using
	'		End Using
	'	Catch ex As Exception
	'		logger.Error(ex.ToString, "SetSOOrderDate Error")
	'		MessageBox.Show(String.Format("Error in SetSOOrderDate: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
	'		Return False
	'	Finally
	'	End Try
	'End Function

	'Function SetSOOrderDate(OrderDate As Date, cmd As SqlCommand)
	'	Try
	'		Return SetOrderDate(OrderDate, cmd)
	'	Catch ex As Exception
	'		logger.Error(ex.ToString, "SetSOOrderDate Error")
	'		MessageBox.Show(String.Format("Error in SetSOOrderDate: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
	'		Return False
	'	Finally
	'	End Try
	'End Function

	'Private Function SetPromDate(PromDate As Date, cmd As SqlCommand) As Boolean
	'	Try
	'		cmd.CommandText = "UPDATE SO_Header SET Promised_Date=" & AddString(PromDate, DbType.Date) & " WHERE Sales_Order=" & AddString(SalesOrder)
	'		cmd.ExecuteNonQuery()
	'		Return True
	'	Catch ex As Exception
	'		logger.Error(ex.ToString, "SetPromDate Error")
	'		MessageBox.Show(String.Format("Error in SetPromDate: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
	'		Return True
	'	Finally
	'	End Try

	'End Function

	'Function SetSOPromisedDate(PromisedDate As Date)
	'	Try
	'		Using con As New SqlConnection(JBConnection)
	'			con.Open()
	'			Using cmd As New SqlCommand
	'				cmd.Connection = con
	'				Return SetPromDate(PromisedDate, cmd)
	'			End Using
	'		End Using
	'	Catch ex As Exception
	'		logger.Error(ex.ToString, "SetSOPromisedDate Error")
	'		MessageBox.Show(String.Format("Error in SetSOPromisedDate: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
	'		Return False
	'	Finally
	'	End Try
	'End Function
	'Function SetSOPromisedDate(PromisedDate As Date, cmd As SqlCommand) As Boolean
	'	Try
	'		Return SetPromDate(PromisedDate, cmd)
	'		Return True
	'	Catch ex As Exception
	'		logger.Error(ex.ToString, "SetSOPromisedDate Error")
	'		MessageBox.Show(String.Format("Error in SetSOPromisedDate: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
	'		Return False
	'	Finally
	'	End Try
	'End Function

	Private Function SetSOTotal(OrderTotal As Double, cmd As SqlCommand)
		Try
			cmd.CommandText = "UPDATE SO_Header SET Total_Price=" & OrderTotal & ", Last_Updated=" & AddString(Now, DbType.DateTime) & " WHERE Sales_Order=" & AddString(SalesOrder)
			cmd.ExecuteNonQuery()
			Return True
		Catch ex As Exception
			logger.Error(ex.ToString, "SetSOTotal Error")
			MessageBox.Show(String.Format("Error in SetSOTotal: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			Return False
		Finally
		End Try
	End Function
	Function SetOrderTotal(OrderTotal As Double) As Boolean
		Try
			Using con As New SqlConnection(JBConnection)
				con.Open()
				Using cmd As New SqlCommand
					cmd.Connection = con
					Return SetSOTotal(OrderTotal, cmd)
				End Using
			End Using
		Catch ex As Exception
			logger.Error(ex.ToString, "SetOrderTotal Error")
			MessageBox.Show(String.Format("Error in SetOrderTotal: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			Return False
		Finally
		End Try
	End Function
	Function SetOrderTotal(OrderTotal As Double, cmd As SqlCommand)
		Try
			Return SetSOTotal(OrderTotal, cmd)
		Catch ex As Exception
			logger.Error(ex.ToString, "SetOrderTotal Error")
			MessageBox.Show(String.Format("Error in SetOrderTotal: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			Return False
		Finally
		End Try
	End Function

	Private Function SetSOTaxamt(TaxAmount As Double, cmd As SqlCommand)
		Try
			cmd.CommandText = "UPDATE SO_Header SET Sales_Tax_Amt=" & TaxAmount & ", Last_Updated=" & AddString(Now, DbType.DateTime) & " WHERE Sales_Order=" & AddString(SalesOrder)
			cmd.ExecuteNonQuery()
			Return True
		Catch ex As Exception
			logger.Error(ex.ToString, "SetSOTaxamt Error")
			MessageBox.Show(String.Format("Error in SetSOTaxamt: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			Return False
		Finally
		End Try
	End Function
	Function SetTaxAmount(TaxAmount As Double, cmd As SqlCommand)
		Try
			Return SetSOTaxamt(TaxAmount, cmd)
		Catch ex As Exception
			logger.Error(ex.ToString, "SetTaxAmount Error")
			MessageBox.Show(String.Format("Error in SetTaxAmount: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			Return False
		Finally
		End Try
	End Function
	Function SetTaxAmount(TaxAmount As Double)
		Try
			Using con As New SqlConnection(JBConnection)
				con.Open()
				Using cmd As New SqlCommand
					cmd.Connection = con
					Return SetSOTaxamt(TaxAmount, cmd)
				End Using
			End Using
		Catch ex As Exception
			logger.Error(ex.ToString, "SetTaxAmount Error")
			MessageBox.Show(String.Format("Error in SetTaxAmount: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			Return False
		Finally
		End Try

	End Function
	Function LoadSalesOrder(SalesOrder As String, cmd As SqlCommand) As Boolean
		Me.SalesOrder = SalesOrder
		Me.cmd = cmd
		cmd.CommandText = "SELECT * FROM SO_Header WHERE Sales_Order=" & AddString(SalesOrder)
		Using dt As New DataTable
			dt.Load(cmd.ExecuteReader)
			If dt.Rows.Count > 0 Then
				Return LoadClass(dt.Rows(0))
			Else
				Return False
			End If
		End Using
	End Function
	Private Function LoadClass(dtr As DataRow) As Boolean
		Try
			'logger.Info("Ship To:" & dtr("Ship_To"))
			Customer = dtr("Customer") & ""
			CustomerPO = dtr("Customer_PO") & ""
			If Not IsDBNull(dtr("Ship_To")) Then
				ShipTo = dtr("Ship_To")
			End If
			ShipVia = dtr("Ship_Via") & ""
			OrderDate = dtr("Order_Date")
			Terms = dtr("Terms") & ""
			TaxCode = dtr("Tax_Code") & ""
			SOStatus = dtr("Status")

			'/ number of Active lines:
			cmd.CommandText = "SELECT COUNT(*) FROM SO_Detail WHERE (Sales_Order=" & AddString(dtr("Sales_Order")) _
				& " AND (Status='Open' AND (Job IS NOT NULL OR PO IS NOT NULL" _
				& " OR Picked_Qty<>0 OR Returned_Qty<>0))) OR (Sales_Order=" & AddString(dtr("Sales_Order")) _
				& " AND (Status='Hold' AND (Job IS NOT NULL OR PO IS NOT NULL" _
				& " OR Picked_Qty<>0 OR Returned_Qty<>0)))"

			IsActive = cmd.ExecuteScalar > 0

			SOCustomer = New cCustomer
			SOCustomer.cmd = cmd
			If Not SOCustomer.Load(Me.Customer, cmd) Then
				Return False
			End If

			cmd.CommandText = "SELECT SO_Detail FROM SO_Detail WHERE Sales_Order=" & AddString(SalesOrder)
			Using dt As New DataTable
				dt.Load(cmd.ExecuteReader)
				For Each dtrd As DataRow In dt.Rows
					Dim sod As New cSODetail
					If sod.LoadSODetail(dtrd("SO_Detail"), cmd) Then
						SalesOrderDetails.Add(sod)
					End If
				Next
			End Using

			Return True
		Catch ex As Exception
			logger.Error(ex.ToString, "LoadClass Error")
			MessageBox.Show(String.Format("Error in LoadClass: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			Return False
		Finally
		End Try

	End Function
End Class
