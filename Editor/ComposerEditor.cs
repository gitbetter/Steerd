using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using Steerd;

[CustomPropertyDrawer(typeof(BehaviorInfo))]
[CanEditMultipleObjects]
public class BehaviorInfoEditor : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        float behaviorRectHeight = 0;
        float targetRectHeight = 0;
        float blendingWeightRectHeight = 0;

        EditorGUIUtility.labelWidth = 60;
        SerializedProperty behaviorProp = property.FindPropertyRelative("behavior");
        behaviorRectHeight = EditorGUI.GetPropertyHeight(behaviorProp);
        var behaviorRect = new Rect(position.x, position.y, position.width, behaviorRectHeight);
        EditorGUI.PropertyField(behaviorRect, behaviorProp, new GUIContent("Behavior"));

        Behavior behavior = behaviorProp.objectReferenceValue as Behavior;
        if (behavior != null) {
            SerializedProperty targetProp = null;
            string targetLabelString = "";
            if ((behavior.flags & Steerd.Flags.SINGLE_TARGET) != Steerd.Flags.NONE) {
                targetProp = property.FindPropertyRelative("target");
                targetLabelString = "Target";
            }
            if ((behavior.flags & Steerd.Flags.PATH_FOLLOWER) != Steerd.Flags.NONE) {
                targetProp = property.FindPropertyRelative("path");
                targetLabelString = "Path";
            }
            if ((behavior.flags & Steerd.Flags.MULTI_TARGET) != Steerd.Flags.NONE) {
                targetProp = property.FindPropertyRelative("targets");
                targetLabelString = "Targets";
            }
            targetRectHeight = EditorGUI.GetPropertyHeight(targetProp);
            var targetRect = new Rect(position.x, position.y + behaviorRectHeight, position.width, targetRectHeight);
            EditorGUI.PropertyField(targetRect, targetProp, new GUIContent(targetLabelString), true);
        }

        EditorGUIUtility.labelWidth = 120;
        SerializedProperty blendingWeightProp = property.FindPropertyRelative("blendingWeight");
        blendingWeightRectHeight = EditorGUI.GetPropertyHeight(blendingWeightProp);
        var blendingWeightRect = new Rect(position.x, position.y + behaviorRectHeight + targetRectHeight, position.width, blendingWeightRectHeight);
        EditorGUI.PropertyField(blendingWeightRect, blendingWeightProp, new GUIContent("Blending Weight"));

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        SerializedProperty behaviorProp = property.FindPropertyRelative("behavior");
        SerializedProperty targetProp = null;
        Behavior behavior = behaviorProp.objectReferenceValue as Behavior;
        if (behavior != null) {
            if ((behavior.flags & Steerd.Flags.SINGLE_TARGET) != Steerd.Flags.NONE) {
                targetProp = property.FindPropertyRelative("target");
            }
            if ((behavior.flags & Steerd.Flags.PATH_FOLLOWER) != Steerd.Flags.NONE) {
                targetProp = property.FindPropertyRelative("path");
            }
            if ((behavior.flags & Steerd.Flags.MULTI_TARGET) != Steerd.Flags.NONE) {
                targetProp = property.FindPropertyRelative("targets");
            }
        }
        SerializedProperty blendingWeightProp = property.FindPropertyRelative("blendingWeight");

        float totalHeight = 0;
        totalHeight += EditorGUI.GetPropertyHeight(behaviorProp);
        if (targetProp != null) {
            totalHeight += EditorGUI.GetPropertyHeight(targetProp);
        }
        totalHeight += EditorGUI.GetPropertyHeight(blendingWeightProp);
        return totalHeight + 10;
    }
}