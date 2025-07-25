Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Security.Cryptography
'Imports MessagingToolkit.QRCode.Codec
Imports System.Web.Script.Serialization
Imports Newtonsoft.Json
Imports QRCoder


Public Class commonFunctions





    Dim mytable As New DataTable
    Dim MyInterface As ErmsInterface.hbInterface

    Dim myLang As String = "hi-IN"
    Const keyValue As String = "Sec&NicHP@22"




    Dim mycommand As New SqlClient.SqlCommand
    Dim ErmsImplementer As New ErmsBLL

    '@@mgogo' New function to get Ballot Box Dashboard Count

    ' NEW FUNCTION TO ADD FOR DASHBOARD COUNTS


    Public Function GetBallotboxDashboardCounts(userlevelcode As String, from_user_id As String) As DataTable
        Dim dt As New DataTable()
        Try
            ' Retrieve the connection string from Web.config or App.config
            ' Make sure you have a connection string named "SecConnection" in your configuration file.
            Using conn As New SqlConnection(ConfigurationManager.ConnectionStrings("SecConnection").ConnectionString) ' [2, 4, 5, 6]
                Using cmd As New SqlCommand("sec.Ballotbox_dashboardcounts", conn) ' [1, 3, 7, 9, 10, 12]
                    cmd.CommandType = CommandType.StoredProcedure ' [1]

                    ' Add parameters to the command
                    cmd.Parameters.AddWithValue("@userlevelcode", userlevelcode) ' [1]
                    cmd.Parameters.AddWithValue("@from_user_id", from_user_id) ' [1]

                    conn.Open() ' [12]
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        dt.Load(reader)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            ' Optional: log the error
            ' For example: Console.WriteLine("Error in GetBallotboxDashboardCounts: " & ex.Message)
            Return New DataTable() ' Return an empty DataTable in case of an error
        End Try
        Return dt
    End Function


    Public Function GetBallotBoxDetailList(
    userlevelcode As String,
    usercode As String,
    actioncode As Integer
) As DataTable
        Dim dt As New DataTable()

        Try
            ' Assumes you have a connection string named "SecConnection" in Web.config/App.config
            Using conn As New SqlConnection(
            ConfigurationManager.ConnectionStrings("SecConnection").ConnectionString
        )
                Using cmd As New SqlCommand("sec.Mobile_GetBallotBox_wh_dis_blk_list", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    ' Add the three parameters expected by the proc
                    cmd.Parameters.AddWithValue("@userlevelcode", userlevelcode)
                    cmd.Parameters.AddWithValue("@usercode", usercode)
                    cmd.Parameters.AddWithValue("@actioncode", actioncode)

                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        dt.Load(reader)
                    End Using
                End Using
            End Using

        Catch ex As Exception
            ' TODO: Log exception (ex.Message, ex.StackTrace, etc.)
            ' Return empty table on error so caller can handle gracefully
            Return New DataTable()
        End Try

        Return dt
    End Function




    Public Function GetBallotBoxBlocksParked(
    userlevelcode As String,
    prevCode As String,
    actioncode As Integer
) As DataTable

        Dim dt As New DataTable()

        Try
            ' Re‑use your SecConnection string
            Using conn As New SqlConnection(
            ConfigurationManager.ConnectionStrings("SecConnection").ConnectionString
        )
                Using cmd As New SqlCommand("sec.Mobile_GetBlocks_fordistrict_parked", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    ' SP parameters
                    cmd.Parameters.AddWithValue("@userlevelcode", userlevelcode)
                    cmd.Parameters.AddWithValue("@usercode", prevCode)
                    cmd.Parameters.AddWithValue("@action", actioncode)

                    conn.Open()
                    Using rdr = cmd.ExecuteReader()
                        dt.Load(rdr)
                    End Using
                End Using
            End Using

        Catch ex As Exception
            ' TODO: log ex.Message / ex.StackTrace somewhere
            Return New DataTable()
        End Try

        Return dt
    End Function






    Public Function GetBallotBoxSizeList(
    userlevelcode As String,
    usercode As String,
    zonecode As String,
    actioncode As Integer,
    isJurisdiction As Integer
) As DataTable

        Dim dt As New DataTable()

        Try
            Using conn As New SqlConnection(
            ConfigurationManager.ConnectionStrings("SecConnection").ConnectionString
        )
                Using cmd As New SqlCommand("sec.Mobile_GetBallotBoxCountBySize", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.AddWithValue("@userlevelcode", userlevelcode)
                    cmd.Parameters.AddWithValue("@usercode", usercode)
                    cmd.Parameters.AddWithValue("@zonecode", zonecode)
                    cmd.Parameters.AddWithValue("@action", actioncode)
                    cmd.Parameters.AddWithValue("@is_jurisdiction", isJurisdiction)

                    conn.Open()
                    Using rdr = cmd.ExecuteReader()
                        dt.Load(rdr)
                    End Using
                End Using
            End Using

        Catch ex As Exception
            ' TODO: log ex.Message / ex.StackTrace
            Return New DataTable()
        End Try

        Return dt
    End Function





    Public Function GetBallotBoxSerialList(
        userlevelcode As String,
        usercode As String,
        zonecode As String,
        box_size As String,
        is_jurisdiction As Integer,
        Optional pageNumber As Integer = 1,
        Optional pageSize As Integer = 50,
        Optional action As Integer = 1 ' default: received
    ) As DataTable

        Dim dt As New DataTable()

        Try
            Using con As New SqlConnection(ConfigurationManager.ConnectionStrings("conn").ConnectionString)
                Using cmd As New SqlCommand("Mobile_BallotBox_GetPagedItems", con)
                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber)
                    cmd.Parameters.AddWithValue("@PageSize", pageSize)
                    cmd.Parameters.AddWithValue("@userlevelcode", userlevelcode)
                    cmd.Parameters.AddWithValue("@usercode", usercode)
                    cmd.Parameters.AddWithValue("@action", action)
                    cmd.Parameters.AddWithValue("@zonecode", zonecode)
                    cmd.Parameters.AddWithValue("@box_size", If(String.IsNullOrEmpty(box_size), DBNull.Value, box_size))
                    cmd.Parameters.AddWithValue("@is_jurisdiction", is_jurisdiction)

                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(dt)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            ' Log or handle error appropriately
            Throw New ApplicationException("Error fetching ballot box serial list: " & ex.Message)
        End Try

        Return dt
    End Function




    '@@mgogo ' End of new function`




    Public Shared Function existsReservewords(ByVal strValue As String) As Boolean
        Dim _reservedWords() As String = {"SCRIPT", "CREATE", "INSERT", "SELECT", "DELETE", "DROP", "UPDATE"}
        'validate for reserve words
        Dim i As Integer
        For i = 0 To 6
            If UCase(strValue).Contains(_reservedWords(i)) Then
                Return True
                Exit For
            End If
        Next
        Return False
    End Function
    Public Function GenerateRandomString() As String
        Dim length = 18
        Dim chars As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_+"
        Dim random As New Random()
        Dim result(length - 1) As Char
        For i As Integer = 0 To length - 1
            result(i) = chars(random.Next(0, chars.Length))
        Next
        Return New String(result)
    End Function
    Public Sub BindYesNoRadioButton(ByVal rbtn As RadioButtonList)
        Try
            rbtn.RepeatDirection = RepeatDirection.Horizontal
            rbtn.Items.Insert(0, New ListItem(Resources.Resource.lblNo, "N"))
            rbtn.Items.Insert(1, New ListItem(Resources.Resource.lblYes, "Y"))
            rbtn.DataBind()
            rbtn.SelectedIndex = 0
        Catch ex As Exception
        End Try
    End Sub
    Public Sub BindMonthDropdown(ByVal ddl As DropDownList)
        Try
            ddl.Items.Insert(0, New ListItem(Resources.Resource.Sel, Resources.Resource.Sel1))
            ddl.Items.Insert(1, New ListItem(Resources.Resource.Jan, Resources.Resource.Jan1))
            ddl.Items.Insert(2, New ListItem(Resources.Resource.Feb, Resources.Resource.Feb1))
            ddl.Items.Insert(3, New ListItem(Resources.Resource.Mar, Resources.Resource.Mar1))
            ddl.Items.Insert(4, New ListItem(Resources.Resource.Apr, Resources.Resource.Apr1))
            ddl.Items.Insert(5, New ListItem(Resources.Resource.May, Resources.Resource.May1))
            ddl.Items.Insert(6, New ListItem(Resources.Resource.Jun, Resources.Resource.Jun1))
            ddl.Items.Insert(7, New ListItem(Resources.Resource.Jul, Resources.Resource.Jul1))
            ddl.Items.Insert(8, New ListItem(Resources.Resource.Aug, Resources.Resource.Aug1))
            ddl.Items.Insert(9, New ListItem(Resources.Resource.Sep, Resources.Resource.Sep1))
            ddl.Items.Insert(10, New ListItem(Resources.Resource.Oct, Resources.Resource.Oct1))
            ddl.Items.Insert(11, New ListItem(Resources.Resource.Nov, Resources.Resource.Nov1))
            ddl.Items.Insert(12, New ListItem(Resources.Resource.Dec, Resources.Resource.Dec1))
            ddl.DataBind()
            ddl.SelectedIndex = 0
        Catch ex As Exception
        End Try
    End Sub
    Public Sub BindYearDropdown(ByVal ddl As DropDownList)
        Try
            Dim yrVal As Int16 = Date.Today.Year

            Dim cntr As Int16 = 1
            ddl.Items.Insert(0, New ListItem(Resources.Resource.Sel, Resources.Resource.Sel1))
            Do While yrVal >= 1993 ' 1994 is the year when SEC HP was established and in 1995 EVM's were procured for first time
                ddl.Items.Insert(cntr, yrVal)
                yrVal -= 1
                cntr += 1
            Loop

            ddl.DataBind()
            ddl.SelectedIndex = 0
        Catch ex As Exception
        End Try
    End Sub
    Public Sub PopulateCommonMasterRadio(ByVal rbtnCommonList As RadioButtonList, ByVal category As String, Optional ByVal PostFor As String = "000")
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@cat_name", category)
            mycommand.Parameters.AddWithValue("@cat_Type", PostFor)
            mytable = MyInterface.SELECTData(mycommand, "getCommonMaster")
            If mytable.TableName = "Success" Then
                Dim dv As DataView = mytable.DefaultView
                dv.Sort = "code"
                'dv.Sort = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "Description_local", "description")
                rbtnCommonList.DataSource = dv
                rbtnCommonList.DataTextField = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "Description_local", "description")
                Select Case category
                    Case "MAKE", "MODEL", "WORKINGSTATUS", "BALLOTBOXSIZE", "NOMINATIONSTATUS", "PARTYTYPE", "SYMBOLTYPE"
                        rbtnCommonList.DataValueField = "abbr"
                    Case Else
                        rbtnCommonList.DataValueField = "code"
                End Select
                'ddlElectionPosts.DataValueField = IIf((category = "NOMINATIONTYPE" Or category = "PRITYPE" Or category = "NOMINATIONSTATUS"), "abbr", "code")
                rbtnCommonList.DataBind()
                ' ddlElectionPosts.Items.Insert(0, New ListItem(IIf(mytable.Rows.Count > 0, "All", "Posts Not Found"), "000"))
                rbtnCommonList.SelectedIndex = 0
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Function getEVMLocationList(ByVal itemType As String, ByVal dis_code As String, ByVal wh_code As String) As DataTable
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@Device_Type", itemType)
            mycommand.Parameters.AddWithValue("@dis_code", dis_code)
            mycommand.Parameters.AddWithValue("@wh_code", wh_code)
            mycommand.Parameters.AddWithValue("@lang", HttpContext.Current.Session("MyLanguage").ToString())
            mytable = MyInterface.SELECTData(mycommand, "getEVMLocationList")

        Catch ex As Exception

        End Try
        Return mytable
    End Function
    Public Sub PopulateCommonMasterDropdown(ByVal ddlCommonList As DropDownList, ByVal category As String, Optional ByVal PostFor As String = "000")
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@cat_name", category)
            mycommand.Parameters.AddWithValue("@cat_Type", PostFor)
            mytable = MyInterface.SELECTData(mycommand, "getCommonMaster")
            If mytable.TableName = "Success" Then
                Dim dv As DataView = mytable.DefaultView
                dv.Sort = "code"
                'dv.Sort = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "Description_local", "description")
                ddlCommonList.DataSource = dv
                ddlCommonList.DataTextField = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "Description_local", "description")
                Select Case category
                    Case "STATIONERY", "MAKE", "MODEL", "BALLOTBOXSIZE", "NOMINATIONTYPE", "WORKINGSTATUS", "PRITYPE", "NOMINATIONSTATUS", "PARTYTYPE", "SYMBOLTYPE"
                        ddlCommonList.DataValueField = "abbr"
                    Case Else
                        ddlCommonList.DataValueField = "code"
                End Select
                'ddlElectionPosts.DataValueField = IIf((category = "NOMINATIONTYPE" Or category = "PRITYPE" Or category = "NOMINATIONSTATUS"), "abbr", "code")
                ddlCommonList.DataBind()
                If PostFor = "000" Then
                    ddlCommonList.Items.Insert(0, New ListItem(Resources.Resource.Sel, Resources.Resource.Sel1))
                End If
                ddlCommonList.SelectedIndex = 0
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub PopulateDistricts(ByVal ddlDis As DropDownList, ByVal Optional OneMany As String = "One")
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mytable = MyInterface.SELECTData(mycommand, "GetDistrict")
            If mytable.TableName = "Success" Then
                Dim dv As DataView = mytable.DefaultView
                dv.Sort = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "District_Name_Local", "District_Name")
                ddlDis.DataSource = dv
                ddlDis.DataTextField = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "District_Name_Local", "District_Name")
                ' "District_Name_Local"
                ddlDis.DataValueField = "District_Code"
                ddlDis.DataBind()
                If OneMany = "One" Then
                    ddlDis.Items.Insert(0, New ListItem(IIf(mytable.Rows.Count > 0, IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "जिला चुनें", "Choose District"),
                                                       IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "जिला नहीं मिला", "No Zila Found")), "00"))
                ElseIf OneMany = "Self" Then
                    ddlDis.Items.Insert(0, New ListItem(IIf(mytable.Rows.Count > 0, IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "सभी जिले", "All Districts"),
                                                        IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "सभी जिले", "All Districts")), "13"))

                    ddlDis.Items.Insert(0, New ListItem(IIf(mytable.Rows.Count > 0, IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "राज्य स्तर पर दर्ज", "Entered At State Level"),
                                                        IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "राज्य स्तर पर दर्ज", "Entered At State Level")), "99999"))
                Else
                    ddlDis.Items.Insert(0, New ListItem(IIf(mytable.Rows.Count > 0, IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "सभी जिले", "All Districts"),
                                                        IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "सभी जिले", "All Districts")), "13"))
                End If

                ddlDis.SelectedIndex = 0
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub PopulatePriDistricts(ByVal ddlDis As DropDownList, ByVal priType As String, ByVal Optional OneMany As String = "One")
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@priType", priType)
            mytable = MyInterface.SELECTData(mycommand, "GetDistrict")
            If mytable.TableName = "Success" Then
                Dim dv As DataView = mytable.DefaultView
                dv.Sort = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "District_Name_Local", "District_Name")
                ddlDis.DataSource = dv
                ddlDis.DataTextField = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "District_Name_Local", "District_Name")
                ' "District_Name_Local"
                ddlDis.DataValueField = "District_Code"
                ddlDis.DataBind()
                If OneMany = "One" Then
                    ddlDis.Items.Insert(0, New ListItem(IIf(mytable.Rows.Count > 0, IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "जिला चुनें", "Choose District"),
                                                       IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "जिला परिषद नहीं मिला", "No Zila Parishad Found")), "00"))
                Else
                    ddlDis.Items.Insert(0, New ListItem(IIf(mytable.Rows.Count > 0, IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "सभी जिले", "All Districts"),
                                                        IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "सभी जिले", "All Districts")), "13"))
                End If
                ddlDis.SelectedIndex = 0
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub PopulatePriBlocks(ByVal ddlBlk As DropDownList, ByVal Dis As String, ByVal priType As String, ByVal Optional OneMany As String = "One") ' Optional ByVal blk_type As String = "A")
        Try
            ddlBlk.Items.Clear()
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()

            mycommand.Parameters.AddWithValue("@dis_Code", Dis)
            mycommand.Parameters.AddWithValue("@blk_type", priType)
            mytable = MyInterface.SELECTData(mycommand, "GetPriBlocks")
            If mytable.TableName = "Success" Then
                Dim UserData = commonFunctions.GetUserDataFromSession
                Dim blockCodeToRemove As String = UserData.BlockCode
                Dim rowToRemove As DataRow = mytable.Select("Block_code = '" & blockCodeToRemove & "'").FirstOrDefault()
                If OneMany = "One" Then
                    If rowToRemove IsNot Nothing Then
                        mytable.Rows.Remove(rowToRemove)
                    End If
                End If

                ddlBlk.DataSource = mytable
                ddlBlk.DataTextField = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "block_name_local", "block_name")
                ddlBlk.DataValueField = "Block_code"
                ddlBlk.DataBind()
                'ddlBlk.DataSource = mytable
                'ddlBlk.DataTextField = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "block_name_local", "block_name")
                ''"block_name_local"
                'ddlBlk.DataValueField = "Block_code"
                'ddlBlk.DataBind()
                If mytable.Rows.Count > 1 Then
                    If OneMany = "One" And (HttpContext.Current.Session("userlevel") Is Nothing Or HttpContext.Current.Session("userlevel") = "000" Or HttpContext.Current.Session("userlevel") = "001") Then
                        If priType = "001" Then
                            ddlBlk.Items.Insert(0, New ListItem(IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "पंचायत समिति चुनें", "Choose Panchayat Samiti"), "00"))
                        ElseIf priType = "002" Then
                            ddlBlk.Items.Insert(0, New ListItem(IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "शहरी निकाय चुनें", "Choose ULB"), "00"))
                        Else
                            ddlBlk.Items.Insert(0, New ListItem(IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "नगर निगम चुनें", "Choose MC"), "00"))
                        End If
                    ElseIf OneMany = "Self" Then
                        If priType = "001" Then
                            ddlBlk.Items.Insert(0, New ListItem(IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "सभी पंचायत समिति", "All Panchayat Samiti"), "00"))
                        ElseIf priType = "002" Then
                            ddlBlk.Items.Insert(0, New ListItem(IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "सभी शहरी निकाय", "All ULBs"), "00"))
                        Else
                            ddlBlk.Items.Insert(0, New ListItem(IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "सभी नगर निगम", "All MCs"), "00"))
                        End If

                        ddlBlk.Items.Insert(0, New ListItem(IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "जिला स्तर पर दर्ज", "Entered At District Level"), "99999"))

                    Else
                        If priType = "001" Then
                            ddlBlk.Items.Insert(0, New ListItem(IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "सभी पंचायत समिति", "All Panchayat Samiti"), "00"))
                        ElseIf priType = "002" Then
                            ddlBlk.Items.Insert(0, New ListItem(IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "सभी शहरी निकाय", "All ULBs"), "00"))
                        Else
                            ddlBlk.Items.Insert(0, New ListItem(IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "सभी नगर निगम", "All MCs"), "00"))
                        End If

                    End If
                End If
                ddlBlk.SelectedIndex = 0
            End If

        Catch ex As Exception

        End Try
    End Sub
    Public Sub populatePanchayatWards(ByVal ddlWard As DropDownList, ByVal panch_code As String, ByVal Optional OneMany As String = "One")
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@Panchayat_Code", panch_code)
            mytable = MyInterface.SELECTData(mycommand, "GetWards")
            If mytable.Rows.Count > 0 Then
                ddlWard.DataSource = mytable
                ddlWard.DataTextField = "wardnoname"
                ddlWard.DataValueField = "ward_no"
                ddlWard.DataBind()
                If OneMany = "One" Then
                    ddlWard.Items.Insert(0, New ListItem(IIf(mytable.Rows.Count > 0, IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "वार्ड/मतदान केंद्र", "Choose Ward/Polling Station"),
                                                        IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "वार्ड/मतदान केंद्र नहीं मिला", "No MC Ward Found")), "00"))

                Else
                    ddlWard.Items.Insert(0, New ListItem(IIf(mytable.Rows.Count > 0, IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "सभी Ward/Polling Station", "All  Ward/Polling Station"),
                                                        IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "वार्ड/मतदान केंद्र नहीं मिला", "No Ward/Polling Station Found")), "00"))

                End If
                ddlWard.SelectedIndex = 0
            Else
                ddlWard.DataSource = Nothing
                ddlWard.DataBind()
            End If
        Catch ex As Exception
            Dim exMsg As String = ex.Message
        End Try
    End Sub
    Public Sub PopulatePanchayats(ByVal ddlPanch As DropDownList, ByVal Blk As String, ByVal Optional OneMany As String = "One")
        Try
            ddlPanch.Items.Clear()
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@Block_Code", Blk)
            mytable = MyInterface.SELECTData(mycommand, "GetPanchayats")
            If mytable.TableName = "Success" Then
                Dim dv As DataView = mytable.DefaultView
                If CInt(Right(Blk, 3)) < 999 And CInt(Right(Blk, 3)) > 990 Then
                    dv.Sort = "panchayat_code"
                Else
                    dv.Sort = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "Panchayat_Name_Hi", "Panchayat_Name_En")
                End If
                ddlPanch.DataSource = dv
                ddlPanch.DataTextField = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "Panchayat_Name_Hi", "Panchayat_Name_En")
                '"Panchayat_Name_Hi"
                ddlPanch.DataValueField = "panchayat_code"
                ddlPanch.DataBind()
                Select Case Right(Blk, 3).ToString
                    Case "999"
                        If OneMany = "One" Then
                            ddlPanch.Items.Insert(0, New ListItem(IIf(mytable.Rows.Count > 0, IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "शहरी स्थानीय निकाय चुनें", "Choose Urban Local Body"),
                                                                IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "शहरी स्थानीय निकाय नहीं मिला", "No ULB Found")), "00"))
                        Else
                            ddlPanch.Items.Insert(0, New ListItem(IIf(mytable.Rows.Count > 0, IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "सभी शहरी स्थानीय निकाय", "All Urban Local Bodies"),
                                                                IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "सभी शहरी स्थानीय निकाय", "All ULBs")), Blk & "000"))
                        End If
                    Case "990", "991", "992", "993", "994", "995", "996", "997", "998"
                        If OneMany = "One" Then
                            ddlPanch.Items.Insert(0, New ListItem(IIf(mytable.Rows.Count > 0, IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "नगर निगम वार्ड चुनें", "Choose Municipal Corporation Ward"),
                                                                IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "नगर निगम वार्ड नहीं मिली", "No MC Found")), "00"))
                        Else
                            ddlPanch.Items.Insert(0, New ListItem(IIf(mytable.Rows.Count > 0, IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "सभी नगर निगम वार्ड", "All MC Wards"),
                                                                IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "सभी नगर निगम वार्ड", "All MC Wards")), Blk & "000"))
                        End If
                    Case Else
                        If OneMany = "One" Then
                            ddlPanch.Items.Insert(0, New ListItem(IIf(mytable.Rows.Count > 0, IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "पंचायत चुनें", "Choose Panchayat"),
                                                                IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "पंचायत नहीं मिली", "No Panchayat Found")), "00"))
                        Else
                            ddlPanch.Items.Insert(0, New ListItem(IIf(mytable.Rows.Count > 0, IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "सभी पंचायतें", "All Panchayats"),
                                                                IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "सभी पंचायतें", "All Panchayats")), Blk & "000"))
                        End If
                End Select
                ddlPanch.SelectedIndex = -1
            End If
        Catch ex As Exception
            Dim exMsg As String = ex.Message
        End Try
    End Sub

    Public Sub populateMCWards(ByVal ddlMC As DropDownList, ByVal mc_code As String, ByVal Optional OneMany As String = "One")
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@blk", mc_code)
            mytable = MyInterface.SELECTData(mycommand, "GetMCWards")
            If mytable.Rows.Count > 0 Then
                ddlMC.DataSource = mytable
                ddlMC.DataTextField = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "mcWardnoNameLocal", "mcwardnoname")
                ddlMC.DataValueField = "mcwardno"
                ddlMC.DataBind()
                If OneMany = "One" Then
                    ddlMC.Items.Insert(0, New ListItem(IIf(mytable.Rows.Count > 0, IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "एमसी वार्ड", "Choose MC Ward"),
                                                        IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "एमसी वार्ड नहीं मिला", "No MC Ward Found")), "00"))

                Else
                    ddlMC.Items.Insert(0, New ListItem(IIf(mytable.Rows.Count > 0, IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "सभी एमसी वार्ड", "All MC Ward"),
                                                        IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "एमसी वार्ड नहीं मिला", "No MC Ward Found")), "00"))

                End If
                ddlMC.SelectedIndex = 0
            Else
                ddlMC.DataSource = Nothing
                ddlMC.DataBind()
            End If
        Catch ex As Exception
            Dim exMsg As String = ex.Message
        End Try
    End Sub
    Public Sub PopulatePollDayPanchayats(ByVal ddlPanch As DropDownList, ByVal Blk As String, ByVal PollDate As String, ByVal Optional OneMany As String = "One")
        Try
            ddlPanch.Items.Clear()
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@code", Blk)
            mycommand.Parameters.AddWithValue("@polldate", PollDate)
            mytable = MyInterface.SELECTData(mycommand, "getPanchayatNameforPollDates")
            If mytable.TableName = "Success" Then
                Dim dv As DataView = mytable.DefaultView
                dv.Sort = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "Panchayat_Name_local", "Panchayat_Name")
                ddlPanch.DataSource = dv
                ddlPanch.DataTextField = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "Panchayat_Name_local", "Panchayat_Name")
                '"Panchayat_Name_Hi"
                ddlPanch.DataValueField = "panchayat_code"
                ddlPanch.DataBind()
                Select Case Right(Blk, 3).ToString
                    Case "999"
                        If OneMany = "One" Then
                            ddlPanch.Items.Insert(0, New ListItem(IIf(mytable.Rows.Count > 0, IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "शहरी स्थानीय निकाय चुनें", "Choose Urban Local Body"),
                                                                IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "शहरी स्थानीय निकाय नहीं मिला", "No ULB Found")), "00"))
                        Else
                            ddlPanch.Items.Insert(0, New ListItem(IIf(mytable.Rows.Count > 0, IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "सभी शहरी स्थानीय निकाय", "All Urban Local Bodies"),
                                                                IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "सभी शहरी स्थानीय निकाय", "All ULBs")), Blk & "000"))
                        End If
                    Case "990", "991", "992", "993", "994", "995", "996", "997", "998"
                        If OneMany = "One" Then
                            ddlPanch.Items.Insert(0, New ListItem(IIf(mytable.Rows.Count > 0, IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "नगर निगम वार्ड चुनें", "Choose Municipal Corporation Ward"),
                                                                IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "नगर निगम वार्ड नहीं मिली", "No MC Found")), "00"))
                        Else
                            ddlPanch.Items.Insert(0, New ListItem(IIf(mytable.Rows.Count > 0, IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "सभी नगर निगम वार्ड", "All MC Wards"),
                                                                IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "सभी नगर निगम वार्ड", "All MC Wards")), Blk & "000"))
                        End If
                    Case Else
                        If OneMany = "One" Then
                            ddlPanch.Items.Insert(0, New ListItem(IIf(mytable.Rows.Count > 0, IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "पंचायत चुनें", "Choose Panchayat"),
                                                                IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "पंचायत नहीं मिली", "No Panchayat Found")), "00"))
                        Else
                            ddlPanch.Items.Insert(0, New ListItem(IIf(mytable.Rows.Count > 0, IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "सभी पंचायतें", "All Panchayats"),
                                                                IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "सभी पंचायतें", "All Panchayats")), Blk & "000"))
                        End If
                End Select
                ddlPanch.SelectedIndex = -1
            End If
        Catch ex As Exception
            Dim exMsg As String = ex.Message
        End Try
    End Sub
    Public Sub PopulatePanchayatWardsForPollDay(ByVal lstWards As ListBox, ByVal panchayatCode As String, ByVal pollDatePhase As String, ByVal opt As String)
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@code", panchayatCode)
            mycommand.Parameters.AddWithValue("@pollDate", pollDatePhase.Split("|")(1))
            mycommand.Parameters.AddWithValue("@pollphase", pollDatePhase.Split("|")(0))
            mycommand.Parameters.AddWithValue("@opt", opt)
            mytable = MyInterface.SELECTData(mycommand, "getPanchayatWardsForPollDay")
            If mytable.TableName = "Success" Then
                Dim dv As DataView = mytable.DefaultView
                dv.Sort = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "WardNoNameLocal", "WardNoName")
                lstWards.DataSource = dv
                lstWards.DataTextField = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "WardNoNameLocal", "WardNoName")
                lstWards.DataValueField = "WardVal"
                lstWards.DataBind()
                lstWards.SelectedIndex = 0

            End If
        Catch ex As Exception
        End Try
    End Sub

    Public Sub PopulateUnitTypes(ByVal rbtn As RadioButtonList, Optional ByVal unitType As String = "AL")
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@unitType", unitType)
            mytable = MyInterface.SELECTData(mycommand, "getUnitItemMaster")
            If mytable.TableName = "Success" Then
                Dim dv As DataView = mytable.DefaultView
                dv.Sort = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "UnitNameLocal", "UnitName")
                rbtn.DataSource = dv
                rbtn.DataTextField = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "UnitNameLocal", "UnitName")
                rbtn.DataValueField = "UnitType"
                rbtn.DataBind()

                rbtn.SelectedIndex = 0
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub PopulateUnitTypes(ByVal ddl As DropDownList, Optional ByVal unitType As String = "AL")
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@unitType", unitType)
            mytable = MyInterface.SELECTData(mycommand, "getUnitItemMaster")
            If mytable.TableName = "Success" Then
                Dim dv As DataView = mytable.DefaultView
                dv.Sort = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "UnitNameLocal", "UnitName")
                ddl.DataSource = dv
                ddl.DataTextField = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "UnitNameLocal", "UnitName")
                ddl.DataValueField = "UnitType"
                ddl.DataBind()

                ddl.SelectedIndex = 0
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub PopulateWareHouses(ByVal ddl As DropDownList, ByVal Dis As String, ByVal WhType As String, Optional OneMany As String = "One")
        Try
            ddl.Items.Clear()
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()

            mycommand.Parameters.AddWithValue("@whtype", WhType)
            mycommand.Parameters.AddWithValue("@dis_Code", Dis)
            mytable = MyInterface.SELECTData(mycommand, "GetWarehouse")
            If mytable.TableName = "Success" Then
                ddl.DataSource = mytable
                ddl.DataTextField = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "WareHouse_Name_local", "WareHouse_Name")
                ddl.DataValueField = "WareHouse_code"
                ddl.DataBind()
                If mytable.Rows.Count > 1 Then
                    If OneMany <> "One" And (HttpContext.Current.Session("userlevel") Is Nothing Or HttpContext.Current.Session("userlevel") = "000" Or HttpContext.Current.Session("userlevel") = "001") Then
                        ddl.Items.Insert(0, New ListItem(IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "सभी गोदाम", "All Warehouses"), "00"))
                    Else
                        ddl.Items.Insert(0, New ListItem(IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "गोदाम चुनें", "Choose Warehouse"), "00"))
                    End If
                End If
                ddl.SelectedIndex = -1
            End If
        Catch ex As Exception
        End Try
    End Sub

    Public Sub GetPollingDates(ByVal ddlPollingDate As RadioButtonList, ByVal priType As String)
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@post", priType)
            Dim dt = MyInterface.SELECTData(mycommand, "getPollingDate")
            If dt.Rows.Count > 0 Then
                ddlPollingDate.Items.Clear()
                ddlPollingDate.DataSource = dt
                ddlPollingDate.DataTextField = "PollingDate"
                ddlPollingDate.DataValueField = "PollingDtTmElectionID"
                ddlPollingDate.DataBind()
                ddlPollingDate.SelectedIndex = 0
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public Function GetEVMDataTable(ByRef Optional dis_code As String = "0000", ByRef Optional unitType As String = "CU", ByRef Optional workingStatus As String = "XX", ByRef Optional warehouseCode As String = "0000") As DataTable
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@workingStatus", workingStatus)
            mycommand.Parameters.AddWithValue("@unitType", unitType)
            mycommand.Parameters.AddWithValue("@dis_code", dis_code)
            mycommand.Parameters.AddWithValue("@wh_code", warehouseCode)
            mytable = MyInterface.SELECTData(mycommand, "getEVMList")
        Catch ex As Exception
        End Try
        Return mytable
    End Function
    Public Function GetEVM_FLCDataTable(ByRef dis_code As String, ByRef unitType As String, ByRef Optional flc_id As Int16 = 0) As DataTable
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@unitType", unitType)
            mycommand.Parameters.AddWithValue("@dis_code", dis_code)
            mycommand.Parameters.AddWithValue("@flc_id", flc_id)
            mytable = MyInterface.SELECTData(mycommand, "getEVMFLCList")
        Catch ex As Exception
        End Try
        Return mytable
    End Function

    Public Function GetEVMDistrictDataTable(ByRef dis_code As String, ByRef Optional unitType As String = "CU", ByRef Optional workingStatus As String = "XX") As DataTable
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@workingStatus", workingStatus)
            mycommand.Parameters.AddWithValue("@unitType", unitType)
            mycommand.Parameters.AddWithValue("@dis_code", dis_code)

            mytable = MyInterface.SELECTData(mycommand, "getEVMListDistrictLevel")
        Catch ex As Exception
        End Try
        Return mytable
    End Function
    Public Function GetEVMBlockDataTable(ByRef blk_code As String, ByRef Optional unitType As String = "CU", ByRef Optional workingStatus As String = "XX") As DataTable
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@workingStatus", workingStatus)
            mycommand.Parameters.AddWithValue("@unitType", unitType)
            mycommand.Parameters.AddWithValue("@blk_code", blk_code)

            mytable = MyInterface.SELECTData(mycommand, "getEVMListBlockLevel")
        Catch ex As Exception
        End Try
        Return mytable
    End Function
    Public Function GetEVMPendingForQRDataTable(ByRef Optional dis_code As String = "0000", ByRef Optional wh_code As String = "0000", ByRef Optional unitType As String = "CU", ByRef Optional workingStatus As String = "XX") As DataTable
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@dis_code", dis_code)
            mycommand.Parameters.AddWithValue("@warehouse_Cd", wh_code)
            If unitType = "BB" Then
                mycommand.Parameters.AddWithValue("@dis_code", dis_code)
                mytable = MyInterface.SELECTData(mycommand, "getBallotBoxListPendingForQR")
            Else
                mycommand.Parameters.AddWithValue("@unitType", unitType)
                mytable = MyInterface.SELECTData(mycommand, "getEVMListPendingForQR")
            End If

        Catch ex As Exception

        End Try
        Return mytable
    End Function
    Public Function GetEVMSummaryDataTable(ByRef Optional dis_code As String = "0000", ByRef Optional workingStatus As String = "XX") As DataTable
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@workingStatus", workingStatus)
            mycommand.Parameters.AddWithValue("@dis_code", dis_code)
            mycommand.Parameters.AddWithValue("@LangCode", HttpContext.Current.Session("MyLanguage").ToString())

            mytable = MyInterface.SELECTData(mycommand, "getEVMListSummary")

        Catch ex As Exception

        End Try
        Return mytable
    End Function


    Public Function GetStationaryDataTable(ByRef Optional dis_code As String = "00", ByRef Optional unitType As String = "FORMS", ByRef Optional workingStatus As String = "XX") As DataTable
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@workingStatus", workingStatus)
            mycommand.Parameters.AddWithValue("@unitType", unitType)
            mycommand.Parameters.AddWithValue("@dis_code", dis_code)
            mytable = MyInterface.SELECTData(mycommand, "getStationaryList")

        Catch ex As Exception

        End Try
        Return mytable
    End Function

    Public Function getBBListForGeneratingQR(ByRef bb_id As String, ByRef dis_code As String, ByRef Optional wh_code As String = "00") As DataTable
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@BBMID", bb_id)
            mycommand.Parameters.AddWithValue("@dis_code", dis_code)
            mycommand.Parameters.AddWithValue("@wh_code", wh_code)
            mytable = MyInterface.SELECTData(mycommand, "getBallotBoxListPendingForQR")

        Catch ex As Exception

        End Try
        Return mytable
    End Function
    Public Function getBBListForViewingQR(ByRef bb_id As String, ByRef dis_code As String, ByRef Optional wh_code As String = "00") As DataTable
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@BBMID", bb_id)
            mycommand.Parameters.AddWithValue("@dis_code", dis_code)
            mycommand.Parameters.AddWithValue("@wh_code", wh_code)
            mytable = MyInterface.SELECTData(mycommand, "getBallotBoxListPendingForQR")

        Catch ex As Exception

        End Try
        Return mytable
    End Function
    Public Function GetBallotBoxDataTable(ByRef Optional dis_code As String = "00", ByRef Optional wh_code As String = "00") As DataTable
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()

            mycommand.Parameters.AddWithValue("@dis_code", dis_code)
            mycommand.Parameters.AddWithValue("@wh_code", wh_code)
            mytable = MyInterface.SELECTData(mycommand, "getBallotBoxList")

        Catch ex As Exception

        End Try
        Return mytable
    End Function

    Public Function EncryptAES(clearText As String, Optional EncryptionKey As String = keyValue) As String
        Dim clearBytes As Byte() = Encoding.Unicode.GetBytes(clearText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {0, 0, 0, 0, 0, 0,
             0, 0, 0, 0, 0, 0,
             1})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write)
                    cs.Write(clearBytes, 0, clearBytes.Length)
                    cs.Close()
                End Using
                clearText = Convert.ToBase64String(ms.ToArray())
            End Using
        End Using
        Return clearText
    End Function
    Public Function DecryptAES(cipherText As String, Optional EncryptionKey As String = keyValue) As String
        Dim cipherBytes As Byte() = Convert.FromBase64String(cipherText)

        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {0, 0, 0, 0, 0, 0,
             0, 0, 0, 0, 0, 0,
             1})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write)
                    cs.Write(cipherBytes, 0, cipherBytes.Length)
                    cs.Close()
                End Using
                cipherText = Encoding.Unicode.GetString(ms.ToArray())
            End Using
        End Using
        Return cipherText
    End Function
    Public Function GetImage(ByVal img As Object) As String
        Return "data:image/jpg;base64," & Convert.ToBase64String(CType(img, Byte()))
    End Function
    Public Sub PopulateSECCommonMasterValues(ByVal ddl As DropDownList, ByVal category As String, Optional ByVal PostFor As String = "000")
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@cat_name", category)
            mycommand.Parameters.AddWithValue("@cat_Type", PostFor)
            mytable = MyInterface.SELECTData(mycommand, "getValuesCommonMaster")
            If mytable.TableName = "Success" Then
                Dim dv As DataView = mytable.DefaultView
                dv.Sort = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "Description_local", "description")
                ddl.DataSource = dv
                ddl.DataTextField = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "Description_local", "description")
                ddl.DataValueField = IIf(category.ToUpper = "GENDER" Or category.ToUpper = "RELATION" Or category.ToUpper = "MARITALSTATUS" Or category.ToUpper = "PARTYNAME", "abbr", "code") '"code"
                ddl.DataBind()
                ddl.Items.Insert(0, New ListItem(IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "-चयन-", "-Select-"), "000"))
                ddl.SelectedIndex = 0
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub PopulateSECCommonMasterValues(ByVal rbtn As RadioButtonList, ByVal category As String, Optional ByVal PostFor As String = "000")
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@cat_name", category)
            mycommand.Parameters.AddWithValue("@cat_Type", PostFor)
            mytable = MyInterface.SELECTData(mycommand, "getValuesCommonMaster")
            If mytable.TableName = "Success" Then
                Dim dv As DataView = mytable.DefaultView
                dv.Sort = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "Description_local", "description")
                rbtn.DataSource = dv
                rbtn.DataTextField = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "Description_local", "description")
                rbtn.DataValueField = IIf(category.ToUpper = "STATIONERY" Or category.ToUpper = "GENDER" Or category.ToUpper = "RELATION" Or category.ToUpper = "MARITALSTATUS" Or category.ToUpper = "PARTYNAME", "abbr", "code") '"code"
                rbtn.DataBind()
                If PostFor = "001" Then
                    rbtn.SelectedIndex = 2
                ElseIf PostFor = "003" Then
                    rbtn.SelectedIndex = 0
                ElseIf PostFor = "002" Then
                    rbtn.SelectedIndex = 1
                Else
                    rbtn.SelectedIndex = 0
                End If
            End If

        Catch ex As Exception
            Dim exM As String = ex.Message
        End Try
    End Sub
    Public Function ShowAvailableStockById(ByVal id As String) As StockItem.StockItem
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@id", id)
            mycommand.CommandText = "sec.ShowAvailableStockById" ' Initialize CommandText
            mytable = MyInterface.SELECTData(mycommand, "sec.ShowAvailableStockById")

            If mytable.TableName = "Success" AndAlso mytable.Rows.Count > 0 Then
                Dim stockItem As New StockItem.StockItem()

                stockItem.ID = mytable.Rows(0)("ID").ToString()
                stockItem.ItemType = mytable.Rows(0)("ItemType").ToString()
                stockItem.ItemType_Desc = mytable.Rows(0)("ItemType_Desc").ToString()
                stockItem.ItemType_Desc_Loc = mytable.Rows(0)("ItemType_Desc_Loc").ToString()
                stockItem.ItemFor = mytable.Rows(0)("ItemFor").ToString()
                stockItem.ItemFor_Desc = mytable.Rows(0)("ItemFor_Desc").ToString()
                stockItem.ItemFor_Desc_Loc = mytable.Rows(0)("ItemFor_Desc_Loc").ToString()
                stockItem.ItemName = mytable.Rows(0)("ItemName").ToString()
                stockItem.ItemNameLocal = mytable.Rows(0)("ItemNameLocal").ToString()
                stockItem.ItemDescription = mytable.Rows(0)("ItemDescription").ToString()
                stockItem.ItemDescriptionLocal = mytable.Rows(0)("ItemDescriptionLocal").ToString()
                stockItem.ItemSpecification = mytable.Rows(0)("ItemSpecification").ToString()
                stockItem.ItemCapacity = mytable.Rows(0)("ItemCapacity").ToString()
                stockItem.EntryBy = mytable.Rows(0)("EntryBy").ToString()
                stockItem.EnteredOn = CDate(mytable.Rows(0)("EnteredOn"))
                stockItem.de_activation_remakrs = mytable.Rows(0)("de_activation_remakrs").ToString()
                Return stockItem
            Else
                Return Nothing ' Or handle the case where no data is found
            End If

        Catch ex As Exception
            Dim exM As String = ex.Message
            ' Handle or log the exception
            Return Nothing
        End Try
    End Function
    Public Function GetUserIPAddress() As String
        Try
            Dim context As HttpContext = HttpContext.Current

            ' Check for X-Forwarded-For header (for load balancers, proxies)
            Dim forwardedForHeader As String = context.Request.Headers("X-Forwarded-For")

            If Not String.IsNullOrEmpty(forwardedForHeader) Then
                ' Get the first IP address in the list (can be comma-separated)
                Dim ipAddresses() As String = forwardedForHeader.Split(New Char() {","c}, StringSplitOptions.RemoveEmptyEntries)
                Return ipAddresses(0).Trim() ' Return the first IP
            End If

            ' Check for Remote IP Address (more reliable if no proxy/load balancer)
            Dim remoteIP As String = context.Request.ServerVariables("REMOTE_ADDR")

            If Not String.IsNullOrEmpty(remoteIP) Then
                Return remoteIP
            End If

            ' If all else fails, try user host address (less reliable)
            Dim userHostAddress As String = context.Request.UserHostAddress

            If Not String.IsNullOrEmpty(userHostAddress) Then
                Return userHostAddress
            End If

            Return "::1" ' Return "Unknown" if no IP address is found

        Catch ex As Exception
            ' Handle exceptions (log it, etc.)
            ' Important:  Don't expose detailed error information to the user in production.
            ' Log the exception details for debugging.
            System.Diagnostics.Debug.WriteLine("Error getting IP: " & ex.Message) ' For debugging
            Return "::1" ' Return an error indicator.
        End Try

    End Function
    Public Shared Function GetUserDataFromSession() As UserData

        If HttpContext.Current.Session("user") IsNot Nothing Then
            Try
                Dim parsedLogin As UserData = JsonConvert.DeserializeObject(Of UserData)(HttpContext.Current.Session("user").ToString())
                Return parsedLogin
            Catch ex As Exception
                ' Handle JSON deserialization errors (log, display message, etc.)
                Console.WriteLine("Error deserializing UserData: " & ex.Message)
                HttpContext.Current.Session.Remove("user") ' Clear the invalid session data
                HttpContext.Current.Response.Redirect("Login.aspx") ' Redirect to login
                Return Nothing ' Or throw the exception if you prefer
            End Try
        Else
            HttpContext.Current.Response.Redirect("Login.aspx") ' Redirect if session is empty
            Return Nothing ' Or throw an exception if you prefer
        End If
    End Function
    Public Function UserLogin(ByVal UserID As String, ByVal user_password As String, ByVal SessionId As String) As Generic_Response

        Dim cls = New Generic_Response()
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@UserID", UserID)
            mycommand.Parameters.AddWithValue("@user_password", user_password)
            mycommand.Parameters.AddWithValue("@ip", GetUserIPAddress())
            mycommand.Parameters.AddWithValue("@SessionID", SessionId)
            mycommand.Parameters.AddWithValue("@lang", HttpContext.Current.Session("MyLanguage").ToString())
            mycommand.Parameters.Add("@status_code", SqlDbType.Int).Direction = ParameterDirection.Output
            mycommand.Parameters.Add("@status_message", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output

            mycommand.CommandText = "[sec].[sec].[UserLogin]" ' Initialize CommandText
            mytable = MyInterface.SELECTData(mycommand, "[sec].[sec].[UserLogin]")

            cls.status_code = CInt(mycommand.Parameters("@status_code").Value)
            cls.status_message = mycommand.Parameters("@status_message").Value
            Dim InnerCls As New UserData()
            If cls.status_code = 200 Then
                For Each row As DataRow In mytable.Rows
                    Dim userData As New UserData()  ' Create a UserData object for each row

                    With userData ' Use With Polparty for cleaner code
                        .StateCode = If(row.Table.Columns.Contains("StateCode") AndAlso Not IsDBNull(row("StateCode")), row("StateCode").ToString(), "")
                        .DistrictCode = If(row.Table.Columns.Contains("DistrictCode") AndAlso Not IsDBNull(row("DistrictCode")), row("DistrictCode").ToString(), "")
                        .BlockCode = If(row.Table.Columns.Contains("BlockCode") AndAlso Not IsDBNull(row("BlockCode")), row("BlockCode").ToString(), "")
                        .PanchayatCode = If(row.Table.Columns.Contains("PanchayatCode") AndAlso Not IsDBNull(row("PanchayatCode")), row("PanchayatCode").ToString(), "")
                        .Code = If(row.Table.Columns.Contains("Code") AndAlso Not IsDBNull(row("Code")), row("Code").ToString(), "")
                        .UserLevel = If(row.Table.Columns.Contains("UserLevel") AndAlso Not IsDBNull(row("UserLevel")), row("UserLevel").ToString(), "")
                        .MobileNo = If(row.Table.Columns.Contains("MobileNo") AndAlso Not IsDBNull(row("MobileNo")), row("MobileNo").ToString(), "")
                        .EmailID = If(row.Table.Columns.Contains("EmailID") AndAlso Not IsDBNull(row("EmailID")), row("EmailID").ToString(), "")
                        .DESG = If(row.Table.Columns.Contains("DESG") AndAlso Not IsDBNull(row("DESG")), row("DESG").ToString(), "")
                        .Locked = If(row.Table.Columns.Contains("Locked") AndAlso Not IsDBNull(row("Locked")), row("Locked").ToString(), "")
                        .District_Name = If(row.Table.Columns.Contains("District_Name") AndAlso Not IsDBNull(row("District_Name")), row("District_Name").ToString(), "")
                        .Block_Name = If(row.Table.Columns.Contains("Block_Name") AndAlso Not IsDBNull(row("Block_Name")), row("Block_Name").ToString(), "")
                        .District_Name_local = If(row.Table.Columns.Contains("District_Name_local") AndAlso Not IsDBNull(row("District_Name_local")), row("District_Name_local").ToString(), "")
                        .block_name_local = If(row.Table.Columns.Contains("block_name_local") AndAlso Not IsDBNull(row("block_name_local")), row("block_name_local").ToString(), "")
                        .panchayat_code = If(row.Table.Columns.Contains("panchayat_code") AndAlso Not IsDBNull(row("panchayat_code")), row("panchayat_code").ToString(), "")
                        .panchayat_name_local = If(row.Table.Columns.Contains("panchayat_name_local") AndAlso Not IsDBNull(row("panchayat_name_local")), row("panchayat_name_local").ToString(), "")
                        .panchayat_name = If(row.Table.Columns.Contains("panchayat_name") AndAlso Not IsDBNull(row("panchayat_name")), row("panchayat_name").ToString(), "")
                        .IsDefaultPassWord = If(row.Table.Columns.Contains("IsDefaultPassWord") AndAlso Not IsDBNull(row("IsDefaultPassWord")), row("IsDefaultPassWord").ToString(), "")
                        .officerName = If(row.Table.Columns.Contains("officerName") AndAlso Not IsDBNull(row("officerName")), row("officerName").ToString(), "")
                        .landLine = If(row.Table.Columns.Contains("landLine") AndAlso Not IsDBNull(row("landLine")), row("landLine").ToString(), "")
                        .NNN_Type = If(row.Table.Columns.Contains("NNN_Type") AndAlso Not IsDBNull(row("NNN_Type")), row("NNN_Type").ToString(), "")
                        .NNN_Code = If(row.Table.Columns.Contains("NNN_Code") AndAlso Not IsDBNull(row("NNN_Code")), row("NNN_Code").ToString(), "")
                        .Description = If(row.Table.Columns.Contains("Description") AndAlso Not IsDBNull(row("Description")), row("Description").ToString(), "")
                        .Description_Local = If(row.Table.Columns.Contains("Description_Local") AndAlso Not IsDBNull(row("Description_Local")), row("Description_Local").ToString(), "")
                        .WareHouse_Address = If(row.Table.Columns.Contains("WareHouse_Address") AndAlso Not IsDBNull(row("WareHouse_Address")), row("WareHouse_Address").ToString(), "")
                        .WareHouse_Address_Local = If(row.Table.Columns.Contains("WareHouse_Address_Local") AndAlso Not IsDBNull(row("WareHouse_Address_Local")), row("WareHouse_Address_Local").ToString(), "")
                    End With
                    InnerCls = userData
                Next row
                cls.data = InnerCls
            End If

        Catch ex As Exception
            Dim exM As String = ex.Message
            ' Handle or log the exception
            cls.status_code = 500
            cls.status_message = ex.Message
            cls.status_message_local = ex.Message
        End Try
        Return cls
    End Function
    Public Function getBallotBoxListSummary(ByVal user_id As String) As Generic_Response
        Dim cls = New Generic_Response()
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@user_id", user_id)
            mycommand.Parameters.AddWithValue("@lang", HttpContext.Current.Session("MyLanguage").ToString())
            mycommand.Parameters.Add("@status_code", SqlDbType.Int).Direction = ParameterDirection.Output
            mycommand.Parameters.Add("@status_message", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output

            mycommand.CommandText = "[sec].[getBallotBoxListSummary]" ' Initialize CommandText
            mytable = MyInterface.SELECTData(mycommand, "[sec].[getBallotBoxListSummary]")
            cls.status_code = CInt(mycommand.Parameters("@status_code").Value)
            cls.status_message = mycommand.Parameters("@status_message").Value

            Dim InnerCls As New List(Of Ballot.BallotSummary)
            For Each row As DataRow In mytable.Rows
                Dim _Data As New Ballot.BallotSummary()

                With _Data
                    .to_user_code = If(row.Table.Columns.Contains("to_user_code") AndAlso Not IsDBNull(row("to_user_code")), row("to_user_code").ToString(), "")
                    .Display_Name = If(row.Table.Columns.Contains("Display_Name") AndAlso Not IsDBNull(row("Display_Name")), row("Display_Name").ToString(), "")
                    .No_Of_Units = If(row.Table.Columns.Contains("No_Of_Units") AndAlso Not IsDBNull(row("No_Of_Units")), row("No_Of_Units").ToString(), "")

                End With

                InnerCls.Add(_Data)
            Next row
            cls.data = InnerCls

        Catch ex As Exception
            Dim exM As String = ex.Message
            ' Handle or log the exception
            cls.status_code = 500
            cls.status_message = ex.Message
            cls.status_message_local = ex.Message
        End Try
        Return cls
    End Function
    Public Function getBallotBoxListSummary_Category_Wise(ByVal user_id As String) As Generic_Response
        Dim cls = New Generic_Response()
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@user_id", user_id)
            mycommand.Parameters.AddWithValue("@lang", HttpContext.Current.Session("MyLanguage").ToString())
            mycommand.Parameters.Add("@status_code", SqlDbType.Int).Direction = ParameterDirection.Output
            mycommand.Parameters.Add("@status_message", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output

            mycommand.CommandText = "[sec].[getBallotBoxListSummary_Category_Wise]" ' Initialize CommandText
            mytable = MyInterface.SELECTData(mycommand, "[sec].[getBallotBoxListSummary_Category_Wise]")
            cls.status_code = CInt(mycommand.Parameters("@status_code").Value)
            cls.status_message = mycommand.Parameters("@status_message").Value

            Dim InnerCls As New List(Of Ballot.BallotSummary)
            For Each row As DataRow In mytable.Rows
                Dim _Data As New Ballot.BallotSummary()

                With _Data
                    .to_user_code = If(row.Table.Columns.Contains("to_user_code") AndAlso Not IsDBNull(row("to_user_code")), row("to_user_code").ToString(), "")
                    .Display_Name = If(row.Table.Columns.Contains("Display_Name") AndAlso Not IsDBNull(row("Display_Name")), row("Display_Name").ToString(), "")
                    .No_Of_Units = If(row.Table.Columns.Contains("No_Of_Units") AndAlso Not IsDBNull(row("No_Of_Units")), row("No_Of_Units").ToString(), "")

                End With

                InnerCls.Add(_Data)
            Next row
            cls.data = InnerCls

        Catch ex As Exception
            Dim exM As String = ex.Message
            ' Handle or log the exception
            cls.status_code = 500
            cls.status_message = ex.Message
            cls.status_message_local = ex.Message
        End Try
        Return cls
    End Function

    Public Function getBallotBoxListSummary_Unit_Wise(ByVal user_id As String, ByVal qr_code As String) As Generic_Response
        Dim cls = New Generic_Response()
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@user_id", user_id)
            mycommand.Parameters.AddWithValue("@qr_code", qr_code)
            mycommand.Parameters.AddWithValue("@lang", HttpContext.Current.Session("MyLanguage").ToString())
            mycommand.Parameters.Add("@status_code", SqlDbType.Int).Direction = ParameterDirection.Output
            mycommand.Parameters.Add("@status_message", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output

            mycommand.CommandText = "[sec].[getBallotBoxListSummary_Unit_Wise]" ' Initialize CommandText
            mytable = MyInterface.SELECTData(mycommand, "[sec].[getBallotBoxListSummary_Unit_Wise]")
            cls.status_code = CInt(mycommand.Parameters("@status_code").Value)
            cls.status_message = mycommand.Parameters("@status_message").Value

            Dim InnerCls As New List(Of Ballot.BallotSummaryList)
            For Each row As DataRow In mytable.Rows
                Dim _Data As New Ballot.BallotSummaryList()

                With _Data
                    .to_user_code = If(row.Table.Columns.Contains("to_user_code") AndAlso Not IsDBNull(row("to_user_code")), row("to_user_code").ToString(), "")
                    .Display_Name = If(row.Table.Columns.Contains("Display_Name") AndAlso Not IsDBNull(row("Display_Name")), row("Display_Name").ToString(), "")
                    .latitude = If(row.Table.Columns.Contains("latitude") AndAlso Not IsDBNull(row("latitude")), row("latitude").ToString(), "")
                    .longitude = If(row.Table.Columns.Contains("longitude") AndAlso Not IsDBNull(row("longitude")), row("longitude").ToString(), "")
                    .button_text = If(row.Table.Columns.Contains("button_text") AndAlso Not IsDBNull(row("button_text")), row("button_text").ToString(), "")
                    .qr_text = If(row.Table.Columns.Contains("qr_text") AndAlso Not IsDBNull(row("qr_text")), row("qr_text").ToString(), "")
                End With

                InnerCls.Add(_Data)
            Next row
            cls.data = InnerCls

        Catch ex As Exception
            Dim exM As String = ex.Message
            ' Handle or log the exception
            cls.status_code = 500
            cls.status_message = ex.Message
            cls.status_message_local = ex.Message
        End Try
        Return cls
    End Function
    Public Function Ballot_Series_Master_Get() As Generic_Response

        Dim cls = New Generic_Response()
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@lang", HttpContext.Current.Session("MyLanguage").ToString())
            mycommand.Parameters.Add("@status_code", SqlDbType.Int).Direction = ParameterDirection.Output
            mycommand.Parameters.Add("@status_message", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output

            mycommand.CommandText = "[sec].[Ballot_Series_Master_Get]" ' Initialize CommandText
            mytable = MyInterface.SELECTData(mycommand, "[sec].[Ballot_Series_Master_Get]")

            cls.status_code = CInt(mycommand.Parameters("@status_code").Value)
            cls.status_message = mycommand.Parameters("@status_message").Value

            Dim InnerCls As New List(Of Ballot.Ballot_Series)
            For Each row As DataRow In mytable.Rows
                Dim _Data As New Ballot.Ballot_Series()

                With _Data
                    .series = If(row.Table.Columns.Contains("series") AndAlso Not IsDBNull(row("series")), row("series").ToString(), "")
                    .box_size = If(row.Table.Columns.Contains("box_size") AndAlso Not IsDBNull(row("box_size")), row("box_size").ToString(), "U")

                    ' Handle series_datetime correctly (convert to DateTime)
                    If row.Table.Columns.Contains("series_datetime") AndAlso Not IsDBNull(row("series_datetime")) Then
                        Dim dateTimeValue As String = row("series_datetime").ToString()
                        Dim parsedDateTime As DateTime

                        If DateTime.TryParse(dateTimeValue, parsedDateTime) Then  ' TryParse for handle null issue
                            .series_datetime = parsedDateTime
                        Else
                            ' Handle parsing error (e.g., log, set default value)
                            .series_datetime = DateTime.MinValue ' Or another default value

                        End If
                    Else
                        .series_datetime = DateTime.MinValue ' Or another default value
                    End If
                    .series_state = If(row.Table.Columns.Contains("series_state") AndAlso Not IsDBNull(row("series_state")), row("series_state").ToString(), "") ' Add series_state
                End With

                InnerCls.Add(_Data)
            Next row
            cls.data = InnerCls

        Catch ex As Exception
            Dim exM As String = ex.Message
            ' Handle or log the exception
            cls.status_code = 500
            cls.status_message = ex.Message
            cls.status_message_local = ex.Message
        End Try
        Return cls
    End Function
    Public Function State_Master_Get() As Generic_Response

        Dim cls = New Generic_Response()
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@lang", HttpContext.Current.Session("MyLanguage").ToString())
            mycommand.Parameters.Add("@status_code", SqlDbType.Int).Direction = ParameterDirection.Output
            mycommand.Parameters.Add("@status_message", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output

            mycommand.CommandText = "[sec].[State_Master_Get]" ' Initialize CommandText
            mytable = MyInterface.SELECTData(mycommand, "[sec].[State_Master_Get]")

            cls.status_code = CInt(mycommand.Parameters("@status_code").Value)
            cls.status_message = mycommand.Parameters("@status_message").Value

            Dim InnerCls As New List(Of State_Master)
            For Each row As DataRow In mytable.Rows
                Dim _Data As New State_Master()

                With _Data
                    .State_Code = If(row.Table.Columns.Contains("State_Code") AndAlso Not IsDBNull(row("State_Code")), row("State_Code").ToString(), "")
                    .State_Name = If(row.Table.Columns.Contains("State_Name") AndAlso Not IsDBNull(row("State_Name")), row("State_Name").ToString(), "")
                End With
                InnerCls.Add(_Data)
            Next row
            cls.data = InnerCls

        Catch ex As Exception
            Dim exM As String = ex.Message
            ' Handle or log the exception
            cls.status_code = 500
            cls.status_message = ex.Message
            cls.status_message_local = ex.Message
        End Try
        Return cls
    End Function
    Public Function District_Master_Get(state_code As String) As Generic_Response

        Dim cls = New Generic_Response()
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@state_code", state_code)
            mycommand.Parameters.AddWithValue("@lang", HttpContext.Current.Session("MyLanguage").ToString())
            mycommand.Parameters.Add("@status_code", SqlDbType.Int).Direction = ParameterDirection.Output
            mycommand.Parameters.Add("@status_message", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output

            mycommand.CommandText = "[sec].[District_Master_Get]" ' Initialize CommandText
            mytable = MyInterface.SELECTData(mycommand, "[sec].[District_Master_Get]")

            cls.status_code = CInt(mycommand.Parameters("@status_code").Value)
            cls.status_message = mycommand.Parameters("@status_message").Value

            Dim InnerCls As New List(Of District_Master)
            For Each row As DataRow In mytable.Rows
                Dim _Data As New District_Master()

                With _Data
                    .District_Code = If(row.Table.Columns.Contains("District_Code") AndAlso Not IsDBNull(row("District_Code")), row("District_Code").ToString(), "")
                    .District_Name = If(row.Table.Columns.Contains("District_Name") AndAlso Not IsDBNull(row("District_Name")), row("District_Name").ToString(), "")
                End With
                InnerCls.Add(_Data)
            Next row
            cls.data = InnerCls
        Catch ex As Exception
            Dim exM As String = ex.Message
            ' Handle or log the exception
            cls.status_code = 500
            cls.status_message = ex.Message
            cls.status_message_local = ex.Message
        End Try
        Return cls
    End Function
    Public Function Polparty_Master_Get(block_code As String) As Generic_Response

        Dim cls = New Generic_Response()
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@block_code", block_code)
            mycommand.Parameters.AddWithValue("@lang", HttpContext.Current.Session("MyLanguage").ToString())
            mycommand.Parameters.Add("@status_code", SqlDbType.Int).Direction = ParameterDirection.Output
            mycommand.Parameters.Add("@status_message", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output

            mycommand.CommandText = "[sec].[Polparty_Master_Get]" ' Initialize CommandText
            mytable = MyInterface.SELECTData(mycommand, "[sec].[Polparty_Master_Get]")

            cls.status_code = CInt(mycommand.Parameters("@status_code").Value)
            cls.status_message = mycommand.Parameters("@status_message").Value

            Dim InnerCls As New List(Of Polparty_Master)
            For Each row As DataRow In mytable.Rows
                Dim _Data As New Polparty_Master()

                With _Data
                    .Polparty_Code = If(row.Table.Columns.Contains("Polparty_Code") AndAlso Not IsDBNull(row("Polparty_Code")), row("Polparty_Code").ToString(), "")
                    .Polparty_Name = If(row.Table.Columns.Contains("Polparty_Name") AndAlso Not IsDBNull(row("Polparty_Name")), row("Polparty_Name").ToString(), "")
                End With
                InnerCls.Add(_Data)
            Next row
            cls.data = InnerCls
        Catch ex As Exception
            Dim exM As String = ex.Message
            ' Handle or log the exception
            cls.status_code = 500
            cls.status_message = ex.Message
            cls.status_message_local = ex.Message
        End Try
        Return cls
    End Function
    Public Function Block_Master_Get(state_code As String, district_code As String) As Generic_Response

        Dim cls = New Generic_Response()
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@state_code", state_code)
            mycommand.Parameters.AddWithValue("@district_code", district_code)
            mycommand.Parameters.AddWithValue("@lang", HttpContext.Current.Session("MyLanguage").ToString())
            mycommand.Parameters.Add("@status_code", SqlDbType.Int).Direction = ParameterDirection.Output
            mycommand.Parameters.Add("@status_message", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output

            mycommand.CommandText = "[sec].[Block_Master_Get]" ' Initialize CommandText
            mytable = MyInterface.SELECTData(mycommand, "[sec].[Block_Master_Get]")

            cls.status_code = CInt(mycommand.Parameters("@status_code").Value)
            cls.status_message = mycommand.Parameters("@status_message").Value

            Dim InnerCls As New List(Of Block_Master)
            For Each row As DataRow In mytable.Rows
                Dim _Data As New Block_Master()

                With _Data
                    .Block_Code = If(row.Table.Columns.Contains("Block_Code") AndAlso Not IsDBNull(row("Block_Code")), row("Block_Code").ToString(), "")
                    .Block_Name = If(row.Table.Columns.Contains("Block_Name") AndAlso Not IsDBNull(row("Block_Name")), row("Block_Name").ToString(), "")
                End With
                InnerCls.Add(_Data)
            Next row
            cls.data = InnerCls
        Catch ex As Exception
            Dim exM As String = ex.Message
            ' Handle or log the exception
            cls.status_code = 500
            cls.status_message = ex.Message
            cls.status_message_local = ex.Message
        End Try
        Return cls
    End Function
    Public Function Warehouse_Master_Get(state_code As String, district_code As String) As Generic_Response

        Dim cls = New Generic_Response()
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@state_code", state_code)
            mycommand.Parameters.AddWithValue("@district_code", district_code)
            mycommand.Parameters.AddWithValue("@lang", HttpContext.Current.Session("MyLanguage").ToString())
            mycommand.Parameters.Add("@status_code", SqlDbType.Int).Direction = ParameterDirection.Output
            mycommand.Parameters.Add("@status_message", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output

            mycommand.CommandText = "[sec].[Warehouse_Master_Get]" ' Initialize CommandText
            mytable = MyInterface.SELECTData(mycommand, "[sec].[Warehouse_Master_Get]")

            cls.status_code = CInt(mycommand.Parameters("@status_code").Value)
            cls.status_message = mycommand.Parameters("@status_message").Value

            Dim InnerCls As New List(Of Warehouse_Master)
            For Each row As DataRow In mytable.Rows
                Dim _Data As New Warehouse_Master()

                With _Data
                    .Warehouse_Code = If(row.Table.Columns.Contains("Warehouse_Code") AndAlso Not IsDBNull(row("Warehouse_Code")), row("Warehouse_Code").ToString(), "")
                    .Warehouse_Name = If(row.Table.Columns.Contains("Warehouse_Name") AndAlso Not IsDBNull(row("Warehouse_Name")), row("Warehouse_Name").ToString(), "")
                End With
                InnerCls.Add(_Data)
            Next row
            cls.data = InnerCls
        Catch ex As Exception
            Dim exM As String = ex.Message
            ' Handle or log the exception
            cls.status_code = 500
            cls.status_message = ex.Message
            cls.status_message_local = ex.Message
        End Try
        Return cls
    End Function
    Public Function ShowAvailable_Ballot(ByVal UserID As String, ByVal lot_id As String, ByVal to_user_code As String) As Generic_Response

        Dim cls = New Generic_Response()
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@user_id", UserID)
            mycommand.Parameters.AddWithValue("@to_user_code", to_user_code)
            mycommand.Parameters.AddWithValue("@lang", HttpContext.Current.Session("MyLanguage").ToString())
            mycommand.Parameters.Add("@status_code", SqlDbType.Int).Direction = ParameterDirection.Output
            mycommand.Parameters.Add("@status_message", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output

            mycommand.CommandText = "[sec].[ShowAvailable_Ballot]" ' Initialize CommandText
            mytable = MyInterface.SELECTData(mycommand, "[sec].[ShowAvailable_Ballot]")

            cls.status_code = CInt(mycommand.Parameters("@status_code").Value)
            cls.status_message = mycommand.Parameters("@status_message").Value

            Dim InnerCls As New List(Of Ballot.Ballot_Stock)
            For Each row As DataRow In mytable.Rows
                Dim _Data As New Ballot.Ballot_Stock()

                With _Data
                    .series = If(row.Table.Columns.Contains("series") AndAlso Not IsDBNull(row("series")), row("series").ToString(), "")
                    .from_user_id = If(row.Table.Columns.Contains("from_user_id") AndAlso Not IsDBNull(row("from_user_id")), row("from_user_id").ToString(), "")
                    .sr_no_range = If(row.Table.Columns.Contains("sr_no_range") AndAlso Not IsDBNull(row("sr_no_range")), row("sr_no_range").ToString(), "")
                    .box_size = If(row.Table.Columns.Contains("box_size") AndAlso Not IsDBNull(row("box_size")), row("box_size").ToString(), "")
                    .to_user_code = If(row.Table.Columns.Contains("to_user_code") AndAlso Not IsDBNull(row("to_user_code")), row("to_user_code").ToString(), "")
                    .MonYrPurchase = If(row.Table.Columns.Contains("MonYrPurchase") AndAlso Not IsDBNull(row("MonYrPurchase")), row("MonYrPurchase").ToString(), "")
                    ' Convert Int64 properties safely
                    Dim totalQty As Long, fromSrNo As Long, toSrNo As Long, itemsAllocated As Long
                    If row.Table.Columns.Contains("total_qty") AndAlso Not IsDBNull(row("total_qty")) AndAlso Long.TryParse(row("total_qty").ToString(), totalQty) Then
                        .total_qty = totalQty
                    Else
                        .total_qty = 0 ' Or another default value
                    End If

                    If row.Table.Columns.Contains("from_sr_no") AndAlso Not IsDBNull(row("from_sr_no")) AndAlso Long.TryParse(row("from_sr_no").ToString(), fromSrNo) Then
                        .from_sr_no = fromSrNo
                    Else
                        .from_sr_no = 0
                    End If

                    If row.Table.Columns.Contains("to_sr_no") AndAlso Not IsDBNull(row("to_sr_no")) AndAlso Long.TryParse(row("to_sr_no").ToString(), toSrNo) Then
                        .to_sr_no = toSrNo
                    Else
                        .to_sr_no = 0
                    End If

                    If row.Table.Columns.Contains("items_allocated_to_selected_user") AndAlso Not IsDBNull(row("items_allocated_to_selected_user")) AndAlso Long.TryParse(row("items_allocated_to_selected_user").ToString(), itemsAllocated) Then
                        .items_allocated_to_selected_user = itemsAllocated
                    Else
                        .items_allocated_to_selected_user = 0
                    End If



                    ' Convert Decimal properties safely
                    Dim latitude As Decimal, longitude As Decimal
                    If row.Table.Columns.Contains("latitude") AndAlso Not IsDBNull(row("latitude")) AndAlso Decimal.TryParse(row("latitude").ToString(), latitude) Then
                        .latitude = latitude
                    Else
                        .latitude = 0 ' Or another default value
                    End If

                    If row.Table.Columns.Contains("longitude") AndAlso Not IsDBNull(row("longitude")) AndAlso Decimal.TryParse(row("longitude").ToString(), longitude) Then
                        .longitude = longitude
                    Else
                        .longitude = 0
                    End If

                    ' Convert Boolean property safely
                    Dim isReceived As Boolean, hasQr As Boolean
                    If row.Table.Columns.Contains("is_received") AndAlso Not IsDBNull(row("is_received")) AndAlso Boolean.TryParse(row("is_received").ToString(), isReceived) Then
                        .is_received = isReceived
                    Else
                        .is_received = False ' Or another default value
                    End If

                    If row.Table.Columns.Contains("has_qr") AndAlso Not IsDBNull(row("has_qr")) AndAlso Boolean.TryParse(row("has_qr").ToString(), hasQr) Then
                        .has_qr = hasQr
                    Else
                        .has_qr = False ' Or another default value
                    End If


                End With

                InnerCls.Add(_Data)
            Next row
            cls.data = InnerCls

        Catch ex As Exception
            Dim exM As String = ex.Message
            ' Handle or log the exception
            cls.status_code = 500
            cls.status_message = ex.Message
            cls.status_message_local = ex.Message
        End Try
        Return cls
    End Function
    Public Function GetStationeryStockSummary(ByVal user_id As String, ByVal ItemFor As String, ByVal ItemType As String, ByVal is_self As Boolean) As Generic_Response

        Dim cls = New Generic_Response()
        Try
            ItemFor = IIf(ItemFor = "003", "MC", IIf(ItemFor = "002", "NPLK", "GPYT"))
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@user_id", user_id)
            mycommand.Parameters.AddWithValue("@ItemFor", ItemFor)
            mycommand.Parameters.AddWithValue("@ItemType", ItemType)
            mycommand.Parameters.AddWithValue("@is_self", is_self)
            mycommand.Parameters.AddWithValue("@lang", HttpContext.Current.Session("MyLanguage").ToString())
            mycommand.Parameters.Add("@status_code", SqlDbType.Int).Direction = ParameterDirection.Output
            mycommand.Parameters.Add("@status_message", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output

            mycommand.CommandText = "[sec].[GetStationeryStockSummary]" ' Initialize CommandText
            mytable = MyInterface.SELECTData(mycommand, "[sec].[GetStationeryStockSummary]")

            cls.status_code = CInt(mycommand.Parameters("@status_code").Value)
            cls.status_message = mycommand.Parameters("@status_message").Value

            Dim InnerCls As New List(Of StockItem.StockSummary)
            For Each row As DataRow In mytable.Rows
                Dim _Data As New StockItem.StockSummary()
                With _Data
                    .ID = If(IsDBNull(row("ID")), "", row("ID").ToString())
                    .ItemType = If(IsDBNull(row("ItemType")), "", row("ItemType").ToString())
                    .ItemFor = If(IsDBNull(row("ItemFor")), "", row("ItemFor").ToString())
                    .ItemName = If(IsDBNull(row("ItemName")), "", row("ItemName").ToString())
                    .ItemDescription = If(IsDBNull(row("ItemDescription")), "", row("ItemDescription").ToString())
                    .MyTotalStock = If(IsDBNull(row("MyTotalStock")), 0, CLng(row("MyTotalStock")))
                    .MyEnteredStock = If(IsDBNull(row("MyEnteredStock")), 0, CLng(row("MyEnteredStock")))
                    .AllocatedToMe = If(IsDBNull(row("AllocatedToMe")), 0, CLng(row("AllocatedToMe")))
                    .AvailableForAllocation = If(IsDBNull(row("AvailableForAllocation")), 0, CLng(row("AvailableForAllocation")))
                    .AllocatedToOthers = If(IsDBNull(row("AllocatedToOthers")), 0, CLng(row("AllocatedToOthers")))
                    .Disposed = If(IsDBNull(row("Disposed")), 0, CLng(row("Disposed")))
                    .TotalConsumed = If(IsDBNull(row("TotalConsumed")), 0, CLng(row("TotalConsumed")))
                End With

                InnerCls.Add(_Data)
            Next row
            cls.data = InnerCls

        Catch ex As Exception
            Dim exM As String = ex.Message
            ' Handle or log the exception
            cls.status_code = 500
            cls.status_message = ex.Message
            cls.status_message_local = ex.Message
        End Try
        Return cls
    End Function
    Public Function ShowPendingQR_Ballot(ByVal UserID As String) As Generic_Response

        Dim cls = New Generic_Response()
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@user_id", UserID)
            mycommand.Parameters.AddWithValue("@lang", HttpContext.Current.Session("MyLanguage").ToString())
            mycommand.Parameters.Add("@status_code", SqlDbType.Int).Direction = ParameterDirection.Output
            mycommand.Parameters.Add("@status_message", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output

            mycommand.CommandText = "[sec].[ShowPendingQR_Ballot]" ' Initialize CommandText
            mytable = MyInterface.SELECTData(mycommand, "[sec].[ShowPendingQR_Ballot]")

            cls.status_code = CInt(mycommand.Parameters("@status_code").Value)
            cls.status_message = mycommand.Parameters("@status_message").Value

            Dim InnerCls As New List(Of Ballot.PendingQR)
            For Each row As DataRow In mytable.Rows
                Dim _Data As New Ballot.PendingQR()

                With _Data
                    .series = If(row.Table.Columns.Contains("series") AndAlso Not IsDBNull(row("series")), row("series").ToString(), "")
                    .box_size = If(row.Table.Columns.Contains("box_size") AndAlso Not IsDBNull(row("box_size")), row("box_size").ToString(), "")
                    .qr_text = If(row.Table.Columns.Contains("qr_text") AndAlso Not IsDBNull(row("qr_text")), row("qr_text").ToString(), "")
                    ' Convert Int64 properties safely
                    Dim srno As Long
                    If row.Table.Columns.Contains("sr_no") AndAlso Not IsDBNull(row("sr_no")) AndAlso Long.TryParse(row("sr_no").ToString(), srno) Then
                        .sr_no = srno
                    Else
                        .sr_no = 0 ' Or another default value
                    End If

                End With

                InnerCls.Add(_Data)
            Next row
            cls.data = InnerCls

        Catch ex As Exception
            Dim exM As String = ex.Message
            ' Handle or log the exception
            cls.status_code = 500
            cls.status_message = ex.Message
            cls.status_message_local = ex.Message
        End Try
        Return cls
    End Function
    Public Function Show_Ballot_ByID(ByVal UserID As String, ByVal from_sr_no As String, ByVal to_sr_no As String, ByVal series As String) As Generic_Response

        Dim cls = New Generic_Response()
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@user_id", UserID)
            mycommand.Parameters.AddWithValue("@from_sr_no", from_sr_no)
            mycommand.Parameters.AddWithValue("@to_sr_no", to_sr_no)
            mycommand.Parameters.AddWithValue("@series", series)
            mycommand.Parameters.AddWithValue("@lang", HttpContext.Current.Session("MyLanguage").ToString())
            mycommand.Parameters.Add("@status_code", SqlDbType.Int).Direction = ParameterDirection.Output
            mycommand.Parameters.Add("@status_message", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output

            mycommand.CommandText = "[sec].[Show_Ballot_ByID]" ' Initialize CommandText
            mytable = MyInterface.SELECTData(mycommand, "[sec].[Show_Ballot_ByID]")

            cls.status_code = CInt(mycommand.Parameters("@status_code").Value)
            cls.status_message = mycommand.Parameters("@status_message").Value

            Dim InnerCls As New List(Of Ballot.Print_QR)
            For Each row As DataRow In mytable.Rows
                Dim _Data As New Ballot.Print_QR()

                With _Data
                    .qr_text_description = If(row.Table.Columns.Contains("qr_text_description") AndAlso Not IsDBNull(row("qr_text_description")), row("qr_text_description").ToString(), "")
                    .qr_text = If(row.Table.Columns.Contains("qr_text") AndAlso Not IsDBNull(row("qr_text")), row("qr_text").ToString(), "")
                End With

                InnerCls.Add(_Data)
            Next row
            cls.data = InnerCls

        Catch ex As Exception
            Dim exM As String = ex.Message
            ' Handle or log the exception
            cls.status_code = 500
            cls.status_message = ex.Message
            cls.status_message_local = ex.Message
        End Try
        Return cls
    End Function
    Public Function Show_Ballot_Owner(from_user_id As String) As Generic_Response

        Dim cls = New Generic_Response()
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@user_id", from_user_id)
            mycommand.Parameters.AddWithValue("@lang", HttpContext.Current.Session("MyLanguage").ToString())
            mycommand.Parameters.Add("@status_code", SqlDbType.Int).Direction = ParameterDirection.Output
            mycommand.Parameters.Add("@status_message", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output

            mycommand.CommandText = "[sec].[Show_Ballot_Owner]" ' Initialize CommandText
            mytable = MyInterface.SELECTData(mycommand, "[sec].[Show_Ballot_Owner]")

            cls.status_code = CInt(mycommand.Parameters("@status_code").Value)
            cls.status_message = mycommand.Parameters("@status_message").Value

            Dim InnerCls As New List(Of Ballot.Ballot_Stock_Owner)
            For Each row As DataRow In mytable.Rows
                Dim _Data As New Ballot.Ballot_Stock_Owner()

                With _Data
                    .series = If(IsDBNull(row("series")), Nothing, row("series").ToString())
                    .box_size = If(IsDBNull(row("box_size")), Nothing, row("box_size").ToString())
                    .total_qty = If(IsDBNull(row("total_qty")), 0, Convert.ToInt64(row("total_qty")))
                    .from_sr_no = If(IsDBNull(row("from_sr_no")), 0, Convert.ToInt64(row("from_sr_no")))
                    .to_sr_no = If(IsDBNull(row("to_sr_no")), 0, Convert.ToInt64(row("to_sr_no")))
                    .purchase_month = If(IsDBNull(row("purchase_month")), 0, Convert.ToInt32(row("purchase_month")))
                    .purchase_year = If(IsDBNull(row("purchase_year")), 0, Convert.ToInt32(row("purchase_year")))
                    .MonYrPurchase = If(IsDBNull(row("MonYrPurchase")), Nothing, row("MonYrPurchase").ToString())
                    .purchaser_user_code = If(IsDBNull(row("purchaser_user_code")), Nothing, row("purchaser_user_code").ToString())
                    .sr_no_range = If(IsDBNull(row("sr_no_range")), Nothing, row("sr_no_range").ToString())
                    .purchaser_user_id = If(IsDBNull(row("purchaser_user_id")), Nothing, row("purchaser_user_id").ToString())
                    .entry_date = If(IsDBNull(row("entry_date")), DateTime.MinValue, Convert.ToDateTime(row("entry_date")))

                End With

                InnerCls.Add(_Data)
            Next row

            cls.data = InnerCls

        Catch ex As Exception
            Dim exM As String = ex.Message
            ' Handle or log the exception
            cls.status_code = 500
            cls.status_message = ex.Message
            cls.status_message_local = ex.Message
        End Try
        Return cls
    End Function
    Public Function Show_Ballot_User_Id(from_user_id As String, to_user_id As String, direction As String) As Generic_Response

        Dim cls = New Generic_Response()
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@from_user_id", from_user_id)
            mycommand.Parameters.AddWithValue("@to_user_id", to_user_id)
            mycommand.Parameters.AddWithValue("@direction", direction)
            mycommand.Parameters.AddWithValue("@lang", HttpContext.Current.Session("MyLanguage").ToString())
            mycommand.Parameters.Add("@status_code", SqlDbType.Int).Direction = ParameterDirection.Output
            mycommand.Parameters.Add("@status_message", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output

            mycommand.CommandText = "[sec].[Show_Ballot_User_Id]" ' Initialize CommandText
            mytable = MyInterface.SELECTData(mycommand, "[sec].[Show_Ballot_User_Id]")

            cls.status_code = CInt(mycommand.Parameters("@status_code").Value)
            cls.status_message = mycommand.Parameters("@status_message").Value

            Dim InnerCls As New List(Of Ballot.Allocate_QR)
            Dim _Data As New Ballot.Allocate_QR()
            For Each row As DataRow In mytable.Rows
                _Data = New Ballot.Allocate_QR()
                With _Data
                    .qr_text_description = If(row.Table.Columns.Contains("qr_text_description") AndAlso Not IsDBNull(row("qr_text_description")), row("qr_text_description").ToString(), "")
                    .from_user_id = If(row.Table.Columns.Contains("from_user_id") AndAlso Not IsDBNull(row("from_user_id")), row("from_user_id").ToString(), "")
                    .to_user_id = If(row.Table.Columns.Contains("to_user_id") AndAlso Not IsDBNull(row("to_user_id")), row("to_user_id").ToString(), "")
                    .is_received = Convert.ToBoolean(If(row.Table.Columns.Contains("is_received") AndAlso Not IsDBNull(row("is_received")), row("is_received").ToString(), "0"))
                End With
                InnerCls.Add(_Data)
            Next row
            cls.data = InnerCls
        Catch ex As Exception
            Dim exM As String = ex.Message
            ' Handle or log the exception
            cls.status_code = 500
            cls.status_message = ex.Message
            cls.status_message_local = ex.Message
        End Try
        Return cls
    End Function
    Public Function stationeryMaster_Post(ID As Integer,
                                          itemType As String,
                                          ItemFor As String,
                                          ItemName As String,
                                          ItemNameLocal As String,
                                          ItemDescription As String,
                                          ItemDescriptionLocal As String,
                                          itemSpecification As String,
                                          EntryBy As String) As Generic_Response
        Dim cls = New Generic_Response
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@ID", ID)
            mycommand.Parameters.AddWithValue("@ItemType", itemType)
            mycommand.Parameters.AddWithValue("@ItemFor", ItemFor)
            mycommand.Parameters.AddWithValue("@ItemName", ItemName)
            mycommand.Parameters.AddWithValue("@ItemNameLocal", ItemNameLocal)
            mycommand.Parameters.AddWithValue("@ItemDescription", ItemDescription)
            mycommand.Parameters.AddWithValue("@ItemDescriptionLocal", ItemDescriptionLocal)
            mycommand.Parameters.AddWithValue("@ItemSpecification", itemSpecification)
            mycommand.Parameters.AddWithValue("@EntryBy", EntryBy)

            mycommand.Parameters.Add("@status_code", SqlDbType.Int).Direction = ParameterDirection.Output
            mycommand.Parameters.Add("@status_message", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
            mycommand.Parameters.Add("@status_message_local", SqlDbType.NVarChar, 200).Direction = ParameterDirection.Output

            mycommand.CommandText = "sec.stationeryMaster_Post" ' Initialize CommandText
            mytable = MyInterface.INSERTData(mycommand, "sec.stationeryMaster_Post")

            cls.status_code = CInt(mycommand.Parameters("@status_code").Value)
            cls.status_message = mycommand.Parameters("@status_message").Value
            cls.status_message_local = mycommand.Parameters("@status_message_local").Value

        Catch ex As Exception
            'Dim exM As String = ex.Message
            ' Handle or log the exception

            cls.status_code = 500
            cls.status_message = ex.Message
            cls.status_message_local = ex.Message
        End Try
        Return cls
    End Function
    Public Function UserLogin_PasswordUpdate(username As String, user_password As String) As Generic_Response
        Dim cls = New Generic_Response
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@username", username)
            mycommand.Parameters.AddWithValue("@user_password", user_password)
            mycommand.Parameters.AddWithValue("@lang", HttpContext.Current.Session("MyLanguage").ToString())

            mycommand.Parameters.Add("@status_code", SqlDbType.Int).Direction = ParameterDirection.Output
            mycommand.Parameters.Add("@status_message", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output

            mycommand.CommandText = "[sec].[sec].[UserLogin_PasswordUpdate]" ' Initialize CommandText
            mytable = MyInterface.INSERTData(mycommand, "[sec].[sec].[UserLogin_PasswordUpdate]")

            cls.status_code = CInt(mycommand.Parameters("@status_code").Value)
            cls.status_message = mycommand.Parameters("@status_message").Value
        Catch ex As Exception
            cls.status_code = 500
            cls.status_message = ex.Message
            cls.status_message_local = ex.Message
        End Try
        Return cls
    End Function
    Public Function deleteBallotMaster(user_id As String, series As String, box_size As String, FromSrNo As String, ToSrNo As String) As Generic_Response
        Dim cls = New Generic_Response
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@user_id", user_id)
            mycommand.Parameters.AddWithValue("@series", series)
            mycommand.Parameters.AddWithValue("@box_size", box_size)
            mycommand.Parameters.AddWithValue("@FromSrNo", FromSrNo)
            mycommand.Parameters.AddWithValue("@ToSrNo", ToSrNo)
            mycommand.Parameters.AddWithValue("@lang", HttpContext.Current.Session("MyLanguage").ToString())
            mycommand.Parameters.Add("@status_code", SqlDbType.Int).Direction = ParameterDirection.Output
            mycommand.Parameters.Add("@status_message", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output

            mycommand.CommandText = "[sec].[deleteBallotMaster]" ' Initialize CommandText
            mytable = MyInterface.INSERTData(mycommand, "[sec].[deleteBallotMaster]")

            cls.status_code = CInt(mycommand.Parameters("@status_code").Value)
            cls.status_message = mycommand.Parameters("@status_message").Value
        Catch ex As Exception
            cls.status_code = 500
            cls.status_message = ex.Message
            cls.status_message_local = ex.Message
        End Try
        Return cls
    End Function
    Public Function saveBallotQR(user_id As String, series As String, box_size As String, sr_no As Int64, qr_text As String) As Generic_Response
        Dim cls = New Generic_Response
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@user_id", user_id)
            mycommand.Parameters.AddWithValue("@series", series)
            mycommand.Parameters.AddWithValue("@box_size", box_size)
            mycommand.Parameters.AddWithValue("@sr_no", sr_no)
            mycommand.Parameters.AddWithValue("@qr_text", qr_text)
            mycommand.Parameters.AddWithValue("@lang", HttpContext.Current.Session("MyLanguage").ToString())

            mycommand.Parameters.Add("@status_code", SqlDbType.Int).Direction = ParameterDirection.Output
            mycommand.Parameters.Add("@status_message", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output

            mycommand.CommandText = "[sec].[saveBallotQR]" ' Initialize CommandText
            mytable = MyInterface.INSERTData(mycommand, "[sec].[saveBallotQR]")

            cls.status_code = CInt(mycommand.Parameters("@status_code").Value)
            cls.status_message = mycommand.Parameters("@status_message").Value
        Catch ex As Exception
            cls.status_code = 500
            cls.status_message = ex.Message
            cls.status_message_local = ex.Message
        End Try
        Return cls
    End Function
    Public Function UpdateBallotBoxStock(series As String, box_size As String, total_qty As Int64, fromSrNo As Int64, toSrNo As Int64, purchase_month As Int32, purchase_year As Int32, from_user_id As String, to_user_id As String, action As Int16) As Generic_Response

        Dim cls = New Generic_Response
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@series", series)
            mycommand.Parameters.AddWithValue("@box_size", box_size)
            mycommand.Parameters.AddWithValue("@total_qty", total_qty)
            mycommand.Parameters.AddWithValue("@fromSrNo", fromSrNo)
            mycommand.Parameters.AddWithValue("@toSrNo", toSrNo)
            mycommand.Parameters.AddWithValue("@purchase_month", purchase_month)
            mycommand.Parameters.AddWithValue("@purchase_year", purchase_year)

            mycommand.Parameters.AddWithValue("@latitude", DBNull.Value)
            mycommand.Parameters.AddWithValue("@longitude", DBNull.Value)
            mycommand.Parameters.AddWithValue("@from_user_id", from_user_id)
            mycommand.Parameters.AddWithValue("@to_user_id", to_user_id)
            mycommand.Parameters.AddWithValue("@state_id", "13")
            mycommand.Parameters.AddWithValue("@qr_text", DBNull.Value)
            mycommand.Parameters.AddWithValue("@lang", HttpContext.Current.Session("MyLanguage").ToString())
            mycommand.Parameters.AddWithValue("@action", action) 'INSERT

            mycommand.Parameters.Add("@status_code", SqlDbType.Int).Direction = ParameterDirection.Output
            mycommand.Parameters.Add("@status_message", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output

            mycommand.CommandText = "[sec].[UpdateBallotBoxStock]" ' Initialize CommandText
            mytable = MyInterface.INSERTData(mycommand, "[sec].[UpdateBallotBoxStock]")

            cls.status_code = CInt(mycommand.Parameters("@status_code").Value)
            cls.status_message = mycommand.Parameters("@status_message").Value
        Catch ex As Exception
            cls.status_code = 500
            cls.status_message = ex.Message
            cls.status_message_local = ex.Message
        End Try
        Return cls
    End Function
    Public Function stationeryMaster_DeActivate(ID As Integer,
                                          de_activation_remakrs As String,
                                          EntryBy As String) As Generic_Response
        Dim cls = New Generic_Response
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@ID", ID)
            mycommand.Parameters.AddWithValue("@de_activation_remakrs", de_activation_remakrs)
            mycommand.Parameters.AddWithValue("@EntryBy", EntryBy)

            mycommand.Parameters.Add("@status_code", SqlDbType.Int).Direction = ParameterDirection.Output
            mycommand.Parameters.Add("@status_message", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
            mycommand.Parameters.Add("@status_message_local", SqlDbType.NVarChar, 200).Direction = ParameterDirection.Output

            mycommand.CommandText = "sec.stationeryMaster_DeActivate" ' Initialize CommandText
            mytable = MyInterface.INSERTData(mycommand, "sec.stationeryMaster_DeActivate")

            cls.status_code = CInt(mycommand.Parameters("@status_code").Value)
            cls.status_message = mycommand.Parameters("@status_message").Value
            cls.status_message_local = mycommand.Parameters("@status_message_local").Value

        Catch ex As Exception
            'Dim exM As String = ex.Message
            ' Handle or log the exception

            cls.status_code = 500
            cls.status_message = ex.Message
            cls.status_message_local = ex.Message
        End Try
        Return cls
    End Function
    Public Function getJsonStringofTable(mytable As DataTable) As String
        Dim json_string As String
        Dim serializer As New JavaScriptSerializer
        Dim packet As New List(Of Dictionary(Of String, Object))
        Dim row As Dictionary(Of String, Object) = Nothing
        Dim count As Integer = 0
        Try
            For Each dr As DataRow In mytable.Rows
                row = New Dictionary(Of String, Object)()
                For Each dc As DataColumn In mytable.Columns
                    row.Add(dc.ColumnName.Trim(), dr(dc))
                Next
                packet.Add(row)
            Next
            json_string = serializer.Serialize(packet)
        Catch ex As Exception
            Dim exM As String = ex.Message
            json_string = ""
        End Try
        Return json_string
    End Function
    Public Function sendOTP(ByVal UserID As String, ByVal ip_Address As String, ByVal Mobile As String, Optional ByVal Email As String = "") As String
        Dim result As String = ""
        Dim resultLocal As String = ""
        Dim objMailSMS As New MailCumSMS
        Try
            Dim randomclass As New Random
            Dim flagSMS As Boolean = False
            Dim flagMail As Boolean = False
            HttpContext.Current.Session("otp" & Mobile) = randomclass.Next(100000, 999999)
            Dim mobtoShow As String = ""
            Dim mailtoShow As String = Email ' email.Replace(email.Substring(2, email.IndexOf("@") - 2), "*")
            Try
                If Email <> "" Then
                    Try ' send mail
                        If Regex.IsMatch(Email.Trim, "^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$") Then
                            'send email 
                            objMailSMS.SendMailToUser(UserID, ip_Address, "0", Email.Trim, "ForgetPassword", HttpContext.Current.Session("otp" & Mobile))
                            flagMail = True
                            Dim len As Int16 = Email.IndexOf("@")
                            mailtoShow = Email.Substring(2, Email.IndexOf("@") - 1)
                            For i As Int16 = 0 To mailtoShow.Length - 1
                                mailtoShow = mailtoShow.Replace(mailtoShow.Chars(i), "*")
                            Next
                            mailtoShow = Email.Substring(0, 2) & mailtoShow & Email.Substring(Email.IndexOf("@") - 1)
                        End If
                    Catch ex As Exception
                        Dim exMsg As String = ex.Message
                    End Try
                End If

                'Parallel.Invoke(Sub()
                If Mobile <> "" Then
                    Dim objMailCumSMS As New MailCumSMS
                    'Dim msg As String = objMailCumSMS.SendSMS_Dit("9418128444", "", "9418128444", HttpContext.Current.Session("otp" & Mobile).ToString)
                    Dim msg As String = objMailCumSMS.SendSMS_Dit(Mobile.ToString, "", Mobile, "ForgetPassword", HttpContext.Current.Session("otp" & Mobile).ToString)

                    '''sendSMS  NIC
                    ''should be used for testing only sendSMS_NIC 
                    ''Dim stas As String = objMailSMS.sendSMS_NIC("9418128444", "OTP to verify ADHAAR Number is " & HttpContext.Current.Session("otp" & Mobile).ToString)
                    'Dim stas As String = objMailSMS.SendSMS(UserID, ip_Address, "0", Mobile, "ForgetPassword", HttpContext.Current.Session("otp" & Mobile).ToString)
                    'If Not String.IsNullOrEmpty(stas) And stas <> "No" Then
                    '    flagSMS = True
                    '    mobtoShow = Mobile.Substring(0, 2) & "******" & Mobile.Substring(Mobile.Length - 2)
                    'End If

                End If
                '                End Sub,
                '            Sub()
                '                If Email <> "" Then
                '                    Try ' send mail
                '                        If Regex.IsMatch(Email.Trim, "^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$") Then
                '                            'send email 
                '                            objMailSMS.SendMailToUser(UserID, ip_Address, "0", Email.Trim, "ForgetPassword", HttpContext.Current.Session("otp" & Mobile))
                '                            flagMail = True
                '                            Dim len As Int16 = Email.IndexOf("@")
                '                            mailtoShow = Email.Substring(2, Email.IndexOf("@") - 1)
                '                            For i As Int16 = 0 To mailtoShow.Length - 1
                '                                mailtoShow = mailtoShow.Replace(mailtoShow.Chars(i), "*")
                '                            Next
                '                            mailtoShow = Email.Substring(0, 2) & mailtoShow & Email.Substring(Email.IndexOf("@") - 1)
                '                        End If
                '                    Catch ex As Exception
                '                        Dim exMsg As String = ex.Message
                '                    End Try
                '                End If
                '            End Sub)

            Catch ex As Exception

            End Try
            If flagSMS = True And flagMail = True Then
                result = "OTP sent to Registered Mobile Number- " & mobtoShow & " and Email ID- " & mailtoShow
                resultLocal = "पंजीकृत मोबाइल नंबर(" & mobtoShow & ") व ईमेल(" & mailtoShow & ") पर ओ टी पी भेज दिया है"
            ElseIf flagMail = True Then
                result = "OTP sent to Registered Email ID- " & mailtoShow
                resultLocal = "पंजीकृत ईमेल(" & mailtoShow & ") पर ओ टी पी भेज दिया है"
            ElseIf flagSMS = True Then
                result = "OTP sent to Registered Mobile Number- " & mobtoShow
                resultLocal = "पंजीकृत मोबाइल नंबर(" & mobtoShow & ") पर ओ टी पी भेज दिया है"
            Else
                result = "Could not sent OTP, Please Try After Some Time"
                resultLocal = "ओ टी पी नहीं जा सका, कृपया बाद में प्रयास करें"
            End If
        Catch ex As Exception
            result = "Please Try After Some Time"
            resultLocal = "कृपया बाद में प्रयास करें"
        End Try
        Return result & "|" & resultLocal
    End Function
    Public Function getQRCODE(ByVal QrText As String) As Byte()
        Dim qrGenerator As New QRCodeGenerator
        Dim qrCodeData As QRCodeData = qrGenerator.CreateQrCode(QrText, QRCodeGenerator.ECCLevel.H)
        Dim qrCode As New QRCode(qrCodeData)
        Dim qrCodeImage As Bitmap = qrCode.GetGraphic(2)
        Dim ms As System.IO.MemoryStream = New MemoryStream()
        qrCodeImage.Save(ms, ImageFormat.Png)
        Dim byteImage As Byte() = ms.ToArray()
        Return byteImage
    End Function

    Public Function GetBlocksToAllocateEVMDataTable(ByRef dis_code As String, ByVal unit_Type As String) As DataTable
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@unitType", unit_Type)
            mycommand.Parameters.AddWithValue("@dis_code", dis_code)
            mytable = MyInterface.SELECTData(mycommand, "getBlocksForEVMAllocation")

        Catch ex As Exception

        End Try
        Return mytable
    End Function
    Public Sub PopulateListForEVMAllocation(ByVal lbox_unmapped_panchayat As ListBox, ByVal block_code As String, ByVal evmUnitype As String, ByVal mapUnmap As String)
        MyInterface = ErmsImplementer
        mycommand.Parameters.Clear()
        mycommand.Parameters.AddWithValue("@blk", block_code)
        mycommand.Parameters.AddWithValue("@unitType", evmUnitype)
        mycommand.Parameters.AddWithValue("@opt", mapUnmap)
        mycommand.Parameters.AddWithValue("@userlevel", HttpContext.Current.Session("UserLevel"))
        'mycommand.Parameters.AddWithValue("@username", HttpContext.Current.Session("UserID"))
        mycommand.Parameters.AddWithValue("@whcode", HttpContext.Current.Session("gblBlockCode"))

        Try
            mytable = MyInterface.SELECTData(mycommand, "GetListForAllocation")
            lbox_unmapped_panchayat.DataSource = mytable

            lbox_unmapped_panchayat.DataTextField = "SrNo"
            lbox_unmapped_panchayat.DataValueField = "Id"
            lbox_unmapped_panchayat.DataBind()
        Catch ex As Exception
        End Try
    End Sub
    'Public Function PopulateStationeryAllocationDataTable(ByVal user_id As String, ByVal itemType As String, ByVal ItemFor As String, Optional ByVal allotedTo As String = "0") As DataTable
    '    Try
    '        MyInterface = ErmsImplementer
    '        mycommand.Parameters.Clear()
    '        mycommand.Parameters.AddWithValue("@itemType", itemType)
    '        mycommand.Parameters.AddWithValue("@user_id", user_id)
    '        mycommand.Parameters.AddWithValue("@itemFor", ItemFor)
    '        mycommand.Parameters.AddWithValue("@allotedTo", allotedTo)
    '        mycommand.Parameters.AddWithValue("@lang", HttpContext.Current.Session("MyLanguage").ToString())
    '        mytable = MyInterface.SELECTData(mycommand, "GetListForStationeryAllocation")

    '    Catch ex As Exception

    '    End Try
    '    Return mytable
    'End Function

    Public Function PopulateStationeryAvailableDataTable(ByVal user_id As String, ByVal itemType As String, ByVal ItemFor As String, ByVal To_User_Code As String, ByVal show_active_only As Boolean) As DataTable
        Try
            ItemFor = IIf(ItemFor = "003", "MC", IIf(ItemFor = "002", "NPLK", "GPYT"))
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@user_id", user_id)
            mycommand.Parameters.AddWithValue("@itemType", itemType)
            mycommand.Parameters.AddWithValue("@itemFor", ItemFor)
            mycommand.Parameters.AddWithValue("@to_user_code", To_User_Code)
            mycommand.Parameters.AddWithValue("@lang", HttpContext.Current.Session("MyLanguage").ToString())
            mycommand.Parameters.Add("@show_active_only", SqlDbType.Bit).Value = show_active_only
            mytable = MyInterface.SELECTData(mycommand, "ShowAvailableStock")
        Catch ex As Exception

        End Try
        Return mytable
    End Function
    Public Sub populateFLCData(ByRef ddl As DropDownList, ByRef Dis As String, ByRef Optional FLC_ID As Int32 = 0)

        MyInterface = ErmsImplementer
        mycommand.Parameters.Clear()

        mycommand.Parameters.AddWithValue("@dis", Dis)
        mycommand.Parameters.AddWithValue("@FLC_Id", FLC_ID)
        mytable = MyInterface.SELECTData(mycommand, "getFLCListForUpdate")
        If mytable.TableName = "Success" Then
            Dim dv As DataView = mytable.DefaultView
            dv.Sort = "FLC_Date"
            ddl.DataSource = dv
            ddl.DataTextField = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "HallNameLocal", "HallName")
            ddl.DataValueField = "FLC_ID"

            ddl.DataBind()
            If FLC_ID = 0 Or ddl.Items.Count > 1 Then
                ddl.Items.Insert(0, New ListItem(Resources.Resource.Sel, Resources.Resource.Sel1))
            End If
            ddl.SelectedIndex = 0
        End If

    End Sub

    Public Function fillMapData(ByRef Optional dis_code As String = "0000", ByRef Optional unitType As String = "CU", ByRef Optional workingStatus As String = "XX", ByRef Optional warehouseCode As String = "0000") As DataTable


        MyInterface = ErmsImplementer
        mycommand.Parameters.Clear()
        mycommand.Parameters.AddWithValue("@workingStatus", workingStatus)
        mycommand.Parameters.AddWithValue("@unitType", unitType)
        mycommand.Parameters.AddWithValue("@dis_code", dis_code)
        mycommand.Parameters.AddWithValue("@wh_code", warehouseCode)
        mytable = MyInterface.SELECTData(mycommand, "getEVMList")
        Return mytable
    End Function


    Public Function fillWarehouseMapData(ByRef warehouseCode As String) As DataTable


        MyInterface = ErmsImplementer
        mycommand.Parameters.Clear()
        mycommand.Parameters.AddWithValue("@lang", HttpContext.Current.Session("MyLanguage").ToString)
        mycommand.Parameters.AddWithValue("@wh_code", warehouseCode)
        mytable = MyInterface.SELECTData(mycommand, "GetWarehouseLatLong")
        Return mytable
    End Function
    Public Function getWareHousesDataTable(ByVal Dis As String) As DataTable

        MyInterface = ErmsImplementer
        mycommand.Parameters.Clear()

        mycommand.Parameters.AddWithValue("@dis_Code", Dis)
        mytable = MyInterface.SELECTData(mycommand, "GetWarehouse")
        Return mytable

    End Function
    Public Function getFLCDataTable(ByVal Dis As String, ByVal Optional FLC_ID As Int32 = 0) As DataTable

        MyInterface = ErmsImplementer
        mycommand.Parameters.Clear()

        mycommand.Parameters.AddWithValue("@dis", Dis)
        mycommand.Parameters.AddWithValue("@FLC_Id", FLC_ID)
        mytable = MyInterface.SELECTData(mycommand, "getFLCListForUpdate")
        Return mytable

    End Function
    Public Function UpdateStationeryStock(ByVal tbl_StationeryMapping As DataTable, ByVal dis_code As String, ByVal itemType As String, ByVal priType As String, ByVal userLevel As String, ByVal from_user_id As String, ByVal to_user_id As String, ByVal is_dispose As Boolean, ByVal is_consumed As Boolean) As DataTable

        MyInterface = ErmsImplementer
        mycommand.Parameters.Clear()
        mycommand.Parameters.AddWithValue("@tbl_StaioneryMapping", tbl_StationeryMapping)
        mycommand.Parameters.AddWithValue("@disCode", dis_code)
        mycommand.Parameters.AddWithValue("@itemType", itemType)
        mycommand.Parameters.AddWithValue("@priType", priType)
        mycommand.Parameters.AddWithValue("@userlevel", userLevel)
        mycommand.Parameters.AddWithValue("@from_user_id", from_user_id)
        mycommand.Parameters.AddWithValue("@to_location_id", to_user_id)
        mycommand.Parameters.AddWithValue("@is_dispose", is_dispose)
        mycommand.Parameters.AddWithValue("@is_consumed", is_consumed)
        mytable = MyInterface.INSERTData(mycommand, "UpdateStationeryStock")
        Return mytable

    End Function
    Public Sub PopulateListForEVMAllocationAtWarehouse(ByVal lbox_unmapped_panchayat As ListBox, ByVal dis_code As String, ByVal wh_code As String, ByVal wh_code_new As String, ByVal evmUnitype As String, ByVal mapUnmap As String)
        MyInterface = ErmsImplementer
        mycommand.Parameters.Clear()
        mycommand.Parameters.AddWithValue("@dis", dis_code)
        mycommand.Parameters.AddWithValue("@unitType", evmUnitype)
        mycommand.Parameters.AddWithValue("@opt", mapUnmap)
        mycommand.Parameters.AddWithValue("@whcode", wh_code)
        mycommand.Parameters.AddWithValue("@whcode_new", wh_code_new)

        Try
            mytable = MyInterface.SELECTData(mycommand, "GetListForAllocationWH")
            lbox_unmapped_panchayat.DataSource = mytable

            lbox_unmapped_panchayat.DataTextField = "SrNo"
            lbox_unmapped_panchayat.DataValueField = "Id"
            lbox_unmapped_panchayat.DataBind()
        Catch ex As Exception
        End Try
    End Sub

End Class
