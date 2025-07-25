Public Class Ballot
    <Serializable()>
    Public Class Ballot_Series
        Public Property series As String
        Public Property box_size As String
        Public Property series_state As String
        Public Property series_datetime As DateTime
    End Class
    Public Class PendingQR
        Public Property series As String
        Public Property box_size As String
        Public Property sr_no As Int64
        Public Property qr_text As String
    End Class
    Public Class Print_QR
        Public Property qr_text_description As String
        Public Property qr_text As String
    End Class
    Public Class Allocate_QR
        Public Property qr_text_description As String
        Public Property from_user_id As String
        Public Property to_user_id As String
        Public Property is_received As Boolean
    End Class
    Public Class Ballot_Stock
        Public Property series As String
        Public Property from_user_id As String
        Public Property box_size As String
        Public Property sr_no_range As String
        Public Property total_qty As Int64
        Public Property from_sr_no As Int64
        Public Property to_sr_no As Int64
        Public Property latitude As Decimal
        Public Property longitude As Decimal
        Public Property is_received As Boolean
        Public Property to_user_code As String
        Public Property MonYrPurchase As String
        Public Property has_qr As Boolean
        Public Property items_allocated_to_selected_user As Int64
    End Class

    Public Class Ballot_Stock_Owner
        Public Property series As String
        Public Property box_size As String
        Public Property total_qty As Int64
        Public Property from_sr_no As Int64
        Public Property to_sr_no As Int64
        Public Property purchase_month As Int32
        Public Property purchase_year As Int32
        Public Property MonYrPurchase As String
        Public Property purchaser_user_code As String
        Public Property purchaser_user_id As String
        Public Property sr_no_range As String
        Public Property entry_date As DateTime
    End Class
    Public Class BallotSummary
        Public Property to_user_code As String
        Public Property No_Of_Units As String
        Public Property Display_Name As String
    End Class
    Public Class BallotSummaryList
        Public Property to_user_code As String
        Public Property Display_Name As String
        Public Property latitude As String
        Public Property longitude As String
        Public Property button_text As String
        Public Property qr_text As String

    End Class
End Class

