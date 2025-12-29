using System.Collections.Generic;
using System.Reflection;
using Editor.Utils.InspectorButton;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace Editor.Utils
{
    public class Button
    {
        public readonly string DisplayName;
        public readonly MethodInfo Method;
        public readonly ButtonAttribute ButtonAttribute;

        public Button(MethodInfo method, ButtonAttribute buttonAttribute)
        {
            ButtonAttribute = buttonAttribute;
            DisplayName = string.IsNullOrEmpty(buttonAttribute.Name)
                ? ObjectNames.NicifyVariableName(method.Name)
                : buttonAttribute.Name;

            Method = method;
        }

        internal void Draw(IEnumerable<object> targets)
        {
            if (!GUILayout.Button(DisplayName)) return;

            foreach (object target in targets)
            {
                Method.Invoke(target, null);
            }
        }
    }
}
#endif