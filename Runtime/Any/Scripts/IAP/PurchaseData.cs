using System;
using Playbox;

[Serializable]
public class PurchaseData
{
    private string productId;
    private string ticketId;
    private string _saveIndentifier;
    
    public Action<bool, ProductDataAdapter> OnValidateCallback;

    public string ProductId
    {
        get => productId;
        set => productId = value;
    }

    public string SaveIndentifier
    {
        get => _saveIndentifier;
        set => _saveIndentifier = value;
    }

    public string TicketId
    {
        get => ticketId;
        set => ticketId = value;
    }
}