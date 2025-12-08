using System.Collections.Generic;
using CI.Utils.Extentions;

namespace Playbox
{
    public class AnalyticsCustomManagement
    {
        public List<IAnalyticsAdapter> Adapters = new();

        public void Initialize()
        {
            "Start Initializing Custom Managerment".PlayboxSplashLogUGUI();
            
            foreach (var adapter in Adapters)
            {
                adapter.Initialize();
            }
        }

        public void RegisterAdapter(IAnalyticsAdapter adapter)
        {
             Adapters.Add(adapter);    
        }

        public void UnregisterAdapter(IAnalyticsAdapter adapter)
        {
             Adapters.Remove(adapter);
        }

        public void TrackEvent(string eventName, IDictionary<string, string> properties = null)
        {
            foreach (var adapter in Adapters)
            {
                if (adapter.IsInitialized()) adapter.TrackEvent(eventName, properties);
            }
        }

        public void Log(string message)
        {
            foreach (var adapter in Adapters)
            {
                if (adapter.IsInitialized()) adapter.Log(message);
            }
        }


        public void TrackAd(PlayboxAdInfo impressionData)
        {
            foreach (var adapter in Adapters)
            {
                if (adapter.IsInitialized()) adapter.TrackAd(impressionData);
            }
        }

        public void LogLevelUp(int level)
        {
            foreach (var adapter in Adapters)
            {
                if (adapter.IsInitialized()) adapter.LogLevelUp(level);
            }
        }

        public void LogContentView(string content)
        {
            foreach (var adapter in Adapters)
            {
                if (adapter.IsInitialized()) adapter.LogContentView(content);
            }
        }

        public void LogTutorial(string tutorial, ETutorialState stateLevel = ETutorialState.Complete,
            string step = "none")
        {
            foreach (var adapter in Adapters)
            {
                if (adapter.IsInitialized()) adapter.LogTutorial(tutorial, stateLevel, step);
            }
        }

        public void TutorialCompleted()
        {
            foreach (var adapter in Adapters)
            {
                if (adapter.IsInitialized()) adapter.TutorialCompleted();
            }
        }

        public void AdToCart(int count)
        {
            foreach (var adapter in Adapters)
            {
                if (adapter.IsInitialized()) adapter.AdToCart(count);
            }
        }

        public void AdRewardCount(int count)
        {
            foreach (var adapter in Adapters)
            {
                if (adapter.IsInitialized()) adapter.AdRewardCount(count);
            }
        }

        public void CurrentBalance(Dictionary<string, long> balance)
        {
            foreach (var adapter in Adapters)
            {
                if (adapter.IsInitialized()) adapter.CurrentBalance(balance);
            }
        }

        public void RealCurrencyPayment(string orderId, double price, string productId, string currencyCode)
        {
            foreach (var adapter in Adapters)
            {
                if (adapter.IsInitialized()) adapter.RealCurrencyPayment(orderId, price, productId, currencyCode);
            }
        }

        public void VirtualCurrencyPayment(string purchaseId, string purchaseType, int purchaseAmount,
            Dictionary<string, int> resources)
        {
            foreach (var adapter in Adapters)
            {
                if (adapter.IsInitialized()) adapter.VirtualCurrencyPayment(purchaseId, purchaseType, purchaseAmount, resources);
            }
        }

        public void AdImpression(string network, double revenue, string placement, string unit)
        {
            foreach (var adapter in Adapters)
            {
                if (adapter.IsInitialized()) adapter.AdImpression(network, revenue, placement, unit);
            }
        }

        public void Tutorial(int step)
        {
            foreach (var adapter in Adapters)
            {
                if (adapter.IsInitialized()) adapter.Tutorial(step);
            }
        }

        public void StartProgressionEvent(string eventName)
        {
            foreach (var adapter in Adapters)
            {
                if (adapter.IsInitialized()) adapter.StartProgressionEvent(eventName);
            }
        }

        public void FinishProgressionEvent(string eventName)
        {
            foreach (var adapter in Adapters)
            {
                if (adapter.IsInitialized()) adapter.FinishProgressionEvent(eventName);
            }
        }

        public void TrackProductRevenue(ProductDataAdapter productDataAdapter)
        {
            foreach (var adapter in Adapters)
            {
                if (adapter.IsInitialized()) adapter.TrackProductRevenue(productDataAdapter);
            }
        }
    }
}