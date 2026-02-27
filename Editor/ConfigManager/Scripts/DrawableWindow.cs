using Newtonsoft.Json.Linq;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using PGUI = Editor.Utils.Layout.PlayboxLayout;

namespace Playbox.SdkWindow
{
    public class DrawableWindow
    {
        protected string Name;
        protected bool Active;
        public bool HasUnsavedChanges;

        private static float _fieldWidth = 250;
        private static float _fieldHeight = 15;
        private const float footerSpace = 10;
        private const float headerSpace = 10;

        protected JObject Configuration;

        public static float FieldWidth
        {
            get => _fieldWidth;
            set => _fieldWidth = value;
        }

        public static float FieldHeight
        {
            get => _fieldHeight;
            set => _fieldHeight = value;
        }

        public virtual void InitName()
        {
            Configuration = new JObject();
        }

        public virtual void HasRenderToggle()
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label(Name, GUILayout.ExpandWidth(false), GUILayout.Height(FieldHeight),
                GUILayout.Width(FieldWidth));
            Active = EditorGUILayout.Toggle("", Active, GUILayout.ExpandWidth(false), GUILayout.Height(FieldHeight),
                GUILayout.Width(FieldWidth));

            GUILayout.EndHorizontal();

            EditorGUILayout.Separator();
        }

        public virtual void Title()
        {
        }

        public virtual void Header()
        {
            GUILayout.Space(headerSpace);
        }

        public virtual void Body()
        {
        }

        public virtual void Footer()
        {
            GUILayout.Space(footerSpace);
        }

        public virtual void Save()
        {
        }

        public virtual void Load()
        {
        }
    }
}

#endif