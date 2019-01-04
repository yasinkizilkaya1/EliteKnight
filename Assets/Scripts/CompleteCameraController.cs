using UnityEngine;

public class CompleteCameraController : MonoBehaviour
{
    public GameManager gameManager;

    private void Update()
    {
        if (gameManager.spawn.CharacterList.Count != 0)
        {
            if (gameManager.spawn.CharacterList[0] != null)
            {
                transform.position = new Vector3(gameManager.spawn.CharacterList[0].transform.position.x, gameManager.spawn.CharacterList[0].transform.position.y, 0);
            }
        }
    }
}