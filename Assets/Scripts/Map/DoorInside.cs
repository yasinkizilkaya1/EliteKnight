
using UnityEngine;

public class DoorInside : MonoBehaviour
{
    #region Constants

    private const string PLAYER_TAG = "Character";

    #endregion

    #region Field

    public Door door;

    #endregion

    #region Unity Method

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(PLAYER_TAG))
        {
            if (door.DoorState == Door.State.Open && door.EnemyCount > 0 && door.Christen == false)
            {
                door.Room.IsCreate = true;
                for (int i = 0; i < door.Room.DoorList.Count; i++)
                {
                    door.Room.DoorList[i].GetComponentInChildren<Door>().ChangeDoorState(Door.State.Close);
                }
            }

            if (door.DoorState == Door.State.Close && door.Christen == true)
            {
                door.GetComponentInChildren<Door>().ChangeDoorState(Door.State.Open);
            }
        }
    }

    #endregion
}