using System;
using System.Runtime.CompilerServices;
using Playbox.SdkWindow;
using UnityEditor;
using UnityEngine;

namespace Editor.Utils.Layout
{
    public static class PlayboxLayout
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BeginHorizontal() => GUILayout.BeginHorizontal();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BeginVertical() => GUILayout.BeginVertical();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EndVertical() => GUILayout.EndVertical();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EndHorizontal() => GUILayout.EndHorizontal();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Separator() => EditorGUILayout.Separator();
        
        public static void Horizontal(Action action, bool isRendering = true)
        {
            if (!isRendering)
                return;
            
            BeginHorizontal();
            
            action?.Invoke();
            
            EndHorizontal();
        }
        
        public static void Vertical(Action action, bool isRendering = true)
        {
            if (!isRendering)
                return;
            
            BeginVertical();
            
            action?.Invoke();
            
            EndVertical();
        }

        public static void TextField(ref string text, Action<string> action = null, bool isRendering = true)
        {
            if (!isRendering)
                return;
            
            string result = GUILayout.TextField(text, GUILayout.ExpandWidth(false),
                GUILayout.Height(DrawableWindow.FieldHeight),
                GUILayout.Width(DrawableWindow.FieldWidth));
            
            text = result;
            
            action?.Invoke(result);
        }
        
        public static void Toggle(ref bool value, string label = "", Action<bool> action = null, bool isRendering = true)
        {
            if (!isRendering)
                return;
            
            bool val = EditorGUILayout.Toggle(label, value, GUILayout.ExpandWidth(false),
                GUILayout.Height(DrawableWindow.FieldHeight), GUILayout.Width(DrawableWindow.FieldWidth));
            
            value = val;
            
            action?.Invoke(val);
        }

        public static void HorizontalToggle(ref bool value, string label = "", Action<bool> action = null,
            bool isRendering = true)
        {
            if (!isRendering)
                return;
            
            BeginHorizontal();
            
            Label(label);
            Toggle(ref value, "", action);
            
            EndVertical();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CheckBox(ref bool value, string label = "", Action<bool> action = null,
            bool isRendering = true) => HorizontalToggle(ref value, "", action, isRendering);

        public static void Label(string text, bool isRendering = true)
        {
            if (!isRendering)
                return;
            
            GUILayout.Label(text, GUILayout.ExpandWidth(false), GUILayout.Height(DrawableWindow.FieldHeight),
                GUILayout.Width(DrawableWindow.FieldWidth));
        }
        
        public static void HorizontalTextField(ref string value, string label = "", Action<string> action = null,
            bool isRendering = true)
        {
            if (!isRendering)
                return;
            
            BeginHorizontal();
            
            Label(label);
            TextField(ref value, action);
            
            EndVertical();
        }
    }
}