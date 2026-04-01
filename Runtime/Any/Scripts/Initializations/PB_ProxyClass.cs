using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Playbox
{
    public class PB_ProxyClass : MonoBehaviour
    {
        public static event Action<bool> OnPauseStatusChanged;
        public static event Action<bool> OnFocusStatusChanged;
        
        public static PB_ProxyClass instance;

        private void OnApplicationFocus(bool hasFocus)
        {
            OnFocusStatusChanged?.Invoke(hasFocus);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            OnPauseStatusChanged?.Invoke(pauseStatus);
        }
        
        public static MonoBehaviour GetOrCreateMonoProxy()
        {
            if (instance != null)
                return instance;
            
            GameObject go = GameObject.Find("AppsFlyerProxy");
            if (go == null)
            {
                go = new GameObject("AppsFlyerProxy");
                DontDestroyOnLoad(go);
            }
            
            var component = go.GetComponent<PB_ProxyClass>();
            if (component == null)
            {
                component = go.AddComponent<PB_ProxyClass>();
            }
            
            instance = component;

            return component; 
        }
    }
}