using System;
using System.Collections;
using UnityEngine;

namespace Playbox
{
    public class PlayboxBehaviour : MonoBehaviour
    {
        [SerializeField]
        protected bool isInitialized = false;
        protected Action initCallback = delegate { };
        
        protected ServiceType serviceType = ServiceType.PlayboxBehaviour;

        public string playboxName => GetType().Name;
        
        [HideInInspector]
        public bool ConsentDependency = false;
        
        public static PlayboxBehaviour AddToGameObject<T>(GameObject target, bool hasConsentDependency = false) where T : PlayboxBehaviour
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

           
            var component = target.gameObject.AddComponent<T>();
            component.ConsentDependency = hasConsentDependency;
                
            return component;
         
        }

        public ServiceType GetServiceType() => serviceType;
        
        public virtual void Initialization()
        {
            serviceType = ServiceType.PlayboxBehaviour;
        }

        public virtual void GetInitStatus(Action OnInitComplete)
        {
            initCallback = OnInitComplete;
        }

        public bool IsInitialization() => isInitialized;
        protected void ApproveInitialization()
        {
            isInitialized = true;
            initCallback?.Invoke();
            
            initCallback = null;
        }

        public virtual void Close()
        {
        }

        public void DelayInvoke(Action action, float delay)
        {
            StartCoroutine(Invoker(action, delay));
        }

        private IEnumerator Invoker(Action action, float delay = 0)
        {
            yield return new WaitForSeconds(delay);
            
            action?.Invoke();
            
            yield return null;
        }
    }
}