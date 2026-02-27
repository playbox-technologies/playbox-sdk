using System;
using ConfigManager.Scripts.DevToDev;
using Editor.Utils.Layout;

#if UNITY_EDITOR
using System.Collections.Generic;
using DevToDev.Analytics;
using UnityEngine;

namespace Playbox.SdkWindow
{
    public class DevToDevWindow : DrawableWindow
    {
        private DevToDevData _devToDevData;
        
        private List<string> _options = new();
        
        public override void InitName()
        {
            base.InitName();
            
            _options.Clear();

            foreach (var item in Enum.GetNames(typeof(DTDLogLevel)))
            {
                _options.Add(item);
            }

            if (_devToDevData == null)
            {
                _devToDevData = new DevToDevData();
            }
                
            Name = DevToDevConfiguration.Name;
        }

        public override void HasRenderToggle()
        {
        }

        public override void Body()
        {
            if (_devToDevData == null)
            {
                Debug.LogError("DevToDevData is null");
                return;
            }
            
            PGUI.SpaceLine();
            PGUI.Foldout(ref _devToDevData.active, DevToDevConfiguration.Name,() =>
            {
                PGUI.Popup("Log Level", ref _devToDevData.logLevel, _options.ToArray());
                PGUI.Separator();
                PGUI.HorizontalTextField(ref _devToDevData.iosKey, "iOS Key :");
                PGUI.Separator();
                PGUI.HorizontalTextField(ref _devToDevData.androidKey, "Android Key :");
         
            });
        }

        public override void Save()
        {
            DevToDevConfiguration.DevToDevData = _devToDevData;
            
            DevToDevConfiguration.SaveJsonConfig();
        }

        public override void Load()
        {
            DevToDevConfiguration.LoadJsonConfig();
            
           _devToDevData = DevToDevConfiguration.DevToDevData;
           
            base.Load();
        }
    }
}

#endif