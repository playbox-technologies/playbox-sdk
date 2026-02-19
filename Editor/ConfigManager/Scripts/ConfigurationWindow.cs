using System.Collections.Generic;
using ConfigManager.Scripts.ConfigManagers;
using Playbox.SdkConfigurations;

#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Playbox.SdkWindow
{
    public class ConfigurationWindow : EditorWindow
    {
        private List<DrawableWindow> drawableWindowList = new();
        private Vector2 scrollPosition;

        private float FieldHeight = EditorGUIUtility.singleLineHeight * 1.02f;
        private float FieldWidth => position.width * 0.488f;
    
        [MenuItem("Playbox/Configuration")]
        public static void ShowWindow()
        {
            var window = GetWindow<ConfigurationWindow>("Playbox Configuration");
            window.hasUnsavedChanges = true;
        }

        private void CreateGUI()
        {
            drawableWindowList.Add(new AppsFlyerWindow());
            drawableWindowList.Add(new DevToDevWindow());
            drawableWindowList.Add(new AppLovinWindow());
            drawableWindowList.Add(new FacebookSdkWindow());
        
            GlobalPlayboxConfig.Load();
        
            foreach (var item in drawableWindowList)
            {
                item.InitName();
                item.Load();
            }
        
            hasUnsavedChanges = false;
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            
            EditorGUILayout.Separator();
            
            GUILayout.Label(titleContent, EditorStyles.boldLabel,GUILayout.ExpandWidth(false),GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
        
            EditorGUILayout.Separator();
            
            foreach (var item in drawableWindowList)
            {
                DrawableWindow.FieldHeight = FieldHeight;
                DrawableWindow.FieldWidth = FieldWidth;
                
                GUILayout.BeginVertical();
                
                item.HasRenderToggle();
                
                item.Title();
                item.Header();
                item.Body();
                item.Footer();

                hasUnsavedChanges = hasUnsavedChanges || item.hasUnsavedChanges;
                
                GUILayout.EndVertical();
            }

            GUILayout.BeginHorizontal();
            
            GUILayout.Label("",GUILayout.ExpandWidth(false),GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
            
            if (GUILayout.Button("Save Configuration",GUILayout.ExpandWidth(false),GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth)))
            {
                GlobalPlayboxConfig.Clear();
            
                foreach (var item in drawableWindowList)
                {
                    item.Save();
                }
            
                GlobalPlayboxConfig.Save();
            
                hasUnsavedChanges = false;
            }
            
            GUILayout.EndHorizontal();
            
            EditorGUILayout.EndScrollView();
            
            GUILayout.EndVertical();
        }

        public override void SaveChanges()
        {
            GlobalPlayboxConfig.Clear();
            
            foreach (var item in drawableWindowList)
            {
                item.Save();
            }
            
            GlobalPlayboxConfig.Save();

            base.SaveChanges();
        }

        public override void DiscardChanges()
        {
            base.DiscardChanges();
        }
    }
}

#endif