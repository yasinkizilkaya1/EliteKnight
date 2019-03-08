using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Inventory))]
public class InventoryEditor : Editor
{
    private const string inventoryPropItemImagesName = "ItemImage";
    private const string inventoryRropItemsName = "Items";
    private const string inventoryRropItemDropName = "itemDropButton";

    private bool[] showItemSlots = new bool[Inventory.numItemSlot];
    private SerializedProperty itemImagesProperty;
    private SerializedProperty itemsProperty;
    private SerializedProperty itemDropProperty;

    private void OnEnable()
    {
        itemImagesProperty = serializedObject.FindProperty(inventoryPropItemImagesName);
        itemsProperty = serializedObject.FindProperty(inventoryRropItemsName);
        itemDropProperty = serializedObject.FindProperty(inventoryRropItemDropName);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        for (int i = 0; i < Inventory.numItemSlot; i++)
        {
            ItemSlotGUI(i);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void ItemSlotGUI(int index)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        showItemSlots[index] = EditorGUILayout.Foldout(showItemSlots[index], "Item Slot");

        if (showItemSlots[index])
        {
            EditorGUILayout.PropertyField(itemImagesProperty.GetArrayElementAtIndex(index));
            EditorGUILayout.PropertyField(itemsProperty.GetArrayElementAtIndex(index));
            EditorGUILayout.PropertyField(itemDropProperty.GetArrayElementAtIndex(index));
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }
}