using FixMathematics;
using UnityEditor;
using UnityEngine;

namespace OrthoPhysics.UnityEditor
{
    [CustomPropertyDrawer(typeof(FixVector2))]
    public class FixVector2Drawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            var contentRect = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            var labels = new[] { new GUIContent("X"), new GUIContent("Y") };
            var properties = new[] { property.FindPropertyRelative("x"), property.FindPropertyRelative("y")};
            DrawerUtility.DrawMultiplePropertyFields(contentRect, labels, properties);
            EditorGUI.EndProperty();
        }
    }
}
