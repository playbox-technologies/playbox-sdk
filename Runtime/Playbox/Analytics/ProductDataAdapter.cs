using Newtonsoft.Json.Linq;

namespace Playbox
{
    public class ProductDataAdapter
    {
        public string TransactionId { get; set; }
        public string DefinitionId { get; set; }
        public decimal MetadataLocalizedPrice { get; set; }
        public string MetadataIsoCurrencyCode { get; set; }
        public string Receipt { get; set; }

        public override string ToString()
        {
            JObject jo = new JObject();
            
            jo["TransactionId"] = TransactionId;
            jo["DefinitionId"] = DefinitionId;
            jo["MetadataLocalizedPrice"] = MetadataLocalizedPrice;
            jo["MetadataIsoCurrencyCode"] = MetadataIsoCurrencyCode;
            jo["Receipt"] = Receipt;
            
            return jo.ToString();
        }
    }
}