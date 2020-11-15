Imports MySql.Data.MySqlClient
Public Class MY_DB
    Dim con As MySqlConnection = New MySqlConnection("datasource=localhost;port=3306;username=root;password=;database=patient_management_db")

    'get the conn 
    Public ReadOnly Property getConnection As MySqlConnection
        Get
            Return con
        End Get
    End Property

    'open the conn
    Public Sub openConnection()
        If con.State = ConnectionState.Closed Then
            con.Open()
        End If
    End Sub

    'close the conn
    Public Sub closeConnection()
        If con.State = ConnectionState.Open Then
            con.Close()
        End If
    End Sub

End Class
