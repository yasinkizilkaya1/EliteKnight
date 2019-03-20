using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GunSlot))]
public class GunSlotEditor : Editor
{
    private const string inventoryPropItemImagesName = "ItemImage";
    private const string inventoryRropItemsName = "Items";

    private bool[] showItemSlots;
    private SerializedProperty mItemImagesProperty;
    private SerializedProperty mItemsProperty;

    private void OnEnable()
    {
        showItemSlots = new bool[GunSlot.numItemSlot];
        mItemImagesProperty = serializedObject.FindProperty(inventoryPropItemImagesName);
        mItemsProperty = serializedObject.FindProperty(inventoryRropItemsName);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        for (int i = 0; i < GunSlot.numItemSlot; i++)
        {
            ItemSlotGUI(i);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void ItemSlotGUI(int index)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        showItemSlots[index] = EditorGUILayout.Foldout(showItemSlots[index], "Gun Slot");

        if (showItemSlots[index])
        {
            EditorGUILayout.PropertyField(mItemImagesProperty.GetArrayElementAtIndex(index),new GUIContent("Icon"));
            EditorGUILayout.PropertyField(mItemsProperty.GetArrayElementAtIndex(index),new GUIContent("Item"));
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }
}