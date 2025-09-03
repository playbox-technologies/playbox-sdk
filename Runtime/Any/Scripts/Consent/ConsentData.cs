using System;
using System.Collections;
using CI.Utils.Extentions;
using GoogleMobileAds.Ump.Api;
using UnityEngine;

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
            "Starting Consent Update".PlayboxSplashLogUGUI();
            
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
            "Show Consent Method".PlayboxSplashLogUGUI();
            
            if(isDebug)
                GoogleUmpManager.RequestConsentInfoDebug(_debugSettings);
            else
                GoogleUmpManager.RequestConsentInfo();
            
            
            mono.StartCoroutine(ConsentUpdate(() =>
            {
                
#if UNITY_IOS
                
                IOSConsent.ShowATTUI(mono, (result) =>
                {
                    
                    "ATT Callback".PlayboxSplashLogUGUI();
                    callback?.Invoke();
                    
                    ATE = result;
                });
#endif
                

#if UNITY_ANDROID || UNITY_EDITOR
                callback?.Invoke();
#endif
            }));
            
        }
    }
}
