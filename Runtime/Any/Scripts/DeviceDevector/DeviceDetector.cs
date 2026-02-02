using UnityEngine.Device;

namespace Playbox.DeviceDevector
{
    public static class DeviceDetector
    {
        public static bool ValidateGPU()
        {
            return SystemInfo.graphicsDeviceName.ToLower().Contains("SwiftShader".ToLower());
        }

        public static bool ValidateCPU()
        {
           return SystemInfo.processorCount < 4;
        }
        
        public static bool IsTestDevice() => 
            ValidateGPU() || 
            ValidateCPU();
    }
}