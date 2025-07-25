Imports System.Data

Public Class validation
    Dim strAlphaNumericPattern As Regex
    Public mycontrol As String
    Dim i As Integer
    Dim strchk As String
    Dim iLoop As Integer
    Dim strKey As Integer

    Dim resWords() As String = {"ALERT ", "alert", "alert ", "SCRIPT ", " SCRIPT", " SCRIPT ", "CREATE ", " CREATE", " CREATE ", "INSERT ", " INSERT", " INSERT ", "DELETE ", " DELETE", " DELETE ", "UPDATE ", " UPDATE", " UPDATE ", "HAVING ", " HAVING", " HAVING ", "IFRAME ", " IFRAME", " IFRAME ", "FRAME ", " FRAME", " FRAME ", " <", "< ", "<", " < ", " >", "> ", ">", " > ", "SRC ", " SRC", " SRC ", "SRC"}
    Public Function ValidChar(ByVal strValue As String) As Boolean
        'for reserve words
        If strValue <> "" Then
            If reservewords(strValue) Then
                Return False
                Exit Function
            End If
            strAlphaNumericPattern = New Regex("^[0-9०-९\p{L}\p{M} \40 \,\|\।\!\`\~\.\:\;\-\+\%\*\@\(\)\/\?_]+$")  ''Allowed Char
            If strAlphaNumericPattern.IsMatch(strValue) Then
                Return True
                Exit Function
            Else
                Return False
                Exit Function
            End If
            Return True
        Else
            Return True
        End If

    End Function
    'validate for blank field
    Public Function blank(ByVal strValue As String) As Boolean
        If Len(strValue) = 0 Then
            Return True
            Exit Function
        End If
        Return False
    End Function
    'validate for reserve words
    Public Function reservewords(ByVal strValue As String) As Boolean
        'Dim i As Integer
        'For i = 0 To resWords.Length - 1
        '    If InStr(UCase(strValue), resWords(i)) = 0 Then
        '    Else
        '        Return True
        '        Exit Function
        '    End If
        'Next
        Return False
    End Function

    Public Function Email(ByVal strValue As String) As Boolean
        'for blank
        If blank(strValue) Then
            Return False
            Exit Function
        End If

        'for reserve words
        strAlphaNumericPattern = New Regex("^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,12}|[0-9]{1,3})(\]?)$")
        If strAlphaNumericPattern.IsMatch(strValue) Then
            Return True
            Exit Function
        Else
            'mycontrol = control
            Return False
            Exit Function
        End If
    End Function
    Public Function numeric(ByVal strValue As String) As Boolean
        If blank(strValue) Then
            Return False
            Exit Function
        End If
        If reservewords(strValue) Then
            Return False
            Exit Function
        End If
        strAlphaNumericPattern = New Regex("^([0-9])+$")
        If strAlphaNumericPattern.IsMatch(strValue) Then
            Return True
            Exit Function
        Else
            Return False
            Exit Function
        End If
        Return True
    End Function
    Public Function float(ByVal strValue As String) As Boolean
        If blank(strValue) Then
            Return False
            Exit Function
        End If
        If reservewords(strValue) Then
            Return False
            Exit Function
        End If
        strAlphaNumericPattern = New Regex("^([0-9.])+$")
        If strAlphaNumericPattern.IsMatch(strValue) Then
            Return True
            Exit Function
        Else
            Return False
            Exit Function
        End If
        Return True
    End Function

    Public Function mobile(ByVal strValue As String) As Boolean


        'for reserve words
        If reservewords(strValue) Then
            ' mycontrol = control
            Return False
            Exit Function
        End If

        strAlphaNumericPattern = New Regex("^[0-9]{10,12}$")
        If strAlphaNumericPattern.IsMatch(strValue) Then
            Return True
            Exit Function
        ElseIf strValue.StartsWith("0") Or strValue.StartsWith("1") Or strValue.StartsWith("2") Or strValue.StartsWith("3") Or
            strValue.StartsWith("4") Or strValue.StartsWith("5") Or strValue.StartsWith("6") Then
            Return False
            Exit Function
        Else
            'mycontrol = control
            Return False
            Exit Function
        End If
    End Function


    Public Function engtextwithspace(ByVal strValue As String) As Boolean
        'for blank
        If blank(strValue) Then

            Return False
            Exit Function
        End If
        'for reserve words
        If reservewords(strValue) Then
            Return False
            Exit Function
        End If
        'strAlphaNumericPattern = New Regex("^([a-zA-Z0-9\s\n-,\\/.])+$")
        strAlphaNumericPattern = New Regex("^[a-zA-Z ]+$")
        If strAlphaNumericPattern.IsMatch(strValue) Then
            Return True
            Exit Function
        Else
            Return False
            Exit Function
        End If
        Return True
    End Function

    Public Function engtextwithdot(ByVal strValue As String) As Boolean
        'for blank
        If blank(strValue) Then

            Return False
            Exit Function
        End If
        'for reserve words
        If reservewords(strValue) Then
            Return False
            Exit Function
        End If
        'strAlphaNumericPattern = New Regex("^([a-zA-Z0-9\s\n-,\\/.])+$")
        strAlphaNumericPattern = New Regex("^[a-zA-Z. ]+$")
        If strAlphaNumericPattern.IsMatch(strValue) Then
            Return True
            Exit Function
        Else
            Return False
            Exit Function
        End If
        Return True
    End Function
    Public Function Alphanumeric(ByVal strValue As String) As Boolean
        'for blank
        If blank(strValue) Then

            Return False
            Exit Function
        End If
        'for reserve words
        If reservewords(strValue) Then
            Return False
            Exit Function
        End If
        'strAlphaNumericPattern = New Regex("^([a-zA-Z0-9\s\n-,\\/.])+$")
        strAlphaNumericPattern = New Regex("^[a-zA-Z0-9 ]+$")
        If strAlphaNumericPattern.IsMatch(strValue) Then
            Return True
            Exit Function
        Else
            Return False
            Exit Function
        End If
        Return True
    End Function
    Public Function Alphanumericwithdothiphen(ByVal strValue As String) As Boolean
        'for blank
        If blank(strValue) Then

            Return False
            Exit Function
        End If
        'for reserve words
        If reservewords(strValue) Then
            Return False
            Exit Function
        End If
        'strAlphaNumericPattern = New Regex("^([a-zA-Z0-9\s\n-,\\/.])+$")
        strAlphaNumericPattern = New Regex("^[a-zA-Z0-9.\- ]+$")
        If strAlphaNumericPattern.IsMatch(strValue) Then
            Return True
            Exit Function
        Else
            Return False
            Exit Function
        End If
        Return True
    End Function
    Public Function decimalwithprecision(ByVal strValue As String, ByVal first As Integer, ByVal second As Integer) As Boolean
        Dim isValidDecimal As Boolean = True
        Dim decValue As Decimal
        If Decimal.TryParse(strValue, decValue) Then
            'validate if precision is 9 and scale is 5'
            Dim sqlDec As SqlTypes.SqlDecimal = New SqlTypes.SqlDecimal(decValue)

            Try
                sqlDec = SqlTypes.SqlDecimal.ConvertToPrecScale(sqlDec, first, second)
            Catch exception As SqlTypes.SqlTruncateException
                isValidDecimal = False
            End Try
        Else
            isValidDecimal = False
        End If
        Return isValidDecimal
    End Function

    Public Function pin(ByVal strValue As String) As Boolean


        'for reserve words
        If reservewords(strValue) Then
            ' mycontrol = control
            Return False
            Exit Function
        End If

        strAlphaNumericPattern = New Regex("^(\d{6}){1}?$")
        If strAlphaNumericPattern.IsMatch(strValue) Then
            Return True
            Exit Function
        Else
            'mycontrol = control
            Return False
            Exit Function
        End If
    End Function

    Public Function text(ByVal strValue As String) As Boolean
        'for blank
        If blank(strValue) Then

            Return False
            Exit Function
        End If
        'for reserve words
        If reservewords(strValue) Then
            Return False
            Exit Function
        End If
        'strAlphaNumericPattern = New Regex("^([a-zA-Z0-9\s\n-,\\/.-])+$")
        strAlphaNumericPattern = New Regex("^[a-zA-Z0-9 # % @!$ [\]\-_,.:()/]*$")
        If strAlphaNumericPattern.IsMatch(strValue) Then
            Return True
            Exit Function
        Else
            Return False
            Exit Function
        End If
        Return True
    End Function



    Public Function Aadhar(ByVal strValue As String) As Boolean
        If blank(strValue) Then
            Return False
            Exit Function
        End If
        If reservewords(strValue) Then
            Return False
            Exit Function
        End If
        strAlphaNumericPattern = New Regex("^([0-9]{12})+$")
        If strAlphaNumericPattern.IsMatch(strValue) Then
            Return True
            Exit Function
        Else
            Return False
            Exit Function
        End If
        Return True
    End Function

    Public Function phone(ByVal strValue As String) As Boolean
        If blank(strValue) Then
            Return False
            Exit Function
        End If
        If reservewords(strValue) Then
            Return False
            Exit Function
        End If
        strAlphaNumericPattern = New Regex("^\d{5,8}$")
        If strAlphaNumericPattern.IsMatch(strValue) Then
            Return True
            Exit Function
        Else
            Return False
            Exit Function
        End If
        Return True
    End Function

End Class
