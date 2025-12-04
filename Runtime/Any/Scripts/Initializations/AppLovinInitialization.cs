#pragma warning disable CS0618

using System.Collections;
using Playbox.Consent;
using Playbox.SdkConfigurations;
using UnityEngine;

namespace Playbox
{
    public class AppLovinInitialization : PlayboxBehaviour
    {

        public override void Initialization()
        {
            base.Initialization();
            
            if (ConsentData.IsChildUser)
                return;
            
            AppLovinConfiguration.LoadJsonConfig();
            
            if(!AppLovinConfiguration.Active)
                return;

            MaxSdkCallbacks.OnSdkInitializedEvent += OnSdkInitializedEvent;
            
            MaxSdk.SetHasUserConsent(ConsentData.HasUserConsent);
            
            MaxSdk.SetSdkKey(AppLovinConfiguration.AdvertisementSdk);
        
            MaxSdk.InitializeSdk();

            StartCoroutine(initUpd());

        }

        public override void Close()
        {
            base.Close();
            MaxSdkCallbacks.OnSdkInitializedEvent -= OnSdkInitializedEvent;
        }

        private IEnumerator initUpd()
        {
            while (true)
            {
                if (MaxSdk.IsInitialized())
                {
                    ApproveInitialization();
                    yield break;
                }
                else
                {
                    MaxSdk.InitializeSdk();
                }

                yield return new WaitForSeconds(1f);
            }
        }

        private void OnSdkInitializedEvent(MaxSdkBase.SdkConfiguration sdkConfiguration)
        {
            
            if (RuntimePlatform.IPhonePlayer == Application.platform)
            {
                Advertisement.RegisterUnitID(AppLovinConfiguration.IOSKey_rew, this);
                InterstitialAd.RegisterUnitID(AppLovinConfiguration.IOSKey_inter, this);
                
                Debug.Log("AppLovin iPhone");
            }

            if (RuntimePlatform.Android == Application.platform)
            {
                Advertisement.RegisterUnitID(AppLovinConfiguration.AndroidKey_rew, this);
                InterstitialAd.RegisterUnitID(AppLovinConfiguration.AndroidKey_iter, this);
                
                Debug.Log("AppLovin Android");
            }

#if UNITY_EDITOR
            Advertisement.RegisterUnitID(AppLovinConfiguration.IOSKey_rew, this);
            InterstitialAd.RegisterUnitID(AppLovinConfiguration.IOSKey_inter, this);
                
            Debug.Log("AppLovin Test Unity iOS");
#endif
            
            Debug.Log("AppLovin initialized");
            
            Advertisement.OnSdkInitializedEvent?.Invoke(sdkConfiguration.ToString());
            InterstitialAd.OnSdkInitializedEvent?.Invoke(sdkConfiguration.ToString());
        }
        
    }
}
#pragma warning restore CS0618