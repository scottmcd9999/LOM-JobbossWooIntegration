Imports System.Data.SqlClient
Public Class cCustomer
	Private logger As NLog.Logger = NLog.LogManager.GetCurrentClassLogger

	Event MissingData()
	Event Saved()
	Event Loaded()
	Event DeleteFail()
	Event DeleteSuccess()

	Property Customer As String
	Property SalesRep As String
	Property DefaultShipVia As String
	Property DefaultShipTo As Integer
	Property DefaultBillingAddress As Integer
	Property SalesCode As String
	Property TaxCode As String
	Property CurrencyDef As String
	Property VOILocation As String
	Property VOIShipTo As String
	Property VOIShipToAddressID As Integer
	Property VOIShipVia As String
	Property VOIShipType As String
	Property VOIProcessType As String
	Property IsNew As Boolean
	Property ShowInList As Boolean
	Property LeadDays As Integer
	Property RequiresPO As Boolean

	Property DiscountLevel As Integer
	Property DiscountPct As Decimal
	Property ConversionRate As Double
	Property Errors As String

	Property CustomerCode As String
	Property CustomerName As String

	Property Email As String
	Property Terms As String
	Property CurrDef As String
	Property MainAddr As cAddress
	Property ShipAddr As cAddress
	Property RemitAddr As cAddress
	Property Contact As cContact
	Property cmd As SqlCommand
	Property IsAuto As Boolean
	Property ClassErrors As String
	Private Sub AddError(ErrorMsg As String)
		If ClassErrors Is Nothing Then
			ClassErrors = ErrorMsg
		Else
			ClassErrors = ClassErrors & crlf() & ErrorMsg
		End If
	End Sub

	Function AddCustomer() As Boolean
		Try
			Dim hdr As String = "INSERT INTO Customer(Customer, Sales_Rep,User_Values,Terms,Type,Ship_Via,Sales_Code,Tax_Code,Name,Email_Address," _
						& "URL,Status,Ship_Lead_Days,Print_Statement,Credit_Limit,Curr_Balance,Customer_Since,Accept_BO,Pricing_Level,Currency_Def," _
						& "Note_Text,Last_Updated,Tax_ID,Acct_Mgr,Rating,Send_Report_By_Email,PST_ID)"

			Dim vals As String = AddString(CustomerCode)
			vals = vals & ",NULL,NULL"
			vals = vals & "," & AddString(terms,, "NULL")
			vals = vals & ",NULL,NULL,NULL,NULL" 'type shipvia salescode taxcode
			vals = vals & "," & AddString(CustomerName)
			vals = vals & "," & AddString(Email,, "NULL")
			vals = vals & ",NULL" 'URL
			vals = vals & ",'Active'" 'status
			vals = vals & ",1,0,0,0" 'ship lead days to curr balance
			vals = vals & "," & AddString(Now, DbType.Date) 'cus since
			vals = vals & ",1,NULL" 'accept bo, pricing level
			vals = vals & "," & CurrDef
			vals = vals & ",NULL" 'note text
			vals = vals & "," & AddString(Now, DbType.DateTime) 'last updated
			vals = vals & ",NULL,NULL,7,0,NULL" 'tax id, acct mgr, rating, send rpt, pst

			cmd.CommandText = hdr & " VALUES(" & vals & ")"
			cmd.ExecuteNonQuery()
			Return True
		Catch ex As Exception
			AddError(ex.ToString)
			logger.Error(ex.ToString, "Jobboss_Woo_Integration.cCustomer.AddCustomer Error")
			If Not IsAuto Then
				MessageBox.Show(String.Format("Error in Jobboss_Woo_Integration.cCustomer.AddCustomer: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			End If
			Return False
		Finally

		End Try
	End Function



	Private Function LoadClass(cmd As SqlCommand) As Boolean
		Try
			cmd.CommandText = "SELECT * FROM Customer WHERE Customer=" & AddString(Customer)
			'MsgBox(Customer)
			Using dt As New DataTable
				dt.Load(cmd.ExecuteReader)
				If dt.Rows.Count > 0 Then
					Dim dtr As DataRow = dt.Rows(0)
					Me.Customer = dtr("Customer")
					CustomerCode = dtr("Customer")
					If Not IsDBNull(dtr("Name")) Then
						CustomerName = dtr("Name")
					End If
					If Not IsDBNull(dtr("Ship_Via")) Then
						DefaultShipVia = dtr("Ship_Via")
					End If

					Terms = dtr("Terms") & ""
					SalesCode = dtr("Sales_Code") & ""
					TaxCode = dtr("Tax_Code") & ""
					SalesRep = dtr("Sales_Rep") & ""
					If Not IsDBNull(dtr("Currency_Def")) Then
						CurrencyDef = dtr("Currency_Def")
					End If

					Dim pref As New cPreferences With {.cmd = cmd}
					If Not pref.Load() Then
						AddError("Could not load Preferences")
						Return False
					End If

					If pref.BaseCurrency <> CurrencyDef Then
						Dim curdef As New cCurrencyDef
						If Not curdef.LoadCurrency(CurrencyDef, cmd) Then
							AddError("Could not load Currency Definition")
							Return False
						Else
							ConversionRate = curdef.ConversionRate(pref.BaseCurrency, CurrencyDef)
						End If
					End If

					cmd.CommandText = "SELECT Address FROM Address WHERE Customer=" & AddString(Customer) & " AND Type LIKE '__1'"
					Using dtST As New DataTable
						dtST.Load(cmd.ExecuteReader)
						If dtST.Rows.Count > 0 Then
							Dim shipad As New cAddress
							shipad.Address = dtST.Rows(0).Item("Address")
							If shipad.Load(cmd) Then
								ShipAddr = shipad
							End If
							DefaultShipTo = dtST.Rows(0).Item("Address")
						End If
					End Using

					cmd.CommandText = "SELECT Address FROM Address WHERE Customer=" & AddString(Customer) & " AND Type LIKE '_1_'"
					Using dtDB As New DataTable
						dtDB.Load(cmd.ExecuteReader)
						If dtDB.Rows.Count > 0 Then
							Dim billad As New cAddress
							billad.Address = dtDB.Rows(0).Item("Address")
							If billad.Load(cmd) Then
								RemitAddr = billad
							End If
							DefaultBillingAddress = dtDB.Rows(0).Item("Address")
						End If
					End Using

					cmd.CommandText = "SELECT Address FROM Address WHERE Customer=" & AddString(Customer) & " AND Type LIKE '1__'"
					Using dtDB As New DataTable
						dtDB.Load(cmd.ExecuteReader)
						If dtDB.Rows.Count > 0 Then
							Dim billad As New cAddress
							billad.Address = dtDB.Rows(0).Item("Address")
							If billad.Load(cmd) Then
								MainAddr = billad
							End If
							DefaultBillingAddress = dtDB.Rows(0).Item("Address")
						End If
					End Using

					If Not IsDBNull(dtr("Pricing_Level")) Then
						cmd.CommandText = "SELECT * FROM User_Code WHERE Type='Discount' AND Code=" & AddString(dtr("Pricing_Level"))
						Using dtp As New DataTable
							dtp.Load(cmd.ExecuteReader)
							If dtp.Rows.Count > 0 Then
								'/ numeric1 values are like 20, 10 etc. 
								DiscountLevel = dtr("Pricing_Level")
								DiscountPct = dtp.Rows(0).Item("Numeric1")
							End If
						End Using
					Else
						DiscountLevel = 0
						DiscountPct = 0
					End If
				End If
			End Using
			Return True
		Catch ex As Exception
			logger.Error(ex.ToString, "LoadClass Error")
			MessageBox.Show(String.Format("Error in LoadClass: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			Return False
		Finally
		End Try
	End Function

	Function Load(Customer As String, cmd As SqlCommand) As Boolean
		Try
			If Customer.Length = 0 Then
				Return False
			Else
				Me.Customer = Customer
				Return LoadClass(cmd)
			End If
		Catch ex As Exception
			logger.Error(ex.ToString, "Load Error")
			MessageBox.Show(String.Format("Error in Load: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			Return False
		Finally
		End Try

	End Function
	Function Load(Customer As String) As Boolean
		Try
			If Customer.Length = 0 Then
				Return False
			Else
				Using con As New SqlConnection(JBConnection)
					con.Open()
					Using cmd As New SqlCommand
						cmd.Connection = con
						Me.Customer = Customer
						Return LoadClass(cmd)
					End Using
				End Using
			End If
		Catch ex As Exception
			logger.Error(ex.ToString, "cCustomer.Load Error")
			MessageBox.Show(String.Format("Error in Load: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			Return False
		Finally
		End Try

	End Function
	Private Function GetAddress(ShipTo As String, cmd As SqlCommand) As Integer
		Try
			cmd.CommandText = "SELECT Address FROM Address WHERE Ship_To_ID=" & AddString(ShipTo) & " AND Customer=" & AddString(Customer)
			Dim obj As Object = cmd.ExecuteScalar
			If obj Is Nothing Then
				Return 0
			Else
				Return obj
			End If
		Catch ex As Exception
			logger.Error(ex.ToString, "GetAddress Error")
			MessageBox.Show(String.Format("Error in GetAddress: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
		Finally
		End Try
	End Function
	Function GetIDFromShipTo(ShipTo As String, cmd As SqlCommand) As Integer
		Return GetAddress(ShipTo, cmd)
	End Function
	Function GetIDFromShipTo(ShipTo As String) As Integer
		Using con As New SqlConnection(JBConnection)
			con.Open()
			Using cmd As New SqlCommand
				cmd.Connection = con
				Return GetAddress(ShipTo, cmd)
			End Using
		End Using
	End Function
End Class
