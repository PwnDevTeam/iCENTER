
Imports System
Imports System.Diagnostics
Imports System.Xml
Imports System.Xml.XPath
Imports System.IO

Public Class Form1

    ' TODO:
    ' Fix respring by using cmd
    ' launchctl stop com.apple.SpringBoard"

    Dim WinSCP As Process = New Process()
    Dim UsrName As String = "root"
    Dim PassWord As String = "alpine"

    Public Function SaveTextToFile(ByVal strData As String, ByVal FullPath As String, Optional ByVal ErrInfo As String = "") As Boolean
        Dim bAns As Boolean = False, objReader As StreamWriter
        Try
            objReader = New StreamWriter(FullPath)
            objReader.Write(strData)
            objReader.Close()
            bAns = True
        Catch Ex As Exception
            ErrInfo = Ex.Message
        End Try
        Return bAns
    End Function


    Sub ShellWait(ByVal file As String, ByVal arg As String)
        Dim procNlite As New Process
        winstyle = 1
        procNlite.StartInfo.FileName = file
        procNlite.StartInfo.Arguments = " " & arg
        procNlite.StartInfo.WindowStyle = winstyle
        Application.DoEvents()
        procNlite.Start()
        Do Until procNlite.HasExited
            Application.DoEvents()
            For i = 0 To 5000000
                Application.DoEvents()
            Next
        Loop
        procNlite.WaitForExit()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Button1.Enabled = False : Button2.Enabled = False : Timer1.Stop()
        Button1.Text = "Preparing Game Center.app"
        ShellWait("unzip.exe", " Game_Center.zip")

        Button1.Text = "Connecting to " & TextBox1.Text & "..."
        WinSCP.StandardInput.WriteLine("open " & TextBox1.Text)

        Button1.Text = "Logging in with information.."
        WinSCP.StandardInput.WriteLine(UsrName)
        WinSCP.StandardInput.WriteLine(PassWord)

        Button1.Text = "Intializing for Game Center.app Installation"
        WinSCP.StandardInput.WriteLine("cd /private/var/stash/Applications")

        Button1.Text = "Uploading Game Center.app"
        WinSCP.StandardInput.WriteLine("put ""Game Center.app""")

        Button1.Text = "Appyling permission(s) to Game Center.app"
        WinSCP.StandardInput.WriteLine("cd ""Game Center.app""")
        WinSCP.StandardInput.WriteLine("chmod 0777 *")

        Button1.Text = "Game Center.app was installed!" & vbCrLf & "Respring your iDevice!"
        WinSCP.StandardInput.Close()

        Dim a As String = WinSCP.StandardOutput.ReadToEnd()
        SaveTextToFile(a, "output.txt")


    End Sub


    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        End
        WinSCP.WaitForExit()         ' Wait until WinSCP finishes
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        WinSCP.StartInfo.FileName = "winscp.com"
        WinSCP.StartInfo.UseShellExecute = False
        WinSCP.StartInfo.RedirectStandardInput = True
        WinSCP.StartInfo.RedirectStandardOutput = True
        WinSCP.StartInfo.CreateNoWindow = True
        WinSCP.Start()         ' Run hidden WinSCP process

        WinSCP.StandardInput.WriteLine("option batch abort")
        WinSCP.StandardInput.WriteLine("option confirm off")

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Button1.Enabled = False : Button2.Enabled = False : Timer1.Stop()
        Button2.Text = "Connecting to " & TextBox1.Text & "..."
        WinSCP.StandardInput.WriteLine("open " & TextBox1.Text)
        Button2.Text = "Logging in with information.."
        WinSCP.StandardInput.WriteLine(UsrName)
        WinSCP.StandardInput.WriteLine(PassWord)
        Button2.Text = "Deleting Game Center.app"
        WinSCP.StandardInput.WriteLine("rmdir ""/private/var/stash/Applications/Game Center.app""")
        '   rm -f "/private/var/stash/Applications/Game Center.app"
        Button2.Text = "Game Center.app was deleted!" & vbCrLf & "Respring your iDevice!"
        WinSCP.StandardInput.Close()

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If (TextBox1.Text = "Enter IP for iDevice Here!") Then
            Button1.Enabled = False
            Button2.Enabled = False
        Else
            Button1.Enabled = True
            Button2.Enabled = True
        End If
    End Sub

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label2.Click
        Process.Start("http://twitter.com/Fallensn0w")
    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click
        MsgBox("This is because iCENTER will connect through SSH and install Game Center application to your iDevice. " & vbCrLf & vbCrLf & "Please, use Cydia and install these before trying this. Otherwise you will not succeed the installation!", MsgBoxStyle.OkOnly, Me.Text)
    End Sub

End Class

