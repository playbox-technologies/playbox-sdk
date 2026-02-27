#if UNITY_EDITOR
using System;
using System.Runtime.CompilerServices;
using Playbox.SdkWindow;
using UnityEditor;
using UnityEngine;

namespace Editor.Utils.Layout
{
    public static class PGUI
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
        
        public static void Push() => EditorGUI.indentLevel++;
        public static void Pop() => EditorGUI.indentLevel--;
        
        public static void Horizontal(Action body, bool isRendering = true)
        {
            if (!isRendering)
                return;
            
            try
            {
                BeginHorizontal();
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
            
            bool val = EditorGUILayout.Toggle(label, value, 
                GUILayout.Height(DrawableWindow.FieldHeight), GUILayout.Width(DrawableWindow.FieldWidth + 30));
            
            value = val;
            
            action?.Invoke(val);
        }

        public static void HorizontalToggle(ref bool value, string label = "", Action<bool> action = null,
            bool isRendering = true)
        {
            if (!isRendering)
                return;

            try
            {
                BeginHorizontal();
            
                Label(label);
                
                GUILayout.FlexibleSpace();
                
                Toggle(ref value, "", action);
            }
            finally
            {
                EndVertical();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CheckBox(ref bool value, string label = "", Action<bool> action = null,
            bool isRendering = true) => HorizontalToggle(ref value, "", action, isRendering);

        public static void Label(string text, bool isRendering = true)
        {
            if (!isRendering)
                return;

            float width = EditorGUIUtility.labelWidth;
            
            EditorGUIUtility.labelWidth = DrawableWindow.FieldWidth;
            
            EditorGUILayout.PrefixLabel(text);
            
            EditorGUIUtility.labelWidth = width;
        }
        
        public static void HorizontalTextField(ref string value, string label = "", Action<string> action = null,
            bool isRendering = true)
        {
            if (!isRendering)
                return;
            
            BeginHorizontal();
            
            Label(label);
            
            GUILayout.FlexibleSpace();
            
            TextField(ref value, action);
            
            EndVertical();
        }

        public static void Foldout(ref bool isShow , string name, Action OnRendering = null)
        {
            try
            {
                isShow = EditorGUILayout.Foldout(isShow, name, true);
                
                Push();
                
                Vertical(() =>
                {
                    OnRendering?.Invoke();
                
                },isShow);

            }
            finally
            {
               Pop();
            }
        }

        public static void Popup(string label,ref int logLevel, string[] options)
        {
            BeginHorizontal();
            
            Label(label);
            
            GUILayout.FlexibleSpace();
            
            logLevel = EditorGUILayout.Popup(logLevel,
                    options,
                    GUILayout.ExpandWidth(true),
                    GUILayout.Height(DrawableWindow.FieldHeight),
                    GUILayout.Width(DrawableWindow.FieldWidth + 15));    
                
            EndHorizontal();
            
        }

        public static void SpaceLine()
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }
    }
}

#endif