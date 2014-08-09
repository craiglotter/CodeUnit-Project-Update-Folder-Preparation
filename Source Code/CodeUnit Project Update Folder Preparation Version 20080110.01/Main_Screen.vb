Imports System.IO

Public Class Main_Screen

    Private busyworking As Boolean = False
    Private AutoUpdate As Boolean = False

    Private projectscount As Integer = 0
    Private projectsdone As Integer = 0

    Private updatelist1 As UpdateList

    Private CompletedProjects As String

    Private Sub Error_Handler(ByVal ex As Exception, Optional ByVal identifier_msg As String = "")
        Try
            If ex.Message.IndexOf("Thread was being aborted") < 0 Then
                Dim Display_Message1 As New Display_Message()
                Display_Message1.Message_Textbox.Text = "The Application encountered the following problem: " & vbCrLf & identifier_msg & ": " & ex.ToString
                Display_Message1.Timer1.Interval = 1000
                Display_Message1.ShowDialog()
                Dim dir As System.IO.DirectoryInfo = New System.IO.DirectoryInfo((Application.StartupPath & "\").Replace("\\", "\") & "Error Logs")
                If dir.Exists = False Then
                    dir.Create()
                End If
                dir = Nothing
                Dim filewriter As System.IO.StreamWriter = New System.IO.StreamWriter((Application.StartupPath & "\").Replace("\\", "\") & "Error Logs\" & Format(Now(), "yyyyMMdd") & "_Error_Log.txt", True)
                filewriter.WriteLine("#" & Format(Now(), "dd/MM/yyyy hh:mm:ss tt") & " - " & identifier_msg & ": " & ex.ToString)
                filewriter.WriteLine("")
                filewriter.Flush()
                filewriter.Close()
                filewriter = Nothing
            End If
        Catch exc As Exception
            MsgBox("An error occurred in the application's error handling routine. The application will try to recover from this serious error." & vbCrLf & vbCrLf & exc.ToString, MsgBoxStyle.Critical, "Critical Error Encountered")
        End Try
    End Sub

    Private Sub Activity_Handler(ByVal message As String)
        Try
            Dim dir As System.IO.DirectoryInfo = New System.IO.DirectoryInfo((Application.StartupPath & "\").Replace("\\", "\") & "Activity Logs")
            If dir.Exists = False Then
                dir.Create()
            End If
            dir = Nothing
            Dim filewriter As System.IO.StreamWriter = New System.IO.StreamWriter((Application.StartupPath & "\").Replace("\\", "\") & "Activity Logs\" & Format(Now(), "yyyyMMdd") & "_Activity_Log.txt", True)
            filewriter.WriteLine("#" & Format(Now(), "dd/MM/yyyy hh:mm:ss tt") & " - " & message)
            filewriter.WriteLine("")
            filewriter.Flush()
            filewriter.Close()
            filewriter = Nothing
        Catch ex As Exception
            Error_Handler(ex, "Activity Handler")
        End Try
    End Sub

    Private Sub Main_Screen_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Control.CheckForIllegalCrossThreadCalls = False
            Me.Text = My.Application.Info.ProductName & " (" & Format(My.Application.Info.Version.Major, "0000") & Format(My.Application.Info.Version.Minor, "00") & Format(My.Application.Info.Version.Build, "00") & "." & Format(My.Application.Info.Version.Revision, "00") & ")"
            updatelist1 = New UpdateList
            loadSettings()
        Catch ex As Exception
            Error_Handler(ex, "Application Loading")
        End Try
    End Sub

    Private Sub loadSettings()
        Try

            Dim configfile As String = (Application.StartupPath & "\config.sav").Replace("\\", "\")
            If My.Computer.FileSystem.FileExists(configfile) Then
                Dim reader As StreamReader = New StreamReader(configfile)
                Dim lineread As String
                Dim variablevalue As String
                While reader.Peek <> -1
                    lineread = reader.ReadLine
                    If lineread.IndexOf("=") <> -1 Then
                        variablevalue = lineread.Remove(0, lineread.IndexOf("=") + 1)
                        If lineread.StartsWith("CompletedProjects=") Then
                            CompletedProjects = variablevalue
                        End If
                    End If
                End While
                reader.Close()
                reader = Nothing
            End If
        Catch ex As Exception
            Error_Handler(ex, "Load Settings")
        End Try
    End Sub

    Private Sub SaveSettings()
        Try
            Dim configfile As String = (Application.StartupPath & "\config.sav").Replace("\\", "\")
            Dim writer As StreamWriter = New StreamWriter(configfile, False)
            writer.WriteLine("CompletedProjects=" & CompletedProjects)
            writer.Flush()
            writer.Close()
            writer = Nothing

        Catch ex As Exception
            Error_Handler(ex, "Save Settings")
        End Try
    End Sub

    Private Sub Main_Screen_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        Try
            SaveSettings()
            If AutoUpdate = True Then
                If My.Computer.FileSystem.FileExists((Application.StartupPath & "\AutoUpdate.exe").Replace("\\", "\")) = True Then
                    Dim startinfo As ProcessStartInfo = New ProcessStartInfo
                    startinfo.FileName = (Application.StartupPath & "\AutoUpdate.exe").Replace("\\", "\")
                    startinfo.Arguments = "force"
                    startinfo.CreateNoWindow = False
                    Process.Start(startinfo)
                End If
            End If
        Catch ex As Exception
            Error_Handler(ex, "Closing Application")
        End Try
    End Sub

    Private Sub HelpToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HelpToolStripMenuItem1.Click
        Try
            HelpBox1.ShowDialog()
        Catch ex As Exception
            Error_Handler(ex, "Display Help Screen")
        End Try
    End Sub

    Private Sub AutoUpdateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoUpdateToolStripMenuItem.Click
        Try
            AutoUpdate = True
            Me.Close()
        Catch ex As Exception
            Error_Handler(ex, "AutoUpdate")
        End Try
    End Sub

    Private Sub AboutToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem1.Click
        Try

            AboutBox1.ShowDialog()
        Catch ex As Exception
            Error_Handler(ex, "Display About Screen")
        End Try
    End Sub

    Private Sub Control_Enabler(ByVal IsEnabled As Boolean)
        Try
            Select Case IsEnabled
                Case True
                    Button1.Enabled = True
                    MenuStrip1.Enabled = True
                    Me.ControlBox = True
                Case False
                    Button1.Enabled = False
                    MenuStrip1.Enabled = False
                    Me.ControlBox = False
            End Select
        Catch ex As Exception
            Error_Handler(ex, "Control Enabler")
        End Try
    End Sub

    Private Sub UpdateProject(ByVal ProjectPath As String)
        Try
            Dim dinfo As DirectoryInfo = New DirectoryInfo(ProjectPath)
            StatusLabel.Text = "Updating " & dinfo.Name
            Dim currentbuild As String = ""
            Dim nextbuild As String = ""
            Dim releasenumber As String = ""
            Dim nextreleasenumber As String = ""
            If My.Computer.FileSystem.FileExists((dinfo.FullName & "\Build.txt").Replace("\\", "\")) Then
                currentbuild = My.Computer.FileSystem.ReadAllText((dinfo.FullName & "\Build.txt").Replace("\\", "\"))
                currentbuild = currentbuild.Trim
                releasenumber = currentbuild.Substring(currentbuild.Length - 2)
                nextreleasenumber = Integer.Parse(releasenumber) + 1
                If nextreleasenumber.Length < 2 Then
                    nextreleasenumber = "0" & nextreleasenumber
                End If
                nextbuild = Format(Now, "yyyyMMdd") & "." & nextreleasenumber
                Dim writedouble As Boolean = False
                Dim reader As StreamReader = New StreamReader((dinfo.FullName & "\Description.txt").Replace("\\", "\"))
                If (reader.ReadToEnd.EndsWith(vbCrLf)) = False Then
                    writedouble = True
                End If
                reader.Close()
                reader = Nothing
                Dim writer As StreamWriter = New StreamWriter((dinfo.FullName & "\Description.txt").Replace("\\", "\"), True)
                If writedouble = True Then
                    writer.WriteLine()
                End If
                writer.WriteLine()
                writer.WriteLine("*********************************")
                writer.WriteLine()
                writer.WriteLine("Update " & nextbuild & ":")
                writer.WriteLine()
                writer.WriteLine("- ")
                writer.Flush()
                writer.Close()
                ProgressBar1.Value = 20
                writer = New StreamWriter((dinfo.FullName & "\Build.txt").Replace("\\", "\"), False)
                writer.WriteLine(nextbuild)
                writer.Flush()
                writer.Close()
                writer = Nothing
                ProgressBar1.Value = 40
                My.Computer.FileSystem.DeleteDirectory((dinfo.FullName & "\Release").Replace("\\", "\"), FileIO.DeleteDirectoryOption.DeleteAllContents)
                My.Computer.FileSystem.CreateDirectory((dinfo.FullName & "\Release").Replace("\\", "\"))
                ProgressBar1.Value = 60
                Dim projectname As String = ""
                projectname = dinfo.Name.Substring(0, dinfo.Name.LastIndexOf("-")).Trim
                My.Computer.FileSystem.CopyDirectory((dinfo.FullName & "\Source Code\" & projectname).Replace("\\", "\"), (dinfo.FullName & "\Source Code\" & projectname & " Version " & currentbuild).Replace("\\", "\"), True)
                ProgressBar1.Value = 80
                My.Computer.FileSystem.CopyFile((dinfo.FullName & "\Description.txt").Replace("\\", "\"), (dinfo.FullName & "\Source Code\" & projectname & "\Required Files\Description.txt").Replace("\\", "\"), True)
                ProgressBar1.Value = 100
            End If
            dinfo = Nothing
        Catch ex As Exception
            Error_Handler(ex, "Update Project (" & ProjectPath & ")")
        End Try
    End Sub

    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            StatusLabel.Text = "Loading project list"
            updatelist1.CleanProjects()
            Dim di As DirectoryInfo = New DirectoryInfo(CompletedProjects)
            For Each dinfo As DirectoryInfo In di.GetDirectories()
                updatelist1.AddProject(dinfo.Name, False)
                dinfo = Nothing
            Next
            StatusLabel.Text = "Select the project folders to prepare"
            If updatelist1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                For Each dinfo As DirectoryInfo In di.GetDirectories
                    If updatelist1.CheckProjectStatus(dinfo.Name) = True Then
                        projectscount = projectscount + 1
                    End If
                    dinfo = Nothing
                Next
                For Each dinfo As DirectoryInfo In di.GetDirectories
                    If updatelist1.CheckProjectStatus(dinfo.Name) = True Then
                        UpdateProject(dinfo.FullName)
                        projectsdone = projectsdone + 1
                        ProgressBar2.Value = CSng(projectsdone) / CSng(projectscount) * 100
                    End If
                    dinfo = Nothing
                Next
            Else
                e.Cancel = True
            End If
            di = Nothing
        Catch ex As Exception
            Error_Handler(ex, "Prepare Folder")
        End Try
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Try
            Control_Enabler(True)
            If e.Cancelled = False And e.Error Is Nothing Then
                StatusLabel.Text = "Your project folders have been prepared for updating"
            Else
                StatusLabel.Text = "Preparation of your project folders has failed"
            End If
        Catch ex As Exception
            Error_Handler(ex, "Project Folders Prepared")
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            If My.Computer.FileSystem.DirectoryExists(CompletedProjects) = True Then
                FolderBrowserDialog1.SelectedPath = CompletedProjects
            End If
            StatusLabel.Text = ("Select your Completed Projects folder")
            If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                CompletedProjects = FolderBrowserDialog1.SelectedPath
                Control_Enabler(False)
                ProgressBar1.Value = 0
                ProgressBar2.Value = 0
                projectscount = 0
                projectsdone = 0
                BackgroundWorker1.RunWorkerAsync()
            Else
                StatusLabel.Text = "Folder preparation operation aborted"
            End If
        Catch ex As Exception
            Error_Handler(ex, "Begin Preparation Operation")
        End Try
    End Sub

  
End Class
