using System;
using Playbox.Services;
using UnityEngine;

namespace Playbox
{
    public class PlayboxService
    {
        protected bool IsInitialized;
        private Action _initCallback;
        
        protected ServiceType ServiceType = ServiceType.PlayboxService;

        public string PlayboxName => GetType().Name;
        
        [HideInInspector]
        public bool ConsentDependency = false;
        
        /*
        public static PlayboxBehaviour AddToGameObject<T>(GameObject target, bool hasConsentDependency = false) where T : PlayboxBehaviour
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

           
            var component = target.gameObject.AddComponent<T>();
            component.ConsentDependency = hasConsentDependency;
                
            return component;
         
        }
        */
        
        public ServiceType GetServiceType() => ServiceType;
        
        public virtual void Initialization()
        {
            ServiceType = ServiceType.PlayboxService;
        }

        public virtual void GetInitStatus(Action onInitComplete)
        {
            _initCallback = onInitComplete;
        }

        public bool IsInitialization() => IsInitialized;
        protected void ApproveInitialization()
        {
            IsInitialized = true;
            _initCallback?.Invoke();
            
            _initCallback = null;
        }

        public virtual void Close()
        {
        }
        
        public void DelayInvoke(Action action, float delay)
        {
            LLS.PlayerAsyncHelper.Delay(delay, action);
        }
    }
}