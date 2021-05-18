Imports DevExpress.XtraEditors.Controls
Imports System.Data.SqlClient
Public Class ucSetup
    Private logger As NLog.Logger = NLog.LogManager.GetCurrentClassLogger
    Property fLoading As Boolean
    Function LoadJobbossCustomers() As Boolean
        Using con As New SqlConnection(JBConnection)
            con.Open()
            Using cmd As New SqlCommand
                cmd.Connection = con
                cmd.CommandText = "SELECT Customer FROM Customer ORDER BY Customer"
                Using dt As New DataTable
                    dt.Load(cmd.ExecuteReader)
                    With cbJobbossCustomer
                        .Properties.DataSource = dt
                        .Properties.DropDownRows = 25
                        .Properties.DisplayMember = "Customer"
                        .Properties.ValueMember = "Customer"

                    End With
                End Using
            End Using
        End Using
    End Function

    Function TestFTP() As Boolean
        Dim ftp As New cFTP
        With ftp
            .FTPServer = txFTPServer.EditValue
            .FTPPort = txFTPPort.EditValue
            .FTPUser = txFTPUser.EditValue
            .FTPPass = txSSHPass.EditValue
            .FTPFolder = txFTPFolder.EditValue
            .LocalFolder = txLocalFolder.EditValue
            Return .DownloadFile
        End With
    End Function

    Private Sub ucSetup_Load(sender As Object, e As EventArgs) Handles Me.Load
        fLoading = True
        Dim c As New cSettings
        txLocalFolder.EditValue = c.ReadSetting("LocalFolder")
        txFTPFolder.EditValue = c.ReadSetting("FTPFolder")
        txFTPPort.EditValue = c.ReadSetting("FTPPort")
        txFTPServer.EditValue = c.ReadSetting("FTPServer")
        txFTPUser.EditValue = c.ReadSetting("FTPUser")
        txSSHPass.EditValue = c.ReadSetting("FTPPass")
        LoadJobbossCustomers()
        cbJobbossCustomer.EditValue = c.ReadSetting("JobbossCustomer")
        fLoading = False
    End Sub
    Private Sub txLocalFolder_EditValueChanged(sender As Object, e As EventArgs) Handles txLocalFolder.EditValueChanged
        Dim c As New cSettings
        c.WriteSetting("LocalFolder", txLocalFolder.EditValue)
    End Sub

    Private Sub txLocalFolder_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles txLocalFolder.ButtonClick
        Using fb As New DevExpress.XtraEditors.XtraFolderBrowserDialog
            With fb
                .Description = "Locate Local Folders"
                .DialogStyle = DevExpress.Utils.CommonDialogs.FolderBrowserDialogStyle.Wide
                If txLocalFolder.Text.Length > 0 Then
                    .SelectedPath = txLocalFolder.Text
                End If
                If .ShowDialog Then
                    txLocalFolder.EditValue = .SelectedPath
                End If
            End With
        End Using
    End Sub

    Private Sub txFTPServer_EditValueChanged(sender As Object, e As EventArgs) Handles txFTPServer.EditValueChanged
        Dim c As New cSettings
        c.WriteSetting("FTPServer", txFTPServer.EditValue.ToString)
    End Sub

    Private Sub txFTPPort_EditValueChanged(sender As Object, e As EventArgs) Handles txFTPPort.EditValueChanged
        Dim c As New cSettings
        c.WriteSetting("FTPPort", txFTPPort.EditValue.ToString)
    End Sub

    Private Sub txFTPUser_EditValueChanged(sender As Object, e As EventArgs) Handles txFTPUser.EditValueChanged
        Dim c As New cSettings
        c.WriteSetting("FTPUser", txFTPUser.EditValue.ToString)
    End Sub

    Private Sub txFTPFolder_EditValueChanged(sender As Object, e As EventArgs) Handles txFTPFolder.EditValueChanged
        Dim c As New cSettings
        c.WriteSetting("FTPFolder", txFTPFolder.EditValue.ToString)
    End Sub

    Private Sub txSSHPass_EditValueChanged(sender As Object, e As EventArgs) Handles txSSHPass.EditValueChanged
        Dim c As New cSettings
        c.WriteSetting("FTPPass", txSSHPass.EditValue.ToString, True)
    End Sub



    Private Sub txSSHPass_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles txSSHPass.ButtonClick
        txSSHPass.Properties.UseSystemPasswordChar = Not txSSHPass.Properties.UseSystemPasswordChar
    End Sub

    Private Sub cbJobbossCustomer_EditValueChanged(sender As Object, e As EventArgs) Handles cbJobbossCustomer.EditValueChanged
        If fLoading Then
            Return
        End If
        Dim c As New cSettings
        c.WriteSetting("JobbossCustomer", cbJobbossCustomer.EditValue.ToString)

    End Sub
End Class
