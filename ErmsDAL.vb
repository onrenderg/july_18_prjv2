Imports System.Data
Imports System.Data.SqlClient

Public Class ErmsDAL
    Dim connString As String = ConfigurationManager.ConnectionStrings("SecConnection").ToString()
    Public HBconnection As New SqlConnection(connString)

    Public Function HBDALgetappsettings() As SqlConnection
        HBconnection = New SqlConnection(connString)
        HBDALgetappsettings = HBconnection
    End Function
    Public Sub HBDALopen()
        Try
            If HBconnection.State = ConnectionState.Open Then
                HBconnection.Close()
            End If
            HBconnection.Open()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Sub HBDALclose()
        Try
            If HBconnection.State = ConnectionState.Open Then
                HBconnection.Close()
            End If
        Catch ex As Exception
        Finally
            HBconnection.Dispose()
        End Try

    End Sub
End Class
