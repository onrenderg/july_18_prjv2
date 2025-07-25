Public Class StockItem

    Public Class StockItem
        Public Property ID As String
        Public Property ItemType As String
        Public Property ItemType_Desc As String
        Public Property ItemType_Desc_Loc As String
        Public Property ItemFor As String
        Public Property ItemFor_Desc As String
        Public Property ItemFor_Desc_Loc As String
        Public Property ItemName As String
        Public Property ItemNameLocal As String
        Public Property ItemDescription As String
        Public Property ItemDescriptionLocal As String
        Public Property ItemSpecification As String
        Public Property de_activation_remakrs As String
        Public Property ItemCapacity As String
        Public Property EntryBy As String
        Public Property EnteredOn As Date
    End Class

    Public Class StockSummary
        Public Property ID As String
        Public Property ItemType As String
        Public Property ItemFor As String
        Public Property ItemName As String
        Public Property ItemDescription As String
        Public Property MyTotalStock As Int64
        Public Property MyEnteredStock As Int64
        Public Property AllocatedToMe As Int64
        Public Property AvailableForAllocation As Int64
        Public Property AllocatedToOthers As Int64
        Public Property Disposed As Int64
        Public Property TotalConsumed As Int64
    End Class

End Class
