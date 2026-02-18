using System;
using Playbox.SdkWindow;
using UnityEditor;
using UnityEngine;

namespace Editor.Utils.Layout
{
    public static class PlayboxLayout
    {
        public static void Horizontal(Action action, bool isRendering = true)
        {
            if (!isRendering)
                return;
            
            GUILayout.BeginHorizontal();
            
            action?.Invoke();
            
            GUILayout.EndHorizontal();
        }
        
        public static void Vertical(Action action, bool isRendering = true)
        {
            if (!isRendering)
                return;
            
            GUILayout.BeginVertical();
            
            action?.Invoke();
            
            GUILayout.EndVertical();
        }

        public static void TextField(string text, Action<string> action, bool isRendering = true)
        {
            if (!isRendering)
                return;
            
            string result = GUILayout.TextField(text, GUILayout.ExpandWidth(false),
                GUILayout.Height(DrawableWindow.FieldHeight),
                GUILayout.Width(DrawableWindow.FieldWidth));
            
            action?.Invoke(result);
        }
        
        public static void Toggle(string text,bool value, Action<bool> action, bool isRendering = true)
        {
            if (!isRendering)
                return;
            
            bool val = EditorGUILayout.Toggle(text, value, GUILayout.ExpandWidth(false),
                GUILayout.Height(DrawableWindow.FieldHeight), GUILayout.Width(DrawableWindow.FieldWidth));
            
            action?.Invoke(val);
        }

        public static void Label(string text, bool isRendering = true)
        {
            if (!isRendering)
                return;
            
            GUILayout.Label(text, GUILayout.ExpandWidth(false), GUILayout.Height(DrawableWindow.FieldHeight),
                GUILayout.Width(DrawableWindow.FieldWidth));
        }
        
        public static void Separator() => EditorGUILayout.Separator();
        
    }
}