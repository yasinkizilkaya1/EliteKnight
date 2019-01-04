using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Constants

    private const string Keys_Bill = ".asset";
    private const string Keys_Path = "Assets/Data/KeyData/";
    private const string TAG_CHARACTER = "Body";
    private const string TAG_SUPPORT = "Support";
    private const string TAG_LOBBY = "Lobby";
    private const string Saving_Key_Name = "Keys";
    private const int MAXVALUE = 100;

    #endregion

    #region Fields

    public GameObject StageObject;
    public GameObject PlayerItemPanelObject;
    public GameObject SliderObject;
    public GameObject PauseMenuObject;
    public GameObject LoadingPanelObject;
    public GameObject LoadingSliderObject;
    public GameObject PanelSettingObject;
    public GameObject AutoReloadObject;

    public Text LoadingText;
    public CharacterID characterID;
    public Spawn spawn;
    public TowerEnemy towerEnemy;
    public AmmoBar ammoBar;
    public Slider slider;
    private Keys keys;

    public string SelectedCardNameString;

    public bool isPause;
    public bool isPlayerDead;

    public List<Toggle> ToggleList;
    public List<Text> TextList;
    public List<string> DefaultkeycapList;
    public List<string> keycapList;

    public KeyCode UpEnum;
    public KeyCode DownEnum;
    public KeyCode RightEnum;
    public KeyCode LeftEnum;
    public KeyCode ReloadEnum;
    public KeyCode RunEnum;

    #endregion

    #region Unity Methods

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (isPlayerDead)
        {
            StartCoroutine(GameOver());
        }

        if (slider != null)
        {
            if (slider.value <= 100)
            {
                StartCoroutine(Loading());
            }

            if (slider.value == 100 && isPlayerDead == false)
            {
                if (SelectedCardNameString == TAG_SUPPORT)
                {
                    AutoReloadObject.SetActive(false);
                    SliderObject.SetActive(false);
                }
                else
                {
                    SliderObject.SetActive(true);
                    AutoReloadObject.SetActive(true);
                }
                LoadingPanelObject.SetActive(false);
                StageObject.SetActive(true);
                PlayerItemPanelObject.SetActive(true);
                StopAllCoroutines();

                if (spawn.CharacterList.Count != 0)
                {
                    if (spawn.CharacterList[0] == null)
                    {
                        isPlayerDead = true;
                    }
                    else
                    {
                        isPlayerDead = false;
                    }
                }
            }
        }
    }

    private void OnGUI()
    {
        if (isPause == true)
        {
            for (int i = 0; i < TextList.Count; i++)
            {
                if (ToggleList[i].isOn == true)
                {
                    KeysChange(i);
                }
            }
        }
    }

    #endregion

    #region Private Methods

    private void Initialize()
    {
        keys = (Keys)AssetDatabase.LoadAssetAtPath(Keys_Path + Saving_Key_Name + Keys_Bill, typeof(Keys));
        keycapList = new List<string>() { keys.Up, keys.Down, keys.Left, keys.Right, keys.Reload, keys.Run };
        DefaultkeycapList = new List<string>() { "W", "S", "A", "D", "R", "LeftShift" };

        if (characterID == null)
        {
            characterID = GameObject.FindWithTag("selectname").GetComponent<CharacterID>();
            SelectedCardNameString = characterID.SelectedCardNameString;
        }

        StartKeys();
        Time.timeScale = 1;
        slider.maxValue = MAXVALUE;
    }

    private void KeysChange(int id)
    {
        Event e = Event.current;

        if (e.isKey)
        {
            TextList[id].text = e.keyCode.ToString();
            keycapList[id] = e.keyCode.ToString();
        }
    }

    #endregion

    #region Public Methods

    public void StartKeys()
    {
        Key(keycapList[0], keycapList[1], keycapList[2], keycapList[3], keycapList[4], keycapList[5]);

        for (int i = 0; i < ToggleList.Count; i++)
        {
            TextList[i].text = keycapList[i];
        }
    }

    public void DefaultKeys()
    {
        Key(DefaultkeycapList[0], DefaultkeycapList[1], DefaultkeycapList[2], DefaultkeycapList[3], DefaultkeycapList[4], DefaultkeycapList[5]);

        for (int i = 0; i < ToggleList.Count; i++)
        {
            TextList[i].text = DefaultkeycapList[i];
            keycapList[i] = DefaultkeycapList[i];
        }
    }

    public void SaveKeys()
    {
        int Savingkeys = 0;

        for (int x = 0; x < keycapList.Count; x++)
        {
            for (int y = keycapList.Count - 1; y > 0; y--)
            {
                if (keycapList[x] == TextList[y].text)
                {
                    Savingkeys++;
                }
            }
        }

        if (Savingkeys == keycapList.Count - 1)
        {
            Key(keycapList[0], keycapList[1], keycapList[2], keycapList[3], keycapList[4], keycapList[5]);
        }
        else
        {
            DefaultKeys();
        }
    }

    private void Key(string up, string down, string left, string right, string reload, string run)
    {
        UpEnum = (KeyCode)Enum.Parse(typeof(KeyCode), up);
        DownEnum = (KeyCode)Enum.Parse(typeof(KeyCode), down);
        RightEnum = (KeyCode)Enum.Parse(typeof(KeyCode), right);
        LeftEnum = (KeyCode)Enum.Parse(typeof(KeyCode), left);
        ReloadEnum = (KeyCode)Enum.Parse(typeof(KeyCode), reload);
        RunEnum = (KeyCode)Enum.Parse(typeof(KeyCode), run);

        keys.Up = up;
        keys.Down = down;
        keys.Left = left;
        keys.Right = right;
        keys.Reload = reload;
        keys.Run = run;
    }

    public void ApplicationisPause()
    {
        Time.timeScale = 0;
        PauseMenuObject.SetActive(true);
        isPause = true;
    }

    public void ApplicationPlay()
    {
        Time.timeScale = 1;
        PauseMenuObject.SetActive(false);
        PanelSettingObject.SetActive(false);
        isPause = false;
    }

    public void Lobi()
    {
        SelectedCardNameString = "";
        SceneManager.LoadScene(TAG_LOBBY);
    }

    public void Settings()
    {
        PauseMenuObject.SetActive(false);
        PanelSettingObject.SetActive(true);
        isPause = true;
    }

    #endregion

    #region Enumerator Methods

    IEnumerator Loading()
    {
        if (slider.value <= 100)
        {
            yield return new WaitForSeconds(0.8f);
            slider.value++;
        }
    }

    IEnumerator GameOver()
    {
        LoadingPanelObject.SetActive(true);
        LoadingSliderObject.SetActive(false);
        LoadingText.text = "GAME OVER";
        LoadingText.fontSize = 60;
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(TAG_LOBBY);
    }

    #endregion
}