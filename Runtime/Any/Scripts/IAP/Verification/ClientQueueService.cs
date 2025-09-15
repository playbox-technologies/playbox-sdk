using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Playbox.SdkConfigurations;
using UnityEngine;

namespace Playbox.Verification
{
    public class ClientQueueService
    {
        private static string ServerUrl = "";

        private static bool IsReadyConfig => GlobalPlayboxConfig.IsLoaded;
        
        private static string ServerKey = "";
        private static string ServerURL = "";

        private static string GetTokenURL => $"{ServerUrl}";
        private static string GetTokenStatusURL(string token) => $"{ServerUrl}/status/{token}";
        

        public static void Initialization()
        {
            ServerKey = GetServerKey();
            ServerUrl = GetServerAddress();
        }

        public static void ValidateProduct(string productId, string receipt, Action<string> onSuccess = null)
        {
            
        }

        public static async Task<string> GetServerToken(string productId, string receipt)
        {
            HttpClient client = new HttpClient();

            string json = CreateSendObjectJson("asd","asd").ToString();
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("x-api-token", ServerKey);

            using var response = await client.PostAsync(GetTokenURL,content);
            
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadAsStringAsync();
        }
        
        private static JObject CreateSendObjectJson(string productID, string receipt)
        {
            //TimeZoneInfo localZone = TimeZoneInfo.Local;
            
            JObject sendObject = new()
            {
                ["os_version"] = SystemInfo.operatingSystem,
                ["device_name"] = SystemInfo.deviceName,
                ["device_model"] = SystemInfo.deviceModel,
                ["app_version"] = Data.Playbox.AppVersion,
                ["product_id"] = productID,
                ["game_id"] = Data.Playbox.GameId,
                ["version"] = Data.Playbox.AppVersion,
                ["receipt"] = receipt
            };

#if UNITY_ANDROID
            sendObject["platform"] = "android";
#elif UNITY_IOS
            sendObject["platform"] = "ios";
#endif
            
#if UNITY_ANDROID
            sendObject["manufacturer"] = "android";
#elif UNITY_IOS
            sendObject["manufacturer"] = "apple";
#endif
            
            return sendObject;
        }
        
        private static string GetServerAddress()
        {
            GlobalPlayboxConfig.Load();
            
            VerificatorConfig.LoadJsonConfig();

            if (IsReadyConfig)
            {
                return VerificatorConfig.ServerURL;
            }
            
            return "";

        }
        
        private static string GetServerKey()
        {
            GlobalPlayboxConfig.Load();
            
            VerificatorConfig.LoadJsonConfig();

            if (IsReadyConfig)
            {
                return VerificatorConfig.ServerKey;
            }
            
            return "";

        }

    }
}