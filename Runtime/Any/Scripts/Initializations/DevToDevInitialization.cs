using DevToDev.Analytics;
using Playbox.SdkConfigurations;
using UnityEngine;

namespace Playbox
{
    public class DevToDevInitialization : PlayboxBehaviour
    {

        public override void Initialization()
        {
            GlobalPlayboxConfig.Load();
            
            DevToDevConfiguration.LoadJsonConfig();
            
            if(!DevToDevConfiguration.Active)
                return;

#if UNITY_ANDROID
            DTDAnalytics.Initialize(DevToDevConfiguration.AndroidKey);
#endif
#if UNITY_IOS
            DTDAnalytics.Initialize(DevToDevConfiguration.IOSKey);
            
#endif
            
            
            DTDAnalytics.SetLogLevel(DevToDevConfiguration.LOGLevel);
            
            DTDAnalytics.SetTrackingAvailability(true);
            
            
            //DTDAnalytics.CoppaControlEnable();
            DTDAnalytics.StartActivity();

            DTDAnalytics.GetDeviceId((a) =>
            {
                if (!string.IsNullOrEmpty(a))
                {
                    ApproveInitialization();
                    
                    DTDUserCard.Set("device_ram", SystemInfo.systemMemorySize);
                    DTDUserCard.Set("gpu_memory", SystemInfo.graphicsMemorySize);
                    DTDUserCard.Set("resolution", $"Screen: {Screen.width}x{Screen.height}");
                }
            });

            Application.quitting += DTDAnalytics.StopActivity;
        }

        public override void Close()
        {
            base.Close();
            
            //DTDAnalytics.StopActivity();
        }
    }
}
