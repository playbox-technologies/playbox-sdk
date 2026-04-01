using System;
using System.Collections.Generic;
using Any.Scripts.Initializations;
using ConfigManager.Scripts.ConfigManagers;
using Playbox.Consent;
using UnityEngine;

namespace Playbox
{
    public static class PlayboxMain
    {
        private static List<BaseAnalyticsRegistrator> analyticsRegistrator = new();
        private static List<PlayboxBehaviour> _behaviours = new();
        
        private static AdAnalytics.AdAnalytics _adAnalytics = new();
        
        private static readonly HashSet<ServiceType> InitStatus = new();

        public static Action PostInitialization = delegate { };
        public static Action PreInitialization = delegate { };
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Start()
        {
            PB_ProxyClass.GetOrCreateMonoProxy();
            
            
            analyticsRegistrator.ForEach(a => a.Register());
            
            AddComponentsToInitialization();
            
            PostInitialization += () =>
            {
                Analytics.RegisterAnalyticsCustomManagement();
                
                _adAnalytics.Initialize();
                
            };
            
            PreInitialization?.Invoke();
            
            LLS.PlayerAsyncHelper.Delay(0.1f, Initialization);
        }

        public static bool IsValidate(ServiceType serviceType)
        {
            return InitStatus.Contains(serviceType);
        }

        public static void Initialization()
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
            
            ConsentData.ShowConsent(PB_ProxyClass.GetOrCreateMonoProxy(), () =>
            {
                foreach (var item in _behaviours)
                {
                    if (item != null && item.ConsentDependency)
                    {
                        item.Initialization();
                    }
                }
                
                LLS.PlayerAsyncHelper.Delay(0.1f, PostInitialization);
            });
            
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
            AddService<FirebaseInitialization>();
            AddService<DevToDevInitialization>(true);
            AddService<AppLovinInitialization>(true);
            AddService<AppsFlyerInitialization>(true);
            AddService<FacebookSdkInitialization>(true);
        }

        private static void AddService<T>(bool isWaitConsent = false) where T : PlayboxBehaviour, new()
        {
            PlayboxBehaviour service = new T();
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
