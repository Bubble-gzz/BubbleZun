/*
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR
namespace BubbleZun.Interaction
{
    
    [CustomEditor(typeof(MouseInteractable), true)]
    public class MouseInteractableInspector : InteractableInspector
    {
        private SerializedProperty blockNonUIObject;
        private SerializedProperty useScreenSpace;
        private SerializedProperty transparent;
        private SerializedProperty detectArea;
        private bool isInCanvas;
        private bool showMouseDetection = true;

        protected override void OnEnable()
        {
            base.OnEnable();
            var targetComponent = (MouseInteractable)target;
            isInCanvas = targetComponent.GetComponentInParent<Canvas>() != null;

            blockNonUIObject = serializedObject.FindProperty("blockNonUIObject");
            useScreenSpace = serializedObject.FindProperty("useScreenSpace");
            transparent = serializedObject.FindProperty("transparent");
            detectArea = serializedObject.FindProperty("detectArea");
        }

        protected override void DrawOtherSettingSection()
        {
            EditorGUILayout.Space(5);
            using (new EditorGUILayout.VerticalScope(sectionStyle))
            {
                showMouseDetection = EditorGUILayout.Foldout(showMouseDetection, "Mouse Detection", true, foldoutStyle);
                if (showMouseDetection)
                {
                    using (new EditorGUI.IndentLevelScope())
                    {
                        if (isInCanvas) EditorGUILayout.PropertyField(blockNonUIObject);
                        EditorGUILayout.PropertyField(transparent);
                        if (!isInCanvas) EditorGUILayout.PropertyField(detectArea);
                    }
                }
            }
        }
    }
} 
#endif
*/