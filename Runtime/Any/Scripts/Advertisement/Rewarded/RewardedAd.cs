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
        private static void ShowAd(string placement) => MaxSdk.ShowRewardedAd(_unitId, placement);
        private static AwaitableCompletionSource<bool> _isLoadSource;

        public static void RegisterUnitID(string androidID, string iOSID)
        {
            _unitId = Application.platform switch
            {
                RuntimePlatform.Android => androidID,
                RuntimePlatform.IPhonePlayer => iOSID,
                _ => iOSID
            };
            
            Debug.Log(_unitId);
        }
        
        [Obsolete("This method is experimental and may change in future versions of the SDK.")]
        public static async Awaitable<bool> LoadAsync()
        {
            await Awaitable.MainThreadAsync();
            
            if (!IsInitialized)
                throw new Exception("Max SDK Ad is not initialized");
            
            if(!string.IsNullOrEmpty(_unitId))
                throw new Exception($"Rewarded Ad is not registered _unitId:{_unitId}");
            
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

        private static void OnAdLoadFailed(string arg1, MaxSdkBase.ErrorInfo arg2)
        {
            _isLoadSource.TrySetResult(true);
        }

        private static void OnAdLoaded(string arg1, MaxSdkBase.AdInfo arg2)
        {
            _isLoadSource.TrySetResult(false);
        }
    }
}

#endif