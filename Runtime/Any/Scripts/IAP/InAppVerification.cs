using System;
using System.Collections;
using System.Collections.Generic;
using CI.Utils.Extentions;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Playbox
{
    public class InAppVerification : PlayboxBehaviour
    { 
        private float verifyUpdateRate = 0.8f;
        
        private const string Uri = "https://api.playbox.services/v1/iap/verify";
        private const string UriStatus = "https://api.playbox.services/v1/iap/status";
        private string Bearer = $"Bearer {Data.Playbox.PlayboxKey}";
        
        private Dictionary<string, PurchaseData> _verificationQueue = new(); 
        private List<PurchaseData> _keyBuffer = new();
        
        private static InAppVerification _instance;

        public override void Initialization()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        
            DontDestroyOnLoad(gameObject);
            
            isInitialized = true;
            StartCoroutine(UpdatePurchases());
        }
        
        public static void Validate(string productID,string receipt,double price, string currency, Action<bool, ProductDataAdapter> callback)
        {
            if(_instance == null) return;
            if(string.IsNullOrEmpty(productID)) return;
            if(string.IsNullOrEmpty(receipt)) return;
            if(callback == null) return;
            
            _instance.SendRequest(productID, receipt,price,currency,callback);
        }

        public void SendRequest(string productID,string receipt,double price, string currency, Action<bool,ProductDataAdapter> callback)
        {
            StartCoroutine(Request(productID,receipt,price,currency, callback));
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public IEnumerator Request(string productID,string receipt, double price, string currency, Action<bool,ProductDataAdapter> callback)
        {
            UnityWebRequest sendPurchaseRequest = new UnityWebRequest(Uri, "POST");
            
            sendPurchaseRequest.SetRequestHeader("Content-Type", "application/json");
            sendPurchaseRequest.SetRequestHeader("Authorization", Bearer);
            sendPurchaseRequest.SetRequestHeader("X-User-ID", Data.Playbox.PlayboxKey);
        
            var sendObject = CreateSendObjectJson(productID, receipt);
            
            sendObject["price"] = price;
            sendObject["currency"] = currency;

            var bodyRaw = System.Text.Encoding.UTF8.GetBytes(sendObject.ToString());

            sendPurchaseRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            sendPurchaseRequest.downloadHandler = new DownloadHandlerBuffer();
            
            yield return sendPurchaseRequest.SendWebRequest();

            if (sendPurchaseRequest.result is UnityWebRequest.Result.ProtocolError or UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.DataProcessingError)
            {
                $"Request Failed: {sendPurchaseRequest.error}".PlayboxError();
            }

            if (sendPurchaseRequest.isDone)
            {
                JObject outObject = JObject.Parse(sendPurchaseRequest.downloadHandler.text);
            
                string ticketID = outObject["ticket"]?.ToString();
            
                PurchaseData data = new PurchaseData
                {
                    ProductId = productID,
                    TicketId = ticketID,
                    OnValidateCallback = callback
                };            
   
                _keyBuffer.Add(data);
            }
        
        }

        private JObject CreateSendObjectJson(string productID, string receipt)
        {
            JObject sendObject = new()
            {
                //["os_version"] = SystemInfo.operatingSystem,
                //["device_name"] = SystemInfo.deviceName,
                //["device_model"] = SystemInfo.deviceModel,
                ["app_version"] = Data.Playbox.AppVersion,
                ["product_id"] = productID,
                ["bundle_id"] = Data.Playbox.GameId,
                ["app_version"] = Data.Playbox.AppVersion,
                ["receipt"] = receipt
            };

#if UNITY_ANDROID
            sendObject["platform"] = "android";
#elif UNITY_IOS
            sendObject["platform"] = "ios";
#endif
            
            return sendObject;
        }

        private IEnumerator UpdatePurchases() {

            List<string> removeFromListByTicket = new();
            
            while (true)
            {
                foreach (var item in _keyBuffer)
                {
                    _verificationQueue[item.TicketId] = item;
                }
            
                _keyBuffer.Clear();

                foreach (var item in _verificationQueue)
                {
                    yield return GetStatus(item, b => { 
                        if(b)
                        {
                            removeFromListByTicket.Add(item.Key);
                        }
                    });
                }

                foreach (var item in removeFromListByTicket)
                {
                    _verificationQueue.Remove(item);
                }
            
                removeFromListByTicket.Clear();
            
                yield return new WaitForSeconds(verifyUpdateRate);
            }
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator GetStatus(KeyValuePair<string,PurchaseData> purchaseDataItem, Action<bool> removeFromQueueCallback)
        {
         
            UnityWebRequest statusRequest = new UnityWebRequest($"{UriStatus}/{purchaseDataItem.Key}", "GET");
        
            statusRequest.SetRequestHeader("Content-Type", "application/json");
            statusRequest.SetRequestHeader("Authorization", Bearer);
        
            statusRequest.downloadHandler = new DownloadHandlerBuffer();
        
            yield return statusRequest.SendWebRequest();

            if (statusRequest.result == UnityWebRequest.Result.ProtocolError ||
                statusRequest.result == UnityWebRequest.Result.ConnectionError ||
                statusRequest.result == UnityWebRequest.Result.DataProcessingError)
            {
                $"Request Failed: {statusRequest.error}".PlayboxError();
            }
            
            if (statusRequest.isDone)
            {
                JObject json = JObject.Parse(statusRequest.downloadHandler.text);
                
                string status = json["status"]?.ToString();
                
                switch (VerificationStatusHelper.GetStatusByString(status))
                {
                    case VerificationStatusHelper.EStatus.none:
                        
                        removeFromQueueCallback?.Invoke(true);
                    
                        break;
                
                    case VerificationStatusHelper.EStatus.pending:
                    
                        removeFromQueueCallback?.Invoke(false);
                        break;
                
                    case VerificationStatusHelper.EStatus.verified:
                        
                        purchaseDataItem.Value.OnValidateCallback?.Invoke(true, new ProductDataAdapter()
                        {
                            MetadataLocalizedPrice = decimal.Parse(json["price_usd"]?.ToString() ?? string.Empty),
                            MetadataIsoCurrencyCode = "USD"
                        });
                        removeFromQueueCallback?.Invoke(true);
                        
                        break;
                
                    case VerificationStatusHelper.EStatus.unverified:
                        
                        purchaseDataItem.Value.OnValidateCallback?.Invoke(false, new ProductDataAdapter()
                        {
                            MetadataLocalizedPrice = decimal.Parse(json["price_usd"]?.ToString() ?? string.Empty),
                            MetadataIsoCurrencyCode = "USD"
                        });
                        removeFromQueueCallback?.Invoke(true);
                        
                        break;
                
                    case VerificationStatusHelper.EStatus.error:
                        
                        removeFromQueueCallback?.Invoke(true);
                        break;
                
                    case VerificationStatusHelper.EStatus.timeout:
                        
                        removeFromQueueCallback?.Invoke(true);
                        break;
                    
                    default:
                        purchaseDataItem.Value.OnValidateCallback?.Invoke(false, new ProductDataAdapter()
                        {
                            MetadataLocalizedPrice = decimal.Parse(json["price_usd"]?.ToString() ?? string.Empty),
                            MetadataIsoCurrencyCode = "USD"
                        });
                        removeFromQueueCallback?.Invoke(true);
                        break;
                }
            }
        }
    }
}
