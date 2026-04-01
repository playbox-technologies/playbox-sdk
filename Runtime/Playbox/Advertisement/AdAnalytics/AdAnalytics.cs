using System;

namespace Playbox.AdAnalytics
{
    public class AdAnalytics :  IDisposable
    {
        public void Initialize()
        {
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnAdRevenuePaid;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnAdRevenuePaid;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnAdRevenuePaid;
            MaxSdkCallbacks.AppOpen.OnAdRevenuePaidEvent += OnAdRevenuePaid;
            MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += OnAdRevenuePaid;
        }

        private void OnAdRevenuePaid(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if(adInfo == null)
                return;
            
            PlayboxAdInfo info = new PlayboxAdInfo
            {
                AdUnitIdentifier = adInfo.AdUnitIdentifier,
                AdFormat = adInfo.AdFormat,
                NetworkName = adInfo.NetworkName,
                NetworkPlacement = adInfo.NetworkPlacement,
                Placement = adInfo.Placement,
                CreativeIdentifier = adInfo.CreativeIdentifier,
                Revenue = adInfo.Revenue,
                RevenuePrecision = adInfo.RevenuePrecision,
                LatencyMillis = adInfo.LatencyMillis,
                DspName = adInfo.DspName
            };

            info.CountryCode = MaxSdk.GetSdkConfiguration().CountryCode;
            
            Analytics.TrackAd(info);
        }

        public void Dispose()
        {
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent -= OnAdRevenuePaid;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent -= OnAdRevenuePaid;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent -= OnAdRevenuePaid;
            MaxSdkCallbacks.AppOpen.OnAdRevenuePaidEvent -= OnAdRevenuePaid;
            MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent -= OnAdRevenuePaid;
        }
    }
}