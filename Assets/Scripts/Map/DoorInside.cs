
using UnityEngine;

public class DoorInside : MonoBehaviour
{
    #region Constants

    private const string PLAYER_TAG = "Player";

    #endregion

    #region Field

    public Door door;
    private int mFirstOpenDoor;

    #endregion

    #region Unity Method

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(PLAYER_TAG))
        {
            if (mFirstOpenDoor == 0 && door.EnemyCount > 0)
            {
                door.Room.IsCreate = true;
                mFirstOpenDoor++;

                for (int i = 0; i < door.Room.DoorList.Count; i++)
                {
                    door.Room.DoorList[i].GetComponentInChildren<Door>().DoorClose();
                }
            }
        }
    }

    #endregion
}