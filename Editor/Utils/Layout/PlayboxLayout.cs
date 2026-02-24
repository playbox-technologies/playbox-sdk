#if UNITY_EDITOR
using System;
using System.Runtime.CompilerServices;
using Playbox.SdkWindow;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DropdownList(Action body, bool isOpen) => Vertical(body, isOpen);
        
        public static void Horizontal(Action body, bool isRendering = true)
        {
            if (!isRendering)
                return;
            
            BeginHorizontal();

            try
            {
                BeginHorizontal();
                GUILayout.Space(20);
                body?.Invoke();
            }
            finally
            {
                EndHorizontal();
            }
            
        }
        
        public static void Vertical(Action body, bool isRendering = true)
        {
            if (!isRendering)
                return;
            
            try
            {
                BeginVertical();
                GUILayout.Space(20);
                body?.Invoke();
            }
            finally
            {
                EndVertical();
            }
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

        public static void Foldout(ref bool isShow , string name, Action OnRendering = null)
        {
            isShow = EditorGUILayout.Foldout(isShow, name, true);
            
            EditorGUI.indentLevel++;
            
            Vertical(() =>
            {
                OnRendering?.Invoke();
                
            },isShow);
            
        }
    }
}

#endif