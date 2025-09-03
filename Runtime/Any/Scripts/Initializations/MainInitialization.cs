using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Any.Scripts.Initializations;
using AppsFlyerSDK;
using CI.Utils.Extentions;
using Firebase.Crashlytics;
using Playbox.Consent;
#if UNITY_EDITOR
#endif
using Playbox.SdkConfigurations;
using Playbox.Verification;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Playbox
{
    public class MainInitialization : PlayboxBehaviour
    {
        [SerializeField] private bool isAutoInitialize = true;
        [SerializeField] private bool useInAppValidation = true;
        [SerializeField] private bool isDebugSplash;
        [SerializeField] private bool usePlayboxIAP;
        [SerializeField] private UnityEvent OnPostInitializatioon;
        
        private List<PlayboxBehaviour> behaviours = new();
        
        private const string objectName = "[Global] MainInitialization";
        
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
            PostInitialization += () =>
            {
                OnPostInitializatioon?.Invoke();
                OnPostInitializatioon = null;
            };
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void Start()
        {
            if (!isAutoInitialize)
                return;
            
            try
            {
                Initialization();
            }
            catch (Exception e)
            {
                if (IsValidate<FirebaseInitialization>())
                {
                    Crashlytics.LogException(e);
                }
            }
        }


        public static bool IsValidate<T>() where T : PlayboxBehaviour
        {
            if(initStatus == null)
                return false;
            
            initStatus.TryGetValue(typeof(T).Name, out bool validate);
                return validate;
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        public override void Initialization()
        {
            GlobalPlayboxConfig.Load();
            
            if(Application.isPlaying)
                DontDestroyOnLoad(gameObject);
            
            ApproveInitialization();
            
            PreInitialization?.Invoke();
            
            behaviours.Add(AddToGameObject<PlayboxSplashUGUILogger>(gameObject, isDebugSplash));
            behaviours.Add(AddToGameObject<FirebaseInitialization>(gameObject));
            behaviours.Add(AddToGameObject<InAppVerification>(gameObject, useInAppValidation));
            
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
            InitStatus[nameof(InAppVerification)] = false;
            
            foreach (var item in behaviours)
            {
                if(item != null)
                    item.GetInitStatus(() =>
                    {
                        item.playboxName.PlayboxInfo("INITIALIZED");
                        item.playboxName.PlayboxSplashLogUGUI();
                        InitStatus[item.playboxName] = true;
                        
                    });
            }
            
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
            
            ConsentData.ShowConsent(this, () =>
            {
                "Show Consent".PlayboxSplashLogUGUI();
                
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
                
                //ClientQueueService.Initialization();
                
                PostInitialization?.Invoke();
            });
        }

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
        
#if UNITY_EDITOR
        [MenuItem("Playbox/Initialization/Create")]
        public static void CreateAnalyticsObject()
        { 
            var findable = GameObject.Find(objectName);

            if (findable == null)
            {
                var go = new GameObject(objectName); 
                
                go.AddComponent<MainInitialization>();
            }
            else
            {
                if (findable.TryGetComponent(out MainInitialization main))
                {
                    DestroyImmediate(main);
                }
                else
                {
                    findable.AddComponent<MainInitialization>();   
                }
            }
        }
        
        [MenuItem("Playbox/Initialization/Remove")]
        public static void RemoveAnalyticsObject()
        {
            var go = GameObject.Find(objectName);

            if (go != null)
            {
                DestroyImmediate(go);
            }
        }
#endif
    }
}
