using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(GunSlot))]
public class GunSlotEditor : Editor
{
    private const string mInventoryPropItemImagesName = "ItemImage";
    private const string mInventoryRropItemsName = "Items";

    private bool[] mShowItemSlots;
    private SerializedProperty mItemImagesProperty;
    private SerializedProperty mItemsProperty;

    private void OnEnable()
    {
        mShowItemSlots = new bool[GunSlot.numItemSlot];
        mItemImagesProperty = serializedObject.FindProperty(mInventoryPropItemImagesName);
        mItemsProperty = serializedObject.FindProperty(mInventoryRropItemsName);
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

        mShowItemSlots[index] = EditorGUILayout.Foldout(mShowItemSlots[index], "Gun Slot");

        if (mShowItemSlots[index])
        {
            EditorGUILayout.PropertyField(mItemImagesProperty.GetArrayElementAtIndex(index),new GUIContent("Icon"));
            EditorGUILayout.PropertyField(mItemsProperty.GetArrayElementAtIndex(index),new GUIContent("Item"));
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }
}
#endif