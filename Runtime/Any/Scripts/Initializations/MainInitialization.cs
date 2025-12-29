using System;
using System.Collections.Generic;
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
        
        private List<PlayboxBehaviour> behaviours = new();
        
        private static Dictionary<string,bool> initStatus = new();

        public static Dictionary<string, bool> InitStatus
        {
            get => initStatus ??= new Dictionary<string, bool>();
            set => initStatus = value;
        }

        public static Action PostInitialization = delegate { };
        public static Action PreInitialization = delegate { };


        private void Awake()
        {
            analyticsRegistrator.ForEach(a => a.Register());
            
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

            "AutoStarting Playbox SDK".PlayboxInfo();
            
            Initialization();
         
        }
        
        public static bool IsValidate<T>() where T : PlayboxBehaviour
        {
            if(initStatus == null)
                return false;
            
            initStatus.TryGetValue(typeof(T).Name, out bool validate);
                return validate;
        }

        public override void Initialization()
        {
            GlobalPlayboxConfig.Load();
            
            if(Application.isPlaying)
                DontDestroyOnLoad(gameObject);
            
            PreInitialization?.Invoke();
            
            behaviours.Add(AddToGameObject<PlayboxSplashUGUILogger>(gameObject, isDebugSplash));
            behaviours.Add(AddToGameObject<FirebaseInitialization>(gameObject));
            
            behaviours.Add(AddToGameObject<DevToDevInitialization>(gameObject,true,true));
            behaviours.Add(AddToGameObject<AppLovinInitialization>(gameObject,true,true));
            behaviours.Add(AddToGameObject<AppsFlyerInitialization>(gameObject,true,true));
            behaviours.Add(AddToGameObject<FacebookSdkInitialization>(gameObject,true,true));
            
            InitStatus[nameof(PlayboxSplashUGUILogger)] = false;
            InitStatus[nameof(FirebaseInitialization)] = false;
            InitStatus[nameof(AppsFlyerInitialization)] = false;
            InitStatus[nameof(DevToDevInitialization)] = false;
            InitStatus[nameof(FacebookSdkInitialization)] = false;
            InitStatus[nameof(AppLovinInitialization)] = false;
            
            foreach (var item in behaviours)
            {
                if(item != null)
                    item.GetInitStatus(() =>
                    {
                        item.playboxName.PlayboxSplashLogUGUI();
                        InitStatus[item.playboxName] = true;
                        
                    });
            }
            
            ConsentData.ShowConsent(this, () =>
            {
                foreach (var item in behaviours)
                {
                    if (item != null)
                    {
                        if (item.ConsentDependency)
                        {
                            item.Initialization();
                        }
                    }
                }
                
                Invoke(nameof(PostInit),1);
            });
            
            foreach (var item in behaviours)
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
        
        void PostInit() => PostInitialization?.Invoke();
        
        private void OnDestroy()
        {
            foreach (var item in behaviours)
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
