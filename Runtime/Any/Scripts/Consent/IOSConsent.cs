#if UNITY_IOS

using System;
using System.Collections;
using AppsFlyerSDK;
using Unity.Advertisement.IosSupport;
using UnityEngine;
using Utils.Tools.Extentions;

namespace Playbox.Consent
{
    public class IOSConsent
    {
        public static void ShowATTUI(MonoBehaviour mono, Action<bool> onComplete)
        {

            mono.StartCoroutine(IosATTStatus(status =>
            {
                if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.AUTHORIZED)
                {
                    onComplete?.Invoke(true);
                    "ATT: AUTHORIZED".PlayboxSplashLogUGUI();
                }

                if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.DENIED)
                {
                    onComplete?.Invoke(false);
                    "ATT: DENIED".PlayboxSplashLogUGUI();
                }
                
                if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.RESTRICTED)
                {
                    onComplete?.Invoke(false);
                    "ATT: RESTRICTED".PlayboxSplashLogUGUI();
                }
                
                if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
                {
                    "ATT: NOT_DETERMINED".PlayboxSplashLogUGUI();
                }
                
            }));
        }

        private static IEnumerator IosATTStatus( Action<ATTrackingStatusBinding.AuthorizationTrackingStatus> action)
        {
            
            yield return new WaitForSeconds(0.4f);

            if (Application.isEditor)
            {
                action?.Invoke(ATTrackingStatusBinding.AuthorizationTrackingStatus.AUTHORIZED);
                "ATT: EDITOR".PlayboxSplashLogUGUI();
                
                yield break;
            }
            
            var attStatus = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
            if (attStatus == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                ATTrackingStatusBinding.RequestAuthorizationTracking();
            }

            AppsFlyer.waitForATTUserAuthorizationWithTimeoutInterval(20);
            
            while (true)
            {
                var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
                if (status != ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
                {
                    break;
                }

                yield return new WaitForSecondsRealtime(0.5f);
            }

            var finalStatus = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
            
            action?.Invoke(finalStatus);
        }
    }
}

#endif