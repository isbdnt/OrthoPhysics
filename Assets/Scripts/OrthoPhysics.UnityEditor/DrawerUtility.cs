using System;
using UnityEditor;
using UnityEngine;

namespace OrthoPhysics.UnityEditor
{
    public static class DrawerUtility
    {
        public static float SubLabelSpacing = 4;
        public static float SubLabelExtraWidth = 4;

        public static void DrawMultiplePropertyFields(Rect position, GUIContent[] subLabels, SerializedProperty[] properties)
        {
            // backup gui settings
            var indent = EditorGUI.indentLevel;
            var labelWidth = EditorGUIUtility.labelWidth;

            // draw properties
            var propsCount = subLabels.Length;
            var width = (position.width - (propsCount - 1) * SubLabelSpacing) / propsCount;
            var contentPos = new Rect(position.x, position.y, width, position.height);
            EditorGUI.indentLevel = 0;
            for (var i = 0; i < propsCount; i++)
            {
                EditorGUIUtility.labelWidth = EditorStyles.label.CalcSize(subLabels[i]).x + SubLabelExtraWidth;
                EditorGUI.PropertyField(contentPos, properties[i], subLabels[i], true);
                contentPos.x += width + SubLabelSpacing;
            }

            // restore gui settings
            EditorGUIUtility.labelWidth = labelWidth;
            EditorGUI.indentLevel = indent;
        }

        public static void DrawMultiplePropertyFields(Rect position, GUIContent[] subLabels, Action<int, Rect, GUIContent> propertyFieldDrawer)
        {
            // backup gui settings
            var indent = EditorGUI.indentLevel;
            var labelWidth = EditorGUIUtility.labelWidth;

            // draw properties
            var propsCount = subLabels.Length;
            var width = (position.width - (propsCount - 1) * SubLabelSpacing) / propsCount;
            var contentPos = new Rect(position.x, position.y, width, position.height);
            EditorGUI.indentLevel = 0;
            for (var i = 0; i < propsCount; i++)
            {
                EditorGUIUtility.labelWidth = EditorStyles.label.CalcSize(subLabels[i]).x + SubLabelExtraWidth;
                propertyFieldDrawer(i, contentPos, subLabels[i]);
                contentPos.x += width + SubLabelSpacing;
            }

            // restore gui settings
            EditorGUIUtility.labelWidth = labelWidth;
            EditorGUI.indentLevel = indent;
        }
    }
}
