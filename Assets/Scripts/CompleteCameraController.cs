using UnityEngine;

public class CompleteCameraController : MonoBehaviour
{
    public GameManager gameManager;

    private void Update()
    {
        if (gameManager.spawn.listCharacterList.Count != 0)
        {
            if (gameManager.spawn.listCharacterList[0] != null)
            {
                transform.position = new Vector3(gameManager.spawn.listCharacterList[0].transform.position.x, gameManager.spawn.listCharacterList[0].transform.position.y, 0);
            }
        }
    }
}