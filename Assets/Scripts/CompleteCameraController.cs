using UnityEngine;

public class CompleteCameraController : MonoBehaviour
{
    public GameManager GameManager;

    private void Update()
    {
        if (GameManager.Character != null)
        {
            transform.position = new Vector3(GameManager.Character.transform.position.x, GameManager.Character.transform.position.y, 0);
        }
    }
}