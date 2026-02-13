using System;
using System.Collections;
using GoogleMobileAds.Ump.Api;
using UnityEngine;
using Utils.Tools.Extentions;

#if UNITY_IOS
using Unity.Advertisement.IosSupport;
using UnityEngine.iOS;
#endif

namespace Playbox.Consent
{
    public class ConsentData
    {
        public static bool IsConsentComplete = false;
        public static bool Gdpr = false;
        public static bool ConsentForData = false;
        public static bool ConsentForAdsPersonalized = false;
        public static bool ConsentForAdStogare = false;
        public static bool ATE = false;
        public static string AdvertisingId = "";

        public static bool IsChildUser = false;
        public static bool HasUserConsent = true;
        public static bool HasDoNotSell = false;
        
        private static ConsentDebugSettings _debugSettings = new();

        private static Action _consentCallback;

        public static ConsentDebugSettings DebugSettings
        {
            get => _debugSettings;
            set => _debugSettings = value;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public static void ConsentAllow()
        {
            IsConsentComplete = true;
            Gdpr = true;
            ConsentForData = true;
            ConsentForAdsPersonalized = true;
            ConsentForAdStogare = true;
            IsChildUser = false;
            HasUserConsent = true;
            HasDoNotSell = true;

            "Consent Allow".PlayboxInfo();
            //consentCallback?.Invoke(true);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public static void ConsentDeny()
        {
            IsConsentComplete = true;
            Gdpr = true;
            ConsentForData = true;
            ConsentForAdsPersonalized = true;
            ConsentForAdStogare = true;
            IsChildUser = false;
            HasUserConsent = true;
            HasDoNotSell = true;
            
            "Consent Deny".PlayboxInfo();
            //consentCallback?.Invoke(false);
        }

        private static IEnumerator ConsentUpdate(Action consentComplete)
        {
            "Starting Consent Update".PlayboxInfo();
            
            while (true)
            {
                if (IsConsentComplete)
                {
                    consentComplete?.Invoke();
                    yield break;
                }

                yield return new WaitForSeconds(0.1f);
            }
        }
        
        public static void ShowConsent(MonoBehaviour mono, Action callback, bool isDebug = false)
        {
            
            if(isDebug)
                GoogleUmpManager.RequestConsentInfoDebug(_debugSettings);
            else
                GoogleUmpManager.RequestConsentInfo();
            
            mono.StartCoroutine(ConsentUpdate(() =>
            {
                
#if UNITY_IOS

                bool isAttComplete = false;
                
                IOSConsent.ShowATTUI(mono, (result) =>
                {
                    isAttComplete = true;
                    callback?.Invoke();
                    
                    ATE = Device.advertisingTrackingEnabled && result;
                });
                
                if (isAttComplete)
                    return;
#endif
                
                
                Application.RequestAdvertisingIdentifierAsync((advertisingId, trackingEnabled, errorMsg) =>
                {
                    AdvertisingId = advertisingId;
                });

#if UNITY_ANDROID
                callback?.Invoke();
#endif
            }));
            
        }
    }
}