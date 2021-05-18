Imports System.Data.SqlClient
Public Class cEmployee

	Private logger As NLog.Logger = NLog.LogManager.GetCurrentClassLogger

	Property Employee As String
	Property CommissionPct As Double
	Function FillClass(dtr As DataRow) As Boolean

		Employee = dtr("Employee")
		CommissionPct = dtr("Commission_Pct")
		Return True

	End Function
	Function Load(Employee As String) As Boolean
		Try
			Using con As New SqlConnection(JBConnection)
				con.Open()
				Using cmd As New SqlCommand
					cmd.Connection = con
					cmd.CommandText = "SELECT * FROM Employee WHERE Employee=" & AddString(Employee)
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
		Catch ex As Exception
			logger.Error(ex.ToString, "Load Error")
			MessageBox.Show(String.Format("Error in Load: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			Return False
		Finally
		End Try
	End Function
	Function Load(Employee As String, cmd As SqlCommand) As Boolean

		Try
			cmd.CommandText = "SELECT * FROM Employee WHERE Employee=" & AddString(Employee)
			Using dt As New DataTable
				dt.Load(cmd.ExecuteReader)
				If dt.Rows.Count > 0 Then
					Dim dtr As DataRow = dt.Rows(0)
					Return FillClass(dtr)
				Else
					Return False
				End If
			End Using
		Catch ex As Exception
			logger.Error(ex.ToString, "Load Error")
			MessageBox.Show(String.Format("Error in Load: {0}{1}{2}", Environment.NewLine, Environment.NewLine, ex), "Error")
			Return False
		Finally
		End Try

	End Function
End Class
