Imports System.Data.SqlClient
Public Class cPacklistDetail
	Private logger As NLog.Logger = NLog.LogManager.GetCurrentClassLogger
	Property Packlist As String
	Property PacklistDetail As Integer
	Property SODetail As Integer
	Property SODClass As cSODetail
	Property ShipTo As Integer
	Property Quantity As Double
	Property PLHOID As String
	Property ObjectID As String
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
	Function AddPacklistDetail() As Boolean
		Try
			Dim hdr As String = "INSERT INTO Packlist_Detail(Packlist,SO_Detail,Invoice_Line,Due_Date,Tracking_Nbr,Unit_Price,Price_UofM,Quantity," _
					& "Promised_Qty,Backorder_Qty,Instructions,Note_Text,Last_Updated,Order_Unit,Price_Unit_Conv,Freight_Amt,BOL_Comments,Cartons," _
					& "Hazardous_Material,NMFC_Code,Pallets,Weight,Freight_Class,ObjectID,Packlist_OID)"

			Dim mat As New cMaterial
			If Not mat.Load(SODClass.Material, cmd) Then
				AddError("Unable to load Material: " & SODClass.Material)
				Return False
			End If

			Dim vals As String = AddString(Packlist)
			vals = vals & "," & SODClass.SODetail
			vals = vals & ",NULL,NULL,NULL" 'invoice ine, due date, tracking nbr
			vals = vals & "," & SODClass.UnitPrice
			vals = vals & "," & AddString(SODClass.PriceUoM)
			vals = vals & "," & SODClass.PickedQuantity 'quantity
			vals = vals & "," & SODClass.Quantity 'promised qty
			vals = vals & "," & SODClass.Quantity - SODClass.PickedQuantity 'backorder
			vals = vals & ",NULL,'Added by Woo Integration'"
			vals = vals & "," & AddString(Now, DbType.DateTime)
			vals = vals & "," & AddString(mat.PurchaseUofM)
			vals = vals & "," & mat.PriceUnitConversion '
			vals = vals & ",0,NULL,0,0,NULL,0,0,NULL" 'freight amt, bol, cartons, hazmats, nmfc,pallets, weight, frt classs
			ObjectID = System.Guid.NewGuid.ToString.ToUpper
			vals = vals & "," & AddString(ObjectID)
			vals = vals & "," & AddString(PLHOID)

			cmd.CommandText = hdr & " VALUES(" & vals & ")"
			cmd.ExecuteNonQuery()

			cmd.CommandText = "SELECT Packlist_Detail FROM Packlist_Detail WHERE Packlist_DetailKey=(SELECT SCOPE_IDENTITY())"
			PacklistDetail = cmd.ExecuteScalar
			Return True
		Catch ex As Exception
			logger.Error(ex.ToString, "Jobboss_Woo_Integration.cPacklistDetail.AddPacklistDetail Error")
			If Not IsAuto Then
				MessageBox.Show(String.Format("Error in Jobboss_Woo_Integration.cPacklistDetail.AddPacklistDetail: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			End If
			Return False
		Finally

		End Try





	End Function
End Class
