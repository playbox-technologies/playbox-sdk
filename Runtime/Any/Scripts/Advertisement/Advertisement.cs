﻿using System;
using System.Collections;
using CI.Utils.Extentions;
using UnityEngine;

namespace Playbox
{
    /// <summary>
    /// <param name="Ready">
    /// The commercials are loaded.
    /// </param>>
    /// <param name="NotReady">
    /// The ads are not loaded.
    /// </param>>
    /// <param name="NullStatus">
    /// The unitId of the advertisement is equal to Null.
    /// </param>>
    /// <param name="NotInitialized">
    /// MaxSdk is not initialized.
    /// </param>>
    /// </summary>
      public enum AdReadyStatus
        {
            Ready,
            NotReady,
            NullStatus,
            NotInitialized
        }
    /// <summary>
    /// Responsible for advertising, is static. To use it, it must be initialized and AppLovinInitialization must be thrown into it.
    /// </summary>
    public static class Advertisement
    {
        private static string unitId;
        /// <summary>
        /// Returns the status of the advertisement's readiness for display.
        /// </summary>
        public static bool isReady()
        {
            var ready = IsReadyStatus();
            return ready == AdReadyStatus.Ready;
        }
        /// <summary>
        /// Called when an advertisement is loaded.
        /// </summary>
        public static event Action OnLoaded;
        /// <summary>
        /// Called when advertising is not loaded.
        /// </summary>
        public static event Action<string> OnLoadedFailed;
        /// <summary>
        /// Called when an advertisement has been closed by a player.
        /// </summary>
        public static event Action<string> OnPlayerClosedAd;
        /// <summary>
        /// Called when an ad was clicked.
        /// </summary>
        public static event Action<string> OnPlayerOnClicked;
        /// <summary>
        /// Called when an advertisement has not loaded.
        /// </summary>
        public static Action<string,string> OnAdLoadFailedEvent;
        /// <summary>
        /// Called when an advertisement has been viewed.
        /// <param name=""></param>>
        /// </summary>
        public static Action<string,string> OnAdReceivedRewardEvent;
        /// <summary>
        /// Called when an advertisement has been closed by the user.
        /// </summary>
        public static Action<string,string> OnAdHiddenEvent;
        /// <summary>
        /// Called when an ad was clicked.
        /// </summary>
        public static Action<string> OnSdkInitializedEvent;
        
        /// <summary>
        /// Called when an advertisement is displayed.
        /// </summary>
        public static Action OnDisplay;
        /// <summary>
        /// Called when the advertisement failed to display.
        /// </summary>
        public static Action OnFailedDisplay;
        /// <summary>
        /// Called when an advertisement has been closed by the user.
        /// </summary>
        public static Action OnRewarderedClose;
        /// <summary>
        /// Called when an advertisement has been viewed.
        /// </summary>
        public static Action OnRewarderedReceived;
        public static Action<string> OnPlayerOpened;
        
        private static AppLovinInitialization appLovinInitialization;
        
        /// <summary>
        /// A method for initializing fields for ads, callbacks, and starting to load them.
        /// <param name="unitId">
        /// Ad token from AppLovin(Unique for each platform).
        /// </param>>
        /// <param name="aInitialization">
        /// AppLovin initialization script, required for basic services to work.
        /// </param>>
        /// </summary>
        public static void RegisterReward(string unitId, AppLovinInitialization aInitialization)
        {
            UnitId = unitId;
            appLovinInitialization = aInitialization;
            
            InitCallback();
            Load();

            aInitialization.StartCoroutine(rewardUpdate());
        }

        /// <summary>
        /// UnitId of the advertisement, retrieved from AppLovin.
        /// UnitId рекламы, достается из AppLovin.
        /// </summary>
        public static string UnitId
        {
            get => unitId;
            set => unitId = value;
        }
        
        /// <summary>
        /// Loading ads.
        /// </summary>
        public static void Load()
        {
            if(isReady())
                return;
            
            if (MaxSdk.IsInitialized())
                MaxSdk.LoadRewardedAd(UnitId);
        }
        /// <summary>
        /// Loading ads after a certain amount of time.
        /// Загрузка рекламы после определенного времени.
        /// <param name="delay">
        /// The time after which the advertisement will start loading.
        /// </param>>
        /// </summary>
        public static void Load(float delay)
        {
            if (appLovinInitialization)
            {
                appLovinInitialization.DelayInvoke(() => { Load(); }, delay);
            }
        }
        /// <summary>
        /// Starts showing ads if they are ready to be shown, otherwise they will try to load again.
        /// </summary>
        public static void Show()
        {
            if (isReady())
            {
                MaxSdk.ShowRewardedAd(unitId);    
            }
            else
            {
                Load();
            }
        }

        /// <summary>
        /// Returns the ready state of the advertisement.
        /// <inheritdoc cref="AdReadyStatus"/>>
        /// </summary>
        public static AdReadyStatus IsReadyStatus()
        {
            if (!MaxSdk.IsInitialized())
            {
                MaxSdk.InitializeSdk();
                return AdReadyStatus.NotInitialized;
            }

            if (string.IsNullOrEmpty(unitId))
            {
                return AdReadyStatus.NullStatus;
            }

            if (MaxSdk.IsRewardedAdReady(unitId))
            {
                return AdReadyStatus.Ready;
            }
            else
            {
                return AdReadyStatus.NotReady;
            }
        }

        static IEnumerator rewardUpdate()
        {
            while (true)
            {
                if(!isReady())
                    Load(0.3f);
                yield return new WaitForSecondsRealtime(0.5f);
            }
        }

        private static void InitCallback()
        {
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
        }
        
        private static void OnRewardedAdReceivedRewardEvent(string arg1, MaxSdkBase.Reward reward, MaxSdkBase.AdInfo info)
        {
            Analytics.TrackAd(info);
            OnRewarderedReceived?.Invoke();  
            OnAdReceivedRewardEvent?.Invoke(arg1, reward.ToString());
            
            Load();

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

        private static void OnRewardedAdFailedToDisplayEvent(string arg1, MaxSdkBase.ErrorInfo reward, MaxSdkBase.AdInfo info)
        {
            OnFailedDisplay?.Invoke();
            Load();
        }

        private static void OnRewardedAdHiddenEvent(string arg1, MaxSdkBase.AdInfo info)
        {
            OnRewarderedClose?.Invoke();
            OnPlayerClosedAd?.Invoke(UnitId);
            OnAdHiddenEvent?.Invoke(arg1, info.ToString());
            Load();
        }

        private static void OnRewardedAdClickedEvent(string arg1, MaxSdkBase.AdInfo info)
        {
            OnPlayerOnClicked?.Invoke(arg1);
        }

        private static void OnRewardedAdDisplayedEvent(string arg1, MaxSdkBase.AdInfo info)
        {
            OnDisplay?.Invoke();
            OnPlayerOpened?.Invoke(arg1);
        }

        private static void OnRewardedAdLoadFailedEvent(string arg1, MaxSdkBase.ErrorInfo info)
        {
            OnLoadedFailed?.Invoke(info.ToString().PlayboxInfoD(arg1));
            OnAdLoadFailedEvent?.Invoke(arg1, info.ToString());
            Load(1);
        }

        private static void OnRewardedAdLoadedEvent(string arg1, MaxSdkBase.AdInfo info)
        { 
            OnLoaded?.Invoke();
        }
    }
}