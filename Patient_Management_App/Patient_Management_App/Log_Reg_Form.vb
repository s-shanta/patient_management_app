Imports System.IO
Imports MySql.Data.MySqlClient

Public Class Log_Reg_Form

    Private Sub ButtonClose_Click(sender As Object, e As EventArgs) Handles ButtonClose.Click
        Close()
    End Sub

    Private Sub ButtonMinimize_Click(sender As Object, e As EventArgs) Handles ButtonMinimize.Click
        WindowState = FormWindowState.Minimized
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Panel2.Location.X > -405 Then
            Panel2.Location = New Point(Panel2.Location.X - 10, Panel2.Location.Y)
        Else
            Timer1.Stop()
            LabelGoToLogin.Enabled = True
            LabelGoToRegister.Enabled = True
        End If
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        If Panel2.Location.X < 0 Then
            Panel2.Location = New Point(Panel2.Location.X + 10, Panel2.Location.Y)
        Else
            Timer2.Stop()
            LabelGoToLogin.Enabled = True
            LabelGoToRegister.Enabled = True
        End If
    End Sub

    Private Sub LabelGoToRegister_Click(sender As Object, e As EventArgs) Handles LabelGoToRegister.Click
        Timer1.Start()
        LabelGoToLogin.Enabled = False
        LabelGoToRegister.Enabled = False
    End Sub

    Private Sub LabelGoToLogin_Click(sender As Object, e As EventArgs) Handles LabelGoToLogin.Click
        Timer2.Start()
        LabelGoToLogin.Enabled = False
        LabelGoToRegister.Enabled = False
    End Sub

    Private Sub ButtonBrowse_Click(sender As Object, e As EventArgs) Handles Button2.Click

    End Sub

    Private Sub ButtonBrowse_Click_1(sender As Object, e As EventArgs) Handles ButtonBrowse.Click
        Dim opf As New OpenFileDialog

        opf.Filter = "Select Image(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif"

        If opf.ShowDialog() = Windows.Forms.DialogResult.OK Then
            PictureBoxProfileImage.Image = Image.FromFile(opf.FileName)
        End If

    End Sub

    Private Sub Button_Login_Click(sender As Object, e As EventArgs) Handles Button_Login.Click

        'login

        Dim mydb As New MY_DB
        Dim adapter As New MySqlDataAdapter
        Dim table As New DataTable

        Dim command As MySqlCommand = New MySqlCommand("SELECT * FROM `user` WHERE `username`=@un AND `pass`=@pass", mydb.getConnection)

        If verifyFiels("login") Then

            command.Parameters.Add("@un", MySqlDbType.VarChar).Value = TextBoxUserName.Text
            command.Parameters.Add("@pass", MySqlDbType.VarChar).Value = TextBoxPassword.Text

            adapter.SelectCommand = command
            adapter.Fill(table)

            If table.Rows.Count > 0 Then

                'show the main form and close this one
                MessageBox.Show("Logged in Successfully ! ", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Dim mf As New MainForm
                mf.Show()
                Me.Close()

            Else

                MessageBox.Show(" Invalid UserName or Password ", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Else

            MessageBox.Show(" Empty UserName or Password ", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End If
    End Sub

    Private Sub Button_Register_Click(sender As Object, e As EventArgs) Handles Button_Register.Click
        'register

        Dim fname As String = TextBoxFName.Text
        Dim lname As String = TextBoxLName.Text
        Dim username As String = TextBoxUsernameRegister.Text
        Dim password As String = TextBoxPasswordRegister.Text
        Dim user As New USER

        ' user verifyFields function

        If verifyFiels("register") Then
            Dim pic As MemoryStream = New MemoryStream
            PictureBoxProfileImage.Image.Save(pic, PictureBoxProfileImage.Image.RawFormat)

            'check if the username already exists
            If Not user.userNameExists(username) Then

                ' add the new user 

                If user.insertUser(fname, lname, username, password, pic) Then

                    MessageBox.Show("Registration Completed Successfully ! ", "Register", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("Something is Wrong ! Registration Not Completed !!! ", "Register", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
            Else
                MessageBox.Show("This username already exists !!!", "Invalid UserName", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Else
            MessageBox.Show("* Required Fields - Username / Password / Image", "Registration", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub


    'create a function to check empty fields
    Public Function verifyFiels(ByVal operation As String) As Boolean
        Dim check As Boolean = False

        'if it is a register operation 

        If operation = "register" Then

            If ((TextBoxUsernameRegister.Text.Trim().Equals("")) _
                OrElse (TextBoxPasswordRegister.Text.Trim().Equals("")) _
                OrElse (PictureBoxProfileImage.Image Is Nothing)) Then

                check = False
            Else
                check = True
            End If

        End If

        'if it is a login operation 

        If operation = "login" Then

            If ((TextBoxUserName.Text.Trim().Equals("")) OrElse (TextBoxPassword.Text.Trim().Equals(""))) Then

                check = False
            Else
                check = True
            End If

        End If

        Return check

    End Function
End Class