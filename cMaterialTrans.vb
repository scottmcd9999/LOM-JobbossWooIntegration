Imports System.Data.SqlClient
Public Class cMaterialTrans
	Private logger As NLog.Logger = NLog.LogManager.GetCurrentClassLogger
	Property cmd As SqlCommand
	Property MaterialTransID As Integer
	Property SourceID As Integer
	Property Vendor As String
	Property Document As String
	Property TranType As String
	Property MatTransDate As Date
	Property StockUofM As String
	Property CostUofM As String
	Property Qty As Double
	Property UnitCost As Double
	Property OID As String
	Property SODetail As cSODetail
	Property ClassError
	Property IsAuto As Boolean
	Private Sub AddError(ErrorMsg As String)
		If ClassError Is Nothing Then
			ClassError = ErrorMsg
		Else
			ClassError = ClassError & Environment.NewLine & ErrorMsg
		End If
	End Sub

	Function AddSOLinePick_FIFO(ByVal SODetail As cSODetail, IsAuto As Boolean) As Boolean

		Try
			'Dim sod As New cSODetail
			'If Not sod.LoadSODetail(SODetail.SODetail, cmd) Then
			'	AddError("Unable to load Sales Order Detail Line " & SODetail.SODetail)
			'	Return False
			'End If
			'/ gets material trans records for all items currently in inventory that have Lot numbers associated with them:
			cmd.CommandText = "SELECT * FROM (" _
				& " SELECT mt.quantity,CASE WHEN mt.Material IS NULL THEN CASE WHEN s.Material IS NULL THEN 'NO MATERIAL' ELSE s.Material END" _
				& " ELSE mt.Material END AS Mat,mt.Material_Trans_Date, mt.Lot, mt.Material_Trans" _
				& " FROM material_trans mt LEFT OUTER JOIN source s ON mt.Source=s.Source" _
				& " INNER JOIN Material_Location ml ON ml.Material=mt.Material AND ml.Lot=mt.Lot WHERE mt.Vendor_Trans Is NULL And mt.Tran_Type='Receipt'" _
				& " UNION " _
				& " SELECT mt.quantity, mt.Material AS Mat, mt.Material_Trans_Date, mt.lot, mt.Material_Trans FROM Material_Trans mt" _
				& " INNER JOIN Material_Location ml ON ml.Material=mt.Material AND ml.Lot=mt.Lot" _
				& " WHERE mt.Vendor_Trans IS NULL AND tran_type='LocTfr' AND mt.Quantity>0" _
				& " )" _
				& " AS MatRcv WHERE MatRcv.Mat=" & AddString(SODetail.Material) & " AND MatRcv.Lot IS NOT NULL" _
				& " AND mt.Material_Trans_Date>DATEADD(yy,-1,getdate()) ORDER BY MatRcv.Material_Trans_Date "

			Dim remqty As Double = SODetail.Quantity

			Using dt As New DataTable
				dt.Load(cmd.ExecuteReader)
				If dt.Rows.Count = 0 Then
					Return False
				End If

				For Each dtr As DataRow In dt.Rows
					If remqty = 0 Then
						Exit For
					End If
					'/ do we have any of that lot still available?
					cmd.CommandText = "SELECT ISNULL(On_Hand_Qty,0) AS QtyAvl, Location_ID, Material_Location FROM Material_Location ml WHERE Material=" & AddString(dtr("Material")) _
						& " And Lot=" & dtr("Lot")
					Dim qtyonhand As Double = cmd.ExecuteScalar
					If qtyonhand > remqty Then
						'/ we can use this location to pick:
						If qtyonhand > remqty Then
							If AddSOLinePick(SODetail.SODetail, SODetail.Material, dtr("Location_ID"), remqty, "") Then
								cmd.CommandText = "UPDATE Material_Location SET On_Hand_Qty=" & qtyonhand - remqty & ",Last_Updated=" _
									& AddString(Now, DbType.DateTime) & " WHERE Material_Location=" & dtr("Material_Location")
								SODetail.PickedQuantity = SODetail.PickedQuantity + remqty
							Else
								AddError("Unable to pick inventory from " & dtr("Material_Location"))
								Return False
							End If
							remqty = 0
						End If
					Else
						If AddSOLinePick(SODetail.SODetail, SODetail.Material, dtr("Location_ID"), remqty - qtyonhand, "") Then
							cmd.CommandText = "DELETE FROM Material_Location WHERE Material_Location=" & dtr("Material_Location")
							SODetail.PickedQuantity = SODetail.PickedQuantity + (remqty - qtyonhand)
						Else
							AddError("Unable to pick inventory from " & dtr("Material_Location"))
							Return False
						End If

						remqty = remqty - qtyonhand
					End If
					cmd.ExecuteNonQuery()
				Next

				If remqty = 0 Then
					Return True
				Else
					Return False
				End If
			End Using
		Catch ex As Exception
			logger.Error(ex.ToString, "Jobboss_Woo_Integration.cMaterialTrans.AddSOLinePick_FIFO Error")
			If Not IsAuto Then
				MessageBox.Show(String.Format("Error in Jobboss_Woo_Integration.cMaterialTrans.AddSOLinePick_FIFO: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			End If
			Return False
		Finally

		End Try

	End Function
	Function AddSOLinePick_LotNumber(ByVal SODetail As cSODetail, LotNumber As String, IsAuto As Boolean) As Boolean

		Try
			'Dim remqty As Double = SODetail.Quantity

			'cmd.CommandText = "SELECT * FROM Material_Location WHERE Material=" & AddString(SODetail.Material) _
			'	& " AND Lot=" & AddString(LotNumber)
			''/ do we have any of that lot still available?
			'cmd.CommandText = "SELECT ISNULL(On_Hand_Qty,0) AS QtyAvl, Location_ID, Material_Location FROM Material_Location ml WHERE Material=" & AddString(dtr("Material")) _
			'			& " And Lot=" & AddString(LotNumber)
			'Dim qtyonhand As Double = cmd.ExecuteScalar
			'If qtyonhand > remqty Then
			'	'/ we can use this location to pick:
			'	If qtyonhand > remqty Then
			'		If AddSOLinePick(SODetail.SODetail, SODetail.Material, dtr("Location_ID"), remqty) Then
			'			cmd.CommandText = "UPDATE Material_Location SET On_Hand_Qty=" & qtyonhand - remqty & ",Last_Updated=" _
			'				& AddString(Now, DbType.DateTime) & " WHERE Material_Location=" & dtr("Material_Location")
			'			SODetail.PickedQuantity = SODetail.PickedQuantity + remqty
			'		Else
			'			AddError("Unable to pick inventory from " & dtr("Material_Location"))
			'			Return False
			'		End If
			'		remqty = 0
			'	End If
			'Else
			'	If AddSOLinePick(SODetail.SODetail, SODetail.Material, dtr("Location_ID"), remqty - qtyonhand) Then
			'		cmd.CommandText = "DELETE FROM Material_Location WHERE Material_Location=" & dtr("Material_Location")
			'		SODetail.PickedQuantity = SODetail.PickedQuantity + (remqty - qtyonhand)
			'	Else
			'		AddError("Unable to pick inventory from " & dtr("Material_Location"))
			'		Return False
			'	End If

			'	remqty = remqty - qtyonhand
			'End If
			'cmd.ExecuteNonQuery()

			'If remqty = 0 Then
			'	Return True
			'Else
			'	Return False
			'End If

		Catch ex As Exception
			logger.Error(ex.ToString, "cMaterialTrans.AddSOLinePick_LotNumber Error")
			If Not IsAuto Then
				MessageBox.Show(String.Format("Error in AddSOLinePick_LotNumber: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			End If
			Return False
		Finally

		End Try
	End Function
	Function AddSOLinePick(SODetail As Integer, Material As String, LocationID As String, Quantity As Integer, Lot As String) As Boolean

		Try
			Dim mat As New cMaterial With {.cmd = cmd}
			If Not mat.Load(Material) Then
				ClassError = "Unable to load Material " & Material & ": " & mat.Errors
				Return False
			End If

			cmd.CommandText = "INSERT INTO Material_Trans(SO_Detail,Location_ID,Lot,Tran_Type,Material_Trans_Date,Unit_Cost,Purch_Unit_Weight,Stock_UofM," _
				& " Cost_UofM,Reason,Quantity,Last_Updated,ObjectID)"

			Dim vals As String = AddString(SODetail)
			vals = vals & "," & AddString(LocationID)
			vals = vals & "," & AddString(Lot,, "NULL") 'Lot
			vals = vals & "," & AddString("Issue") 'tran type
			vals = vals & "," & AddString(Now, DbType.Date) 'mattrans date

			Dim prefs As New cPreferences With {.cmd = cmd}

			If Not prefs.Load Then
				Return False
			End If

			Dim unitcost As Double
			Select Case prefs.InventoryCostMethod
				Case "Last"
					unitcost = mat.LastCost
				Case "Standard"
					unitcost = mat.StandardCost
				Case "Average"
					unitcost = mat.AverageCost
			End Select

			vals = vals & "," & AddString(unitcost, DbType.Double) 'unit cost
			vals = vals & "," & AddString(1, DbType.Double) 'purch_unit_weight, always 1 for SO Issues
			vals = vals & "," & AddString(mat.StockUofM)
			vals = vals & "," & AddString(mat.CostUofM)
			vals = vals & ",NULL"  'reason & AddString(Reason)
			'/ issued are negative quantities:
			vals = vals & "," & AddString(Quantity * -1, DbType.Double) 'quantity
			vals = vals & "," & AddString(Now, DbType.DateTime) 'last updated
			vals = vals & "," & AddString(System.Guid.NewGuid.ToString) 'objectid

			cmd.CommandText = cmd.CommandText & " VALUES(" & vals & ")"
			cmd.ExecuteNonQuery()

			cmd.CommandText = "SELECT Material_Trans FROM Material_Trans WHERE Material_TransKey=(SELECT Scope_Identity())"
			MaterialTransID = cmd.ExecuteScalar

			'logger.Info("MatTransID: " & MaterialTransID)
			Return True
		Catch ex As Exception
			logger.Error(ex.ToString, "cMaterialTrans.AddSOLinePick Error")
			If Not IsAuto Then
				MessageBox.Show(String.Format("Error in AddSOLinePick: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			End If
			Return False
		Finally

		End Try
	End Function
	Function AddMaterialAdjustment(Material As String, LocationID As String, Reason As String, Quantity As Integer) As Boolean

		Try
			Dim mat As New cMaterial With {.cmd = cmd, .IsAuto = IsAuto}
			If Not mat.Load(Material) Then
				Return False
			End If

			cmd.CommandText = "INSERT INTO Material_Trans(Material,Location_ID,Lot,Tran_Type,Material_Trans_Date,Unit_Cost,Stock_UofM," _
				& " Cost_UofM,Reason,Quantity,Last_Updated,ObjectID)"

			Dim vals As String = AddString(Material)
			vals = vals & "," & AddString(LocationID)
			vals = vals & ",NULL" 'Lot
			vals = vals & "," & AddString("Adjustment") 'tran type
			vals = vals & "," & AddString(Now, DbType.Date) 'mattrans date
			vals = vals & ",0" 'unit cost
			vals = vals & "," & AddString(mat.StockUofM)
			vals = vals & "," & AddString(mat.CostUofM)
			vals = vals & "," & AddString(Reason)
			vals = vals & "," & AddString(Quantity, DbType.Double) 'quantity
			vals = vals & "," & AddString(Now, DbType.DateTime) 'last updated
			vals = vals & "," & AddString(System.Guid.NewGuid.ToString) 'objectid

			cmd.CommandText = cmd.CommandText & " VALUES(" & vals & ")"
			cmd.ExecuteNonQuery()

			cmd.CommandText = "SELECT Material_Trans FROM Material_Trans WHERE Material_TransKey=(SELECT Scope_Identity())"
			MaterialTransID = cmd.ExecuteScalar

			logger.Info("MatTransID: " & MaterialTransID)
			'If Reason = "Assembly" Then
			'	'/ this is a SubAssembly pick, and we need to store data in usr_SAM
			'	'/ add to the log:
			'	cmd.CommandText = "INSERT INTO usr_SAM_Trans(Material_Trans) VALUES(" & MaterialTransID & ")"
			'	cmd.ExecuteNonQuery()
			'End If

			Return True
		Catch ex As Exception
			If IsAuto Then
				logger.Info(ex.ToString)
			Else
				MessageBox.Show("Error in  MAM.cMaterialTrans.AddMaterialAdjustment: " & Environment.NewLine & Environment.NewLine & ex.ToString, "Error")
			End If
			Return False
		End Try
	End Function

	Function AddVendorTransJobReceipt() As Boolean
		Try
			'/ adds the Vendor Transaction Job receipt - this has no value in the Vendor Trans field. The second transaction will be almost identical, 
			'/ except it will include the ID from this transaction.
			Dim hdr As String = "INSERT INTO Material_Trans(Vendor,Document,Source,Location_ID,Lot,Tran_Type,Material_Trans_Date,Unit_Cost,Stock_UofM," _
					& " Cost_UofM,Reason,Quantity,Last_Updated,ObjectID)"
			Dim vals As String = AddString(Vendor)
			vals = vals & "," & AddString(Document, , "NULL")
			vals = vals & "," & AddString(SourceID, DbType.Double)
			vals = vals & ",NULL,NULL" 'loc id, lot
			vals = vals & "," & AddString(TranType)
			vals = vals & "," & AddString(Now, DbType.DateTime)
			vals = vals & ",0" 'unit code
			vals = vals & ",'ea'" 'stock uofm
			vals = vals & ",NULL,NULL" 'cost uofm, reason
			vals = vals & "," & AddString(Qty, DbType.Double)
			vals = vals & "," & AddString(Now, DbType.DateTime)
			OID = System.Guid.NewGuid.ToString.ToUpper
			vals = vals & "," & AddString(OID)

			cmd.CommandText = hdr & " VALUES(" & vals & ")"
			cmd.ExecuteNonQuery()

			cmd.CommandText = "SELECT Material_Trans FROM Material_Trans WHERE Material_TransKey=(SELECT SCOPE_IDENTITY())"
			MaterialTransID = cmd.ExecuteScalar

			Return True
		Catch ex As Exception
			logger.Error(ex.ToString, "AddJobReceipt Error")
			MessageBox.Show(String.Format("Error in AddJobReceipt: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			Return False
		Finally
		End Try

	End Function
	Function AddJobReceipt(VendorTrans As Integer) As Boolean
		Dim hdr As String = "INSERT INTO Material_Trans(Vendor,Document,Source,Location_ID,Lot,Tran_Type,Material_Trans_Date,Addl_Cost,Unit_Cost,Stock_UofM," _
					& " Cost_UofM,Reason,Quantity,Last_Updated,Vendor_Trans,ObjectID)"
		Dim vals As String = AddString(Vendor)
		vals = vals & "," & AddString(Document,, "NULL")
		vals = vals & "," & AddString(SourceID, DbType.Double)
		vals = vals & ",NULL,NULL" 'loc id, lot
		vals = vals & "," & AddString(TranType)
		vals = vals & "," & AddString(Now, DbType.DateTime) 'matransdate
		vals = vals & ",0" '& AddString(addl) addl cost
		vals = vals & ",0" 'unit cost
		vals = vals & ",'ea'" 'stock uofm
		vals = vals & ",NULL,NULL" 'cost uofm, reason
		vals = vals & "," & AddString(Qty, DbType.Double)
		vals = vals & "," & AddString(Now, DbType.DateTime)
		vals = vals & "," & AddString(VendorTrans, DbType.Double)
		OID = System.Guid.NewGuid.ToString.ToUpper
		vals = vals & "," & AddString(OID)

		cmd.CommandText = hdr & " VALUES(" & vals & ")"
		cmd.ExecuteNonQuery()

		cmd.CommandText = "SELECT Material_Trans FROM Material_Trans WHERE Material_TransKey=(SELECT SCOPE_IDENTITY())"
		MaterialTransID = cmd.ExecuteScalar

		Return True
	End Function
	Function AddInvoiceTrans() As Boolean
		Dim hdr As String = "INSERT INTO Material_Trans(Vendor,Document,Source,Location_ID,Lot,Tran_Type,Material_Trans_Date,Addl_Cost,Unit_Cost,Stock_UofM," _
					& " Purch_Unit_Weight,Cost_UofM,Reason,Quantity,Last_Updated,Vendor_Trans,Tax_Assigned,ObjectID)"
		Dim vals As String = AddString(Vendor)
		vals = vals & "," & AddString(Document,, "NULL")
		vals = vals & "," & AddString(SourceID, DbType.Double)
		vals = vals & ",NULL,NULL" 'loc id, lot
		vals = vals & "," & AddString(TranType)
		vals = vals & "," & AddString(Now, DbType.DateTime) 'matransdate
		vals = vals & ",0" '& AddString(addl) addl cost
		vals = vals & "," & UnitCost 'unit cost
		vals = vals & "," & AddString(StockUofM) 'stock uofm
		vals = vals & ",1" 'purch unit weight
		vals = vals & "," & AddString(CostUofM)
		vals = vals & ",NULL" ' reason
		vals = vals & "," & AddString(Qty, DbType.Double)
		vals = vals & "," & AddString(Now, DbType.DateTime)
		vals = vals & ",NULL" '& AddString(VendorTrans, DbType.Double)
		OID = System.Guid.NewGuid.ToString.ToUpper
		vals = vals & ",0" 'tax assigned
		vals = vals & "," & AddString(OID)

		cmd.CommandText = hdr & " VALUES(" & vals & ")"
		cmd.ExecuteNonQuery()

		cmd.CommandText = "SELECT Material_Trans FROM Material_Trans WHERE Material_TransKey=(SELECT SCOPE_IDENTITY())"
		MaterialTransID = cmd.ExecuteScalar

		Return True
	End Function
	Function AddInvoiceVendorTrans(VendorTrans As String) As Boolean
		Try
			Dim hdr As String = "INSERT INTO Material_Trans(Vendor,Document,Source,Location_ID,Lot,Tran_Type,Material_Trans_Date,Addl_Cost,Unit_Cost,Stock_UofM," _
								& " Purch_Unit_Weight,Cost_UofM,Reason,Quantity,Last_Updated,Vendor_Trans,Tax_Assigned,ObjectID)"
			Dim vals As String = AddString(Vendor)
			vals = vals & "," & AddString(Document,, "NULL")
			vals = vals & "," & AddString(SourceID, DbType.Double)
			vals = vals & ",NULL,NULL" 'loc id, lot
			vals = vals & "," & AddString(TranType)
			vals = vals & "," & AddString(Now, DbType.DateTime) 'matransdate
			vals = vals & ",0" '& AddString(addl) addl cost
			vals = vals & "," & UnitCost 'unit cost
			vals = vals & "," & AddString(StockUofM, , "NULL") 'stock uofm
			vals = vals & ",1" 'purch unit weight
			vals = vals & "," & AddString(CostUofM,, "NULL")
			vals = vals & ",NULL" ' reason
			vals = vals & "," & AddString(Qty, DbType.Double)
			vals = vals & "," & AddString(Now, DbType.DateTime)
			vals = vals & "," & AddString(VendorTrans, DbType.Double)
			OID = System.Guid.NewGuid.ToString.ToUpper
			vals = vals & ",0" 'tax assigned
			vals = vals & "," & AddString(OID)

			cmd.CommandText = hdr & " VALUES(" & vals & ")"
			cmd.ExecuteNonQuery()

			cmd.CommandText = "SELECT Material_Trans FROM Material_Trans WHERE Material_TransKey=(SELECT SCOPE_IDENTITY())"
			MaterialTransID = cmd.ExecuteScalar

			Return True
		Catch ex As Exception
			logger.Error(ex.ToString, "ConcurIntegration.cMaterialTrans.AddInvoiceVendorTrans Error")
			If Not IsAuto Then
				MessageBox.Show(String.Format("Error in ConcurIntegration.cMaterialTrans.AddInvoiceVendorTrans: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			End If
			Return False
		Finally

		End Try
	End Function
End Class
