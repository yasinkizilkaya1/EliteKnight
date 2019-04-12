
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
            if (door.DoorState == Door.State.Open && door.Room.EnemyCount > 0 && door.IsChristen == false)
            {
                door.Room.IsCreate = true;
                for (int i = 0; i < door.Room.DoorList.Count; i++)
                {
                    door.Room.DoorList[i].GetComponentInChildren<Door>().ChangeDoorState(Door.State.Close);
                    door.Room.DoorList[i].GetComponentInChildren<Door>().IsLock = false;
                }
            }

            if (door.DoorState == Door.State.Close && door.IsChristen == true && door.IsLock == false)
            {
                door.GetComponentInChildren<Door>().ChangeDoorState(Door.State.Open);
            }
        }
    }

    #endregion
}