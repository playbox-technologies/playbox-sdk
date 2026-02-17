using UnityEngine;

namespace Playbox
{
    public static class RewardedAd
    {
        public static bool IsReady { get; private set; } = false;
        
        private static string _unitId;

        private static bool IsInitialized => MaxSdk.IsInitialized();
        private static void LoadAd() => MaxSdk.LoadRewardedAd(_unitId);
        private static bool isLoaded => MaxSdk.IsRewardedAdReady(_unitId);
        private static void ShowAd(string placement) => MaxSdk.ShowRewardedAd(_unitId, placement);

        private static MonoBehaviour _context;

        public static void RegisterUnitID(string androidID, string iOSID, MonoBehaviour context)
        {
            _unitId = Application.platform switch
            {
                RuntimePlatform.Android => androidID,
                RuntimePlatform.IPhonePlayer => iOSID,
                _ => iOSID
            };

            _context = context;
        }

        public static void Load()
        {
            if(IsReady)
                return;
            
            if(IsInitialized)
                LoadAd();
        }
        
        public static void Show(string placement = "default") => ShowAd(placement);

        private static void HealthCheck()
        {
            IsReady = IsInitialized && isLoaded;
        }
    }
}