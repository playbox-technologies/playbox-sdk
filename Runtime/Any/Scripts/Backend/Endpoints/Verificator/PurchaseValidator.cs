using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Playbox;
using UnityEngine;

namespace Any.Scripts.Backend.Verificator
{
    public static class PurchaseValidator
    {
        public static async void Validate(ProductDataAdapter productData, Action<bool, ProductDataAdapter> callback)
        {
            JObject sendObject = new()
            {
                ["product_id"] = productData.DefinitionId,
                ["receipt"] = productData.Receipt,
                ["price"] = productData.MetadataLocalizedPrice,
                ["currency"] = productData.MetadataIsoCurrencyCode
            };
        
            var a = await HttpService.PostJsonAsync("/iap/verify", sendObject.ToString());
            
            var jsonData = JObject.Parse(a.Body);
         
            var ct = CancellationToken.None;
         
            await PollSafeAsync(ct, jsonData["ticket"]?.ToString(), callback);
        }
        
        private static async Task PollSafeAsync(CancellationToken ct, string key,Action<bool, ProductDataAdapter> callback)
        {
            while (!ct.IsCancellationRequested)
            {
                using var linkedCts =
                    CancellationTokenSource.CreateLinkedTokenSource(ct);

                linkedCts.CancelAfter(1000);

                var result = await HttpService.GetAsync($"/iap/status/{key}", linkedCts.Token);

                if(string.IsNullOrEmpty(result.Body))
                    continue;
                
                var resultData = JObject.Parse(result.Body);

                string stringResult = resultData["status"]?.ToString();


                var status = VerificationStatusHelper.GetStatusByString(stringResult);
            
                if (status == VerificationStatusHelper.EStatus.unverified)
                {
                    
                    decimal price = 0m;

                    if (resultData.TryGetValue("price_usd", out var value) &&
                        decimal.TryParse(
                            value?.ToString(),
                            NumberStyles.Any,
                            CultureInfo.InvariantCulture,
                            out var parsed))
                    {
                        price = parsed;
                    }
                    
                    callback.Invoke(false, new ProductDataAdapter
                    {
                        MetadataLocalizedPrice = price,
                        MetadataIsoCurrencyCode = "USD"
                    });
                    
                    break;
                }
                if (status == VerificationStatusHelper.EStatus.verified)
                {
                    callback.Invoke(true, new ProductDataAdapter
                    {
                        MetadataLocalizedPrice = decimal.Parse(resultData["price_usd"]?.ToString() ?? string.Empty),
                        MetadataIsoCurrencyCode = "USD"
                    });
                    
                    break;
                }
                
                await Task.Delay(100, ct);
            }
        }
    }
}