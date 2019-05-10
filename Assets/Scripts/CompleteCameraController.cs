using UnityEngine;

public class CompleteCameraController : MonoBehaviour
{
    #region Fields

    public GameManager GameManager;
    public Camera MainCamera;

    #endregion

    #region Unity Methods

    private void Update()
    {
        if (GameManager.Character != null)
        {
            transform.position = new Vector3(GameManager.Character.transform.position.x, GameManager.Character.transform.position.y, 0);
        }

        if(GameManager.IsBossSpawn)
        {
            MainCamera.orthographicSize = 20;
        }
    }

    #endregion
}