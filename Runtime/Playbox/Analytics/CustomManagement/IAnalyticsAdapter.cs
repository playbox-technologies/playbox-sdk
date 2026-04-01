using System;
using System.Collections.Generic;

namespace Playbox
{
    public interface IAnalyticsAdapter
    {
        public bool Initialized => IsInitialized();
        
        public void Initialize();
        public void SubscribeToInitializeEvent(Action initializeEvent = null);
        public bool IsInitialized();
        
        public void TrackEvent(string eventName, IDictionary<string, string> properties = null);
        public void Log(string message);
        public void TrackAd(PlayboxAdInfo impressionData);
        public void LogLevelUp(int level);
        public void AdToCart(int count);
        public void AdRewardCount(int count);
        public void CurrentBalance(Dictionary<string, long> balance);
        public void RealCurrencyPayment(string orderId, double price, string productId, string currencyCode);
        public void VirtualCurrencyPayment(string purchaseId, string purchaseType, int purchaseAmount,
            Dictionary<string, int> resources);
        public void Tutorial(int step);
        public void TrackProductRevenue(ProductDataAdapter productDataAdapter);
    }
}