using UnityEngine;

public class DoorInside : MonoBehaviour
{
    #region Constants

    private const string PLAYER_TAG = "Character";

    #endregion

    #region Field

    public Door door;
    public Collider2D Collider2D;

    #endregion

    #region Unity Method

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag(Door.TAG_CHARCTER))
        {
            if (door.DoorState == Door.State.Close && door.IsLock == false)
            {
                door.ChangeDoorState(Door.State.Open);
                Collider2D.enabled = false;
            }
            else if (door.DoorState == Door.State.Open && door.Room.EnemyCount > 0)
            {
                for (int i = 0; i < door.Room.Doors.Count; i++)
                {
                    door.Room.Doors[i].GetComponentInChildren<Door>().ChangeDoorState(Door.State.Close);
                    door.Room.Doors[i].GetComponentInChildren<Door>().IsLock = true;
                }
                Collider2D.enabled = true;
                door.Collider2D.enabled = false;
                door.Room.IsEnemyCreate = true;
            }
        }
    }

    #endregion
}