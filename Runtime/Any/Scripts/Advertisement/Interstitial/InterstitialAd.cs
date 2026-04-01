#if UNITY_6000_0_OR_NEWER
using System;
using UnityEngine;

namespace Playbox
{
    public class InterstitialAdEvents
    {
        public Action<string> OnClicked;
        public Action<string> OnDisplayed;
        public Action<string> OnDisplayedFailed;
        public Action<string> OnAdClosed;
    }
    
    public static class InterstitialAd
    {
        private static string _unitId;

        private static bool IsInitialized => MaxSdk.IsInitialized();
        private static void LoadAd() => MaxSdk.LoadInterstitial(_unitId);
        private static bool IsLoaded => MaxSdk.IsInterstitialReady(_unitId);

        public static InterstitialAdEvents AdEvents
        {
            get => _interstitialAdEvents;
            private set => _interstitialAdEvents = value;
        }

        private static void ShowAd(string placement) => MaxSdk.ShowInterstitial(_unitId, placement);
        private static AwaitableCompletionSource<bool> _isLoadSource;
        
        private static InterstitialAdEvents _interstitialAdEvents;

        public static void RegisterUnitID(string androidID, string iOSID)
        {
            _interstitialAdEvents = new InterstitialAdEvents();
            
            _unitId = Application.platform switch
            {
                RuntimePlatform.Android => androidID,
                RuntimePlatform.IPhonePlayer => iOSID,
                _ => iOSID
            };


            MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnClicked;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnDisplayed;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnDisplayFailed;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnAdClosed;

        }
        
        public static async Awaitable<bool> LoadAsync()
        {
            await Awaitable.MainThreadAsync();
            
            if (!IsInitialized)
                throw new Exception("Max SDK Ad is not initialized");
            
            if(string.IsNullOrEmpty(_unitId))
                throw new Exception($"Rewarded Ad is not registered");
            
            if(IsLoaded) return true;
            
            _isLoadSource = new AwaitableCompletionSource<bool>();

            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnAdLoaded;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnAdLoadFailed;
            
            LoadAd();

            bool result = await _isLoadSource.Awaitable;
            
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent -= OnAdLoaded;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent -= OnAdLoadFailed;
            
            return result;
        }
        
        public static async void Show(string placement = "default")
        {
            await Awaitable.MainThreadAsync();
           
            if(IsLoaded) 
                ShowAd(placement);
        }
        
        #region MasSDKCallback
        
        private static void OnAdLoadFailed(string id, MaxSdkBase.ErrorInfo arg2)
        {
            _isLoadSource.TrySetResult(true);
        }

        private static void OnAdLoaded(string id, MaxSdkBase.AdInfo arg2)
        {
            _isLoadSource.TrySetResult(false);
        }
        
        private static void OnAdClosed(string id, MaxSdkBase.AdInfo arg2)
        {
            AdEvents.OnAdClosed?.Invoke(id);
        }

        private static void OnDisplayFailed(string id, MaxSdkBase.ErrorInfo arg2, MaxSdkBase.AdInfo arg3) => AdEvents.OnDisplayedFailed?.Invoke(id);

        private static void OnDisplayed(string id, MaxSdkBase.AdInfo arg2)
        {
            AdEvents.OnDisplayed?.Invoke(id);
        }

        private static void OnClicked(string id, MaxSdkBase.AdInfo arg2)
        {
            AdEvents.OnClicked?.Invoke(id);
        }
        
        #endregion
    }
}

#endif