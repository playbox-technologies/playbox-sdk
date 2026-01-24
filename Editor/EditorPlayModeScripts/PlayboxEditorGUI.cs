#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Playbox
{
    public class PlayboxEditorGUI: EditorWindow
    {
        private const string objectName = "[Global] MainInitialization";
        

        [MenuItem("Playbox/Initialization/Create")]
        public static void CreateAnalyticsObject()
        {
            var findable = GameObject.Find(objectName);

            if (findable != null)
            {
                if (findable.TryGetComponent(out MainInitialization main))
                {
                    DestroyImmediate(main);
                }
                else
                {
                    findable!.AddComponent<MainInitialization>();
                }
            }
            else
            {
                var go = new GameObject(objectName);

                go.AddComponent<MainInitialization>();
            }
        }
        
        [MenuItem("Playbox/Initialization/Remove")]
        public static void RemoveAnalyticsObject()
        {
            var go = GameObject.Find(objectName);

            if (go != null)
            {
                DestroyImmediate(go);
            }
        }

    }
}

#endif