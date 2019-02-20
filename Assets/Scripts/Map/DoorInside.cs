
using UnityEngine;

public class DoorInside : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";

    public Door door;
    private int FirstOpenDoor;

    #region Unity Method

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(PLAYER_TAG))
        {
            if (FirstOpenDoor == 0 && door.EnemyCount > 0)
            {
                door.Room.IsCreate = true;
                FirstOpenDoor++;
                door.GetComponent<Door>().DoorClose();
            }
        }
    }

    #endregion
}