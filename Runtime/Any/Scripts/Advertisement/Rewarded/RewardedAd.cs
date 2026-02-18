#if UNITY_6000_0_OR_NEWER
using System;
using UnityEngine;

namespace Playbox
{
    public static class RewardedAd
    {
        private static string _unitId;

        private static bool IsInitialized => MaxSdk.IsInitialized();
        private static void LoadAd() => MaxSdk.LoadRewardedAd(_unitId);
        private static bool IsLoaded => MaxSdk.IsRewardedAdReady(_unitId);

        public static RewardedAdEvents AdEvents
        {
            get => _rewardedAdEvents;
            private set => _rewardedAdEvents = value;
        }

        private static void ShowAd(string placement) => MaxSdk.ShowRewardedAd(_unitId, placement);
        private static AwaitableCompletionSource<bool> _isLoadSource;
        
        private static RewardedAdEvents _rewardedAdEvents;

        public static void RegisterUnitID(string androidID, string iOSID)
        {
            _rewardedAdEvents = new RewardedAdEvents();
            
            _unitId = Application.platform switch
            {
                RuntimePlatform.Android => androidID,
                RuntimePlatform.IPhonePlayer => iOSID,
                _ => iOSID
            };


            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnClicked;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnPayedReward;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnDisplayed;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnDisplayFailed;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnAdClosed;

        }
        

        [Obsolete("This method is experimental and may change in future versions of the SDK.")]
        public static async Awaitable<bool> LoadAsync()
        {
            await Awaitable.MainThreadAsync();
            
            if (!IsInitialized)
                throw new Exception("Max SDK Ad is not initialized");
            
            if(string.IsNullOrEmpty(_unitId))
                throw new Exception($"Rewarded Ad is not registered");
            
            if(IsLoaded) return true;
            
            _isLoadSource = new AwaitableCompletionSource<bool>();

            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnAdLoaded;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnAdLoadFailed;
            
            LoadAd();

            bool result = await _isLoadSource.Awaitable;
            
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent -= OnAdLoaded;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent -= OnAdLoadFailed;
            
            return result;

        }
        
        [Obsolete("This method is experimental and may change in future versions of the SDK.")]
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

        private static void OnPayedReward(string id, MaxSdkBase.Reward arg2, MaxSdkBase.AdInfo arg3)
        {
            AdEvents.OnReceivedReward?.Invoke(id, arg2);
            
            const string adImpressionsCount = "ad_impressions_count";

            if (PlayerPrefs.HasKey(adImpressionsCount))
            {
                int adImpressions = PlayerPrefs.GetInt(adImpressionsCount, 0);
                
                Analytics.Events.AdRewardCount(adImpressions);
                
                var division = Math.DivRem(adImpressions,30,out var remainder);
                
                if (division > 0 && remainder == 0)
                {
                    Analytics.Events.AdToCart(adImpressions);
                }
            }
            else
            {
                PlayerPrefs.SetInt(adImpressionsCount, 1);
            }
        }

        private static void OnClicked(string id, MaxSdkBase.AdInfo arg2)
        {
            AdEvents.OnClicked?.Invoke(id);
        }
        
        #endregion
    }
}

#endif