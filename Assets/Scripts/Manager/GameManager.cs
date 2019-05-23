using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    #region Enums

    public enum GameSettings
    {
        Continue,
        Stop
    }

    #endregion

    #region Constants

    public const string TAG_LOBBY = "Lobby";

    #endregion

    #region Fields

    public GameObject PauseMenuObject;
    public GameObject PanelSettingObject;
    public GameObject PanelPauseObject;
    public GameObject PanelClipObject;

    public UIManager UIManager;
    public CharacterData CharacterData;
    public Character Character;
    public GunSlot GunSlot;
    public Inventory Inventory;
    public Map Map;

    public bool IsPause;
    public bool IsPlayerDead;
    public bool IsBossSpawn;
    public bool IsUpdateChests;
    private bool mIsKeyChange;

    public KeySetting KeySetting;

    public Toggle mCurrentSelectedToggle;
    private Text mCurrentSelectedText;

    public List<Chest> Chests;

    #endregion

    #region Unity Methods

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (IsUpdateChests && Chests.Count > 0)
        {
            foreach (Chest chest in Chests)
            {
                chest.CharacterHavingGunsUnload();
            }
            IsUpdateChests = false;
        }

        if (mIsKeyChange && mCurrentSelectedToggle.isOn && mCurrentSelectedToggle != null)
        {
            KeysChange();
        }
        else if (Input.GetKeyDown(KeySetting.Keys[8].CurrentKey))
        {
            if (PauseMenuObject.activeInHierarchy == true || mIsKeyChange == false)
            {
                PlayScene();
            }
        }

        if (Input.GetKeyDown(KeySetting.Keys[7].CurrentKey) && IsPause == false)
        {
            GameSetting(GameManager.GameSettings.Stop);
        }
        else if (Input.GetKeyDown(KeySetting.Keys[8].CurrentKey) || Input.GetKeyDown(KeySetting.Keys[7].CurrentKey))
        {
            if (Inventory.gameObject.activeInHierarchy == true)
            {
                GameSetting(GameManager.GameSettings.Continue);
            }
        }

        if (Map.IsCharacterCreate == true && Character.CurrentHP == 0)
        {
            IsPlayerDead = true;
            Character.isDead = true;
            Destroy(Character.gameObject);
            StartCoroutine(UIManager.GameOver());
            UIManager.HealthBarSlider.gameObject.SetActive(false);
            Map.IsCharacterCreate = false;
        }
        else if (Map.IsCharacterCreate == true)
        {
            UIManager.HealthBarSlider.value = Character.CurrentHP;
            UIManager.ArmorBarSlider.value = Character.CurrentDefence;
            UIManager.EnergyBarSlider.value = Character.Energy;
            StartCoroutine(UIManager.SetActiveBars());

            if (Character == null)
            {
                StartCoroutine(UIManager.GameOver());
            }
        }
    }

    #endregion

    #region Private Methods

    private void Initialize()
    {
        Chests = new List<Chest>();
        Time.timeScale = 1;
        KeysLoad();
    }

    private void KeysChange()
    {
        foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                mIsKeyChange = false;
                mCurrentSelectedText.text = key.ToString();
            }
        }
    }

    private void KeysSave()
    {
        int Savingkeys = 0;

        for (int x = 0; x < UIManager.ButtonTexts.Count; x++)
        {
            for (int y = 0; y < UIManager.ButtonTexts.Count; y++)
            {
                if (UIManager.ButtonTexts[x].text == UIManager.ButtonTexts[y].text)
                {
                    Savingkeys++;
                }
            }
        }

        if (Savingkeys == UIManager.ButtonTexts.Count)
        {
            for (int i = 0; i < KeySetting.Keys.Count; i++)
            {
                KeySetting.Keys[i].CurrentKey = (KeyCode)Enum.Parse(typeof(KeyCode), UIManager.ButtonTexts[i].text);
            }
        }
        else
        {
            KeysDefaultSetting();
        }
    }

    private void KeysLoad()
    {
        for (int i = 0; i < KeySetting.Keys.Count; i++)
        {
            UIManager.ButtonTexts[i].text = KeySetting.Keys[i].CurrentKey.ToString();
        }
    }

    private void KeysDefaultSetting()
    {
        KeySetting.KeysReset();

        for (int i = 0; i < KeySetting.Keys.Count; i++)
        {
            UIManager.ButtonTexts[i].text = KeySetting.Keys[i].CurrentKey.ToString();
        }
    }

    private void PlayScene()
    {
        Time.timeScale = 1;
        PauseMenuObject.SetActive(false);
        PanelSettingObject.SetActive(false);
        PanelPauseObject.SetActive(true);
        PanelClipObject.SetActive(true);
        UIManager.GunSlotObject.SetActive(true);
        IsPause = false;
    }

    #endregion

    #region Public Methods

    public void GameSetting(GameSettings gameSettings)
    {
        switch (gameSettings)
        {
            case GameSettings.Continue:
                Inventory.gameObject.SetActive(false);
                Time.timeScale = 1;
                IsPause = false;
                break;
            case GameSettings.Stop:
                Inventory.gameObject.SetActive(true);
                Time.timeScale = 0;
                IsPause = true;
                break;
            default:
                Debug.Log("GameSetting value be on the way from");
                break;
        }
    }

    #endregion

    #region Events

    public void OnSetPauseButtonClicked()
    {
        Time.timeScale = 0;
        PauseMenuObject.SetActive(true);
        IsPause = true;
    }

    public void OnSetPlayButtonClicked()
    {
        PlayScene();
    }

    public void OnSetLobyButtonClicked()
    {
        CharacterData = null;
        SceneManager.LoadScene(TAG_LOBBY);
    }

    public void OnSetSettingButtonClicked()
    {
        PanelPauseObject.SetActive(false);
        PanelClipObject.SetActive(false);
        UIManager.GunSlotObject.SetActive(false);
        PauseMenuObject.SetActive(false);
        PanelSettingObject.SetActive(true);
        IsPause = true;
    }

    public void OnSetDefaultKeysButtonClicked()
    {
        KeysDefaultSetting();
    }

    public void OnSetSaveKeysButtonClicked()
    {
        KeysSave();
    }

    public void OnSetControlKeyChangeButtonClicked(Toggle toggle)
    {
        mIsKeyChange = true;

        for (int i = 0; i < UIManager.Toggles.Count; i++)
        {
            if (UIManager.Toggles[i] == toggle)
            {
                mCurrentSelectedToggle = toggle;
                mCurrentSelectedText = UIManager.ButtonTexts[i];
            }
        }
    }

    #endregion
}