using System.Collections.Generic;
using ConfigManager.Scripts.ConfigManagers;
using Editor.ConfigManager.Scripts.SdkWindow;
using Playbox.SdkConfigurations;

#if UNITY_EDITOR
using Editor.Utils.Layout;
using UnityEditor;
using UnityEngine;

namespace Playbox.SdkWindow
{
    public class ConfigurationWindow : EditorWindow
    {
        private readonly List<DrawableWindow> drawableWindowList = new();
        private Vector2 _scrollPosition;

        private readonly float _fieldHeight = EditorGUIUtility.singleLineHeight * 1.02f;
        private float FieldWidth => position.width * 0.4f;
        
        private bool isLoadConfig = false;
    
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
        
           isLoadConfig = GlobalPlayboxConfig.Load();


           if (isLoadConfig)
           {
               foreach (var item in drawableWindowList)
               {
                   item.InitName();
                   item.Load();
               }   
           }
            hasUnsavedChanges = false;
        }

        private void OnGUI()
        {
            
            PGUI.Vertical((() =>
            {
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                
                EditorGUILayout.Separator();
            
                GUILayout.Label(titleContent, EditorStyles.boldLabel,GUILayout.ExpandWidth(false),GUILayout.Height(_fieldHeight), GUILayout.Width(FieldWidth));
        
                EditorGUILayout.Separator();
                
                DrawableWindow.FieldHeight = _fieldHeight;
                DrawableWindow.FieldWidth = FieldWidth;

                if (isLoadConfig)
                {
                    foreach (var item in drawableWindowList)
                    {
                        item.HasRenderToggle();

                        item.Title();
                        item.Header();
                        item.Body();
                        item.Footer();

                        hasUnsavedChanges = hasUnsavedChanges || item.HasUnsavedChanges;
                    }
                    
                    PGUI.Horizontal(() =>
                    {
                        if (GUILayout.Button("Save Configuration",GUILayout.ExpandWidth(false),GUILayout.Height(_fieldHeight), GUILayout.Width(FieldWidth)))
                        {
                            GlobalPlayboxConfig.Clear();
            
                            foreach (var item in drawableWindowList)
                            {
                                item.Save();
                            }
            
                            GlobalPlayboxConfig.Save();
            
                            hasUnsavedChanges = false;
                        }
                    });
                    
                }
                else
                {
                    PGUI.Vertical((() =>
                    {
                        PGUI.Label("Configuration file not found. Create a new file.");
                        
                        PGUI.Separator();
                        
                        if (GUILayout.Button("Create Configuration",GUILayout.ExpandWidth(false),GUILayout.Height(_fieldHeight), GUILayout.Width(FieldWidth)))
                        {
                            GlobalPlayboxConfig.Save();
                            
                            isLoadConfig = GlobalPlayboxConfig.Load();
                            
                            if (isLoadConfig)
                            {
                                foreach (var item in drawableWindowList)
                                {
                                    item.InitName();
                                    item.Load();
                                }   
                            }
                        }
                    }));
                }
                
                EditorGUILayout.EndScrollView();
                
            }));
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