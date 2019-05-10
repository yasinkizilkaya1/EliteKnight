using UnityEngine;

public class DoorInside : MonoBehaviour
{
    #region Field

    public Door Door;
    public Collider2D Collider2D;

    public bool IsLogin;

    #endregion

    #region Unity Method

    private void Update()
    {
        if (IsLogin)
        {
            Door.Collider2D.enabled = true;
            IsLogin = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag(Door.mTAG_CHARCTER))
        {
            Door.IsRoomLogin = true;

            if (Door.DoorState == Door.State.Close && Door.IsLock == false)
            {
                IsLogin=true;
                Door.ChangeDoorState(Door.State.Open);
                Collider2D.enabled = false;
                Door.Collider2D.enabled = true;
            }
            else if (Door.DoorState == Door.State.Open && Door.Room.EnemyCount > 0 )
            {
                Collider2D.enabled = true;
                Door.Collider2D.enabled = true;
                Door.Room.IsEnemyCreate = true;

                for (int i = 0; i < Door.Room.Doors.Count; i++)
                {
                    Door.Room.Doors[i].ChangeDoorState(Door.State.Close);
                    Door.Room.Doors[i].IsLock = true;
                }
            }
        }
    }

    #endregion
}