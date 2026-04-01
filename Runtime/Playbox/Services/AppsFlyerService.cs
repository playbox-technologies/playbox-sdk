using AppsFlyerSDK;
using Playbox.Consent;
using Playbox.SdkConfigurations;
using Playbox.Tools.Extentions;

namespace Playbox.Services
{
    public class AppsFlyerService : PlayboxService
    {
        
        public override void Initialization()
        {
            base.Initialization();

            ServiceType = ServiceType.AppsFlyer;
            
            AppsFlyerConfiguration.LoadJsonConfig();
            
            if(!AppsFlyerConfiguration.Active)
                return;
            
            AppsFlyerConsent consent = new AppsFlyerConsent(
                    ConsentData.Gdpr,
                    ConsentData.ConsentForData,
                    ConsentData.ConsentForAdsPersonalized,
                    ConsentData.ConsentForAdStogare);
            
            AppsFlyer.setConsentData(consent);

           var proxy = PbProxyClass.GetOrCreateMonoProxy();

#if UNITY_IOS
                AppsFlyer.initSDK(AppsFlyerConfiguration.IOSKey, AppsFlyerConfiguration.IOSAppId, proxy);
            
#elif UNITY_ANDROID
            
            AppsFlyer.initSDK(AppsFlyerConfiguration.AndroidKey, AppsFlyerConfiguration.AndroidAppId, proxy);
#endif 
            
            AppsFlyer.setSharingFilterForPartners();
            
            AppsFlyer.enableTCFDataCollection(true);
            
            AppsFlyer.startSDK();
            
            AppsFlyer.setIsDebug(true);      
            
            LLS.PlayerAsyncHelper.WaitUntil(() => !string.IsNullOrEmpty(AppsFlyer.getAppsFlyerId()));
            
            ApproveInitialization();
        }

        public void onConversionDataFail(string error)
        {
            "onConversionDataFail".PbInfo();
        }

        public void onAppOpenAttribution(string attributionData)
        {
            "onAppOpenAttribution".PbInfo();
        }

        public void onAppOpenAttributionFailure(string error)
        {
            "onAppOpenAttributionFailure".PbInfo();
        }
    }
}