using System;
using System.Collections;
using Playbox.SdkConfigurations;
using UnityEngine;
using Utils.Tools.Extentions;

namespace Playbox
{
    /// <summary>
    /// Responsible for advertising, is static. To use it, it must be initialized and AppLovinInitialization must be thrown into it.
    /// </summary>
    public static class InterstitialAd
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
        public static void RegisterUnitID(string unitId, AppLovinInitialization aInitialization)
        {
            UnitId = unitId;
            appLovinInitialization = aInitialization;
            
            InitCallback();
            Load();

            aInitialization.StartCoroutine(InterstitialUpdate());
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
                MaxSdk.LoadInterstitial(UnitId);
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
        public static void Show(string placement = "default")
        {
            if (isReady())
            {
                MaxSdk.ShowInterstitial(unitId, placement);    
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

            if (MaxSdk.IsInterstitialReady(unitId))
            {
                return AdReadyStatus.Ready;
            }
            else
            {
                return AdReadyStatus.NotReady;
            }
        }

        static IEnumerator InterstitialUpdate()
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
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialAdLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialAdLoadFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialAdDisplayedEvent;
            MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialAdClickedEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialAdHiddenEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnAdRevenuePaid;
        }

        private static void OnAdRevenuePaid(string id, MaxSdkBase.AdInfo info)
        {
            Analytics.TrackAd(new PlayboxAdInfo
            {
                AdUnitIdentifier = info.AdUnitIdentifier,
                AdFormat = info.AdFormat,
                NetworkName = info.NetworkName,
                NetworkPlacement = info.NetworkPlacement,
                Placement = info.Placement,
                CreativeIdentifier = info.CreativeIdentifier,
                Revenue = info.Revenue,
                RevenuePrecision = info.RevenuePrecision,
                LatencyMillis = info.LatencyMillis,
                DspName = info.DspName
            });
        }

        private static void OnInterstitialAdFailedToDisplayEvent(string arg1, MaxSdkBase.ErrorInfo reward, MaxSdkBase.AdInfo info)
        {
            OnFailedDisplay?.Invoke();
            Load();
        }

        private static void OnInterstitialAdHiddenEvent(string arg1, MaxSdkBase.AdInfo info)
        {
            OnPlayerClosedAd?.Invoke(UnitId);
            OnAdHiddenEvent?.Invoke(arg1, info.ToString());
            Load();
        }

        private static void OnInterstitialAdClickedEvent(string arg1, MaxSdkBase.AdInfo info)
        {
            OnPlayerOnClicked?.Invoke(arg1);
        }

        private static void OnInterstitialAdDisplayedEvent(string arg1, MaxSdkBase.AdInfo info)
        {
            OnDisplay?.Invoke();
            OnPlayerOpened?.Invoke(arg1);
        }

        private static void OnInterstitialAdLoadFailedEvent(string arg1, MaxSdkBase.ErrorInfo info)
        {
            OnLoadedFailed?.Invoke(info.ToString().PlayboxInfoD(arg1));
            OnAdLoadFailedEvent?.Invoke(arg1, info.ToString());
            Load(1);
        }

        private static void OnInterstitialAdLoadedEvent(string arg1, MaxSdkBase.AdInfo info)
        { 
            OnLoaded?.Invoke();
        }
    }
}