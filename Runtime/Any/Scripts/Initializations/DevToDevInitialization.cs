using System.Collections;
using DevToDev.Analytics;
using Playbox.SdkConfigurations;
using UnityEngine;

namespace Playbox
{
    public class DevToDevInitialization : PlayboxBehaviour
    {

        public override void Initialization()
        {
            DevToDevConfiguration.LoadJsonConfig();
            
            if(!DevToDevConfiguration.Active)
                return;

            DTDAnalytics.SetLogLevel(DevToDevConfiguration.LOGLevel);
            
#if UNITY_ANDROID
            DTDAnalytics.Initialize(DevToDevConfiguration.AndroidKey);
#endif
#if UNITY_IOS
            DTDAnalytics.Initialize(DevToDevConfiguration.IOSKey);
            
#endif
            
            DTDAnalytics.SetTrackingAvailability(true);
            
            //DTDAnalytics.StartActivity();

            ApproveInitialization();
            
            DTDAnalytics.GetDeviceId((a) =>
            {
                if (!string.IsNullOrEmpty(a))
                {
                    DTDUserCard.Set("dtd_device_id_present", 1);
                }
                else
                {
                    DTDUserCard.Set("dtd_device_id_present", 0);
                }
            });

            StartCoroutine(SendUserCard());

            //Application.quitting += DTDAnalytics.StopActivity;
        }

        IEnumerator SendUserCard()
        {
            
            yield return null;
            yield return new WaitForEndOfFrame();
            
            DTDUserCard.Set("device_ram", SystemInfo.systemMemorySize);
            DTDUserCard.Set("gpu_memory", SystemInfo.graphicsMemorySize);
            DTDUserCard.Set("screen_width", Screen.width);
            DTDUserCard.Set("screen_height", Screen.height);
            DTDUserCard.Set("device_name", SystemInfo.deviceName);
            DTDUserCard.Set("graphicsDeviceName", SystemInfo.graphicsDeviceName);
            DTDUserCard.Set("graphicsDeviceType", SystemInfo.graphicsDeviceType.ToString());
        }

        public override void Close()
        {
            base.Close();
            
            //DTDAnalytics.StopActivity();
        }
    }
}
