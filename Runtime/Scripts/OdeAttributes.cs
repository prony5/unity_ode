using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityODE
{
    #region OdeReadOnly
    public class OdeReadOnly : PropertyAttribute { }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(OdeReadOnly))]
    public class OdeReadOnlyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false; // Disable fields
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true; // Enable fields
        }
    }
#endif
    #endregion

    #region OdeReadOnlyOnPlay
    public class OdeReadOnlyOnPlay : PropertyAttribute { }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(OdeReadOnlyOnPlay))]
    public class OdeReadOnlyOnPlayDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (Application.isPlaying)
            {
                GUI.enabled = false; // Disable fields
                EditorGUI.PropertyField(position, property, label, true);
                GUI.enabled = true; // Enable fields
            }
            else
                EditorGUI.PropertyField(position, property, label, true);
        }
    }
#endif
    #endregion
}

