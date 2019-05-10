using System.IO;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class EnemyDesignerWindow : EditorWindow
{
    private Texture2D mHeaderSectionTexture;
    private Texture2D mWarriorSectionTexture;

    private Color mHeaderSectionColor = new Color(13f / 255f, 32f / 255f, 44f / 255f, 1f);

    private Rect mHeaderSection;
    private Rect mWarriorSection;

    private GUISkin mSkin;
    private GUISkin mSkin2;

    public static CharacterData asset;
    public static CharacterData CharacterData;
    public static int ID;

    [MenuItem("Window/Character Designer")]
    private static void OpenWindow()
    {
        EnemyDesignerWindow window = (EnemyDesignerWindow)GetWindow(typeof(EnemyDesignerWindow));
        window.minSize = new Vector2(400, 200);
        window.Show();
    }

    private static void CloseWindow()
    {
        EnemyDesignerWindow window = (EnemyDesignerWindow)GetWindow(typeof(EnemyDesignerWindow));
        window.Close();
    }

    private void OnEnable()
    {
        ID = TotalMapCount;
        InitTextures();
        InitData();
        mSkin = Resources.Load<GUISkin>("GUISkin/EnemyDesignerSkin");
        mSkin2 = Resources.Load<GUISkin>("GUISkin/EnemyDesignerSkin2");
    }

    public static void InitData()
    {
        CharacterData = (CharacterData)ScriptableObject.CreateInstance(typeof(CharacterData));
    }

    private void InitTextures()
    {
        mHeaderSectionTexture = new Texture2D(1, 1);
        mHeaderSectionTexture.SetPixel(0, 0, mHeaderSectionColor);
        mHeaderSectionTexture.Apply();

        mWarriorSectionTexture = Resources.Load<Texture2D>("Icons/backgound_warrior_blue");
    }

    private void OnGUI()
    {
        DrawLayouts();
        DrawHeader();
        DrawWarriorSettings(CharacterData);
    }

    private void DrawLayouts()
    {
        mHeaderSection.x = 0;
        mHeaderSection.y = 0;
        mHeaderSection.width = Screen.width;
        mHeaderSection.height = 50;

        mWarriorSection.x = 0;
        mWarriorSection.y = 50;
        mWarriorSection.width = Screen.width;
        mWarriorSection.height = Screen.width - 50;

        GUI.DrawTexture(mHeaderSection, mHeaderSectionTexture);
        GUI.DrawTexture(mWarriorSection, mWarriorSectionTexture);
    }

    private void DrawHeader()
    {
        GUILayout.BeginArea(mHeaderSection);
        GUILayout.Label("Enemy Designer", mSkin.GetStyle("Header1"));
        GUILayout.EndArea();
    }

    public void DrawWarriorSettings(CharacterData charData)
    {
        asset = ScriptableObject.CreateInstance<CharacterData>();

        GUILayout.BeginArea(mWarriorSection);

        GUILayout.Label("       ", mSkin.GetStyle("Header1"));

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Name", mSkin2.GetStyle("Header2"));
        charData.Name = EditorGUILayout.TextField(charData.Name);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Health", mSkin2.GetStyle("Header2"));
        charData.Health = EditorGUILayout.IntField(charData.Health);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Energy", mSkin2.GetStyle("Header2"));
        charData.Energy = EditorGUILayout.IntField(charData.Energy);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Power", mSkin2.GetStyle("Header2"));
        charData.Power = EditorGUILayout.IntField(charData.Power);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Defance", mSkin2.GetStyle("Header2"));
        charData.Defence = EditorGUILayout.IntField(charData.Defence);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Speed", mSkin2.GetStyle("Header2"));
        charData.Speed = EditorGUILayout.IntField(charData.Speed);
        EditorGUILayout.EndHorizontal();

        asset.Name = charData.Name;
        asset.Health = charData.Health;
        asset.MaxHealth = charData.Health;
        asset.Energy = charData.Energy;
        asset.MaxEnergy = charData.Energy;
        asset.Power = charData.Power;
        asset.Defence = charData.Defence;
        asset.Speed = charData.Speed;

        if (charData.Name == null || charData.Name.Length < 1 || charData.Name.Length > 16)
        {
            EditorGUILayout.HelpBox("This character needs a [Name] before it can be created.", MessageType.Warning);
        }
        else if (GUILayout.Button("Finish and Save", GUILayout.Height(30)))
        {
            ID += 1;
            asset.Id = ID;
            AssetDatabase.CreateAsset(asset, "Assets/Data/CharacterData/" + charData.Name + ".asset");
            AssetDatabase.SaveAssets();
            Selection.activeObject = asset;
            CloseWindow();
        }

        GUILayout.EndArea();
    }

    public static int TotalMapCount
    {
        get
        {
            return Directory.GetFiles(Application.dataPath + "/Data/CharacterData/", "*.asset", SearchOption.AllDirectories).Length;
        }
    }

    public class GeneralSettings : EditorWindow
    {
        public enum SettingsType
        {
            Warrior
        }
    }
}

#endif