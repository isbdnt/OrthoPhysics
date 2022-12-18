using FixMathematics;
using UnityEditor;
using UnityEngine;

namespace OrthoPhysics.UnityEditor
{
    [CustomPropertyDrawer(typeof(FixVector3))]
    public class FixVector3Drawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            var contentRect = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            var labels = new[] { new GUIContent("X"), new GUIContent("Y"), new GUIContent("Z") };
            var properties = new[] { property.FindPropertyRelative("x"), property.FindPropertyRelative("y"), property.FindPropertyRelative("z") };
            DrawerUtility.DrawMultiplePropertyFields(contentRect, labels, properties);
            EditorGUI.EndProperty();
        }
    }
}
