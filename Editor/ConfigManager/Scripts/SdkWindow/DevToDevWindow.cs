using System;
using Playbox.SdkConfigurations;

#if UNITY_EDITOR
using System.Collections.Generic;
using DevToDev.Analytics;
using UnityEditor;
using UnityEngine;

namespace Playbox.SdkWindow
{
    public class DevToDevWindow : DrawableWindow
    {
        private string ios_key = "";
        private string android_key = "";
    
        private string prev_ios_version = "";
        private string prev_android_version = "";
        
        DTDLogLevel         logLevel   = 0;
        private List<string> _options = new();
        
        public override void InitName()
        {
            base.InitName();
            
            _options.Clear();

            foreach (var item in Enum.GetNames(typeof(DTDLogLevel)))
            {
                _options.Add(item);
            }
            
            
            Name = DevToDevConfiguration.Name;
        }

        public override void Body()
        {
            if (!Active)
                return;
            
            prev_ios_version = ios_key;
            prev_android_version = android_key;

            GUILayout.BeginHorizontal();
            
            GUILayout.Label("LogLevel",GUILayout.ExpandWidth(false),GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
            
            logLevel = (DTDLogLevel)EditorGUILayout.Popup("", (int)logLevel, _options.ToArray(), GUILayout.ExpandWidth(false),GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));

            GUILayout.EndHorizontal();
            
            EditorGUILayout.Separator();
            
            GUILayout.BeginHorizontal();
        
            GUILayout.Label("IOS : ",GUILayout.ExpandWidth(false),GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
            ios_key = GUILayout.TextField(ios_key, GUILayout.ExpandWidth(false), GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
        
            GUILayout.EndHorizontal();
            
            EditorGUILayout.Separator();
        
            GUILayout.BeginHorizontal();
        
            GUILayout.Label("Android : ",GUILayout.ExpandWidth(false),GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
            android_key = GUILayout.TextField(android_key, GUILayout.ExpandWidth(false), GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
        
            GUILayout.EndHorizontal();
            
            HasUnsavedChanges = !(string.Equals(prev_ios_version, ios_key, StringComparison.OrdinalIgnoreCase) &&
                                  string.Equals(prev_android_version, android_key, StringComparison.OrdinalIgnoreCase));
        
        }

        public override void Save()
        {
            DevToDevConfiguration.AndroidKey = android_key;
            DevToDevConfiguration.IOSKey = ios_key;
            DevToDevConfiguration.Active = Active;
            DevToDevConfiguration.LOGLevel = logLevel;
            
            DevToDevConfiguration.SaveJsonConfig();
        }

        public override void Load()
        {
            DevToDevConfiguration.LoadJsonConfig();
        
            android_key = DevToDevConfiguration.AndroidKey;
            ios_key = DevToDevConfiguration.IOSKey;
            Active = DevToDevConfiguration.Active;
            logLevel = DevToDevConfiguration.LOGLevel;
        
            base.Load();
        }
    }
}

#endif