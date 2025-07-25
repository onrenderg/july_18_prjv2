Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Net
Imports System.Web
Imports System.IO
Imports System.Security.Cryptography
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security


Public Class SMSHttpPostClient
        Public Function sendSingleSMS(ByVal username As String, ByVal password As String, ByVal senderid As String, ByVal mobileNo As String, ByVal message As String, ByVal secureKey As String) As String

            Dim dataStream As Stream

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls

            Dim request As HttpWebRequest = CType(WebRequest.Create("https://msdgweb.mgov.gov.in/esms/sendsmsrequest"), HttpWebRequest)

            request.ProtocolVersion = HttpVersion.Version10

            request.KeepAlive = False

            request.ServicePoint.ConnectionLimit = 1

            DirectCast(request, HttpWebRequest).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)"

            request.Method = "POST"

            ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(Function() True)

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 Or System.Security.Authentication.SslProtocols.Tls Or System.Security.Authentication.SslProtocols.Default Or System.Security.Authentication.SslProtocols.Tls11 Or System.Security.Authentication.SslProtocols.Tls12

            Dim encryptedPassword As String = encryptedPasswod(password)

            Dim NewsecureKey As String = hashGenerator(username.Trim(), senderid.Trim(), message.Trim(), secureKey.Trim())

            Dim smsservicetype As String = "singlemsg"

            Dim query As String = "username=" & HttpUtility.UrlEncode(username.Trim()) & "&password=" + HttpUtility.UrlEncode(encryptedPassword) & "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) & "&content=" + HttpUtility.UrlEncode(message.Trim()) & "&mobileno=" + HttpUtility.UrlEncode(mobileNo) & "&senderid=" + HttpUtility.UrlEncode(senderid.Trim()) & "&key=" + HttpUtility.UrlEncode(NewsecureKey.Trim())

            Dim byteArray As Byte() = Encoding.ASCII.GetBytes(query)

            request.ContentType = "application/x-www-form-urlencoded"

            request.ContentLength = byteArray.Length

            dataStream = request.GetRequestStream()

            dataStream.Write(byteArray, 0, byteArray.Length)

            dataStream.Close()

            Dim response As WebResponse = request.GetResponse()

            Dim Status As String = (CType(response, HttpWebResponse)).StatusDescription

            dataStream = response.GetResponseStream()

            Dim reader As StreamReader = New StreamReader(dataStream)

            Dim responseFromServer As String = reader.ReadToEnd()

            reader.Close()

            dataStream.Close()

            response.Close()

            Return responseFromServer

        End Function

        Public Function sendBulkSMS(ByVal username As String, ByVal password As String, ByVal senderid As String, ByVal mobileNos As String, ByVal message As String, ByVal secureKey As String) As String

            Dim dataStream As Stream

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls

            Dim request As HttpWebRequest = CType(WebRequest.Create("https://msdgweb.mgov.gov.in/esms/sendsmsrequest"), HttpWebRequest)

            request.ProtocolVersion = HttpVersion.Version10

            request.KeepAlive = False

            request.ServicePoint.ConnectionLimit = 1

            DirectCast(request, HttpWebRequest).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)"

            request.Method = "POST"

            ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(Function() True)

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 Or System.Security.Authentication.SslProtocols.Tls Or System.Security.Authentication.SslProtocols.Default Or System.Security.Authentication.SslProtocols.Tls11 Or System.Security.Authentication.SslProtocols.Tls12

            Dim encryptedPassword As String = encryptedPasswod(password)

            Dim NewsecureKey As String = hashGenerator(username.Trim(), senderid.Trim(), message.Trim(), secureKey.Trim())

            Console.Write(NewsecureKey)

            Console.Write(encryptedPassword)

            Dim smsservicetype As String = "bulkmsg"

            Dim query As String = "username=" & HttpUtility.UrlEncode(username.Trim()) & "&password=" + HttpUtility.UrlEncode(encryptedPassword) & "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) & "&content=" + HttpUtility.UrlEncode(message.Trim()) & "&bulkmobno=" + HttpUtility.UrlEncode(mobileNos) & "&senderid=" + HttpUtility.UrlEncode(senderid.Trim()) & "&key=" + HttpUtility.UrlEncode(NewsecureKey.Trim())

            Console.Write(query)

            Dim byteArray As Byte() = Encoding.ASCII.GetBytes(query)

            request.ContentType = "application/x-www-form-urlencoded"

            request.ContentLength = byteArray.Length

            dataStream = request.GetRequestStream()

            dataStream.Write(byteArray, 0, byteArray.Length)

            dataStream.Close()

            Dim response As WebResponse = request.GetResponse()

            Dim Status As String = (CType(response, HttpWebResponse)).StatusDescription

            dataStream = response.GetResponseStream()

            Dim reader As StreamReader = New StreamReader(dataStream)

            Dim responseFromServer As String = reader.ReadToEnd()

            reader.Close()

            dataStream.Close()

            response.Close()

            Return responseFromServer

        End Function

        Public Function sendUnicodeSMS(ByVal username As String, ByVal password As String, ByVal senderid As String, ByVal mobileNos As String, ByVal Unicodemessage As String, ByVal secureKey As String) As String

            Dim dataStream As Stream

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls

            Dim request As HttpWebRequest = CType(WebRequest.Create("https://msdgweb.mgov.gov.in/esms/sendsmsrequest"), HttpWebRequest)

            request.ProtocolVersion = HttpVersion.Version10

            request.KeepAlive = False

            request.ServicePoint.ConnectionLimit = 1

            DirectCast(request, HttpWebRequest).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)"

            request.Method = "POST"

            ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(Function() True)

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 Or System.Security.Authentication.SslProtocols.Tls Or System.Security.Authentication.SslProtocols.Default Or System.Security.Authentication.SslProtocols.Tls11 Or System.Security.Authentication.SslProtocols.Tls12

            Dim U_Convertedmessage As String = ""

            For Each c As Char In Unicodemessage

                Dim j As Integer = CInt(c.ToString)

                Dim sss As String = "&#" & j & ";"

                U_Convertedmessage = U_Convertedmessage + sss

            Next

            Dim encryptedPassword As String = encryptedPasswod(password)

            Dim NewsecureKey As String = hashGenerator(username.Trim(), senderid.Trim(), U_Convertedmessage.Trim(), secureKey.Trim())

            Dim smsservicetype As String = "unicodemsg"

            Dim query As String = "username=" & HttpUtility.UrlEncode(username.Trim()) & "&password=" + HttpUtility.UrlEncode(encryptedPassword) & "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) & "&content=" + HttpUtility.UrlEncode(U_Convertedmessage.Trim()) & "&bulkmobno=" + HttpUtility.UrlEncode(mobileNos) & "&senderid=" + HttpUtility.UrlEncode(senderid.Trim()) & "&key=" + HttpUtility.UrlEncode(NewsecureKey.Trim())

            Dim byteArray As Byte() = Encoding.ASCII.GetBytes(query)

            request.ContentType = "application/x-www-form-urlencoded"

            request.ContentLength = byteArray.Length

            dataStream = request.GetRequestStream()

            dataStream.Write(byteArray, 0, byteArray.Length)

            dataStream.Close()

            Dim response As WebResponse = request.GetResponse()

            Dim Status As String = (CType(response, HttpWebResponse)).StatusDescription

            dataStream = response.GetResponseStream()

            Dim reader As StreamReader = New StreamReader(dataStream)

            Dim responseFromServer As String = reader.ReadToEnd()

            reader.Close()

            dataStream.Close()

            response.Close()

            Return responseFromServer

        End Function

    Public Function sendOTPMSG(ByVal username As String, ByVal password As String, ByVal senderid As String, ByVal mobileNo As String, ByVal message As String, ByVal secureKey As String, ByVal templateid As String) As String

        Dim dataStream As Stream

        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls

        Dim request As HttpWebRequest = CType(WebRequest.Create("https://msdgweb.mgov.gov.in/esms/sendsmsrequestDLT"), HttpWebRequest)

        request.ProtocolVersion = HttpVersion.Version10

        request.KeepAlive = False

        request.ServicePoint.ConnectionLimit = 1

        DirectCast(request, HttpWebRequest).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)"

        request.Method = "POST"

        ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(Function() True)

        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 Or System.Security.Authentication.SslProtocols.Tls Or System.Security.Authentication.SslProtocols.Default Or System.Security.Authentication.SslProtocols.Tls11 Or System.Security.Authentication.SslProtocols.Tls12

        Dim encryptedPassword As String = encryptedPasswod(password)

        Dim key As String = hashGenerator(username.Trim(), senderid.Trim(), message.Trim(), secureKey.Trim())

        Dim smsservicetype As String = "otpmsg"

        Dim query As String = "username=" & HttpUtility.UrlEncode(username.Trim()) &
            "&password=" + HttpUtility.UrlEncode(encryptedPassword) &
            "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) &
            "&content=" + HttpUtility.UrlEncode(message.Trim()) &
            "&mobileno=" + HttpUtility.UrlEncode(mobileNo) &
            "&senderid=" + HttpUtility.UrlEncode(senderid.Trim()) &
            "&key=" + HttpUtility.UrlEncode(key.Trim()) &
            "&templateid=" + HttpUtility.UrlEncode(templateid.Trim())


        Dim byteArray As Byte() = Encoding.ASCII.GetBytes(query)

        request.ContentType = "application/x-www-form-urlencoded"

        request.ContentLength = byteArray.Length

        dataStream = request.GetRequestStream()

        dataStream.Write(byteArray, 0, byteArray.Length)

        dataStream.Close()

        Dim response As WebResponse = request.GetResponse()

        Dim Status As String = (CType(response, HttpWebResponse)).StatusDescription

        dataStream = response.GetResponseStream()

        Dim reader As StreamReader = New StreamReader(dataStream)

        Dim responseFromServer As String = reader.ReadToEnd()

        reader.Close()

        dataStream.Close()

        response.Close()

        Return responseFromServer

    End Function

    Public Function sendUnicodeOTPSMS(ByVal username As String, ByVal password As String, ByVal senderid As String, ByVal mobileNos As String, ByVal UnicodeOTPmsg As String, ByVal secureKey As String) As String

            Dim dataStream As Stream

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls

            Dim request As HttpWebRequest = CType(WebRequest.Create("https://msdgweb.mgov.gov.in/esms/sendsmsrequest"), HttpWebRequest)

            request.ProtocolVersion = HttpVersion.Version10

            request.KeepAlive = False

            request.ServicePoint.ConnectionLimit = 1

            DirectCast(request, HttpWebRequest).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)"

            request.Method = "POST"

            ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(Function() True)

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 Or System.Security.Authentication.SslProtocols.Tls Or System.Security.Authentication.SslProtocols.Default Or System.Security.Authentication.SslProtocols.Tls11 Or System.Security.Authentication.SslProtocols.Tls12

            Dim U_Convertedmessage As String = ""

            For Each c As Char In UnicodeOTPmsg

                Dim j As Integer = CInt(c.ToString)

                Dim sss As String = "&#" & j & ";"

                U_Convertedmessage = U_Convertedmessage + sss

            Next

            Dim encryptedPassword As String = encryptedPasswod(password)

            Dim NewsecureKey As String = hashGenerator(username.Trim(), senderid.Trim(), U_Convertedmessage.Trim(), secureKey.Trim())

            Dim smsservicetype As String = "unicodeotpmsg"

            Dim query As String = "username=" & HttpUtility.UrlEncode(username.Trim()) & "&password=" + HttpUtility.UrlEncode(encryptedPassword) & "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) & "&content=" + HttpUtility.UrlEncode(U_Convertedmessage.Trim()) & "&bulkmobno=" + HttpUtility.UrlEncode(mobileNos) & "&senderid=" + HttpUtility.UrlEncode(senderid.Trim()) & "&key=" + HttpUtility.UrlEncode(NewsecureKey.Trim())

            Dim byteArray As Byte() = Encoding.ASCII.GetBytes(query)

            request.ContentType = "application/x-www-form-urlencoded"

            request.ContentLength = byteArray.Length

            dataStream = request.GetRequestStream()

            dataStream.Write(byteArray, 0, byteArray.Length)

            dataStream.Close()

            Dim response As WebResponse = request.GetResponse()

            Dim Status As String = (CType(response, HttpWebResponse)).StatusDescription

            dataStream = response.GetResponseStream()

            Dim reader As StreamReader = New StreamReader(dataStream)

            Dim responseFromServer As String = reader.ReadToEnd()

            reader.Close()

            dataStream.Close()

            response.Close()

            Return responseFromServer

        End Function

        Protected Function encryptedPasswod(ByVal password As String) As String

            Dim encPwd As Byte() = Encoding.UTF8.GetBytes(password)

            Dim sha1 As HashAlgorithm = HashAlgorithm.Create("SHA1")

            Dim pp As Byte() = sha1.ComputeHash(encPwd)

            System.Text.Encoding.UTF8.GetString(pp)

            Dim sb As StringBuilder = New StringBuilder()

            For Each b As Byte In pp

                sb.Append(b.ToString("x2"))

            Next

            Return sb.ToString()

        End Function

        Protected Function hashGenerator(ByVal Username As String, ByVal sender_id As String, ByVal message As String, ByVal secure_key As String) As String

            Dim sb As StringBuilder = New StringBuilder()

            sb.Append(Username).Append(sender_id).Append(message).Append(secure_key)

            Dim genkey As Byte() = Encoding.UTF8.GetBytes(sb.ToString())

            Dim sha1 As HashAlgorithm = HashAlgorithm.Create("SHA512")

            Dim sec_key As Byte() = sha1.ComputeHash(genkey)

            Dim sb1 As StringBuilder = New StringBuilder()

            For i As Integer = 0 To sec_key.Length - 1

                sb1.Append(sec_key(i).ToString("x2"))

            Next

            Return sb1.ToString()

        End Function

    End Class

'Class MyPolicy

' Inherits ICertificatePolicy

' Public Function CheckValidationResult(ByVal srvPoint As ServicePoint, ByVal certificate As X509Certificate, ByVal request As WebRequest, ByVal certificateProblem As Integer) As Boolean

' Return True

' End Function

'End Class

