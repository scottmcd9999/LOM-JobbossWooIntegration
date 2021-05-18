Imports System.Data.SqlClient
Public Class cPacklist_Header
	Private logger As NLog.Logger = NLog.LogManager.GetCurrentClassLogger

	Public Sub New()
		PlDetails = New List(Of cPacklistDetail)
	End Sub

	Property Packlist As String
	Property PlDetails As List(Of cPacklistDetail)
	Property Customer As String
	Property Contact As Integer
	Property ShipTo As Integer
	Property ShipVia As String
	Property PacklistDate As Date
	Property PacklistType As String
	Property PLHOID As String
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
	Function AddPacklistHeader(IsAuto As Boolean) As Boolean
		Try
			Dim hdr As String = "INSERT INTO Packlist_Header(Packlist,Customer_Vendor,Invoice,Ship_To,Ship_Via,Contact,Packlist_Date,Type,Invoiced," _
					& " Comment,Last_Updated,Freight_Amt,BOL_Number,Carrier_Bill_To,BOL_Terms,Third_Party_Acct,ObjectID)"

			Packlist = NextPacklistNumber(cmd, IsAuto)
			If Packlist = "0" Then
				AddError("Unable to get new Packlist Number")
				Return False
			End If

			Dim vals As String = AddString(Packlist)
			vals = vals & "," & AddString(Customer)
			vals = vals & ",0"
			vals = vals & "," & ShipTo
			vals = vals & "," & AddString(ShipVia,, "NULL")
			vals = vals & "," & Contact
			vals = vals & "," & AddString(PacklistDate, DbType.Date)
			vals = vals & "," & AddString(PacklistType,, "SOShip")
			vals = vals & ",0"
			vals = vals & ",'Added by Woo Integration'"
			vals = vals & "," & AddString(Now, DbType.DateTime)
			vals = vals & ",0,NULL,NULL,NULL,NULL"
			PLHOID = System.Guid.NewGuid.ToString.ToUpper
			vals = vals & "," & AddString(PLHOID)

			cmd.CommandText = hdr & " VALUES(" & vals & ")"
			cmd.ExecuteNonQuery()

			cmd.CommandText = "UPDATE Auto_Number SET Last_Nbr='" & Packlist & "' WHERE Type='Packlist'"
			cmd.ExecuteNonQuery()

			Return True
		Catch ex As Exception
			logger.Error(ex.ToString, "Jobboss_Woo_Integration.cPacklist_Header.AddPacklistHeader Error")
			AddError(ex.ToString)
			If Not IsAuto Then
				MessageBox.Show(String.Format("Error in Jobboss_Woo_Integration.cPacklist_Header.AddPacklistHeader: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			End If
			Return False
		Finally

		End Try



	End Function
End Class
