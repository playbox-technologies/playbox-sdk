using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Any.Scripts.Initializations;
using Playbox.Consent;
using Playbox.SdkConfigurations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Utils.Tools.Extentions;

namespace Playbox
{
    public class MainInitialization : PlayboxBehaviour
    {
        [SerializeField] private bool isAutoInitialize = true;
        [SerializeField] private bool isDebugSplash;
        [SerializeField] private UnityEvent OnPostInitializatioon;
        
        [SerializeField] private List<BaseAnalyticsRegistrator> analyticsRegistrator = new();
        
        [SerializeField]
        private List<PlayboxBehaviour> _behaviours = new();
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
                OnPostInitializatioon = null;
                
                Analytics.RegisterAnalyticsCustomManagement();
            };
            
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void Start()
        {
            if (!isAutoInitialize)
                return;
            
            PreInitialization?.Invoke();
            
            "AutoStarting Playbox SDK".PlayboxInfo();
            
            StartCoroutine(InitializationAfterFrame());

        }
        
        private IEnumerator InitializationAfterFrame()
        {
            yield return null;
            yield return new WaitForEndOfFrame();
            
            "Playbox SDK Init After Frame".PlayboxInfo();
            
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
                item?.GetInitStatus(() =>
                {
                    item.playboxName.PlayboxSplashLogUGUI();
                    InitStatus.Add(item.GetSerivceType());
                });
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
                
                Invoke(nameof(PostInit),1);
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
        
        private void PostInit() => PostInitialization?.Invoke();

        private void AddComponentsToInitialization()
        {
            _behaviours.Add(AddToGameObject<FirebaseInitialization>(gameObject));
            
            _behaviours.Add(AddToGameObject<DevToDevInitialization>(gameObject,true,true));
            _behaviours.Add(AddToGameObject<AppLovinInitialization>(gameObject,true,true));
            _behaviours.Add(AddToGameObject<AppsFlyerInitialization>(gameObject,true,true));
            _behaviours.Add(AddToGameObject<FacebookSdkInitialization>(gameObject,true,true));
        }

        private void OnDestroy()
        {
            foreach (var item in _behaviours)
            { 
                if(item != null)
                    item.Close();   
            }
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            var mainInit = FindFirstObjectByType<MainInitialization>();

            if (mainInit != this)
            {
                if(mainInit != null)
                    Destroy(mainInit.gameObject);
            }
        }
        
    }
}
