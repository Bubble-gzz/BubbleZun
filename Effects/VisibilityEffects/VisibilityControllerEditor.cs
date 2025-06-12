using UnityEngine;
using UnityEditor;
using BubbleZun.Effects.VisibilityEffects;
#if UNITY_EDITOR
[CustomEditor(typeof(VisibilityController))]
public class VisibilityControllerEditor : Editor
{
    // Tweening
    private SerializedProperty status;
    private SerializedProperty hideOnStart;
    private SerializedProperty tweenPos;
    private SerializedProperty startPos;
    private SerializedProperty endPos;
    private SerializedProperty tweenPosEase;
    private SerializedProperty tweenAlpha;
    private SerializedProperty tweenAlphaEase;
    private SerializedProperty tweenDuration;

    // Disabling
    private SerializedProperty disableWhenHide;
    private SerializedProperty objectRoot;
    private SerializedProperty interactionRoot;

    // Callback
    private SerializedProperty onShow;
    private SerializedProperty onHide;

    // Foldouts
    private bool showTweening = true;
    private bool showDisabling = true;
    private bool showCallback = true;

    private GUIStyle foldoutStyle;
    private GUIStyle sectionStyle;

    private void InitStyles()
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

    private void OnEnable()
    {
        // Tweening
        status = serializedObject.FindProperty("status");
        hideOnStart = serializedObject.FindProperty("hideOnStart");
        tweenPos = serializedObject.FindProperty("tweenPos");
        startPos = serializedObject.FindProperty("startPos");
        endPos = serializedObject.FindProperty("endPos");
        tweenPosEase = serializedObject.FindProperty("tweenPosEase");
        tweenAlpha = serializedObject.FindProperty("tweenAlpha");
        tweenAlphaEase = serializedObject.FindProperty("tweenAlphaEase");
        tweenDuration = serializedObject.FindProperty("tweenDuration");

        // Disabling
        disableWhenHide = serializedObject.FindProperty("disableWhenHide");
        objectRoot = serializedObject.FindProperty("objectRoot");
        interactionRoot = serializedObject.FindProperty("interactionRoot");

        // Callback
        onShow = serializedObject.FindProperty("onShow");
        onHide = serializedObject.FindProperty("onHide");
    }

    public override void OnInspectorGUI()
    {
        InitStyles();
        serializedObject.Update();

        EditorGUILayout.PropertyField(hideOnStart);
        EditorGUILayout.PropertyField(status);
        // Tweening Section
        EditorGUILayout.Space(5);
        using (new EditorGUILayout.VerticalScope(sectionStyle))
        {
            showTweening = EditorGUILayout.Foldout(showTweening, "Tweening", true, foldoutStyle);
            if (showTweening)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    EditorGUILayout.PropertyField(tweenDuration);

                    // Position Tweening
                    using (new EditorGUILayout.VerticalScope(GUI.skin.box))
                    {
                        EditorGUILayout.PropertyField(tweenPos);
                        if (tweenPos.boolValue)
                        {
                            using (new EditorGUI.IndentLevelScope())
                            {
                                EditorGUILayout.PropertyField(startPos);
                                EditorGUILayout.PropertyField(endPos);
                                EditorGUILayout.PropertyField(tweenPosEase);
                            }
                        }
                    }

                    // Alpha Tweening
                    using (new EditorGUILayout.VerticalScope(GUI.skin.box))
                    {
                        EditorGUILayout.PropertyField(tweenAlpha);
                        if (tweenAlpha.boolValue)
                        {
                            using (new EditorGUI.IndentLevelScope())
                            {
                                EditorGUILayout.PropertyField(tweenAlphaEase);
                            }
                        }
                    }
                }
            }
        }

        // Disabling Section
        EditorGUILayout.Space(5);
        using (new EditorGUILayout.VerticalScope(sectionStyle))
        {
            showDisabling = EditorGUILayout.Foldout(showDisabling, "Disabling", true, foldoutStyle);
            if (showDisabling)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    EditorGUILayout.PropertyField(disableWhenHide);
                    if (disableWhenHide.boolValue)
                    {
                        using (new EditorGUI.IndentLevelScope())
                        {
                            EditorGUILayout.PropertyField(objectRoot);
                            EditorGUILayout.PropertyField(interactionRoot);
                        }
                    }
                }
            }
        }

        // Callback Section
        EditorGUILayout.Space(5);
        using (new EditorGUILayout.VerticalScope(sectionStyle))
        {
            showCallback = EditorGUILayout.Foldout(showCallback, "Callback", true, foldoutStyle);
            if (showCallback)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    EditorGUILayout.PropertyField(onShow);
                    EditorGUILayout.PropertyField(onHide);
                }
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif