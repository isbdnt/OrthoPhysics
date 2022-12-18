using FixMathematics;
using UnityEditor;
using UnityEngine;

namespace OrthoPhysics.UnityEditor
{
    [CustomPropertyDrawer(typeof(Fix64))]
    public class Fix64Drawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.NextVisible(true))
            {
                property.longValue = ((Fix64)EditorGUI.FloatField(position, label, (float)Fix64.FromRaw(property.longValue))).rawValue;
            }
        }
    }
}
