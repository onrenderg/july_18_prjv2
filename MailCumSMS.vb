Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports System.Security.Policy
Imports Microsoft.VisualBasic

Public Class MailCumSMS
    Public Shared API As String
    ' Dim objActivityLog As New ActivityLog
    Shared senderIDDit As String = ConfigurationManager.AppSettings().Get("senderIDDit").ToString
    Shared senderUserNameDit As String = ConfigurationManager.AppSettings().Get("senderUserNameDit").ToString
    Shared senderUserPasswordDit As String = ConfigurationManager.AppSettings().Get("senderUserPasswordDit").ToString
    Shared secureKeyDit As String = ConfigurationManager.AppSettings().Get("secureKeyDit").ToString
    Shared template_id As String = ConfigurationManager.AppSettings().Get("template_id").ToString

    Public Sub SendMailToUser(ByVal UserID As String, ByVal REMOTE_ADDR As String, ByVal reqID As String, ByVal EmailID As String, ByVal mailPurpose As String, ByVal msgText As String, Optional ByVal reqStatus As String = "")
        Dim msgSentRes As String = "failed"
        'Dim mailfrom As New MailAddress("himbhoomi-hp@nic.in", "State Election Commission, Himachal Pradesh")
        Dim mailfrom As New MailAddress("electoralofficer-sec@hp.gov.in", "State Election Commission, Himachal Pradesh")

        Dim mailto As New MailAddress(EmailID, EmailID)

        Dim mailmsg As String
        mailmsg = "<HTML>"
        mailmsg = mailmsg & "<HEAD>"
        mailmsg = mailmsg & "<TITLE> State Election Commission, HP! </TITLE>"
        mailmsg = mailmsg & "</HEAD>"
        mailmsg = mailmsg & "<BODY>"
        mailmsg = mailmsg & "<TABLE cellpadding=""4"" >"
        mailmsg = mailmsg & "<TR><TD><FONT  color=""black"">"
        mailmsg = mailmsg & "Dear Sir/Madam,<BR><BR><BR>"
        If mailPurpose = "ForgetPassword" Then
            mailmsg = mailmsg & msgText & " is your OTP to reset Data Profiler Application password."
            mailmsg = mailmsg & "</td></tr></table>"
        ElseIf mailPurpose = "ERR" Then
            mailmsg = mailmsg & "           Exception for Request ID, " & reqID & "... <br/><br/>"
            mailmsg = mailmsg & reqStatus.Trim & " <br/><br/>"
            mailmsg = mailmsg & "</td></tr></table>"
        ElseIf mailPurpose = "NomReqSubmit" Then
            'mailmsg = mailmsg & msgText & " <br/><br/>"
            mailmsg = mailmsg & "           नामांकन पत्र की क्रम संख्या<u>" & msgText.Split("|")(0).ToString & "</u><br/> यह नामांकन पत्र मुझे "
            mailmsg = mailmsg & "<u>"& msgText.Split("|")(1).ToString & "</u> अभ्यर्थी/प्रस्थापक द्वारा <u>"& msgText.Split("|")(2).ToString & "</u> स्थान पर"
            mailmsg = mailmsg & "<u>" & msgText.Split("|")(3).ToString & "</u> समय पर <u>" & msgText.Split("|")(4).ToString & "</u> तारीख को परिदŸा किया गया था।<br/><br/>"
            'mailmsg = mailmsg & "Your scrutiny date is " & reqStatus & " <br/><br/>"
            mailmsg = mailmsg & "</td></tr></table>"
        ElseIf mailPurpose = "NomReqAccept" Then
            mailmsg = mailmsg & "           Your Nomination Request with Reference number " + msgText + " has been accepted. <br/><br/>"
            'mailmsg = mailmsg & "Your scrutiny date is " & reqStatus & " <br/><br/>"
            mailmsg = mailmsg & "</td></tr></table>"
        ElseIf mailPurpose = "NomReqRschduled" Then
            mailmsg = mailmsg & "           Your Nomination Request with Reference number " + msgText + " has been rescheduled. <br/><br/>"
            'mailmsg = mailmsg & "Now your scrutiny date is " & reqStatus & " <br/><br/>"
            mailmsg = mailmsg & "</td></tr></table>"
        ElseIf mailPurpose = "NomReqReject" Then
            mailmsg = mailmsg & "           Your Nomination Request with Reference number " + msgText + " has been rejected. <br/><br/>"
            mailmsg = mailmsg & "Remark :  " & reqStatus & " <br/><br/>"
            mailmsg = mailmsg & "</td></tr></table>"

        End If

        mailmsg = mailmsg & "<br/><br/>Please do not reply to this E-mail as it is a system generated E-mail."
        mailmsg = mailmsg & "</FONT></TD></TR></TABLE><BR><BR>"
        mailmsg = mailmsg & "</BODY>"
        mailmsg = mailmsg & "</HTML>"

        If mailPurpose.ToLower() = "test" Then
            mailmsg = "Test Mail"
        End If

        Dim Email As New System.Net.Mail.MailMessage(mailfrom, mailto)
        With Email
            .Subject = "State Election Commission, HP"
            .IsBodyHtml = True
            .Priority = System.Net.Mail.MailPriority.Normal
            .DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
            .Body = mailmsg
        End With
        ' 'will be used for sending attachments
        '    If File.Exists(fname) Then
        '        Dim attachment As System.Net.Mail.Attachment
        '        attachment = New System.Net.Mail.Attachment(fname)
        '        Email.Attachments.Add(attachment)
        '    Else
        '        updateSMSEmailStatus(reqID, EmailID, "mail", 1, "File Not Found. FileName: " & fname)
        '    End If

        ''for authenticated mails ---START
        ''    Dim basicAuthenticationInfo As New System.Net.NetworkCredential("dg-prison-hp@nic.in", "hap$PY91")
        ''---END
        Dim mailClient As New System.Net.Mail.SmtpClient()
        Try
            mailClient.Host = "relay.nic.in"
            mailClient.Port = 25
            ' ''for authenticated mails ---START
            ''mailClient.UseDefaultCredentials = False
            ''mailClient.Credentials = basicAuthenticationInfo
            ''mailClient.EnableSsl = False
            ' ''for authenticated mails ---END

            ''for non- authenticated mails ---START
            mailClient.UseDefaultCredentials = True
            ''---END
            'uncomment below two lines to send mail
            mailClient.Send(Email)
            msgSentRes = "success"
        Catch ex As Exception
            updateSMSEmailStatus(UserID, EmailID, "", msgText, msgSentRes, ex.Message)
            ' objActivityLog.insertErrorLog(UserID, REMOTE_ADDR, "MailCumSMS", "SendMailToUser", ex.Message)
            Throw
            ' Throw New Exception(ex.Message.ToString)
        End Try
    End Sub
    'Public Sub SendPDFsToUser(ByVal reqID As String, ByVal reqStatus As String, ByVal EmailID As String, ByVal mailPurpose As String, ByVal PDFs() As String)

    '    Dim mailfrom As New MailAddress("himbhoomi-hp@nic.in", "Himbhoomi Team")

    '    Dim mailto As New MailAddress(EmailID, EmailID)

    '    Dim mailmsg As String
    '    mailmsg = "<HTML>"
    '    mailmsg = mailmsg & "<HEAD>"
    '    mailmsg = mailmsg & "<TITLE> Revenue Department, HP! </TITLE>"
    '    mailmsg = mailmsg & "</HEAD>"
    '    mailmsg = mailmsg & "<BODY>"
    '    mailmsg = mailmsg & "<TABLE cellpadding=""4"" >"
    '    mailmsg = mailmsg & "<TR><TD><FONT  color=""black"">"
    '    mailmsg = mailmsg & "Dear Sir/Madam,<BR><BR><BR>"
    '    If mailPurpose = "OWNER_LR_ADHAR" Then
    '        mailmsg = mailmsg & "           Your Aadhaar number " & reqID & " has been linked with your land records in " & reqStatus & ". RoR copies are attached herewith for your information. For further details, yo may visit http://himachal.nic.in/revenue. <br/><br/>"
    '        mailmsg = mailmsg & "           Thank You.<br/><br/><br/><br/><br/>"
    '    ElseIf mailPurpose = "ERR" Then
    '        mailmsg = mailmsg & "           Exception for Request ID, " & reqID & "... <br/><br/>"
    '        mailmsg = mailmsg & reqStatus.Trim & " <br/><br/>"
    '        mailmsg = mailmsg & "</td></tr></table>"
    '    End If
    '    mailmsg = mailmsg & "<br/><br/>Please do not reply to this E-mail as it is a system generated E-mail."
    '    mailmsg = mailmsg & "</FONT></TD></TR></TABLE><BR><BR>"
    '    mailmsg = mailmsg & "</BODY>"
    '    mailmsg = mailmsg & "</HTML>"

    '    Dim Email As New System.Net.Mail.MailMessage(mailfrom, mailto)
    '    With Email
    '        .Subject = "Revenue Department, HP"
    '        .IsBodyHtml = True
    '        .Priority = System.Net.Mail.MailPriority.Normal
    '        .DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
    '        .Body = mailmsg
    '    End With
    '    For i As Integer = 0 To PDFs.Length - 1
    '        ' 'can be used for sending attachments
    '        If File.Exists(PDFs(i)) Then
    '            Dim attachment As System.Net.Mail.Attachment
    '            attachment = New System.Net.Mail.Attachment(PDFs(i))
    '            Email.Attachments.Add(attachment)
    '        Else
    '            updateSMSEmailStatus(reqID, EmailID, "mail", 1, "File Not Found. FileName: " & PDFs(i))
    '        End If
    '    Next


    '    ''for authenticated mails ---START
    '    ''    Dim basicAuthenticationInfo As New System.Net.NetworkCredential("dg-prison-hp@nic.in", "hap$PY91")
    '    ''---END
    '    Dim mailClient As New System.Net.Mail.SmtpClient()
    '    Try
    '        mailClient.Host = "relay.nic.in"
    '        mailClient.Port = 25
    '        ' ''for authenticated mails ---START
    '        ''mailClient.UseDefaultCredentials = False
    '        ''mailClient.Credentials = basicAuthenticationInfo
    '        ''mailClient.EnableSsl = False
    '        ' ''for authenticated mails ---END

    '        ''for non- authenticated mails ---START
    '        mailClient.UseDefaultCredentials = True
    '        ''---END
    '        'uncomment below two lines to send mail
    '        mailClient.Send(Email)

    '    Catch ex As Exception
    '        ' objActivityLog.insertErrorLog(UserID, REMOTE_ADDR, "MailCumSMS", "SendMailToUser", ex.Message)
    '        ' Throw New Exception(ex.Message.ToString)
    '    End Try
    'End Sub

    Public Function SendSMS(ByVal UserID As String, ByVal REMOTE_ADDR As String, ByVal reqID As String, ByVal MobileNo As String, ByVal msgPurpose As String, ByVal msgText As String) As String
        '' SMS service stopped (paid service)
        ''Return
        ' Send SMS START
        Dim senderUserName, senderUserPassword, senderID As String
        senderUserName = ConfigurationManager.AppSettings().Get("senderUserName").ToString
        senderUserPassword = ConfigurationManager.AppSettings().Get("senderUserPassword").ToString
        senderID = ConfigurationManager.AppSettings().Get("senderID").ToString
        Dim msgSentRes As String = ""
        Dim MySMS As String = ""
        Dim msgSent As String = "No"
        If Trim(MobileNo) <> "" Then
            Try
                If msgPurpose = "ForgetPassword" Then
                    MySMS = msgText & " is your OTP to reset Data Profiler Application password."
                ElseIf msgPurpose = "NomReqSubmit" Then
                    'MySMS = msgText
                    MySMS = "Your Nomination request for post of " & reqID & " has been received. State Election Commission, Himachal Pradesh"
                ElseIf msgPurpose = "MobileAppOTP" Then
                    MySMS = "Namaskar! " & msgText & " is your OTP for Voting day Monitoring System (VDMS). State Election Commission, Himachal Pradesh"

                End If

                'comment below line to send SMS
                'Dim msgSent As String = "SMS Stopped At Code level"

                '''code to use NIC gateway for sendind SMS[Paid service]
                '' sendBulkSMS_HTTPS("allapp.auth", "E63*$yWr", "NICSMS", Trim(MobileNo), MySMS) '--- send sms over https
                'msgSentRes = sendBulkSMS_HTTPS(senderUserName, senderUserPassword, senderID, Trim(MobileNo), MySMS) '--- send sms over https

                '' code to send SMS through DIT SMS Gateway
                Dim objDitSMS As New SMSHttpPostClient
                Dim msgStatus As String = objDitSMS.sendOTPMSG(senderUserNameDit, senderUserPasswordDit, senderIDDit, Trim(MobileNo), MySMS, secureKeyDit, template_id)
                msgSentRes = msgStatus
                If msgSent.ToLower = "ok" Then
                Else
                    msgSent = "No"
                End If
            Catch ex As Exception
                msgSent = "No"
                ' objActivityLog.insertErrorLog(UserID, REMOTE_ADDR, "MailCumSMS", "SendSMS-" & Trim(MobileNo) & ", " & MySMS, ex.Message)
            Finally
                updateSMSEmailStatus(UserID, "", Trim(MobileNo), MySMS, msgSentRes, msgPurpose)
            End Try
        End If
        ' Send SMS END
        Return msgSent
    End Function

    Private Function encryptQueryString(ByVal strQueryString As String) As String
        Dim encoding As New System.Text.ASCIIEncoding()

        Dim barr As Byte()

        If strQueryString Is Nothing Then
            barr = encoding.GetBytes("")
        Else
            barr = encoding.GetBytes(strQueryString)
        End If

        Dim str64 As String = Convert.ToBase64String(barr, 0, barr.Length, Base64FormattingOptions.None)
        Return str64

    End Function
    Private Function decryptQueryString(ByVal strQueryString As String) As String


        Dim barr As Byte()

        If strQueryString Is Nothing Then
            barr = Convert.FromBase64String("")
        Else
            barr = Convert.FromBase64String(strQueryString)
        End If
        Dim encoding As New System.Text.ASCIIEncoding()
        Dim strdec As String = encoding.GetString(barr)

        Return (strdec)

    End Function
    Public Function SendSMS_Dit(ByVal UserID As String, ByVal REMOTE_ADDR As String, ByVal MobileNo As String, ByVal msgPurpose As String, ByVal msgText As String) As String
        '' SMS service stopped (paid service)
        ''Return
        ' Send SMS START

        Dim msgSentRes As String = ""
        Dim MySMS As String = ""
        Dim msgSent As String = "No"
        If Trim(MobileNo) <> "" Then
            Try
                If msgPurpose = "ForgetPassword" Then
                    MySMS = "Namaskar! " & msgText & " is your OTP to reset password of Data Profiler Management Information System, HP."
                ElseIf msgPurpose = "MobileAppOTP" Then
                    MySMS = "Namaskar! " & msgText & " is your OTP for Voting Day Monitoring System (VDMS). State Election Commission, Himachal Pradesh."
                End If
                ' objActivityLog.insertErrorLog(UserID, REMOTE_ADDR, "MailCumSMS", "SendSMS_Dit-" & Trim(MobileNo) & ", " & MySMS, "Going to send SMS through DIT SMS Gateway")
                '' code to send SMS through DIT SMS Gateway
                Dim objDitSMS As New SMSHttpPostClient
                Dim msgStatus As String = objDitSMS.sendOTPMSG(senderUserNameDit, senderUserPasswordDit, senderIDDit, Trim(MobileNo), MySMS, secureKeyDit, template_id)
                ' objActivityLog.insertErrorLog(UserID, REMOTE_ADDR, "MailCumSMS", "SendSMS_Dit-" & Trim(MobileNo) & ", " & MySMS, "Back from DIT SMS Gateway: " & msgStatus)

                msgSentRes = msgStatus
                If msgSentRes.ToLower.Substring(0, 3) = "402" Then
                    msgSent = "Yes"
                Else
                    msgSent = "No"
                End If
            Catch ex As Exception
                msgSent = "No"
                '  objActivityLog.insertErrorLog(UserID, REMOTE_ADDR, "MailCumSMS", "SendSMS_Dit-" & Trim(MobileNo) & ", " & MySMS, ex.Message)
            Finally
                updateSMSEmailStatus(UserID, "", Trim(MobileNo), MySMS, msgSentRes, msgPurpose)
            End Try
        End If
        ' Send SMS END
        Return msgSent
    End Function

    Public Function IsUnicode(ByVal input As String) As Boolean
        Dim asciiBytesCount As Integer = Encoding.ASCII.GetByteCount(input)
        Dim unicodBytesCount As Integer = Encoding.UTF8.GetByteCount(input)
        Return (asciiBytesCount <> unicodBytesCount)
    End Function

    Public Shared Function ConvertStringToHex(ByVal input As String, ByVal encoding As Encoding) As String
        Dim stringBytes = encoding.GetBytes(input)
        Dim sbBytes = New StringBuilder((stringBytes.Length * 2))
        For Each b As Byte In stringBytes
            sbBytes.AppendFormat("{0:X2}", b)
        Next
        Return sbBytes.ToString
    End Function

    Public Shared Function RemoteCertificateValidate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As Net.Security.SslPolicyErrors) As Boolean
        If ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateChainErrors) = SslPolicyErrors.RemoteCertificateChainErrors) Then
            Return False
        ElseIf ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateNameMismatch) = SslPolicyErrors.RemoteCertificateNameMismatch) Then
            Dim z As Zone
            z = Zone.CreateFromUrl(CType(sender, HttpWebRequest).RequestUri.ToString())
            If (z.SecurityZone = System.Security.SecurityZone.Intranet Or z.SecurityZone = System.Security.SecurityZone.MyComputer) Then
                Return True
            End If
            Return False
        Else
            Return True
        End If
    End Function
    Public Shared Sub Check_SSL_Certificate()
        ServicePointManager.ServerCertificateValidationCallback = New System.Net.Security.RemoteCertificateValidationCallback(AddressOf RemoteCertificateValidate)
    End Sub
    Public Function CountCharacter(ByVal value As String, ByVal ch As Char) As Integer
        Dim cnt As Integer = 0
        For Each c As Char In value
            If c = ch Then cnt += 1
        Next
        Return cnt
    End Function
    Private Sub updateSMSEmailStatus(ByVal username As String, ByVal email As String, ByVal mobile As String,
                                      ByVal msgText As String, ByVal msgStatus As String, ByVal msgMode As String)
        Try
            Dim mycommand As New SqlCommand
            Dim MyInterface As ErmsInterface.hbInterface
            Dim Implementer As New ErmsBLL
            Dim mytable As New Data.DataTable

            MyInterface = Implementer
            mycommand.Parameters.Clear()
            ' @userName,@mobile,@email,@msgText,@msgStatus,@msgMode
            mycommand.Parameters.AddWithValue("@userName", username)
            mycommand.Parameters.AddWithValue("@email", email)
            mycommand.Parameters.AddWithValue("@mobile", mobile)
            mycommand.Parameters.AddWithValue("@msgText", msgText)
            mycommand.Parameters.AddWithValue("@msgStatus", msgStatus)
            mycommand.Parameters.AddWithValue("@msgMode", msgMode)

            mytable = MyInterface.UPDATEData(mycommand, "updateSMSEmailStatus")
        Catch ex As Exception

        End Try
    End Sub

    Public Function sendBulkSMS_DIT(ByVal username As String, ByVal password As String, ByVal senderid As String, ByVal mobileNos As String, ByVal message As String) As String

        Dim dataStream As Stream

        Dim request As HttpWebRequest = DirectCast(WebRequest.Create("http://msdgweb.mgov.gov.in/esms/sendsmsrequest"), HttpWebRequest)
        request.ProtocolVersion = HttpVersion.Version10
        ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(Function() True)
        DirectCast(request, HttpWebRequest).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)"
        request.Method = "POST"
        Try
            Dim smsservicetype As [String] = "bulkmsg"
            ' for bulk msg     
            'Dim query As [String] = "uname=" + HttpUtility.UrlEncode(username) + "&pass=" + HttpUtility.UrlEncode(password) + "&send=" + HttpUtility.UrlEncode(senderid) + "&dest=" + HttpUtility.UrlEncode(mobileNos) + "&msg=" + HttpUtility.UrlEncode(message)
            'Dim query As [String] = "username=" + HttpUtility.UrlEncode(username) + "&pin=" + HttpUtility.UrlEncode(password) + "&message=" + HttpUtility.UrlEncode(message) + "&mnumber=" + HttpUtility.UrlEncode(mobileNos) + "&signature=" + HttpUtility.UrlEncode(senderid)
            Dim query As [String] = "username=hpgovt-SEC&password=SEC@234&smsservicetype=" + smsservicetype + "&content=" + message + "&bulkmobno=" + mobileNos + "&senderid=hpgovt"

            'Dim query As [String] = "uname=" + HttpUtility.UrlEncode(username) + "&pass=" + HttpUtility.UrlEncode(password) + "&send=" + HttpUtility.UrlEncode(smsservicetype) + "&content=" + HttpUtility.UrlEncode(message) + "&dest=" + HttpUtility.UrlEncode(mobileNos) + "&send=" + HttpUtility.UrlEncode(senderid)
            Dim byteArray As Byte() = Encoding.ASCII.GetBytes(query)
            request.ContentType = "application/x-www-form-urlencoded"
            request.ContentLength = byteArray.Length
            dataStream = request.GetRequestStream()
            dataStream.Write(byteArray, 0, byteArray.Length)
            dataStream.Close()
            Dim response As WebResponse = request.GetResponse()
            Dim Status As [String] = DirectCast(response, HttpWebResponse).StatusDescription
            dataStream = response.GetResponseStream()
            Dim reader As New StreamReader(dataStream)
            Dim responseFromServer As String = reader.ReadToEnd()
            reader.Close()
            dataStream.Close()
            'Response.Write("Your request received. Will be processed soon");
            Return Status.ToString()
        Catch ex As Exception
            'Response.Write(ex.StackTrace.ToString());
            Return ex.StackTrace.ToString()
        End Try
    End Function


End Class
