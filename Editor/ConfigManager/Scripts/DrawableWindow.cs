using Newtonsoft.Json.Linq;

#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

using PGUI = Editor.Utils.Layout.PlayboxLayout;

namespace Playbox.SdkWindow
{
    public class DrawableWindow
    {
        public string name;
        public bool active;
        public bool hasUnsavedChanges;
        
        protected static float field_width = 350;
        protected static float field_height = 15;
        protected static float footerSpace = 10;
        protected static float headerSpace = 10;
    
        protected JObject configuration;

        public static float FieldWidth
        {
            get => field_width;
            set => field_width = value;
        }

        public static float FieldHeight
        {
            get => field_height;
            set => field_height = value;
        }

        public virtual void InitName()
        {
            configuration = new JObject();
        }

        public virtual void HasRenderToggle()
        {
            GUILayout.BeginHorizontal();
            
            GUILayout.Label(name,GUILayout.ExpandWidth(false),GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
            active = EditorGUILayout.Toggle("", active,GUILayout.ExpandWidth(false),GUILayout.Height(FieldHeight), GUILayout.Width(FieldWidth));
            
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