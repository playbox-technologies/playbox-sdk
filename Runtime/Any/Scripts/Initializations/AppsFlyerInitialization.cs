using System.Collections;
using System.Collections.Generic;
using Playbox.SdkConfigurations;
using AppsFlyerSDK;
using CI.Utils.Extentions;
using Newtonsoft.Json.Linq;
using Playbox.Consent;

using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;


namespace Playbox
{
    public class AppsFlyerInitialization : PlayboxBehaviour,IAppsFlyerConversionData
    {
        private string af_status;
        private string media_source;
        
        public override void Initialization()
        {
            base.Initialization();
            
            AppsFlyerConfiguration.LoadJsonConfig();
            
            if(!AppsFlyerConfiguration.Active)
                return;
            
            AppsFlyerConsent consent = new AppsFlyerConsent(
                    ConsentData.Gdpr,
                    ConsentData.ConsentForData,
                    ConsentData.ConsentForAdsPersonalized,
                    ConsentData.ConsentForAdStogare);

            AppsFlyer.setConsentData(consent);
            

#if UNITY_IOS
                AppsFlyer.initSDK(AppsFlyerConfiguration.IOSKey, AppsFlyerConfiguration.IOSAppId);
            
#elif UNITY_ANDROID
            
            AppsFlyer.initSDK(AppsFlyerConfiguration.AndroidKey, AppsFlyerConfiguration.AndroidAppId);
#endif 
            
            AppsFlyer.setSharingFilterForPartners(new string[] { });
            
            AppsFlyer.enableTCFDataCollection(true);
            
            AppsFlyer.startSDK();
            
            AppsFlyer.setIsDebug(true);      

            
            AppsFlyer.getConversionData("af_status");

            
            StartCoroutine(initUpd());

        }

        private IEnumerator initUpd()
        {
            while (true)
            {
                if (!string.IsNullOrEmpty(AppsFlyer.getAppsFlyerId()))
                {
                    ApproveInitialization();
                    yield break;
                }
                yield return new WaitForSeconds(1f);
            }
        }

        public void onConversionDataFail(string error)
        {
            
        }

        public void onAppOpenAttribution(string attributionData)
        {
            
        }

        public void onAppOpenAttributionFailure(string error)
        {
            
        }
        
        public void onConversionDataSuccess(string conversionData)
        {
            var data = AppsFlyer.CallbackStringToDictionary(conversionData);

            string afStatus = GetStr(data, "af_status");
            string mediaSource = GetStr(data, "media_source");  
            string siteId = GetStr(data, "af_siteid");         
            string campaign = GetStr(data, "campaign");
            string sub1 = GetStr(data, "af_sub1");

            bool fromCrossPromo = afStatus == "Non-organic" && mediaSource == "af_cross_promotion";
            
            if (fromCrossPromo)
            {
                Debug.Log($"[AF] Cross-promo install from {siteId}, campaign={campaign}, sub1={sub1}");
                
                $"[AF] Cross-promo install from {siteId}, campaign={campaign}, sub1={sub1}".PlayboxSplashLogUGUI();
            }

            "cross_promo data begin :".PlayboxInfo();

            data.toJson().PlayboxSplashLogUGUI();
            
            "cross_promo data end :".PlayboxInfo();
        }
        
        static string GetStr(Dictionary<string, object> d, string key) =>
            d != null && d.TryGetValue(key, out var v) && v != null ? v.ToString() : null;
    }
}