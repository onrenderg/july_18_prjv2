Imports System.Data
Imports System.Data.SqlClient
Public Class ErmsInterface
    Interface hbInterface
        Function BulkInsert(SourceTable As DataTable, DestinationTableName As String) As String
        Function INSERTData(cmdparameters As SqlCommand, spname As String) As DataTable
        Function UPDATEData(cmdparameters As SqlCommand, spname As String) As DataTable
        Function SELECTData(cmdparameters As SqlCommand, spname As String) As DataTable
        Function DELETEData(cmdparameters As SqlCommand, spname As String) As DataTable
    End Interface
End Class
