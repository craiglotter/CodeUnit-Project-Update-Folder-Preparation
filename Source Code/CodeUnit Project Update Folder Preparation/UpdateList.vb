Public Class UpdateList

    Dim checkboxcount As Integer = 0

    Private Sub Error_Handler(ByVal ex As Exception, Optional ByVal identifier_msg As String = "")
        Try
            If My.Computer.FileSystem.FileExists((Application.StartupPath & "\Sounds\UHOH.WAV").Replace("\\", "\")) = True Then
                My.Computer.Audio.Play((Application.StartupPath & "\Sounds\UHOH.WAV").Replace("\\", "\"), AudioPlayMode.Background)
            End If
            Dim Display_Message1 As New Display_Message()
            Display_Message1.Message_Textbox.Text = "The Application encountered the following problem: " & vbCrLf & identifier_msg & ":" & ex.Message.ToString
            Display_Message1.Timer1.Interval = 1000
            Display_Message1.ShowDialog()
            If My.Computer.FileSystem.DirectoryExists((Application.StartupPath & "\").Replace("\\", "\") & "Error Logs") = False Then
                My.Computer.FileSystem.CreateDirectory((Application.StartupPath & "\").Replace("\\", "\") & "Error Logs")
            End If
            Dim filewriter As System.IO.StreamWriter = New System.IO.StreamWriter((Application.StartupPath & "\").Replace("\\", "\") & "Error Logs\" & Format(Now(), "yyyyMMdd") & "_Error_Log.txt", True)
            filewriter.WriteLine("#" & Format(Now(), "dd/MM/yyyy hh:mm:ss tt") & " - " & identifier_msg & ":" & ex.ToString)
            filewriter.Flush()
            filewriter.Close()
            filewriter = Nothing
        Catch exc As Exception
            MsgBox("An error occurred in the application's error handling routine. The application will try to recover from this serious error.", MsgBoxStyle.Critical, "Critical Error Encountered")
        End Try
    End Sub

    Public Function CheckProjectStatus(ByVal name As String) As Boolean
        Dim result As Boolean = False
        Try
            For Each Control1 As CheckBox In Me.Panel1.Controls
                If Control1.Text = name Then
                    result = Control1.Checked
                End If
            Next
        Catch ex As Exception
            Error_Handler(ex, "CheckProjectStatus")
        End Try
        Return result
    End Function

    Public Sub CleanProjects()
        Try
            Me.Panel1.Controls.Clear()
            checkboxcount = 0
        Catch ex As Exception
            Error_Handler(ex, "Clean Project Checkboxes")
        End Try
    End Sub

    Public Sub AddProject(ByVal name As String, Optional ByVal selected As Boolean = True)
        Try
            checkboxcount = checkboxcount + 1
            Dim CheckBox1 As CheckBox = New CheckBox()
            CheckBox1.AutoSize = True
            CheckBox1.Location = New System.Drawing.Point(12, (19 * checkboxcount) - 17)
            CheckBox1.Name = "Project" & checkboxcount
            CheckBox1.Size = New System.Drawing.Size(81, 17)
            CheckBox1.TabIndex = checkboxcount
            CheckBox1.Text = name
            CheckBox1.UseVisualStyleBackColor = True
            CheckBox1.Checked = selected

            Me.Panel1.Controls.Add(CheckBox1)

        Catch ex As Exception
            Error_Handler(ex, "Add Project Checkboxes")
        End Try
    End Sub

    Private Sub UpdateList_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        checkboxcount = 0
    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            For Each Control1 As CheckBox In Me.Panel1.Controls
                Control1.Checked = True
            Next
        Catch ex As Exception
            Error_Handler(ex, "Select all Projects")
        End Try
    End Sub


    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            For Each Control1 As CheckBox In Me.Panel1.Controls
                Control1.Checked = False
            Next
        Catch ex As Exception
            Error_Handler(ex, "Deselect all Projects")
        End Try
    End Sub
End Class