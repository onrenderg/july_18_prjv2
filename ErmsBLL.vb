Imports System.Data.SqlClient
Imports System.Data
Public Class ErmsBLL
    Implements ErmsInterface.hbInterface

    Private connstr As New ErmsDAL
    Private cmd As New SqlCommand()
    Private da As New SqlDataAdapter()

    Public Function BulkInsert(SourceTable As DataTable, DestinationTableName As String) As String Implements ErmsInterface.hbInterface.BulkInsert
        Dim result As String = ""
        Return result
    End Function

    Public Function DELETEData(cmdparameters As SqlCommand, spname As String) As DataTable Implements ErmsInterface.hbInterface.DELETEData
        Dim dtResult As New DataTable
        cmd = cmdparameters
        cmd.CommandType = CommandType.StoredProcedure
        Select Case spname
            Case "deleteEciVoter"
                cmd.CommandText = "deleteEciVoter"
            Case "DeleteEVMMaster"
                cmd.CommandText = "sec.DeleteEVMMaster"
            Case "DeleteFLCMaster"
                cmd.CommandText = "sec.DeleteFLCMaster"
            Case "DeleteFLCDetails"
                cmd.CommandText = "sec.DeleteFLCDetails"
            Case "deleteBallotMaster"
                cmd.CommandText = "sec.deleteBallotMaster"
        End Select
        cmd.Connection = connstr.HBDALgetappsettings
        da.SelectCommand = cmd

        Try
            connstr.HBDALopen()
            da.Fill(dtResult)
            dtResult.TableName = "Success"
        Catch e As Exception
            dtResult.TableName = "Error"
            Throw New Exception(e.Message)
        Finally
            connstr.HBDALclose()
        End Try

        Return dtResult
    End Function

    Public Function INSERTData(cmdparameters As SqlCommand, spname As String) As DataTable Implements ErmsInterface.hbInterface.INSERTData
        Dim dtResult As New DataTable

        cmd = cmdparameters
        cmd.CommandType = CommandType.StoredProcedure
        Select Case spname
            Case "InsertErrorDetails"
                cmd.CommandText = "sec.InsertErrorDetails"
            Case "saveEVMMaster"
                cmd.CommandText = "sec.saveEVMMaster"
            Case "saveEVMMapping"
                cmd.CommandText = "sec.saveEVMMapping"
            Case "saveEVMQRCode"
                cmd.CommandText = "sec.saveEVMQRCode"
            Case "saveBallotMaster"
                cmd.CommandText = "sec.saveBallotMaster"
            Case "saveStationeryMapping"
                cmd.CommandText = "sec.saveStationeryMapping"
            Case "UpdateStationeryStock"
                cmd.CommandText = "sec.UpdateStationeryStock"
        End Select
        cmd.Connection = connstr.HBDALgetappsettings
        da.SelectCommand = cmd

        Try
            connstr.HBDALopen()
            da.Fill(dtResult)
            dtResult.TableName = "Success"
        Catch e As Exception
            dtResult.TableName = "Error"
            Throw New Exception(e.Message)
        Finally
            connstr.HBDALclose()
        End Try


        Return dtResult
    End Function

    Public Function SELECTData(cmdparameters As SqlCommand, spname As String) As DataTable Implements ErmsInterface.hbInterface.SELECTData
        Dim dtResult As New DataTable
        cmd = cmdparameters
        cmd.CommandType = CommandType.StoredProcedure
        Select Case spname
            Case "UpdatetokenDetails"
                cmd.CommandText = "sec.sec.UpdatetokenDetails"
            Case "InsertLoginToken"
                cmd.CommandText = "sec.sec.InsertLoginToken"
            Case "checkUserLogin"
                cmd.CommandText = "sec.sec.checkUserLogin"
            Case "UpdateAttempt"
                cmd.CommandText = "sec.sec.UpdateAttempt"
            Case "GetPriTerm"
                cmd.CommandText = "sec.GetPriTerm"
            Case "getUnitItemMaster"
                cmd.CommandText = "sec.getUnitItemMaster"
            Case "getCommonMaster"
                cmd.CommandText = "sec.getCommonMaster"
            Case "GetDistrict"
                cmd.CommandText = "sec.sec.GetDistrict"
            Case "getEVMList"
                cmd.CommandText = "sec.getEVMList"
            Case "getBallotBoxList"
                cmd.CommandText = "sec.getBallotBoxList"
            Case "getPollingDate"
                cmd.CommandText = "sec.sec.getPollingDate"
            Case "GetPriBlocks"
                cmd.CommandText = "sec.sec.GetPriBlocks"
            Case "getPanchayatNameforPollDates"
                cmd.CommandText = "sec.sec.getPanchayatNameforPollDates"
            Case "getValuesCommonMaster"
                cmd.CommandText = "sec.sec.getValuesCommonMaster"
            Case "GetPanchayats"
                cmd.CommandText = "sec.sec.GetPanchayats"
            Case "getElectionProgramsNew"
                cmd.CommandText = "sec.sec.getElectionProgramsNew"
            Case "getEVMListSummary"
                cmd.CommandText = "sec.getEVMListSummary"
            Case "GetChartData"
                cmd.CommandText = "sec.GetChartData"
            Case "getBlocksForEVMAllocation"
                cmd.CommandText = "sec.getBlocksForEVMAllocation"
            Case "GetPriBlocks"
                cmd.CommandText = "sec.sec.GetPriBlocks"
            Case "GetMCWards"
                cmd.CommandText = "sec.sec.GetMCWards"
            Case "GetPanchayats"
                cmd.CommandText = "sec.sec.GetPanchayats"
            Case "GetListForAllocation"
                cmd.CommandText = "sec.GetListForAllocation"
            Case "GetListForStationeryAllocation"
                cmd.CommandText = "sec.GetListForStationeryAllocation"
            Case "ShowAvailableStock"
                cmd.CommandText = "sec.ShowAvailableStock"
            Case "getInventoryCounts"
                cmd.CommandText = "sec.getInventoryCounts"
            Case "GetWarehouse"
                cmd.CommandText = "sec.GetWarehouse"
            Case "getEVMListPendingForQR"
                cmd.CommandText = "sec.getEVMListPendingForQR"
            Case "getBallotBoxListPendingForQR"
                cmd.CommandText = "sec.getBallotBoxListPendingForQR"
            Case "getBallotBoxListSummary"
                cmd.CommandText = "sec.getBallotBoxListSummary"
            Case "getEVMListBlockLevel"
                cmd.CommandText = "sec.getEVMListBlockLevel"
            Case "getEVMListDistrictLevel"
                cmd.CommandText = "sec.getEVMListDistrictLevel"
            Case "GetWards"
                cmd.CommandText = "sec.sec.GetWards"
            Case "GetWarehouseLatLong"
                cmd.CommandText = "sec.GetWarehouseLatLong"
            Case "getFLCListForUpdate"
                cmd.CommandText = "sec.getFLCListForUpdate"
            Case "getEVMFLCList"
                cmd.CommandText = "sec.getEVMFLCList"
            Case "GetStationeryAllocated"
                cmd.CommandText = "sec.GetStationeryAllocated"
            Case "getEVMLocationList"
                cmd.CommandText = "sec.getEVMLocationList"


        End Select
        cmd.Connection = connstr.HBDALgetappsettings
        da.SelectCommand = cmd
        Try
            connstr.HBDALopen()
            da.Fill(dtResult)
            dtResult.TableName = "Success"
        Catch e As Exception
            dtResult.TableName = "Error"
            Throw New Exception(e.Message)
        Finally
            connstr.HBDALclose()
        End Try
        Return dtResult
    End Function

    Public Function UPDATEData(cmdparameters As SqlCommand, spname As String) As DataTable Implements ErmsInterface.hbInterface.UPDATEData
        Dim dtResult As New DataTable
        cmd = cmdparameters
        cmd.CommandType = CommandType.StoredProcedure
        Select Case spname
            Case "updateEciVoter"
                cmd.CommandText = "sec.updateEciVoter"
            Case "UpdateLogDetails"
                cmd.CommandText = "sec.sec.UpdateLogDetails"
            Case "updateUserPassword"
                cmd.CommandText = "sec.sec.updateUserPassword"
            Case "updateFLCDetails"
                cmd.CommandText = "sec.updateFLCDetails"
            Case "updateFLCMaster"
                cmd.CommandText = "sec.updateFLCMaster"
        End Select
        cmd.Connection = connstr.HBDALgetappsettings
        da.SelectCommand = cmd
        Try
            connstr.HBDALopen()
            da.Fill(dtResult)
            dtResult.TableName = "Success"
        Catch e As Exception
            dtResult.TableName = "Error"
            Throw New Exception(e.Message)
        Finally
            connstr.HBDALclose()
        End Try
        Return dtResult
    End Function

End Class
