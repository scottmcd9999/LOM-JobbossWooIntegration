Imports System.Data.SqlClient
Imports ExcelDataReader
Module Main
	Private logger As NLog.Logger = NLog.LogManager.GetCurrentClassLogger
	'/ THIS MODULE IS USED TO 
	'/ -- ADD IMPORT DATA TO THE USER TABLES
	'/ -- ADD THE SALES ORDER TO THE SYSTEM
	'/ PROCESSING THE ORDER, ADDING PACKLIST ETC IS DONE IN SHIPPING MODULE
	Sub Main()
		Dim args As String() = Environment.GetCommandLineArgs
		If args.Length > 1 Then
			Select Case args(1).ToString.ToUpper
				Case "GETFILES"

				Case "ADDFILETODB"
					ProcessFiles(True)
				Case "IMPORT"
					ImportSalesOrders(True)
				Case "SHIPPED"
					'ProcessShippedOrder(32445, True)
				Case Else
			End Select
		Else
			Application.EnableVisualStyles()
			Dim f As New fMain
			f.ShowDialog()
		End If
	End Sub

	Function AddOrderToDatabase(dt As DataTable, fi As IO.FileInfo, IsAuto As Boolean) As Boolean
		Try
			Using con As New SqlConnection(JBConnection)
				con.Open()
				Using trn As SqlTransaction = con.BeginTransaction
					Using cmd As New SqlCommand With {.Connection = con, .Transaction = trn}
						Dim i As Integer = 0
						For Each dtr As DataRow In dt.Rows
							i += 1
							If Not ValidateLine(dtr, IsAuto, fi, i) Then
								Return False
							End If
							cmd.CommandText = "SELECT COUNT(*) FROM usr_Woo_Data WHERE Order_ID=" & CInt(dtr("OrderID"))
							If cmd.ExecuteScalar = 0 Then
								'/ we haven't imported the order already:
								Dim dtord As DataTable = dt.DefaultView.ToTable
								dtord.DefaultView.RowFilter = "OrderID=" & CInt(dtr("OrderID"))
								Dim hdr As String = "INSERT INTO usr_Woo_Data(Account_ID,First_Name,Last_Name,Company,Phone,Email,Remit_Line1,Remit_Line2," _
									& "Remit_City,Remit_State,Remit_Zip,Remit_Country,Ship_Line1,Ship_Line2,Ship_City,Ship_State,Ship_Zip,Ship_Country," _
									& "Order_ID,Order_Date,Order_SubTotal,Order_Total,Material,Order_Quantity,Unit_Price,Ship_Via,Freight,Addl_Cost,Payment_ID,Payment_Amount,Payment_By," _
									& " File_Name,Date_Added,Processed,Date_Processed,Order_Created,Date_Order_Created)"

								For Each ord As DataRow In dtord.Rows
									Dim vals As String = CInt(ord("AccountID"))
									vals = vals & "," & AddString(ord("FirstName"))
									vals = vals & "," & AddString(ord("LastName"))
									vals = vals & "," & AddString(ord("CompanyName"),, "NULL")
									vals = vals & "," & AddString(ord("Phone"),, "NULL")
									vals = vals & "," & AddString(ord("Email"),, "NULL")
									vals = vals & "," & AddString(ord("RemitLine1"),, "NULL")
									vals = vals & "," & AddString(ord("RemitLine2"),, "NULL")
									vals = vals & "," & AddString(ord("RemitCity"),, "NULL")
									vals = vals & "," & AddString(ord("RemitState"),, "NULL")
									vals = vals & "," & AddString(ord("RemitZip"),, "NULL")
									vals = vals & "," & AddString(ord("RemitCountry"),, "NULL")
									vals = vals & "," & AddString(ord("ShipLine1"),, "NULL")
									vals = vals & "," & AddString(ord("ShipLine2"),, "NULL")
									vals = vals & "," & AddString(ord("ShipCity"),, "NULL")
									vals = vals & "," & AddString(ord("ShipState"),, "NULL")
									vals = vals & "," & AddString(ord("ShipZip"),, "NULL")
									vals = vals & "," & AddString(ord("ShipCountry"),, "NULL")
									vals = vals & "," & AddString(ord("OrderID"))
									vals = vals & "," & AddString(ord("OrderDate"), DbType.Date, "NULL")
									vals = vals & "," & AddString(ord("OrderTotal"), DbType.Double, 0)
									vals = vals & "," & AddString(ord("Order Subtotal"), DbType.Double, 0)
									vals = vals & "," & AddString(ord("Material"),, "NULL")
									vals = vals & "," & AddString(ord("OrderQuantity"),, "NULL")
									vals = vals & "," & CDbl(ord("UnitPrice"))
									vals = vals & "," & AddString(ord("ShipVia"),, "NULL")
									vals = vals & "," & AddString(ord("Freight"), DbType.Double, "NULL")
									vals = vals & "," & AddString(ord("AddlCost"), DbType.Double, "NULL")
									vals = vals & "," & AddString(ord("PaymentID"))
									vals = vals & "," & CDbl(ord("PaymentAmount"))
									vals = vals & "," & AddString(ord("PaymentBy"),, "NULL")
									vals = vals & "," & AddString(fi.FullName)
									vals = vals & "," & AddString(Now, DbType.DateTime)
									vals = vals & ",0,NULL,0,NULL"

									cmd.CommandText = hdr & " VALUES(" & vals & ")"
									cmd.ExecuteNonQuery()
								Next
							End If
						Next
					End Using
					trn.Commit()
				End Using
			End Using
			Return True
		Catch ex As Exception
			logger.Error(ex.ToString, "Jobboss_Woo_Integration.Main.AddOrderData Error")
			If Not IsAuto Then
				MessageBox.Show(String.Format("Error in Jobboss_Woo_Integration.Main.AddOrderData: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			End If
			Return False
		Finally

		End Try
	End Function
	Function ValidateLine(dtr As DataRow, IsAuto As Boolean, fi As IO.FileInfo, Linenum As Integer) As Boolean
		Try
			Dim errs As New cErrors
			Dim fOK As Boolean = True
			If IsDBNull(dtr("FirstName")) OrElse String.IsNullOrEmpty(dtr("FirstName")) Then
				errs.LogError(fi.FullName, "Line " & Linenum.ToString & " Missing First Name")
				fOK = False
			End If
			If IsDBNull(dtr("LastName")) OrElse String.IsNullOrEmpty(dtr("LastName")) Then
				errs.LogError(fi.FullName, "Line " & Linenum.ToString & " Missing Last Name")
				fOK = False
			End If
			If IsDBNull(dtr("RemitLine1")) OrElse String.IsNullOrEmpty(dtr("RemitLine1")) Then
				errs.LogError(fi.FullName, "Line " & Linenum.ToString & " Missing Remit Line1")
				fOK = False
			End If
			If IsDBNull(dtr("RemitZip")) OrElse String.IsNullOrEmpty(dtr("RemitZip")) Then
				errs.LogError(fi.FullName, "Line " & Linenum.ToString & " Missing Remit Zip")
				fOK = False
			End If
			If IsDBNull(dtr("ShipLine1")) OrElse String.IsNullOrEmpty(dtr("ShipLine1")) Then
				errs.LogError(fi.FullName, "Line " & Linenum.ToString & " Missing Shipping Line1")
				fOK = False
			End If
			If IsDBNull(dtr("ShipZip")) OrElse String.IsNullOrEmpty(dtr("ShipZip")) Then
				errs.LogError(fi.FullName, "Line " & Linenum.ToString & " Missing Shipping Zip")
				fOK = False
			End If
			If IsDBNull(dtr("OrderID")) OrElse String.IsNullOrEmpty(dtr("OrderID")) Then
				errs.LogError(fi.FullName, "Line " & Linenum.ToString & " Missing OrderID")
				fOK = False
			End If
			If IsDBNull(dtr("OrderDate")) OrElse String.IsNullOrEmpty(dtr("OrderDate")) Then
				errs.LogError(fi.FullName, "Line " & Linenum.ToString & " Missing Order Date")
				fOK = False
			End If
			If IsDBNull(dtr("Material")) OrElse String.IsNullOrEmpty(dtr("Material")) Then
				errs.LogError(fi.FullName, "Line " & Linenum.ToString & " Missing Material")
				fOK = False
			End If
			If IsDBNull(dtr("OrderQuantity")) OrElse String.IsNullOrEmpty(dtr("OrderQuantity")) Then
				errs.LogError(fi.FullName, "Line " & Linenum.ToString & " Missing Order Quantity")
				fOK = False
			End If
			If IsDBNull(dtr("UnitPrice")) OrElse String.IsNullOrEmpty(dtr("UnitPrice")) Then
				errs.LogError(fi.FullName, "Line " & Linenum.ToString & " Missing Unit Price")
				fOK = False
			End If
			If IsDBNull(dtr("PaymentID")) OrElse String.IsNullOrEmpty(dtr("PaymentID")) Then
				errs.LogError(fi.FullName, "Line " & Linenum.ToString & " Missing PaymentID")
				fOK = False
			End If
			If IsDBNull(dtr("PaymentAmount")) OrElse String.IsNullOrEmpty(dtr("PaymentAmount")) Then
				errs.LogError(fi.FullName, "Line " & Linenum.ToString & " Missing Payment Amount")
				fOK = False
			End If

			Return fOK

		Catch ex As Exception
			logger.Error(ex.ToString, "Jobboss_Woo_Integration.Main.ValidateLine Error")
			If Not IsAuto Then
				MessageBox.Show(String.Format("Error in Jobboss_Woo_Integration.Main.ValidateLine: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			End If
			Return False
		Finally

		End Try


	End Function
	Function ProcessFiles(IsAuto As Boolean) As Boolean
		Try
			Dim filepath As String = GetFolder("LocalFolder", IsAuto)
			If String.IsNullOrEmpty(filepath) Then
				If IsAuto Then
					SendAlert("", IsAuto, "The Inbound File Path has not been set")
				Else
					MessageBox.Show("The Inbound File Path has not been set", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
				End If
				Return False
			End If
			filepath = IO.Path.Combine(filepath, "Inbound")
			If Not IO.Directory.Exists(filepath) Then
				If IsAuto Then
					SendAlert("", IsAuto, "The Inbound File Path could not be located")
				Else
					MessageBox.Show("The Inbound File Path could not be located", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
				End If
				Return False
			End If

			Dim di As New IO.DirectoryInfo(filepath)
			Dim fiarr As IO.FileInfo() = di.GetFiles("*")
			Dim errs As New cErrors

			If fiarr.Count = 0 Then
				'MessageBox.Show("No web order files were found, so no files have been processed.", "No Files Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
				Return False
			End If

			For Each fi As IO.FileInfo In fiarr
				Dim dt As DataTable = GetDatatableFromFile(fi, IsAuto)
				If dt Is Nothing Then
					'/ couldn't open the file for some reason:
					errs.LogError(fi.FullName, "Unable to open file")
					If IsAuto Then
						SendAlert(fi.FullName, IsAuto, "Unable to open the Import File " & fi.FullName)
					Else
						MessageBox.Show("Unable to open the Import File: " & crlf() & fi.FullName, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
					End If
				Else
					If AddOrderToDatabase(dt, fi, IsAuto) Then
						MoveFile(fi.FullName, IsAuto)
					Else
						'/ move to exceptions folder
						MoveFile(fi.FullName, IsAuto, True)
					End If
				End If
			Next

			Return True
		Catch ex As Exception
			logger.Error(ex.ToString, "Jobboss_Woo_Integration.Main.ProcessCustomers Error")
			If Not IsAuto Then
				MessageBox.Show(String.Format("Error in Jobboss_Woo_Integration.Main.ProcessCustomers: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			End If
			Return False
		Finally

		End Try

	End Function
	'///////////////////////////////////////////////////////////////////
	'/ We are processing Shipped ORders in the Shipping Module:
	'///////////////////////////////////////////////////////////////////
	'Function ProcessShippedOrder(OrderID As Integer, IsAuto As Boolean) As Boolean
	'	'/ we now create the Pick, Packlist, etc:
	'	Dim errs As New cErrors
	'	Dim dt As New DataTable
	'	Using con As New SqlConnection(JBConnection)
	'		con.Open()
	'		Using trn As SqlTransaction = con.BeginTransaction
	'			Using cmd As New SqlCommand With {.Connection = con, .Transaction = trn}
	'				cmd.CommandText = "SELECT * FROM usr_Woo_Data WHERE Order_Created=1 AND Processed=0 AND Date_Processed IS NULL" _
	'				& " AND Date_Order_Created IS NOT NULL AND Order_ID=" & OrderID
	'				dt.Load(cmd.ExecuteReader)
	'				For Each dtr As DataRow In dt.Rows
	'					Dim cust As New cCustomer
	'					If Not cust.Load(dtr("Jobboss_Customer")) Then
	'						errs.LogError(dtr("File_Name"), "Unable to load Customer, processing order " & dtr("Sales_Order"))
	'						Return False
	'					End If
	'					'/ go ahead and create the Packlist HEader:
	'					Dim plh As New cPacklist_Header
	'					With plh
	'						.IsAuto = IsAuto
	'						.cmd = cmd
	'						.Contact = dtr("Contact_ID") ' cust.Contact.Contact
	'						.Customer = cust.CustomerCode
	'						.IsAuto = IsAuto
	'						.PacklistDate = Now
	'						.PacklistType = "SOShip"
	'						.ShipTo = cust.ShipAddr.Address
	'						.ShipVia = dtr("Ship_Via") & ""
	'						If Not .AddPacklistHeader(IsAuto) Then
	'							errs.LogError(dtr("File_Name"), "Unable to create Packlist Header")
	'							Return False
	'						End If
	'					End With
	'					'/ pick the line:
	'					Dim soh As New cSOHeader With {.cmd = cmd, .IsAuto = IsAuto}
	'					If Not soh.LoadSalesOrder(dtr("Sales_Order"), cmd) Then
	'						errs.LogError(dtr("File_Name"), "Unable to load Sales Order " & dtr("Sales_Order"))
	'						Return False
	'					End If
	'					'/ usr_WOO_SODetail is filled in the Shipping Module, after shipping department fills the order:
	'					cmd.CommandText = "SELECT wd.File_Name,wsd.SO_Detail,wsd.Location,wsd.Serial_Number,wsd.Quantity,wd.Sales_Order" _
	'						& " FROM usr_Woo_SO_Detail wsd INNER JOIN usr_Woo_Data wd ON wsd.Woo_Data_ID=wd.ID WHERE wd.Order_ID=" & OrderID
	'					Using dtsd As New DataTable
	'						dtsd.Load(cmd.ExecuteReader)
	'						For Each dtsdr As DataRow In dtsd.Rows
	'							'/ create a pick for each row in this table. Note a single SODetail could have multiple picks,
	'							'/ since there are serialized items:
	'							Dim mattrans As New cMaterialTrans With {.cmd = cmd, .IsAuto = IsAuto}
	'							With mattrans
	'								Dim sod As New cSODetail
	'								If Not sod.LoadSODetail(dtsdr("SO_Detail"), cmd) Then
	'									errs.LogError(dtr("File_Name"), "Unable to load Sales Order Detail for " & dtr("Sales_Order"))
	'									Return False
	'								End If
	'								If Not .AddSOLinePick(sod.SODetail, sod.Material, dtsdr("Location"), dtsdr("Quantity"), dtsdr("Serial_Number")) Then
	'									errs.LogError(dtr("File_Name"), "Unable to pick Sales Order Line: " & .ClassError)
	'									Return False
	'								End If
	'								'/ add a Packlist Detail:
	'								Dim pld As New cPacklistDetail
	'								With pld
	'									.Packlist = plh.Packlist
	'									.IsAuto = IsAuto
	'									.cmd = cmd
	'									.SODClass = sod
	'									.PLHOID = plh.PLHOID
	'									If Not .AddPacklistDetail Then
	'										errs.LogError(dtr("File_Name"), "Unable to create Packlist Detail: " & .ClassErrors.ToString)
	'										Return False
	'									Else
	'										plh.PlDetails.Add(pld)
	'										'/ set the Delivery to Shipped:
	'										cmd.CommandText = "UPDATE Delivery SET Packlist=" & AddString(plh.Packlist) & ",Shipped_Date=" & AddString(Now, DbType.Date) _
	'											& ",Last_Updated=" & AddString(Now, DbType.DateTime) & ",Shipped_Quantity=" & pld.Quantity _
	'											& ",Remaining_Quantity=" & .SODClass.Quantity - pld.Quantity & " WHERE ObjectID=" & AddString(.SODClass.Delivery.DeliveryOID)
	'										cmd.ExecuteNonQuery()
	'										.SODClass.Delivery.ShippedDate = CDate(Now.ToShortDateString)
	'									End If
	'								End With
	'							End With
	'						Next
	'					End Using

	'					cmd.CommandText = "SELECT * FROM Packlist_Detail pld WHERE Packlist=" & AddString(plh.Packlist)
	'					Using dtp As New DataTable
	'						dtp.Load(cmd.ExecuteReader)
	'						'/ create the Invoice header:
	'						Dim invhdr As New cInvoiceHeader
	'						With invhdr
	'							.IsAuto = IsAuto
	'							.cmd = cmd
	'							.plh = plh
	'							.cust = cust
	'							.soh = soh
	'							.DocumentDate = dtr("Order_Date")
	'							If Not .AddInvoiceHeader Then
	'								errs.LogError(dtr("File_Name"), "Unable to add Invoice Header: " & .ClassErrors.ToString)
	'								Return False
	'							End If
	'						End With
	'						''/ now add invoice lines:
	'						For Each dtpr As DataRow In dtp.Rows
	'							Dim det As New cInvoiceDetail
	'							With det
	'								.IsAuto = IsAuto
	'								.cmd = cmd
	'								.ShippedDate = plh.PacklistDate
	'								If Not .AddDetail() Then
	'									errs.LogError(dtr("File_Name"), "Unable to add Invoice Detail: " & .ClassErrors.ToString)
	'									Return False
	'								End If
	'							End With
	'						Next
	'					End Using
	'					'/ add a payment:


	'				Next
	'			End Using
	'		End Using
	'	End Using

	'	If dt.Rows.Count > 0 Then

	'	End If
	'End Function


	Function ImportSalesOrders(IsAuto As Boolean) As Boolean
		Try
			Dim errs As New cErrors
			Dim dt As New DataTable
			Using con As New SqlConnection(JBConnection)
				con.Open()
				Using cmd As New SqlCommand With {.Connection = con}
					cmd.CommandText = "SELECT * FROM usr_Woo_Data WHERE Order_Created=0 AND Date_Order_Created IS NULL"
					dt.Load(cmd.ExecuteReader)
				End Using
			End Using

			If dt.Rows.Count > 0 Then
				Return ImportSalesOrder(dt, IsAuto)
			Else
				Return True
			End If
		Catch ex As Exception
			logger.Error(ex.ToString, "Jobboss_Woo_Integration.Main.ImportOrders Error")
			If Not IsAuto Then
				MessageBox.Show(String.Format("Error in Jobboss_Woo_Integration.Main.ImportOrders: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			End If
			Return False
		Finally

		End Try
	End Function
	Function ImportSalesOrder(dt As DataTable, IsAuto As Boolean) As Boolean
		Try
			Dim errs As New cErrors
			Using con As New SqlConnection(JBConnection)
				con.Open()
				Using trn As SqlTransaction = con.BeginTransaction
					Using cmd As New SqlCommand With {.Connection = con, .Transaction = trn}
						'/ get distinct Orders:
						Dim dto As New DataTable
						dto = dt.DefaultView.ToTable(True, "Order_ID")
						For Each dtr As DataRow In dto.Rows
							'/ get all the rows for that ORder:
							Dim ords As New DataTable
							ords = dt.Select("Order_ID=" & AddString(dtr("Order_ID"))).CopyToDataTable
							'/ get the first row, add the Customer:
							Dim hdr As DataRow = ords.Rows(0)
							'/ get the FileInfo object:
							'Dim fi As New IO.FileInfo(hdr("File_Name"))
							Dim cust As New cCustomer
							cust = AddCustomer(hdr, IsAuto, cmd)
							If cust Is Nothing Then
								'/ had an issue finding or adding the Customer:
								errs.LogError(hdr("File_Name"), "Error Adding Or Locating Customer: " & cust.ClassErrors.ToString)
								Return False
							End If
							'/ make sure the Customer has a Main address type:
							ValidateCustomerAddresses(cust.Customer, cmd, IsAuto)
							'/ add the Sales Order Header:
							Dim soh As New cSOHeader With {.cmd = cmd}
							With soh
								.Customer = cust.CustomerCode
								.OrderDate = hdr("Order_Date")
								.PromisedDate = hdr("Order_Date")
								.CustomerPO = hdr("Order_ID")
								.ShipTo = cust.ShipAddr.Address
								.ShipVia = cust.DefaultShipVia
								.SOStatus = "Open"
								.Terms = "COD"
								.Note = "Added by Woo Integration"
								If Not .AddSalesOrderHeader(cmd) Then
									errs.LogError(hdr("File_Name"), "Unable to add Sales Order Header: " & soh.Errors.ToString)
									Return False
								End If
							End With
							For Each ord As DataRow In ords.Rows
								Dim sod As New cSODetail
								With sod
									.IsAuto = IsAuto
									.SalesOrder = soh.SalesOrder
									.Quantity = ord("Order_Quantity")
									.PromisedDate = ord("Order_Date")
									.Material = ord("Material")
									.Status = "Open"
									.UnitPrice = ord("Unit_Price")
									.Customer = soh.Customer
									.Notes = "Added by Woo Integration"
									If Not sod.AddSalesOrderDetailLine(cmd) Then
										errs.LogError(hdr("File_Name"), "Unable to add Sales Order Line: " & sod.Errors.ToString)
										Return False
									Else
										'/ add a delivery:
										Dim deliv As New cDelivery
										With deliv
											.IsAuto = IsAuto
											.SODetail = sod.SODetail
											.PromisedDate = sod.PromisedDate
											.Quantity = sod.Quantity
											If Not .AddDeliveryForSOD(cmd) Then
												errs.LogError(hdr("File_Name"), "Unable to add Delivery for Sales Order line: " & .Errors.ToString)
												Return False
											End If
										End With
										sod.Delivery = deliv
										soh.SalesOrderDetails.Add(sod)
									End If
								End With
								cmd.CommandText = "UPDATE usr_Woo_Data SET SO_Detail=" & sod.SODetail & " WHERE ID=" & ord("ID")
								cmd.ExecuteNonQuery()
								cmd.CommandText = "SELECT SUM(Total_Price) AS Balance FROM SO_Detail WHERE Sales_Order=" & AddString(soh.SalesOrder)
								Dim soamt As Double = cmd.ExecuteScalar
								cmd.CommandText = "UPDATE Customer SET Curr_Balance=Curr_Balance+" & soamt & " WHERE Customer=" & AddString(soh.Customer)
								cmd.ExecuteNonQuery()
								cmd.CommandText = "UPDATE SO_Header SET Total_Price=" & soamt & " WHERE Customer=" & AddString(soh.Customer)
								cmd.ExecuteNonQuery()
								cmd.CommandText = "UPDATE usr_Woo_Data SET Order_Created=1, Date_Order_Created=" & AddString(Now, DbType.DateTime) _
									& ",Sales_Order=" & AddString(soh.SalesOrder) & ",Jobboss_Customer=" & AddString(soh.Customer) _
									& ",Last_Updated=" & AddString(Now, DbType.DateTime) & " WHERE Order_ID=" & CInt(dtr("Order_ID"))
								cmd.ExecuteNonQuery()
							Next
							ords.Clear()
						Next
					End Using
					trn.Commit()
				End Using
			End Using

			Return True

		Catch ex As Exception
			logger.Error(ex.ToString, "Jobboss_Woo_Integration.Main.ImportOrder Error")
			If Not IsAuto Then
				MessageBox.Show(String.Format("Error in Jobboss_Woo_Integration.Main.ImportOrder: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			End If
			Return False
		Finally

		End Try
	End Function
	Function ValidateCustomerAddresses(Customer As String, cmd As SqlCommand, IsAuto As Boolean) As Boolean
		cmd.CommandText = "SELECT COUNT(*) FROM Address WHERE Customer=" & AddString(Customer)
		If cmd.ExecuteScalar = 0 Then
			Return False
		End If
		If cmd.ExecuteScalar = 1 Then
			'/ only one address, so it must be all 3:
			cmd.CommandText = "UPDATE Address SET Type='111' WHERE Customer=" & AddString(Customer)
			cmd.ExecuteNonQuery()
			Return True
		Else
			Dim mainaddr As Boolean
			Dim remitaddr As Boolean
			Dim shipaddr As Boolean
			cmd.CommandText = "SELECT COUNT(*) FROM Address WHERE Customer=" & AddString(Customer) _
				& " AND Type LIKE '1__'" ' OR Type LIKE '_1_' OR Type LIKE '__1')"
			mainaddr = cmd.ExecuteScalar > 0
			cmd.CommandText = "SELECT COUNT(*) FROM Address WHERE Customer=" & AddString(Customer) _
				& " AND Type LIKE '_1_'" ' OR Type LIKE '_1_' OR Type LIKE '__1')"
			remitaddr = cmd.ExecuteScalar > 0
			cmd.CommandText = "SELECT COUNT(*) FROM Address WHERE Customer=" & AddString(Customer) _
				& " AND Type LIKE '__1'" ' OR Type LIKE '_1_' OR Type LIKE '__1')"
			shipaddr = cmd.ExecuteScalar > 0

			If mainaddr And remitaddr And shipaddr Then
				Return True
			Else
				'/ one more is not set, so we just grab the first address and make it the default.
				Dim addr As Integer
				cmd.CommandText = "SELECT TOP 1 Address FROM Address WHERE Customer=" & AddString(Customer)
				addr = cmd.ExecuteScalar
				Dim addrtype As String
				If Not mainaddr Then
					addrtype = "1"
				Else
					addrtype = "0"
				End If
				If Not remitaddr Then
					addrtype = addrtype & "1"
				Else
					addrtype = addrtype & "0"
				End If
				If Not shipaddr Then
					addrtype = addrtype & "1"
				Else
					addrtype = addrtype & "0"
				End If
				cmd.CommandText = "UPDATE Address SET Type=" & AddString(addrtype) & " WHERE Address=" & addr
				cmd.ExecuteNonQuery()
				Return True
			End If

		End If


	End Function
	Function AddPayment(dt As DataTable, IsAuto As Boolean, cmd As SqlCommand) As Boolean
		Dim errs As New cErrors

		Dim finpref As New cFinPrefs
		finpref.GetFinancialPreferences()

	End Function


	Function AddAddress(dtr As DataRow, isauto As Boolean, cmd As SqlCommand, file As IO.FileInfo, cust As cCustomer,
						AddrType As String, IsMain As Boolean, IsOnly As Boolean) As cAddress
		Try
			Dim errs As New cErrors
			Dim addr As New cAddress
			Dim line1 As String
			Dim line2 As String
			Dim city As String
			Dim state As String
			Dim zip As String
			Dim country As String
			'/ determine if the address already exists:
			If AddrType = "Remit" Then
				cmd.CommandText = "SELECT * FROM Address WHERE Name=" & AddString(cust.CustomerName) & " AND Line1=" & AddString(dtr("RemitLine1")) _
										& " AND Zip=" & AddString(dtr("RemitZip")) & " AND City=" & AddString(dtr("RemitCity")) & " AND TYPE LIKE '_1_'"
				line1 = "RemitLine1"
				line2 = "RemitLine2"
				city = "RemitCity"
				state = "RemitState"
				zip = "RemitZip"
				country = "RemitCountry"
			Else
				cmd.CommandText = "SELECT * FROM Address WHERE Name=" & AddString(cust.CustomerName) & " AND Line1=" & AddString(dtr("ShipLine1")) _
										& " AND Zip=" & AddString(dtr("ShipZip")) & " AND City=" & AddString(dtr("ShipCity")) & " AND Type LIKE '__1'"
				line1 = "ShipLine1"
				line2 = "ShipLine2"
				city = "ShipCity"
				state = "ShipState"
				zip = "ShipZip"
				country = "ShipCountry"
			End If

			Using dta As New DataTable
				dta.Load(cmd.ExecuteReader)
				If dta.Rows.Count > 0 Then
					'/ found a match, so we load that address:
					addr.Address = dta.Rows(0).Item("Address")
					If addr.Load(cmd) Then
						Return addr
						'Else
						'	errs.LogError(file.FullName, "Unable to Load " & AddrType & " Address for Customer " & cust.CustomerName)
						'	Return Nothing
					End If
				End If
			End Using
			'/ didnt' find the Address, so add it:
			With addr
				.Customer = cust.CustomerCode
				.Billable = True
				.Shippable = True
				If IsOnly Then
					.PrimaryMain = True
					.PrimaryRemit = True
					.PrimaryShip = True
				Else
					.PrimaryMain = IsMain
					.PrimaryRemit = AddrType = "REMIT"
					.PrimaryShip = AddrType = "SHIP"
				End If

				If IsMain Then
					.ShipToID = "MAIN"
				Else
					.ShipToID = AddrType
				End If

				.Name = cust.CustomerName
				.Line1 = dtr(line1)
				.Line2 = dtr(line2) & ""
				.City = dtr(city) & ""
				.State = dtr(state) & ""
				.Zip = dtr(zip) & ""
				.Country = dtr(country)

				If Not .AddAddress(cmd) Then
					errs.LogError(file.FullName, "Unable to add " & AddrType & " Address: " & addr.ClassErrors)
					If isauto Then
						SendAlert(file.FullName, isauto)
					Else
						MessageBox.Show("Unable to add Address: " & crlf() & addr.ClassErrors, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
					End If
					Return Nothing
				Else
					ValidateCustomerAddresses(cust.Customer, cmd, isauto)
					If IsOnly Then
						cust.MainAddr = addr
						cust.RemitAddr = addr
						cust.ShipAddr = addr
					Else
						If IsMain Then
							cust.MainAddr = addr
						End If
						If AddrType = "REMIT" Then
							cust.RemitAddr = addr
						End If
						If AddrType = "SHIP" Then
							cust.ShipAddr = addr
						End If
					End If
					Return addr
				End If
			End With
		Catch ex As Exception
			logger.Error(ex.ToString, "Jobboss_Woo_Integration.Main.AddAddress Error")
			If Not isauto Then
				MessageBox.Show(String.Format("Error in Jobboss_Woo_Integration.Main.AddAddress: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			End If
			Return Nothing
		Finally

		End Try
	End Function
	Function AddCustomer(dtr As DataRow, IsAuto As Boolean, cmd As SqlCommand) As cCustomer
		Try
			Dim errs As New cErrors
			Dim finpref As New cFinPrefs
			If Not finpref.GetFinancialPreferences(cmd) Then
				errs.LogError(dtr("File_Name"), "Unable to load Jobboss Financial Preferences")
				Return Nothing
			End If
			Dim cust As New cCustomer With {.cmd = cmd}
			'/ do we already have a Customer for this one?
			cmd.CommandText = "SELECT Customer FROM Customer WHERE Customer=" & AddString(dtr("Account_ID"))
			Using dtc As New DataTable
				dtc.Load(cmd.ExecuteReader)
				If dtc.Rows.Count > 0 Then
					If cust.Load(dtc.Rows(0).Item("Customer"), cmd) Then
						Return cust
					End If
				End If
			End Using
			'/ need to add the Customer:
			With cust
				.IsAuto = IsAuto
				.IsNew = True
				.CustomerCode = dtr("Account_ID")
				.CustomerName = dtr("First_Name") & " " & dtr("Last_Name") & ""
				.CurrDef = finpref.BaseCurrency
				.Terms = "COD"
				.Email = dtr("Email") & ""
				If Not .AddCustomer Then
					If IsAuto Then
						errs.LogError(dtr("File_Name"), "Unable to add Customer: " & .ClassErrors)
						SendAlert(dtr("File_Name"), IsAuto, "Unable to add Customer: " & .ClassErrors)
					Else
						MessageBox.Show("Unable to add Customer: " & crlf() & .ClassErrors, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
					End If
					Return Nothing
				Else
					'Return cust
					cmd.CommandText = "INSERT INTO usr_Woo_Customer_Map(Customer,Account_ID) VALUES(" & AddString(cust.CustomerCode) _
						& "," & cust.CustomerCode & ")"
					cmd.ExecuteNonQuery()
				End If
			End With
			'/ if the remit and ship are the same, we only need to add 1:
			If dtr("Remit_Line1") = dtr("Ship_Line1") And dtr("Remit_City") = dtr("Ship_City") And dtr("Remit_Zip") = dtr("Ship_Zip") Then
				Dim mainaddr As New cAddress
				With mainaddr
					.Customer = cust.CustomerCode
					.Billable = False
					.Shippable = True
					.PrimaryMain = True
					.PrimaryRemit = True
					.PrimaryShip = True
					.Name = "MAIN"
					.ShipToID = "MAIN"
					.Line1 = dtr("Ship_Line1")
					.Line2 = dtr("Ship_Line2") & ""
					.City = dtr("Ship_City") & ""
					.State = dtr("Ship_State") & ""
					.Zip = dtr("Ship_Zip") & ""
					.Country = dtr("Ship_Country")
					If Not .AddAddress(cmd) Then
						errs.LogError(dtr("File_Name"), "Unable to add Address: " & mainaddr.ClassErrors)
						If IsAuto Then
							SendAlert(dtr("File_Name"), IsAuto)
						Else
							MessageBox.Show("Unable to add Address: " & crlf() & mainaddr.ClassErrors, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
						End If
						Return Nothing
					Else
						cust.ShipAddr = mainaddr
						cust.MainAddr = mainaddr
						cust.RemitAddr = mainaddr
					End If
				End With
			Else
				Dim mainaddr As New cAddress
				With mainaddr
					.Customer = cust.CustomerCode
					.Billable = False
					.Shippable = True
					.PrimaryMain = True
					.PrimaryRemit = True
					.PrimaryShip = False
					.Name = "MAIN"
					.ShipToID = "MAIN"
					.Line1 = dtr("Remit_Line1")
					.Line2 = dtr("Remit_Line2") & ""
					.City = dtr("Remit_City") & ""
					.State = dtr("Remit_State") & ""
					.Zip = dtr("Remit_Zip") & ""
					.Country = dtr("Remit_Country")
					If Not .AddAddress(cmd) Then
						errs.LogError(dtr("File_Name"), "Unable to add Address: " & mainaddr.ClassErrors)
						If IsAuto Then
							SendAlert(dtr("File_Name"), IsAuto)
						Else
							MessageBox.Show("Unable to add Address: " & crlf() & mainaddr.ClassErrors, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
						End If
						Return Nothing
					Else
						cust.MainAddr = mainaddr
						cust.RemitAddr = mainaddr
					End If
				End With
				'/ add the shipping address:
				Dim shipaddr = New cAddress
				With shipaddr
					.Customer = cust.CustomerCode
					.Billable = False
					.Shippable = True
					.PrimaryMain = False
					.PrimaryRemit = False
					.PrimaryShip = True
					.Name = "SHIP"
					.ShipToID = "SHIP"
					.Line1 = dtr("Ship_Line1")
					.Line2 = dtr("Ship_Line2") & ""
					.City = dtr("Ship_City") & ""
					.State = dtr("Ship_State") & ""
					.Zip = dtr("Ship_Zip") & ""
					.Country = dtr("Ship_Country")
					If Not .AddAddress(cmd) Then
						errs.LogError(dtr("File_Name"), "Unable to add Address: " & shipaddr.ClassErrors)
						If IsAuto Then
							SendAlert(dtr("File_Name"), IsAuto)
						Else
							MessageBox.Show("Unable to add Address: " & crlf() & shipaddr.ClassErrors, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
						End If
						Return Nothing
					Else
						cust.ShipAddr = shipaddr
					End If
				End With
			End If
			'/ add contact:
			Dim cont As New cContact
			With cont
				.Address = cust.MainAddr.Address
				.ContactName = cust.CustomerName
				.Customer = cust.CustomerCode
				.Email = cust.Email
				.Phone = cust.MainAddr.Phone
				If Not .AddContact(cmd) Then
					If IsAuto Then
						SendAlert(dtr("File_Name"), IsAuto)
					Else
						MessageBox.Show("Unable to add Contact: " & crlf() & cont.ClassErrors)
					End If
				Else
					'/ Add the contact to the row, for use later during processing:
					cust.Contact = cont
					cmd.CommandText = "UPDATE usr_Woo_Data SET Contact_ID=" & cont.Contact & " WHERE ID=" & dtr("ID")
					cmd.ExecuteNonQuery()
				End If
			End With

			Return cust

		Catch ex As Exception
			logger.Error(ex.ToString, "Jobboss_Woo_Integration.Main.AddCustomer Error")
			Try
				Dim errs As New cErrors
				errs.LogError(dtr("File_Name"), ex.ToString)
			Catch ex1 As Exception
				logger.Error(ex.ToString, "AddCustomer - error log")
			End Try

			If Not IsAuto Then
				MessageBox.Show(String.Format("Error in Jobboss_Woo_Integration.Main.AddCustomer: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			End If
			Return Nothing
		Finally

		End Try

	End Function

	Function GetFolder(FolderName As String, IsAuto As Boolean) As String
		Dim c As New cSettings
		Dim fld As String = c.ReadSetting(FolderName)
		If String.IsNullOrEmpty(fld) Then
			If IsAuto Then
				SendAlert("", IsAuto, "There is no setting for the " & FolderName & " folder.")
			Else
				MessageBox.Show("There is no setting for the " & FolderName & " folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
			End If
			Return ""
		Else
			If Not System.IO.Directory.Exists(fld) Then
				If IsAuto Then
					SendAlert("", IsAuto, "The " & FolderName & " folder is missing or invalid")
				Else
					MessageBox.Show("The " & FolderName & " folder is missing or invalid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
				End If
				Return ""
			End If
		End If

		Return fld

	End Function
	Function SendAlert(filepath As String, Optional IsAuto As Boolean = True, Optional Message As String = "") As Boolean
		Try
			'logger.Info("send alert")
			Dim cs As New cSettings
			'Dim enc As New Encryption.cBCEncrypt

			Dim email As New cEmail With {.IsAuto = IsAuto}
			If Not email.LoadSettings() Then
				logger.Info("Could not load email settings")
				email = Nothing
				Return False
			Else
				'/ get the Alert email addresses:
				Using con As New SqlConnection(JBConnection)
					con.Open()
					Using cmd As New SqlCommand
						cmd.Connection = con
						Dim s_numalerts As String = cs.ReadSetting("NumOfAlerts")
						'/ default this to 2, in case the user hasn't set a value for the number of times to be alerted per file:
						Dim numalerts As Integer = 2
						If Not String.IsNullOrEmpty(s_numalerts) Then
							If IsNumeric(s_numalerts) Then
								numalerts = CInt(s_numalerts)
							End If
						End If
						'logger.Info("num alerts " & numalerts.ToString)
						If filepath <> "None" Then
							cmd.CommandText = "SELECT * FROM usr_API_File_Alerts WHERE File_Path=" & AddString(filepath) & " ORDER BY Last_Updated DESC"
							Using dt As New DataTable
								dt.Load(cmd.ExecuteReader)
								If dt.Rows.Count > 0 Then
									If dt.Rows(0).Item("Num_Alerts") > numalerts Then
										Return True
									End If
								End If
							End Using
						Else
							'/ this is an alert based on a missing of invalid setting, file path, etc. We need to alert, but only
							'/ once or twice per day:
							Dim lastnotice As String = cs.ReadSetting("LastSystemNotification")
							Dim s_numsysalerts As String = cs.ReadSetting("SystemAlerts")
							Dim numsysalerts As Integer

							If Not String.IsNullOrEmpty(s_numsysalerts) Then
								If IsNumeric(s_numsysalerts) Then
									numsysalerts = CInt(numsysalerts)
								Else
									numsysalerts = 2
								End If
							End If

							If IsDate(lastnotice) Then
								If DateDiff(DateInterval.Hour, CDate(lastnotice), Now) < numsysalerts Then
									'/ don't send the alert - bail out:
									Return True
								End If
							End If
						End If

						cmd.CommandText = "SELECT * FROM usr_API_Alert_Email"
						Using dt As New DataTable
							dt.Load(cmd.ExecuteReader)
							For Each dtr As DataRow In dt.Rows
								email.AddSendToAddress(dtr("Email_Address"))
							Next
						End Using
						'logger.Info("after alert email retrieve")

						Dim filename As String = ""
						If Not String.IsNullOrEmpty(filepath) Then
							filename = IO.Path.GetFileNameWithoutExtension(filepath)
						Else
							filename = "No File Name"
						End If

						Dim body As String = ""
						If String.IsNullOrEmpty(Message) Then
							cmd.CommandText = "SELECT * FROM usr_API_Errors WHERE Source_Document=" & AddString(filename)
							Using dt As New DataTable
								dt.Load(cmd.ExecuteReader)
								For Each dtr As DataRow In dt.Rows
									body = body & "<br/>" & dtr("Error_Msg")
								Next
							End Using
						Else
							body = Message
						End If

						'logger.Info("after build body")
						email.Subject = "Email Alert from the Concur Integration Utility"
						If Not String.IsNullOrEmpty(Message) Then
							email.Body = Message
						Else
							email.Body = "On " & Now.ToShortDateString & " at " & Now.ToShortTimeString & " the following a error(s) occurred " _
							& " while attempting to process file named " & filename & ":<br/>" _
							& body & "<br/><br/>" _
							& "Modify the information in SAP or Jobboss to resolve this issue, then move the file back into the processing" _
							& " queue, or import the file manually using the Integration Utility interface.<br/><br/>" _
							& "This file has been moved to the Exceptions folder. This file may have been processed multiple times so there may be duplicate issues reported in this alert."

						End If

						If Not email.SendEmail Then
							logger.Info("Error when trying to send Import Alerts: " & email.Errors)
							Return False
						Else
							'logger.Info("after send email")
							If Not String.IsNullOrEmpty(filepath) Then
								If filepath <> "None" Then
									cmd.CommandText = "SELECT COUNT(*) FROM usr_API_File_Alerts WHERE File_Path=" & AddString(filepath)
									If cmd.ExecuteScalar = 0 Then
										cmd.CommandText = "INSERT INTO usr_API_File_Alerts(File_Path,Num_Alerts) VALUES(" & AddString(filepath) _
										& ",1)"
									Else
										cmd.CommandText = "UPDATE usr_API_File_Alerts SET Num_Alerts=(Num_Alerts+1)" _
										& " WHERE ID IN (SELECT TOP 1 ID FROM usr_API_File_Alerts WHERE File_Path=" & AddString(filepath) & ")"
									End If
									cmd.ExecuteNonQuery()
								Else
									'/ this was for a setting.
									cs.WriteSetting("LastSystemNotification", Now)
								End If
							End If
						End If
					End Using
				End Using
			End If

			logger.Info("Email Sent")
			email = Nothing

			Return True
		Catch ex As Exception
			logger.Error(ex.ToString, "AmtexImport.Main.SendAlerts Error")
			If Not IsAuto Then
				MessageBox.Show(String.Format("Error in AmtexImport.Main.SendAlerts: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			End If
		Finally

		End Try
	End Function

	Function GetDatatableFromFile(fri As IO.FileInfo, IsAuto As Boolean) As DataTable
		Try
			Dim xlconfig As New ExcelDataSetConfiguration
			xlconfig.ConfigureDataTable = Function(tablereader) New ExcelDataTableConfiguration With {.UseHeaderRow = True}

			Dim extn As String = IO.Path.GetExtension(fri.FullName)
			Dim dt As New DataTable
			Dim res As DataSet
			Using stm As New IO.FileStream(fri.FullName, IO.FileMode.Open, IO.FileAccess.Read)
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
			If res.Tables.Count > 0 Then
				Return res.Tables(0)
			Else
				Return Nothing
			End If
		Catch ex As Exception
			logger.Error(ex.ToString, "Jobboss_Woo_Integration.Main.GetDatatableFromFile Error")
			If Not IsAuto Then
				MessageBox.Show(String.Format("Error in Jobboss_Woo_Integration.Main.GetDatatableFromFile: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			End If
			Return Nothing
		Finally

		End Try
	End Function

	Function MoveFile(FilePath As String, IsAuto As String, Optional IsException As Boolean = False, Optional MoveToStorage As Boolean = False)
		Try
			Dim errs As New cErrors
			Dim filename As String = IO.Path.GetFileName(FilePath)
			Dim localfld As String = GetFolder("LocalFolder", IsAuto)

			If String.IsNullOrEmpty(localfld) Then
				Return False
			End If
			'/ we don't move anything out of Outbound, so everything here is Inbound:
			Dim excepath As String = IO.Path.Combine(localfld, "Inbound\Exceptions")
			Dim procpath As String = IO.Path.Combine(localfld, "Inbound\Processed")
			Dim storpath As String = IO.Path.Combine(localfld, "Inbound\Storage")
			Dim destfile As String
			'/ converting to lower, since the decryption sometimes fails with mixed case files:
			If IsException Then
				destfile = IO.Path.Combine(excepath, filename).ToString.ToLower
			ElseIf MoveToStorage Then
				destfile = IO.Path.Combine(storpath, filename).ToString.ToLower
			Else
				destfile = IO.Path.Combine(procpath, filename).ToString.ToLower
			End If

			If IO.File.Exists(destfile) Then
				Dim newfile As String = IO.Path.GetFileNameWithoutExtension(FilePath)
				Dim fok As Boolean
				Dim i As Integer = 1
				Do Until fok
					destfile = IO.Path.Combine(procpath, newfile & i.ToString.PadLeft("000") & ".txt")
					fok = Not IO.File.Exists(destfile)
					i += 1
					If i > 1000 Then
						errs.LogError(FilePath, "Unable to rename files - too many others in the Storage folder")
						Return False
					End If
				Loop
			End If

			IO.File.Move(FilePath, destfile)
			'logger.Info("Moved file " & FilePath & " to " & destfile)
			Return True
		Catch ex As Exception
			logger.Error(ex.ToString, "MoveFile Error")

			If Not IsAuto Then
				MessageBox.Show(String.Format("Error in Main.MoveFile: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			End If
			Return False
		Finally

		End Try
	End Function
End Module
