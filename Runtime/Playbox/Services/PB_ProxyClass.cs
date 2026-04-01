using System;
using UnityEngine;

namespace Playbox.Services
{
    public class PbProxyClass : MonoBehaviour
    {
        public static event Action<bool> OnPauseStatusChanged;
        public static event Action<bool> OnFocusStatusChanged;
        
        public static PbProxyClass Instance;

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
            if (Instance != null)
                return Instance;
            
            GameObject go = GameObject.Find("AppsFlyerProxy");
            if (go == null)
            {
                go = new GameObject("AppsFlyerProxy");
                DontDestroyOnLoad(go);
            }
            
            var component = go.GetComponent<PbProxyClass>();
            if (component == null)
            {
                component = go.AddComponent<PbProxyClass>();
            }
            
            Instance = component;

            return component; 
        }
    }
}