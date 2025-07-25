Imports System.IO
Imports System.Security.Cryptography
'Imports MessagingToolkit.QRCode.Codec
Imports System.Web.Script.Serialization
Imports System.Drawing
Imports System.Drawing.Imaging
Imports QRCoder

Public Class commonFunctions_Vand
    Dim mytable As New DataTable
    Dim mycommand As New SqlClient.SqlCommand
    Dim MyInterface As ErmsInterface.hbInterface
    Dim ErmsImplementer As New ErmsBLL
    Dim myLang As String = "hi-IN"
    Const keyValue As String = "Sec&NicHP@22"
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
            Do While yrVal >= 1954 ' 1994 is the year when SEC HP was established and in 1995 EVM's were procured for first time
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
                ddlBlk.DataSource = mytable
                ddlBlk.DataTextField = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "block_name_local", "block_name")
                '"block_name_local"
                ddlBlk.DataValueField = "Block_code"
                ddlBlk.DataBind()
                If mytable.Rows.Count > 1 Then
                    If OneMany <> "One" And (HttpContext.Current.Session("userlevel") Is Nothing Or HttpContext.Current.Session("userlevel") = "000" Or HttpContext.Current.Session("userlevel") = "001") Then
                        If priType = "001" Then
                            ddlBlk.Items.Insert(0, New ListItem(IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "सभी पंचायत समिति", "All Panchayat Samiti"), "00"))
                        ElseIf priType = "002" Then
                            ddlBlk.Items.Insert(0, New ListItem(IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "सभी शहरी निकाय", "All ULBs"), "00"))
                        Else
                            ddlBlk.Items.Insert(0, New ListItem(IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "सभी नगर निगम", "All MCs"), "00"))

                        End If
                    Else
                        If priType = "001" Then
                            ddlBlk.Items.Insert(0, New ListItem(IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "पंचायत समिति चुनें", "Choose Panchayat Samiti"), "00"))
                        ElseIf priType = "002" Then
                            ddlBlk.Items.Insert(0, New ListItem(IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "शहरी निकाय चुनें", "Choose ULB"), "00"))
                        Else
                            ddlBlk.Items.Insert(0, New ListItem(IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "नगर निगम चुनें", "Choose MC"), "00"))

                        End If
                    End If
                End If
                ddlBlk.SelectedIndex = -1
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

    Public Sub PopulateUnitTypes(ByVal rbtn As RadioButtonList)
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
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
    Public Sub PopulateUnitTypes(ByVal ddl As DropDownList)
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
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
    Public Sub PopulateWareHouses(ByVal ddl As DropDownList, ByVal Dis As String, Optional OneMany As String = "One")
        Try
            ddl.Items.Clear()
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()

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

    Public Function getBallotBoxListSummary(ByRef Optional dis_code As String = "0000", ByRef Optional workingStatus As String = "XX") As DataTable
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@workingStatus", workingStatus)
            mycommand.Parameters.AddWithValue("@dis_code", dis_code)
            mytable = MyInterface.SELECTData(mycommand, "getBallotBoxListSummary")

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

    Public Sub generateQRCode(ByRef UnitSrNo As String, ByRef QR_Text As String, ByRef path As String)

        'Dim name As String = UnitSrNo
        'Dim type As String = ".png"
        'path = path + (name + type)
        '' need to encode for making QR code Secure
        'QR_Text = EncryptAES(QR_Text, keyValue)

        'If File.Exists(path) Then
        '    File.Delete(path)
        'End If
        'Try

        '    Dim qrCode As New QRCodeEncoder
        '    qrCode.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE
        '    qrCode.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L
        '    Dim imgQR As System.Drawing.Image
        '    imgQR = qrCode.Encode(QR_Text, System.Text.Encoding.UTF8)
        '    imgQR.Save(path, Drawing.Imaging.ImageFormat.Png)

        'Catch ex As Exception
        '    Dim exM As String = ex.Message
        'End Try
    End Sub
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
                rbtn.SelectedIndex = 0
            End If

        Catch ex As Exception
            Dim exM As String = ex.Message
        End Try
    End Sub
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
        Dim qrCodeImage As Bitmap = qrCode.GetGraphic(20)
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
    Public Sub PopulateListForEVMAllocation(ByVal pri_type As String, ByVal lbox_unmapped_panchayat As ListBox, ByVal block_code As String, ByVal evmUnitype As String, ByVal mapUnmap As String)
        MyInterface = ErmsImplementer
        mycommand.Parameters.Clear()
        mycommand.Parameters.AddWithValue("@pri_type", pri_type)
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
    Public Function PopulateStationeryAllocationDataTable(ByVal block_code As String, ByVal itemType As String, ByVal ItemFor As String) As DataTable
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@itemType", itemType)
            mycommand.Parameters.AddWithValue("@blk", block_code)
            mycommand.Parameters.AddWithValue("@itemFor", ItemFor)
            mycommand.Parameters.AddWithValue("@userlevel", HttpContext.Current.Session("UserLevel"))
            mycommand.Parameters.AddWithValue("@lang", HttpContext.Current.Session("MyLanguage").ToString())
            mytable = MyInterface.SELECTData(mycommand, "GetListForStationeryAllocation")

        Catch ex As Exception

        End Try
        Return mytable
    End Function

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
    Public Sub PopulateEVMSummary(ByVal grdEVMS As GridView, ByVal conCode As String)
        Try
            MyInterface = ErmsImplementer
            mycommand.Parameters.Clear()
            mycommand.Parameters.AddWithValue("@code", conCode)
            mytable = MyInterface.SELECTData(mycommand, "getPanchayatWardsForPollDay")
            If mytable.TableName = "Success" Then
                Dim dv As DataView = mytable.DefaultView
                dv.Sort = IIf(HttpContext.Current.Session("MyLanguage").ToString() = "hi-IN", "WardNoNameLocal", "WardNoName")
                grdEVMS.DataSource = dv
                grdEVMS.DataBind()
                grdEVMS.SelectedIndex = 0

            End If
        Catch ex As Exception
        End Try
    End Sub
End Class
