using System;
using Facebook.Unity;
using Playbox;
using Playbox.Consent;
using Playbox.SdkConfigurations;
using UnityEngine;

namespace Any.Scripts.Initializations
{
    public class FacebookSdkInitialization : PlayboxBehaviour
    {
        public override void Initialization()
        {
            base.Initialization();

            PB_ProxyClass.OnPauseStatusChanged += OnApplicationPause;
            PB_ProxyClass.OnFocusStatusChanged += OnApplicationFocus;
            
            serviceType = ServiceType.Facebook;
            
            FacebookSdkConfiguration.LoadJsonConfig();
            
            if(!FacebookSdkConfiguration.Active)
                return;
            
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
                InitParameters();
                
                ApproveInitialization();
            }
            else
            {
                FB_Init(OnInitCallback);
            }
        }

        private void OnInitCallback()
        {
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
                
                InitParameters();
                
                ApproveInitialization();
            }
            else
            {
                FB_Error_Log();
            }
        }

        private static void FB_Error_Log()
        {
            Analytics.Events.FirebaseEvent("Facebook", $"Firebase initialization failure\n App Id: {Application.identifier}");
        }

        private static void FB_Init(Action callback)
        {
            if (string.IsNullOrEmpty(FacebookSdkConfiguration.FacebookSDKData.appID) || string.IsNullOrEmpty(FacebookSdkConfiguration.FacebookSDKData.clientToken))
            {
                Debug.LogWarning("Facebook SDK configuration is missing");
                return;
            }
            
            FB.Init(FacebookSdkConfiguration.FacebookSDKData.appID,
                FacebookSdkConfiguration.FacebookSDKData.clientToken,
                true,
                true,
                true,
                false,
                true,
                null,
                "en_US",
                null,
                ()=> { callback?.Invoke(); });
        }
        
        private void InitParameters()
        {
            FB.Mobile.SetAdvertiserIDCollectionEnabled(true);
            FB.Mobile.SetAutoLogAppEventsEnabled(true);
            FB.Mobile.SetAdvertiserTrackingEnabled(ConsentData.ATE);
        }
        
        private void CheckActiveSDK()
        {
            if (string.IsNullOrEmpty(FacebookSdkConfiguration.FacebookSDKData.appID) || string.IsNullOrEmpty(FacebookSdkConfiguration.FacebookSDKData.clientToken))
            {
                Debug.LogWarning("Facebook SDK configuration is missing");
                return;
            }
            
            if (FB.IsInitialized) {
                    FB.ActivateApp();
                    
            } else {
                FB_Init( () => {

                    if (!FB.IsInitialized)
                    { 
                        FB_Error_Log();
                        return;
                    }

                    FB.ActivateApp();
                });
            }
            
        }
        
        private void OnApplicationPause(bool pause) { CheckActiveSDK(); }
        private void OnApplicationFocus(bool focus) { CheckActiveSDK(); }
    }
}