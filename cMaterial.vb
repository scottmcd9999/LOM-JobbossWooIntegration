Imports System.Data.SqlClient
Public Class cMaterial
	Private logger As NLog.Logger = NLog.LogManager.GetCurrentClassLogger
	Property cmd As SqlCommand
	Property Material As String
	Property Sales_Code As String
	Property Description As String
	Property PickBuy As String
	Property Rev As String
	Property StockUofM As String
	Property PurchaseUofM As String
	Property PurchaseUnitWeight As Integer
	Property CostUofM As String
	Property PriceUofM As String
	Property LastCost As Double
	Property StandardCost As Double
	Property AverageCost As Double
	Property SellingPrice As Double
	Property ExtendedDesc As String
	Property MaterialType As String
	Property Status As String
	Property MaterialClass As String
	Property UofMConversionFactor As Double
	Property PriceUnitConversion As Double
	Property Taxable As Boolean
	Property LeadDays As Integer
	Property IsAuto As Boolean
	Property Errors As String

	Private Sub AddError(ErrorMsg As String)
		If Errors Is Nothing Then
			Errors = ErrorMsg
		Else
			Errors = Errors & Environment.NewLine & ErrorMsg
		End If
	End Sub

	Private Function FillClass(dtr As DataRow) As Boolean
		Try
			Me.Material = dtr("Material")
			Sales_Code = dtr("Sales_Code") & ""
			Description = dtr("Description") & ""
			PickBuy = dtr("Pick_Buy_Indicator") & ""
			Rev = dtr("Rev") & ""
			StockUofM = dtr("Stocked_UofM") & ""
			PurchaseUofM = dtr("Purchase_UofM") & ""
			CostUofM = dtr("Cost_UofM") & ""
			PriceUofM = dtr("Price_UofM") & ""
			LastCost = dtr("Last_Cost")
			AverageCost = dtr("Average_Cost")
			StandardCost = dtr("Standard_Cost")
			ExtendedDesc = dtr("Ext_Description") & ""
			MaterialType = dtr("Type") & ""
			Status = dtr("Status") & ""
			MaterialClass = dtr("Class") & ""
			UofMConversionFactor = dtr("UofM_Conv_Factor")
			SellingPrice = dtr("Selling_Price")
			Taxable = dtr("Taxable")
			PriceUnitConversion = dtr("Price_Unit_Conv")
			LeadDays = dtr("Lead_Days")
			Return True
		Catch ex As Exception
			logger.Error(ex.ToString, "LoadClass Error")
			Return False 'MessageBox.Show(String.Format("Error in LoadClass: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
		Finally
		End Try
	End Function

	Function Load(Material As String, cmd As SqlCommand) As Boolean
		Try
			cmd.CommandText = "SELECT * FROM Material WHERE Material=" & AddString(Material)
			Using dt As New DataTable
				dt.Load(cmd.ExecuteReader)
				If dt.Rows.Count > 0 Then
					Dim dtr As DataRow = dt.Rows(0)
					Return FillClass(dtr)
				Else
					Return False
				End If
			End Using
			Return True
		Catch ex As Exception
			logger.Error(ex.ToString, "MMT_VOI_SP.cMaterial.Load Error")
			MessageBox.Show(String.Format("Error in Load: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			Return False
		Finally
		End Try

	End Function
	Function Load(Material As String) As Boolean
		Try
			Using con As New SqlConnection(JBConnection)
				con.Open()
				Using cmd As New SqlCommand
					cmd.Connection = con
					cmd.CommandText = "SELECT * FROM Material WHERE Material=" & AddString(Material)
					Using dt As New DataTable
						dt.Load(cmd.ExecuteReader)
						If dt.Rows.Count > 0 Then
							Dim dtr As DataRow = dt.Rows(0)
							Return FillClass(dtr)
						Else
							Return False
						End If
					End Using
				End Using
			End Using
			Return True
		Catch ex As Exception
			logger.Error(ex.ToString, "MMT_VOI_SP.cMaterial.Load Error")
			MessageBox.Show(String.Format("Error in Load: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			Return False
		Finally
		End Try

	End Function

	Function AdjustStockLevel(Location As String, Quantity As Double, Optional Lot As String = "") As Boolean

		Try
			cmd.CommandText = "SELECT * FROM Material_Location WHERE Material='" & Material & "' AND Location_ID='" & Location & "'"
			If Lot.Length > 0 Then
				cmd.CommandText = cmd.CommandText & " AND Lot='" & Lot & "'"
			End If

			Using dt As New DataTable
				dt.Load(cmd.ExecuteReader)
				If dt.Rows.Count > 0 Then
					Dim dtr As DataRow = dt.Rows(0)
					If dtr("On_Hand_Qty") <= Quantity Then
						'/ delete the location:
						cmd.CommandText = "DELETE FROM Material_Location WHERE Material_Location=" & dtr("Material_Location")
					Else
						'/ modify the on hand qty:
						cmd.CommandText = "UPDATE Material_Location SET On_Hand_Qty=(On_Hand_Qty-" & Quantity & "), Last_Updated=" & AddString(Now, DbType.DateTime) _
							& " WHERE Material_Location=" & dtr("Material_Location")
					End If
					cmd.ExecuteNonQuery()
					Return True
				Else
					Return False
				End If
			End Using
		Catch ex As Exception
			logger.Error(ex.ToString, "MMT_VOI_SP.cMaterial.AdjustStockLevel Error")
			MessageBox.Show(String.Format("Error in AdjustStockLevel: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			Return False
		Finally
		End Try
	End Function
	Function GetQuantityPriceForMaterial(Quantity As Integer, cmd As SqlCommand) As Double
		Try
			Dim res As Double '= SellingPrice
			cmd.CommandText = "SELECT Minimum_Qty, Sell_Price FROM Price_Break WHERE Material=" & AddString(Material) & " ORDER BY Minimum_Qty"
			Using dt As New DataTable
				dt.Load(cmd.ExecuteReader)
				If dt.Rows.Count > 0 Then
					For Each dtr As DataRow In dt.Rows
						If dtr("Minimum_Qty") <= Quantity Then
							res = dtr("Sell_Price")
						End If
					Next
				End If
			End Using

			Return res

		Catch ex As Exception
			logger.Error(ex.ToString, "MMT_VOI_SP.cMaterial.GetQuantityPriceForMaterial Error")
			If Not IsAuto Then
				MessageBox.Show(String.Format("Error in GetQuantityPriceForMaterial: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			End If
			Return 0
		Finally

		End Try
	End Function
	Function GetQuantityPriceForMaterial(Quantity As Integer) As Double
		Try
			Using con As New SqlConnection(JBConnection)
				con.Open()
				Using cmd As New SqlCommand
					cmd.Connection = con
					Dim res As Double = SellingPrice
					cmd.CommandText = "SELECT Minimum_Qty, Sell_Price FROM Price_Break WHERE Material=" & AddString(Material) & " ORDER BY Minimum_Qty"
					Using dt As New DataTable
						dt.Load(cmd.ExecuteReader)
						If dt.Rows.Count > 0 Then
							For Each dtr As DataRow In dt.Rows
								If dtr("Minimum_Qty") >= Quantity Then
									res = dtr("Sell_Price")
								End If
							Next
						End If
					End Using

					Return res
				End Using
			End Using
		Catch ex As Exception
			logger.Error(ex.ToString, "MMT_VOI_SP.cMaterial.GetQuantityPriceForMaterial Error")
			If Not IsAuto Then
				MessageBox.Show(String.Format("Error in GetQuantityPriceForMaterial: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			End If
			Return 0
		Finally

		End Try
	End Function

End Class
