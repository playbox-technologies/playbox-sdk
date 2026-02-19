using System.Collections.Generic;
using Utils.Tools.Extentions;

namespace Playbox
{
    public class AnalyticsCustomManagement
    {
        private List<IAnalyticsAdapter> _adapters = new();

        public void Initialize()
        {
            "Start Initializing Custom Managerment".PlayboxSplashLogUGUI();
            
            foreach (var adapter in _adapters)
            {
                adapter.Initialize();
            }
        }

        public void RegisterAdapter(IAnalyticsAdapter adapter)
        {
             _adapters.Add(adapter);

             adapter.GetType().ToString().PbInfo();
        }

        public void UnregisterAdapter(IAnalyticsAdapter adapter)
        {
             _adapters.Remove(adapter);
        }

        public void TrackEvent(string eventName, IDictionary<string, string> properties = null)
        {
            foreach (var adapter in _adapters)
            {
                if (adapter.Initialized) adapter.TrackEvent(eventName, properties);
            }
        }

        public void Log(string message)
        {
            foreach (var adapter in _adapters)
            {
                if (adapter.Initialized) adapter.Log(message);
            }
        }


        public void TrackAd(PlayboxAdInfo impressionData)
        {
            foreach (var adapter in _adapters)
            {
                if (adapter.Initialized) adapter.TrackAd(impressionData);
            }
        }

        public void LogLevelUp(int level)
        {
            foreach (var adapter in _adapters)
            {
                if (adapter.Initialized) adapter.LogLevelUp(level);
            }
        }

        public void AdToCart(int count)
        {
            foreach (var adapter in _adapters)
            {
                if (adapter.Initialized) adapter.AdToCart(count);
            }
        }

        public void AdRewardCount(int count)
        {
            foreach (var adapter in _adapters)
            {
                if (adapter.Initialized) adapter.AdRewardCount(count);
            }
        }

        public void CurrentBalance(Dictionary<string, long> balance)
        {
            foreach (var adapter in _adapters)
            {
                if (adapter.Initialized) adapter.CurrentBalance(balance);
            }
        }

        public void RealCurrencyPayment(string orderId, double price, string productId, string currencyCode)
        {
            foreach (var adapter in _adapters)
            {
                if (adapter.Initialized) adapter.RealCurrencyPayment(orderId, price, productId, currencyCode);
            }
        }

        public void VirtualCurrencyPayment(string purchaseId, string purchaseType, int purchaseAmount,
            Dictionary<string, int> resources)
        {
            foreach (var adapter in _adapters)
            {
                if (adapter.Initialized) adapter.VirtualCurrencyPayment(purchaseId, purchaseType, purchaseAmount, resources);
            }
        }

        public void Tutorial(int step)
        {
            foreach (var adapter in _adapters)
            {
                if (adapter.Initialized) adapter.Tutorial(step);
            }
        }

        public void TrackProductRevenue(ProductDataAdapter productDataAdapter)
        {
            foreach (var adapter in _adapters)
            {
                if (adapter.Initialized) adapter.TrackProductRevenue(productDataAdapter);
            }
        }
    }
}