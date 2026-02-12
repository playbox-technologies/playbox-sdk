using System.Collections;
using System.Threading.Tasks;
using DevToDev.Analytics;
using Playbox.DeviceDevector;
using Playbox.SdkConfigurations;
using UnityEngine;


namespace Playbox
{
    public class DevToDevInitialization : PlayboxBehaviour
    {

        public override void Initialization()
        {
            base.Initialization();

            serviceType = ServiceType.DevToDev;
            
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
            
            //Build
            DTDUserCard.Set("is_debug_build", Debug.isDebugBuild ? 1 : 0);

            
            //Cores
            DTDUserCard.Set("cpu_cores", SystemInfo.processorCount);
            DTDUserCard.Set("cpu_freq", SystemInfo.processorFrequency);
            DTDUserCard.Set("cpu_type", SystemInfo.processorType);

#if UNITY_6000_3
            DTDUserCard.Set("cpu_model", SystemInfo.processorModel);
            DTDUserCard.Set("cpu_manufacturer", SystemInfo.processorManufacturer);
#endif
            
            
            //Names
            DTDUserCard.Set("device_name", SystemInfo.deviceName);
            DTDUserCard.Set("device_model", SystemInfo.deviceModel);
            DTDUserCard.Set("graphicsDeviceName", SystemInfo.graphicsDeviceName);
            DTDUserCard.Set("graphicsDeviceType", SystemInfo.graphicsDeviceType.ToString());
            
            //MEMORY
            DTDUserCard.Set("device_ram", SystemInfo.systemMemorySize);
            DTDUserCard.Set("gpu_memory", SystemInfo.graphicsMemorySize);
            DTDUserCard.Set("system_memory", SystemInfo.systemMemorySize);
            
            //virtual screen(UI)
            DTDUserCard.Set("virtual_screen_width", Screen.width);
            DTDUserCard.Set("virtual_screen_height", Screen.height);
            DTDUserCard.Set("screen_safeArea_height", Screen.safeArea.height);
            DTDUserCard.Set("screen_safeArea_width", Screen.safeArea.width);
            
            //Physical Screen(Device pixels)
            DTDUserCard.Set("physical_screen_height", Screen.currentResolution.height);
            DTDUserCard.Set("physical_screen_width", Screen.currentResolution.width);
            DTDUserCard.Set("physical_screen_dpi", Screen.dpi);
            
            //Versions
            DTDUserCard.Set("graphics_device_version", SystemInfo.graphicsDeviceVersion);
            DTDUserCard.Set("graphics_shader_level", SystemInfo.graphicsShaderLevel);
            
            //Quality Levels
            DTDUserCard.Set("quality_level", QualitySettings.GetQualityLevel());
            DTDUserCard.Set("vSync_count", QualitySettings.vSyncCount);
            DTDUserCard.Set("target_fps", Application.targetFrameRate);
            DTDUserCard.Set("color_space", QualitySettings.activeColorSpace.ToString());
            DTDUserCard.Set("render_pipeline", QualitySettings.renderPipeline.name);
            
#if UNITY_IOS
             DTDUserCard.Set("ad_tracking_id", UnityEngine.iOS.Device.advertisingIdentifier);
#endif
            
            
          

#if UNITY_ANDROID
            var isEmu = DeviceDetector.IsTestDevice();
            
            DTDUserCard.Set("is_test_device", isEmu ? 1 : 0);
            DTDUserCard.Set("emulator_reason", isEmu ? "swiftshader_or_lowcpu" : "none");

            StartCoroutine(SetAdIdSequence());

#endif
        }

        public override void Close()
        {
            base.Close();
        }
        
        #if UNITY_ANDROID

        private IEnumerator SetAdIdSequence()
        {
            Task<string> adIdTask = GetAndroidAdIdAsync();
            
            while (!adIdTask.IsCompleted)
            {
                yield return null;
            }
            if (adIdTask.IsFaulted)
            {
                Debug.LogError("Не удалось получить GAID: " + adIdTask.Exception);
            }
            else
            {
                string gaid = adIdTask.Result;

                if (!string.IsNullOrEmpty(gaid))
                {
                   DTDUserCard.Set("ad_tracking_id", gaid);
                }
            }
        }
        
        private Task<string> GetAndroidAdIdAsync()
        {
            return Task.Run(() =>
            {
                try
                {
                    using (var up = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                    using (var currentActivity = up.GetStatic<AndroidJavaObject>("currentActivity"))
                    using (var client = new AndroidJavaClass("com.google.android.gms.ads.identifier.AdvertisingIdClient"))
                    {
                        using (var adInfo = client.CallStatic<AndroidJavaObject>("getAdvertisingIdInfo", currentActivity))
                        {
                            return adInfo.Call<string>("getId");
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Ошибка получения GAID: {e.Message}");
                    return null;
                }
            });
        }
        #endif
    }
}
