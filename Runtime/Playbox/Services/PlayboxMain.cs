using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConfigManager.Scripts.ConfigManagers;
using Playbox.Consent;
using UnityEngine;

namespace Playbox.Services
{
    public static class PlayboxMain
    {
        private static readonly List<BaseAnalyticsRegistrator> AnalyticsRegistrator = new();
        private static List<PlayboxService> _behaviours = new();
        
        private static AdAnalytics.AdAnalytics _adAnalytics = new();
        
        private static readonly HashSet<ServiceType> InitStatus = new();

        public static Action PostInitialization = delegate { };
        public static Action PreInitialization = delegate { };
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private async static void Start()
        {
            PbProxyClass.GetOrCreateMonoProxy();
            
            
            AnalyticsRegistrator.ForEach(a => a.Register());
            
            AddComponentsToInitialization();
            
            PostInitialization += () =>
            {
                Analytics.RegisterAnalyticsCustomManagement();
                
                _adAnalytics.Initialize();
                
            };
            
            PreInitialization?.Invoke();
            
            await Initialization();
        }

        public static bool IsValidate(ServiceType serviceType)
        {
            return InitStatus.Contains(serviceType);
        }

        public async static Task Initialization()
        {
            GlobalPlayboxConfig.Load();
            
            foreach (var item in _behaviours)
            {
                if (item == null) continue;
                
                var currentItem = item;
                ServiceType st = currentItem.GetServiceType();
                
                if (!InitStatus.Contains(st))
                {
                    currentItem.GetInitStatus(() =>
                    {
                        lock (InitStatus) 
                        {
                            if (!InitStatus.Contains(st))
                            {
                                InitStatus.Add(st);
                            }
                        }
                    });   
                }
            }
            
            bool isConsent = false;
            
            ConsentData.ShowConsent(PbProxyClass.GetOrCreateMonoProxy(), () => isConsent = true);
            
            await LLS.PlayerAsyncHelper.WaitUntil(() => isConsent);

            if (isConsent)
            {
                foreach (var item in _behaviours)
                {
                    if (item != null && item.ConsentDependency)
                    {
                        item.Initialization();
                    }
                }
                
                LLS.PlayerAsyncHelper.Delay(0.1f, PostInitialization);
            }
            
            foreach (var item in _behaviours)
            {
                if (item != null)
                {
                    if (!item.ConsentDependency)
                    {
                        item.Initialization();
                    }
                }
            }
        }
        

        private static void AddComponentsToInitialization()
        {
            AddService<FirebaseService>();
            AddService<DevToDevService>(true);
            AddService<AppLovinService>(true);
            AddService<AppsFlyerService>(true);
            AddService<FacebookSdkService>(true);
        }

        private static void AddService<T>(bool isWaitConsent = false) where T : PlayboxService, new()
        {
            PlayboxService service = new T();
            service.ConsentDependency = isWaitConsent;
            
            _behaviours.Add(service);
        }

        private static void OnDestroy()
        {
            _adAnalytics.Dispose();
            
            foreach (var item in _behaviours)
            { 
                if(item != null)
                    item.Close();   
            }
        }
    }
}
