#if UNITY_EDITOR

using Editor.Utils.Layout;
using Playbox.SdkConfigurations;
using Playbox.SdkWindow;

namespace Editor.ConfigManager.Scripts.SdkWindow
{
    public class AppsFlyerWindow : DrawableWindow
    {
        private string _iosKey = "";
        private string _androidKey = "";
        private string _iosAppID = "";
        private string _androidAppID = "";
        
        public override void InitName()
        {
            base.InitName();
            
            Name = AppsFlyerConfiguration.Name;
        }

        public override void HasRenderToggle()
        {
            
        }

        public override void Body()
        {
            PGUI.SpaceLine();
            
            PGUI.Foldout(ref Active, AppsFlyerConfiguration.Name, () =>
            {
                PGUI.HorizontalTextField(ref _iosKey,"ios sdk key: ");
                PGUI.Separator();
                PGUI.HorizontalTextField(ref _iosAppID,"ios app id : ");
                PGUI.Separator();
                PGUI.HorizontalTextField(ref _androidKey,"android sdk key: ");
                PGUI.Separator();
                PGUI.HorizontalTextField(ref _androidAppID,"android app id : ");
            });
        }

        public override void Save()
        {
            AppsFlyerConfiguration.AndroidKey = _iosKey;
            AppsFlyerConfiguration.IOSKey = _androidKey;
            AppsFlyerConfiguration.Active = Active;
            AppsFlyerConfiguration.IOSAppId = _iosAppID;
            AppsFlyerConfiguration.AndroidAppId = _androidAppID;
        
            AppsFlyerConfiguration.SaveJsonConfig();
        }

        public override void Load()
        {
            AppsFlyerConfiguration.LoadJsonConfig();
        
            _iosKey = AppsFlyerConfiguration.AndroidKey;
            _androidKey = AppsFlyerConfiguration.IOSKey;
            Active = AppsFlyerConfiguration.Active;
            _iosAppID = AppsFlyerConfiguration.IOSAppId;
            _androidAppID = AppsFlyerConfiguration.AndroidAppId;
        
            base.Load();
        }
    }
}

#endif