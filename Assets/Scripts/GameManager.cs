using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Constants

    private const string TAG_LOBBY_MANAGER = "LobbyManager";

    #endregion

    #region Fields

    public LobbyManager LobbyManager;
    public UIManager UIManager;
    public CharacterData CharacterData;
    public GunSlot GunSlot;
    public Spawn Spawn;

    public bool isPause;
    public bool isPlayerDead;

    #endregion

    #region Unity Methods

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (Spawn.CharacterList.Count > 0 && isPlayerDead == false && Spawn.CharacterList[0].isDead)
        {
            isPlayerDead = true;
            StartCoroutine(UIManager.GameOver());
        }
    }

    #endregion

    #region Private Methods

    private void Initialize()
    {
        Time.timeScale = 1;
        LobbyManager = GameObject.FindWithTag(TAG_LOBBY_MANAGER).GetComponent<LobbyManager>();
        CharacterData = LobbyManager.CharacterData;
        Destroy(LobbyManager.gameObject);
    }

    #endregion
}