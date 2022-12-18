using UnityEngine;
using UnityEditor;
using OrthoPhysics.UnityPlayer;

namespace OrthoPhysics.UnityEditor
{
    [CustomPropertyDrawer(typeof(DisableInPlayAttribute))]
    public class DisableInPlayAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (Application.isPlaying)
            {
                using (new EditorGUI.DisabledScope(true))
                {
                    EditorGUI.PropertyField(position, property, label, true);
                }
            }
            else
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }
    }
}