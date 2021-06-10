using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(InventorySlot))]
public class InventorySlotEditor : PropertyDrawer
{
    private bool showArmorSubType;
    private bool hasRestriction;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.indentLevel = 2;
        position.height = 16f;
        hasRestriction = property.FindPropertyRelative("hasRestriction").boolValue;
        if (!hasRestriction)
        {
            property.FindPropertyRelative("ItemType").intValue = -1;
        }
        else if (property.FindPropertyRelative("ItemType").intValue == -1)
        {
            property.FindPropertyRelative("ItemType").intValue = 0;
        }
        showArmorSubType = (ItemType)property.FindPropertyRelative("ItemType").intValue == ItemType.Armor;
        if (!showArmorSubType) property.FindPropertyRelative("armorSubType").intValue = -1;
        else if (property.FindPropertyRelative("armorSubType").intValue == -1) property.FindPropertyRelative("armorSubType").intValue = 0;
        property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label);
        if (property.isExpanded)
        {
            EditorGUI.indentLevel = 3;
            SetPropertyField(IncreasePosY(ref position), property, label, "ID");
            SetPropertyField(IncreasePosY(ref position), property, label, "item");
            SetPropertyField(IncreasePosY(ref position), property, label, "amount");
            SetPropertyField(IncreasePosY(ref position), property, label, "hasRestriction");
            if (hasRestriction)
            {
                EditorGUI.indentLevel = 4;
                SetPropertyField(IncreasePosY(ref position), property, label, "isWhitelist");
                SetPropertyField(IncreasePosY(ref position), property, label, "ItemType");
                if (showArmorSubType) SetPropertyField(IncreasePosY(ref position), property, label, "armorSubType");
                EditorGUI.indentLevel = 3;
            }
            SetPropertyField(IncreasePosY(ref position), property, label, "weight");
            SetPropertyField(IncreasePosY(ref position), property, label, "value");
            SetPropertyField(IncreasePosY(ref position), property, label, "filteredOut");
            SetPropertyField(IncreasePosY(ref position), property, label, "parent");
        }
        EditorGUI.EndProperty();
    }

    private void SetPropertyField(Rect position, SerializedProperty property, GUIContent label, string propertyName)
    {
        label.text = propertyName;
        EditorGUI.PropertyField(position, property.FindPropertyRelative(propertyName), label);
    }

    private Rect IncreasePosY(ref Rect position)
    {
        position.y += EditorGUIUtility.singleLineHeight;
        return position;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return property.isExpanded ? EditorGUIUtility.singleLineHeight * 12 : EditorGUIUtility.singleLineHeight;
    }
}