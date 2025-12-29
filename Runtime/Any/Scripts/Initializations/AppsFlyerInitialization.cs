using System;
using System.Collections;
using System.Collections.Generic;
using Playbox.SdkConfigurations;
using AppsFlyerSDK;
using Playbox.Consent;
using UnityEngine;
using Utils.Tools.Extentions;


namespace Playbox
{
    public class AppsFlyerInitialization : PlayboxBehaviour,IAppsFlyerConversionData
    {
        private string af_status;
        private string media_source;

        public static Action<string, string> OnDeepLinkValue;
        public static Action OnConversionDataSuccess;
        
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
                AppsFlyer.initSDK(AppsFlyerConfiguration.IOSKey, AppsFlyerConfiguration.IOSAppId, this);
            
#elif UNITY_ANDROID
            
            AppsFlyer.initSDK(AppsFlyerConfiguration.AndroidKey, AppsFlyerConfiguration.AndroidAppId, this);
#endif 
            
            AppsFlyer.setSharingFilterForPartners();
            
            AppsFlyer.enableTCFDataCollection(true);
            
            AppsFlyer.startSDK();
            
            AppsFlyer.OnDeepLinkReceived += OnDeepLink;
            
            AppsFlyer.setIsDebug(true);      
            
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
            "onConversionDataFail".PlayboxInfo();
        }

        public void onAppOpenAttribution(string attributionData)
        {
            "onAppOpenAttribution".PlayboxInfo();
        }

        public void onAppOpenAttributionFailure(string error)
        {
            "onAppOpenAttributionFailure".PlayboxInfo();
        }
        
        public void onConversionDataSuccess(string conversionData)
        {
            var d = AppsFlyer.CallbackStringToDictionary(conversionData);
            string status = GetStr(d, "af_status");           
            string media = GetStr(d, "media_source");          
            string site  = GetStr(d, "af_siteid");           
            string camp  = GetStr(d, "campaign");             
            string dlv   = GetStr(d, "deep_link_value");      
            string sub1  = GetStr(d, "deep_link_sub1");       

            bool fromXPromo = status == "Non-organic" && media == "af_cross_promotion";
            if (fromXPromo)
            {
                $"[AF] XPromo install from {site} campaign={camp} dlv={dlv} sub1={sub1}".PlayboxInfo("AF");
                
                OnConversionDataSuccess.Invoke();
            }
        }
        
        private void OnDeepLink(object sender, EventArgs e)
        {
            var args = e as DeepLinkEventsArgs;
            if (args == null || args.status != DeepLinkStatus.FOUND) return;

          
            Dictionary<string, object> payload = null;
#if UNITY_ANDROID
        payload = args.deepLink;
#elif UNITY_IOS
        if (args.deepLink.TryGetValue("click_event", out var ce) && ce is Dictionary<string, object> ceDict)
            payload = ceDict;
#endif
            if (payload == null) return;

            string dlv  = GetStr(payload, "deep_link_value");
            string sub1 = GetStr(payload, "deep_link_sub1");

            $"UDL dlv={dlv} sub1={sub1}".PlayboxInfo("AF");

            OnDeepLinkValue?.Invoke(dlv, dlv);
        }
        
        static string GetStr(Dictionary<string, object> d, string key) =>
            d != null && d.TryGetValue(key, out var v) && v != null ? v.ToString() : null;
    }
}