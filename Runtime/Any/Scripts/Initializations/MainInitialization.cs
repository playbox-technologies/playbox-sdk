using System;
using System.Collections;
using System.Collections.Generic;
using Any.Scripts.Initializations;
using ConfigManager.Scripts.ConfigManagers;
using Playbox.Consent;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Playbox
{
    public class MainInitialization : PlayboxBehaviour
    {
        [SerializeField] private bool isAutoInitialize = true;
        [SerializeField] private bool isDebugSplash;
        [SerializeField] private UnityEvent OnPostInitializatioon;
        
        [SerializeField] private List<BaseAnalyticsRegistrator> analyticsRegistrator = new();
        [SerializeField] private List<PlayboxBehaviour> _behaviours = new();
        
        private AdAnalytics.AdAnalytics _adAnalytics = new();
        
        private static readonly HashSet<ServiceType> InitStatus = new();

        public static Action PostInitialization = delegate { };
        public static Action PreInitialization = delegate { };
        
        private void Awake()
        {
            analyticsRegistrator.ForEach(a => a.Register());

            var splasher = AddToGameObject<PlayboxSplashUGUILogger>(gameObject, isDebugSplash);
            
            if(isDebugSplash)
                splasher.Initialization();
            
            
            AddComponentsToInitialization();
            
            PostInitialization += () =>
            {
                OnPostInitializatioon?.Invoke();
                
                Analytics.RegisterAnalyticsCustomManagement();
                
                _adAnalytics.Initialize();
                
            };
            
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void Start()
        {
            if (!isAutoInitialize)
                return;
            
            PreInitialization?.Invoke();
            
            StartCoroutine(InitializationAfterFrame());

        }
        
        private IEnumerator InitializationAfterFrame()
        {
            yield return null;
            yield return new WaitForEndOfFrame();
            
            Initialization();
            
        }

        public static bool IsValidate(ServiceType serviceType)
        {
            return InitStatus.Contains(serviceType);
        }

        public override void Initialization()
        {
            GlobalPlayboxConfig.Load();
            
            
            if(Application.isPlaying)
                DontDestroyOnLoad(gameObject);
            
            foreach (var item in _behaviours)
            {
                if (item == null) continue;
                
                var currentItem = item;
                ServiceType st = currentItem.GetServiceType();
                
                Debug.Log(st);
                
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
            
            ConsentData.ShowConsent(this, () =>
            {
                foreach (var item in _behaviours)
                {
                    if (item != null && item.ConsentDependency)
                    {
                        item.Initialization();
                    }
                }
                
                Debug.Log("Start Post Initialization");
                
                StartCoroutine(PostInit());
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

        private IEnumerator PostInit()
        {
            yield return new WaitForSeconds(1);

            PostInitialization?.Invoke();
        }

        private void AddComponentsToInitialization()
        {
            AddService<FirebaseInitialization>();
            AddService<DevToDevInitialization>(true);
            AddService<AppLovinInitialization>(true);
            AddService<AppsFlyerInitialization>(true);
            AddService<FacebookSdkInitialization>(true);
        }

        private void AddService<T>(bool isWaitConsent = false) where T : PlayboxBehaviour
        {
            _behaviours.Add(AddToGameObject<T>(gameObject, isWaitConsent));
        }

        private void OnDestroy()
        {
            _adAnalytics.Dispose();
            
            foreach (var item in _behaviours)
            { 
                if(item != null)
                    item.Close();   
            }
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            var initializators = FindObjectsByType<MainInitialization>(FindObjectsInactive.Exclude,FindObjectsSortMode.None);

            foreach (var item in initializators)
            {
                if(item != this && item != null)
                    Destroy(item.gameObject);
            }
        }
        
    }
}
