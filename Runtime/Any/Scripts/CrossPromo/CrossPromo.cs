using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AppsFlyerSDK;
using Newtonsoft.Json.Linq;
using Playbox.SdkConfigurations;
using UnityEngine;
using Utils.Tools.WebQuery;

namespace Playbox
{
    public static class CrossPromo
    {

        /// <summary>
        /// Recorm Cross Promo
        /// </summary>
        /// <param name="promotedID">id of the promoted application from AppsFlyer</param>
        /// <param name="campaign"></param>
        /// <param name="parameters"></param>
        public static void RecordCrossPromoImpression(string promotedID,string campaign, Dictionary<string, string> parameters)
        {
            if (Analytics.IsAppsFlyerInit) AppsFlyer.recordCrossPromoteImpression(promotedID,campaign,parameters);
        }

        public static void OpenStore(string afLink)
        {
#if !UNITY_EDITOR
            if (Analytics.isAppsFlyerInit)
            {
#endif
                AppsFlyerConfiguration.LoadJsonConfig();
                
                Application.OpenURL(afLink);
#if !UNITY_EDITOR
            }
#endif
        }
        
        public static async Task LoadLinkAndOpenStore(string promotedID, string placementID = "main")
        {
           
#if !UNITY_EDITOR
            if (Analytics.isAppsFlyerInit)
            {
#endif
                string os = GetOS();

                string afLink = await GetPromoURL(promotedID, os, placementID);
          
                Application.OpenURL(afLink);
#if !UNITY_EDITOR
            }
#endif
        }
        
        public static async Task<string> GetPromoURL(string bundleID, string os,string placementID = "main")
        {
            using var client = new HttpClient();
            string baseUrl = "https://api.playbox.space/promo/get-link";
            
            string query = QueryStringBuilder.Build(new Dictionary<string, string>
            {
                ["placement_id"] = placementID,
                ["bundle_id"] = bundleID,
                ["os"] = os
            });
        
            string url = $"{baseUrl}?{query}";
  
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("X-Api-Token", Data.Playbox.PlayboxKey);
        
            HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        
            string result = await response.Content.ReadAsStringAsync();
        
            JObject.Parse(result).TryGetValue("appsflyer_link", out JToken jlink);
        
            string appsflyerLink = (string)jlink;
        
            return appsflyerLink;
        }

        public static string GetOS()
        {
            string os = "ios";

#if UNITY_ANDROID
            os = "android";
#endif
            
#if UNITY_IOS
            os = "ios";
#endif
            
            return os;
        }
    }
}