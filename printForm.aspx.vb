Public Class printForm
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            lbl_error.Visible = False
            Dim DistrictName As String = Session("gblDistrictName").ToUpper
            Dim DistrictNameLocal As String = Session("gblDistrictNameLocal").ToUpper
            Dim lang As String = ""
            Try
                lang = Session("MyLanguage")
                If String.IsNullOrEmpty(lang) Then
                    lang = "hi-IN"
                End If
            Catch ex As Exception
                lang = "hi-IN"
            End Try
            Dim UnitType As String = Session("unitType")
            If Session("unitType") = "Ballot Box" Or Session("unitType") = "बैलट बॉक्स" Then ' 
                liheader.Text = IIf(lang = "hi-IN", DistrictNameLocal, DistrictName) + " <b><u>" + UnitType + "</u></b> की क्यू.आर. कोड सूची <br/>"

            Else
                liheader.Text = IIf(lang = "hi-IN", DistrictNameLocal, DistrictName) + " के ईवीएम- <b><u>" + UnitType + "</u></b> की क्यू.आर. कोड सूची <br/>"

            End If
            litable.Text = Session("strText").ToString
            'lbldate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm")
        Catch ex As Exception
            lbl_error.Visible = True
        End Try
    End Sub
End Class