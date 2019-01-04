using System.IO;
using UnityEditor;
using UnityEngine;

public class EnemyDesignerWindow : EditorWindow
{
    private Texture2D headerSectionTexture;
    private Texture2D warriorSectionTexture;

    private Color headerSectionColor = new Color(13f / 255f, 32f / 255f, 44f / 255f, 1f);

    private Rect headerSection;
    private Rect warriorSection;

    private GUISkin skin;
    private GUISkin skin2;

    public static WarriorData asset;
    public static WarriorData warriorData;
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
        skin = Resources.Load<GUISkin>("GUISkin/EnemyDesignerSkin");
        skin2 = Resources.Load<GUISkin>("GUISkin/EnemyDesignerSkin2");
    }

    public static void InitData()
    {
        warriorData = (WarriorData)ScriptableObject.CreateInstance(typeof(WarriorData));
    }

    private void InitTextures()
    {
        headerSectionTexture = new Texture2D(1, 1);
        headerSectionTexture.SetPixel(0, 0, headerSectionColor);
        headerSectionTexture.Apply();

        warriorSectionTexture = Resources.Load<Texture2D>("Icons/backgound_warrior_blue");
    }

    private void OnGUI()
    {
        DrawLayouts();
        DrawHeader();
        DrawWarriorSettings(warriorData);
    }

    private void DrawLayouts()
    {
        headerSection.x = 0;
        headerSection.y = 0;
        headerSection.width = Screen.width;
        headerSection.height = 50;

        warriorSection.x = 0;
        warriorSection.y = 50;
        warriorSection.width = Screen.width;
        warriorSection.height = Screen.width - 50;

        GUI.DrawTexture(headerSection, headerSectionTexture);
        GUI.DrawTexture(warriorSection, warriorSectionTexture);
    }

    private void DrawHeader()
    {
        GUILayout.BeginArea(headerSection);
        GUILayout.Label("Enemy Designer", skin.GetStyle("Header1"));
        GUILayout.EndArea();
    }

    public void DrawWarriorSettings(CharacterData charData)
    {
        asset = ScriptableObject.CreateInstance<WarriorData>();

        GUILayout.BeginArea(warriorSection);

        GUILayout.Label("       ", skin.GetStyle("Header1"));

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Name", skin2.GetStyle("Header2"));
        charData.Name = EditorGUILayout.TextField(charData.Name);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Health", skin2.GetStyle("Header2"));
        charData.Health = EditorGUILayout.IntField(charData.Health);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Energy", skin2.GetStyle("Header2"));
        charData.Energy = EditorGUILayout.IntField(charData.Energy);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Power", skin2.GetStyle("Header2"));
        charData.Power = EditorGUILayout.IntField(charData.Power);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Defance", skin2.GetStyle("Header2"));
        charData.Defence = EditorGUILayout.IntField(charData.Defence);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Speed", skin2.GetStyle("Header2"));
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