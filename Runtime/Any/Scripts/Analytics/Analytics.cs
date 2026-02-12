#define PLAYBOX_SDK

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Any.Scripts.Backend.Verificator;
using Any.Scripts.Initializations;
using AppsFlyerSDK;
using DevToDev.Analytics;
using Firebase.Analytics;
using Firebase.Crashlytics;
using Utils.Tools.Extentions;
using Debug = UnityEngine.Debug;

/*
    af_initiated_checkout - инициация покупки
    af_level_achieved - поднятие уровня
    af_purchase - совершил покупку
    af_tutorial_completion - прошел туториал
    
    af_add_to_cart - 30 просмотров рекламы
    ad_reward - отправляет колличество просмотров рекламы
 */

namespace Playbox
{
    /// <summary>
    /// Clavicular static analytics collection class
    /// </summary>
    public static class Analytics
    {
        public static bool IsAppsFlyerInit => IsValidate(ServiceType.AppsFlyer);
        public static bool IsAppLovinInit => IsValidate(ServiceType.AppLovin);
        public static bool IsDtdInit => IsValidate(ServiceType.DevToDev);
        public static bool IsFsbInit => IsValidate(ServiceType.Facebook);
        public static bool IsFirebaseInit => IsValidate(ServiceType.Firebase);
        
        public static readonly AnalyticsCustomManagement AnalyticsCustomManagement = new();

        public static void RegisterAnalyticsCustomManagement()
        {
            AnalyticsCustomManagement.Initialize();
        }

        private static bool IsValidate(ServiceType serviceType) => MainInitialization.IsValidate(serviceType);
       
        
        /// <summary>
        /// Sends a custom event to AppsFlyer
        /// </summary>
        public static void SendAppsFlyerEvent(string eventName,string parameter_name, int value)
        {
            var dict = new Dictionary<string, string>();
            
            dict.Add(parameter_name, value.ToString());
            
            if (IsAppsFlyerInit)
                AppsFlyer.sendEvent(eventName, dict);
        }

        /// <summary>
        /// Commits a custom event to DTD
        /// </summary>
        public static void TrackEvent(string eventName, List<KeyValuePair<string,string>> arguments)
        {
            
            if(IsDtdInit)
                    DTDAnalytics.CustomEvent(eventName, arguments.ToCustomParameters());
            
            //if (isFirebaseInit)
            //        FirebaseAnalytics.LogEvent(eventName,new Parameter(eventName,JsonUtility.ToJson(arguments)));
            
            AnalyticsCustomManagement.TrackEvent(eventName, arguments.ToDictionary(x => x.Key, x => x.Value));
        }

        public static void TrackEvent(string eventName, KeyValuePair<string,string> eventPair)
        {
            var arguments = new Dictionary<string,string>();
            arguments.Add(eventPair.Key, eventPair.Value);
            
            if(IsDtdInit)
                DTDAnalytics.CustomEvent(eventName, arguments.ToList().ToCustomParameters());
            
            AnalyticsCustomManagement.TrackEvent(eventName, arguments);
        }

        public static void TrackEvent(string eventName)
        {
            if (IsFirebaseInit)
                FirebaseAnalytics.LogEvent(eventName);
            
            if (IsDtdInit)
                DTDAnalytics.CustomEvent(eventName);
            
            AnalyticsCustomManagement.TrackEvent(eventName);
        }

        public static void LogPurshaseInitiation(ProductDataAdapter product)
        {
            if(product == null)
                throw new Exception("Product is null");
            
            TrackEvent("purchasing_init",new KeyValuePair<string, string>("purchasing_init",product.DefinitionId));
            
            if (IsAppsFlyerInit)
                AppsFlyer.sendEvent("af_initiated_checkout",new());
            
        }
        
        public static void LogPurchase(ProductDataAdapter
            purchasedProduct, Action<bool> onValidate  = null)
        {
            if(purchasedProduct == null)
            {
                if(IsFirebaseInit)
                    Crashlytics.LogException(new Exception("[PlayboxLogging] purchasedProduct is null"));
                return;
            }

            string orderId = purchasedProduct.TransactionId;
            string productId = purchasedProduct.DefinitionId;
            var price = purchasedProduct.MetadataLocalizedPrice;
            string currency = purchasedProduct.MetadataIsoCurrencyCode;
            
            
            Dictionary<string, string> eventValues = new ()
            {
                { "af_currency", currency },
                { "af_revenue", price.ToString(CultureInfo.InvariantCulture) },
                { "af_quantity", "1" },
                { "af_content_id", productId }
            };
            
            PurchaseValidator.Validate(purchasedProduct, (isValid, returnProductData) =>
            {
               onValidate?.Invoke(isValid);
                
               Debug.Log($@"Purchase Request {new ProductDataAdapter()
               {
                   MetadataIsoCurrencyCode = returnProductData.MetadataIsoCurrencyCode,
                   MetadataLocalizedPrice = returnProductData.MetadataLocalizedPrice,
                   Receipt = purchasedProduct.Receipt,
                   TransactionId = purchasedProduct.TransactionId,
                   DefinitionId = purchasedProduct.DefinitionId
               }}");
               
                if (isValid)
                {
                    Events.RealCurrencyPayment(orderId, (double)price, productId, currency);
                    Events.AppsFlyerPayment(eventValues);

                    string affiliation = "default";
                    
#if UNITY_IOS
                    affiliation = "App Store";
#endif

#if UNITY_ANDROID
                    affiliation = "Google Play Store";
#endif
                    
                    FirebaseAnalytics.LogEvent(
                        FirebaseAnalytics.EventPurchase,
                        new Parameter[]{
                            new (FirebaseAnalytics.ParameterTransactionID, orderId),
                            new (FirebaseAnalytics.ParameterAffiliation, affiliation),
                            new (FirebaseAnalytics.ParameterValue, (double)price),
                            new (FirebaseAnalytics.ParameterPrice, (double)price),
                            new (FirebaseAnalytics.ParameterCurrency, currency)
                        }
                    );
                    
                    AnalyticsCustomManagement.TrackProductRevenue(new ProductDataAdapter()
                    {
                        DefinitionId = productId,
                        Receipt = purchasedProduct.Receipt,
                        TransactionId = orderId,
                        MetadataLocalizedPrice = returnProductData.MetadataLocalizedPrice,
                        MetadataIsoCurrencyCode = returnProductData.MetadataIsoCurrencyCode
                    });
                    
                }
            });
            
        }

        public static void TrackAd(PlayboxAdInfo impressionData)
        {
            Events.AdImpression(impressionData.NetworkName, impressionData.Revenue, impressionData.Placement, impressionData.AdUnitIdentifier);

            if (IsFirebaseInit)
            {
                double revenue = impressionData.Revenue;
                var impressionParameters = new[]
                {
                    new Parameter("ad_platform", "AppLovin"),
                    new Parameter("ad_source", impressionData.NetworkName),
                    new Parameter("ad_unit_name", impressionData.AdUnitIdentifier),
                    new Parameter("ad_format", impressionData.AdFormat),
                    new Parameter("value", revenue),
                    new Parameter("currency", "USD")
                };
                FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAdImpression, impressionParameters);
                
            }
            
            AnalyticsCustomManagement.TrackAd(impressionData);
        }
        
        public static class Events
        {
            public static void LogLevelUp(int level)
            {
                if (IsDtdInit)
                    DTDAnalytics.LevelUp(level);
            
                SendAppsFlyerEvent("af_level_achieved","level",level);
                AnalyticsCustomManagement.LogLevelUp(level);
            }
            
            /// <summary>
            /// Sends the event if the tutorial is completed
            /// </summary>
            public static void TutorialCompleted()
            {
                if (IsAppsFlyerInit)
                    AppsFlyer.sendEvent("af_tutorial_completion", new());
            }
            
            /// <summary>
            /// Is sent every 30 ad views
            /// </summary>
            public static void AdToCart(int count) // more than 30 ad impressions
            {
                SendAppsFlyerEvent("af_add_to_cart","count", count);
                AnalyticsCustomManagement.AdToCart(count);
            }
            
            /// <summary>
            /// Sends the number of video ad views
            /// </summary>
            /// 
            public static void AdRewardCount(int count) // ad views
            {
                SendAppsFlyerEvent("ad_reward","count", count);
                AnalyticsCustomManagement.AdRewardCount(count);
            }

            /// <summary>
            /// Logs the player's current balance for all virtual currencies to DevToDev.
            /// </summary>
            public static void CurrentBalance(Dictionary<string, long> balance)
            {
                if (IsDtdInit) DTDAnalytics.CurrentBalance(balance);
                AnalyticsCustomManagement.CurrentBalance(balance);
            }
            
            /// <summary>
            /// Logs an accrual (earning) of virtual currency to DevToDev.
            /// </summary>
            public static void CurrencyAccrual(string currencyName, int currencyAmount, string source,
                DTDAccrualType type)
            {
                if (IsDtdInit) DTDAnalytics.CurrencyAccrual(currencyName, currencyAmount, source, type);
            }
            /// <summary>
            /// Logs a real-money purchase transaction to DevToDev.
            /// </summary>
            public static void RealCurrencyPayment(string orderId, double price, string productId, string currencyCode)
            {
                if (IsDtdInit) DTDAnalytics.RealCurrencyPayment(orderId, price, productId, currencyCode);
                AnalyticsCustomManagement.RealCurrencyPayment(orderId, price, productId, currencyCode);
            }
            /// <summary>
            /// Logs a virtual currency purchase transaction to DevToDev.
            /// </summary>
            public static void VirtualCurrencyPayment(string purchaseId, string purchaseType, int purchaseAmount,
                Dictionary<string, int> resources)
            {
                if (IsDtdInit) DTDAnalytics.VirtualCurrencyPayment(purchaseId, purchaseType, purchaseAmount, resources);
                AnalyticsCustomManagement.VirtualCurrencyPayment(purchaseId, purchaseType, purchaseAmount, resources);
            }
            /// <summary>
            /// Logs an ad impression event with revenue details to DevToDev.
            /// </summary>
            public static void AdImpression(string network, double revenue, string placement, string unit)
            {
                if (IsDtdInit) DTDAnalytics.AdImpression(network, revenue, placement, unit);
            }
            /// <summary>
            /// Tracks a tutorial step completion in DevToDev.
            /// </summary>
            public static void Tutorial(int step)
            {
                if (IsDtdInit) DTDAnalytics.Tutorial(step);
            }
            /// <summary>
            /// Logs a successful social network connection event to DevToDev.
            /// </summary>
            public static void SocialNetworkConnect(DTDSocialNetwork socialNetwork)
            {
                if (IsDtdInit) DTDAnalytics.SocialNetworkConnect(socialNetwork);
            }
            /// <summary>
            /// Logs a social network post event to DevToDev.
            /// </summary>
            public static void SocialNetworkPost(DTDSocialNetwork socialNetwork, string reason)
            {
                if (IsDtdInit) DTDAnalytics.SocialNetworkPost(socialNetwork, reason);
            }
            /// <summary>
            /// Logs referral information (source, campaign, etc.) to DevToDev.
            /// </summary>
            public static void Referrer(Dictionary<DTDReferralProperty, string> referrer)
            {
                if (IsDtdInit) DTDAnalytics.Referrer(referrer);
            }
            /// <summary>
            /// Sends a purchase event with provided values to AppsFlyer.
            /// </summary>
            public static void AppsFlyerPayment(Dictionary<string,string> appsFlyerPaymentValues)
            {
                if (IsAppsFlyerInit) AppsFlyer.sendEvent("af_purchase", appsFlyerPaymentValues);
            }
            /// <summary>
            /// Starts tracking a progression event in DevToDev.
            /// </summary>
            public static void StartProgressionEvent(string eventName)
            {
                if (IsDtdInit) DTDAnalytics.StartProgressionEvent(eventName);
            }
            /// <summary>
            /// Starts tracking a progression event with custom parameters in DevToDev.
            /// </summary>
            public static void StartProgressionEvent(string eventName, DTDStartProgressionEventParameters parameters)
            {
                if (IsDtdInit) DTDAnalytics.StartProgressionEvent(eventName, parameters);
            }
            /// <summary>
            /// Marks a progression event as finished in DevToDev.
            /// </summary>
            public static void FinishProgressionEvent(string eventName)
            {
                if (IsDtdInit) DTDAnalytics.FinishProgressionEvent(eventName);
            }
            /// <summary>
            /// Marks a progression event as finished with custom parameters in DevToDev.
            /// </summary>
            public static void FinishProgressionEvent(string eventName, DTDFinishProgressionEventParameters parameters)
            {
                if (IsDtdInit) DTDAnalytics.FinishProgressionEvent(eventName, parameters);
            }
            /// <summary>
            /// Sends a custom log message to Firebase Crashlytics.
            /// </summary>
            public static void CrashlyticsLog(string eventName, string message)
            {
                if (IsFirebaseInit)
                {
                    Crashlytics.Log($"{eventName} : {message}");
                }
            }
            
            public static void FirebaseEvent(string eventName, string message)
            {
                if (IsFirebaseInit)
                {
                    FirebaseAnalytics.LogEvent($"{eventName} : {message}");
                }
            }

            public static void TrackProductRevenue(ProductDataAdapter productDataAdapter)
            {
                AnalyticsCustomManagement.TrackProductRevenue(productDataAdapter);
            }
        }
    }
}