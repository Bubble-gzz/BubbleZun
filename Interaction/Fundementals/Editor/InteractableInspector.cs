using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
#if UNITY_EDITOR
namespace BubbleZun.Interaction
{
    [CustomEditor(typeof(Interactable), true)]
    public class InteractableInspector : Editor
    {
        HashSet<string> customizedProperties = new HashSet<string>();
        private SerializedProperty interactionObject;
        private SerializedProperty bindInteractionObject;
        protected SerializedProperty claimFocusOnInteraction;
        protected GUIStyle titleStyle;
        protected GUIStyle sectionStyle;
        protected GUIStyle foldoutStyle;
        private bool showInteractionObject = true;
        private bool showCallbacks = true;

        protected virtual void InitStyles()
        {
            if (foldoutStyle == null)
            {
                foldoutStyle = new GUIStyle(EditorStyles.foldout)
                {
                    fontStyle = FontStyle.Bold
                };
            }

            if (sectionStyle == null)
            {
                sectionStyle = new GUIStyle(GUI.skin.box)
                {
                    padding = new RectOffset(10, 10, 5, 5),
                    margin = new RectOffset(0, 0, 5, 5)
                };
            }
        }

        protected virtual void OnEnable()
        {
            interactionObject = serializedObject.FindProperty("interactionObject");
            bindInteractionObject = serializedObject.FindProperty("bindInteractionObject");
            claimFocusOnInteraction = serializedObject.FindProperty("claimFocusOnInteraction");
            customizedProperties.Clear();
            customizedProperties.Add("interactionObject");
            customizedProperties.Add("bindInteractionObject");
            customizedProperties.Add("claimFocusOnInteraction");
            customizedProperties.Add("m_Script");
        }

        public override void OnInspectorGUI()
        {
            if (foldoutStyle != null)
            {
                foldoutStyle.normal = EditorStyles.foldout.normal;
                foldoutStyle.onNormal = EditorStyles.foldout.onNormal;
                foldoutStyle.hover = EditorStyles.foldout.hover;
                foldoutStyle.onHover = EditorStyles.foldout.onHover;
                foldoutStyle.focused = EditorStyles.foldout.focused;
                foldoutStyle.onFocused = EditorStyles.foldout.onFocused;
                foldoutStyle.active = EditorStyles.foldout.active;
                foldoutStyle.onActive = EditorStyles.foldout.onActive;
            }

            InitStyles();
            serializedObject.Update();

            DrawInteractionObjectSection();
            DrawCallBackSection();
            DrawOtherSettingSection();

            serializedObject.ApplyModifiedProperties();
        }
        protected virtual void DrawInteractionObjectSection()
        {
            EditorGUILayout.Space(5);
            using (new EditorGUILayout.VerticalScope(sectionStyle))
            {
                showInteractionObject = EditorGUILayout.Foldout(showInteractionObject, "Interaction Object", true, foldoutStyle);
                if (showInteractionObject)
                {
                    using (new EditorGUI.IndentLevelScope())
                    {
                        EditorGUILayout.PropertyField(bindInteractionObject);
                        if (bindInteractionObject.boolValue)
                        {
                            using (new EditorGUI.IndentLevelScope())
                            {
                                EditorGUILayout.PropertyField(interactionObject);
                                if (claimFocusOnInteraction != null) EditorGUILayout.PropertyField(claimFocusOnInteraction);
                            }
                        }
                    }
                }
            }
        }
        protected virtual void DrawOtherSettingSection()
        {
            EditorGUILayout.Space(5);
            var iterator = serializedObject.GetIterator();
            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;
                if (!customizedProperties.Contains(iterator.name))
                {
                    EditorGUILayout.PropertyField(iterator, true);
                }
            }
        }
        protected virtual void DrawCallBackSection()
        {
            EditorGUILayout.Space(5);
            using (new EditorGUILayout.VerticalScope(sectionStyle))
            {
                showCallbacks = EditorGUILayout.Foldout(showCallbacks, "Callbacks", true, foldoutStyle);
                if (showCallbacks)
                {
                    using (new EditorGUI.IndentLevelScope())
                    {
                        var iterator = serializedObject.GetIterator();
                        bool enterChildren = true;
                        
                        while (iterator.NextVisible(enterChildren))
                        {
                            enterChildren = false;
                            if (iterator.propertyType == SerializedPropertyType.Generic && 
                                iterator.type.Contains("UnityEvent"))
                            {
                                EditorGUILayout.PropertyField(iterator, true);
                                customizedProperties.Add(iterator.name);
                            }
                        }
                    }
                }
            }
        }
    }
} 
#endif