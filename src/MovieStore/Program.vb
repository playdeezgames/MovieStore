Imports Microsoft.Data.SqlClient

Module Program
    Sub Main(args As String())
        Using connection As New SqlConnection("Data Source=localhost\SQLEXPRESS;Initial Catalog=GPOS;Integrated Security=SSPI;Trust Server Certificate=True")
            connection.Open()
            MainMenu.Run(connection)
            connection.Close()
        End Using
    End Sub
End Module
