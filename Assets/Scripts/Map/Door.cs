using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    #region Constants

    public const string TAG_CHARCTER = "Character";

    #endregion

    #region Fields

    public Room Room;
    public DoorInside DoorInside;

    private Collider2D mLeftDoorCollider;
    private Collider2D mRightDoorCollider;
    public Collider2D Collider2D;

    public State DoorState;

    public GameObject RightDoor;
    public GameObject LeftDoor;
    public GameObject DoorLock;

    public bool IsLock;

    public Animator DoorAnimator;

    #endregion

    #region Property

    public enum State
    {
        Open,
        Close
    }

    #endregion

    #region Unity Method

    public void Start()
    {
        Initialize();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TAG_CHARCTER))
        {
            if (DoorState == State.Close && IsLock == false)
            {
                ChangeDoorState(State.Open);
                Collider2D.enabled = false;
            }
            else if (DoorState == State.Open && Room.EnemyCount > 0)
            {
                Collider2D.enabled = true;
                DoorInside.Collider2D.enabled = true;
                Room.IsEnemyCreate = true;

                for (int i = 0; i < Room.Doors.Count; i++)
                {
                    Room.Doors[i].GetComponentInChildren<Door>().ChangeDoorState(Door.State.Close);
                    Room.Doors[i].GetComponentInChildren<Door>().IsLock = true;
                }
            }
        }
    }

    #endregion

    #region Private Method

    private void Initialize()
    {
        mRightDoorCollider = RightDoor.GetComponent<Collider2D>();
        mLeftDoorCollider = LeftDoor.GetComponent<Collider2D>();
        DoorState = State.Close;
        StartCoroutine(RoomDoorsLockOpen());
    }

    #endregion

    #region Public Method

    public void ChangeDoorState(State state)
    {
        StartCoroutine(DoorEvent(state));
    }

    #endregion

    #region IEnumerator Method

    IEnumerator DoorEvent(State state)
    {
        bool isbool = state == State.Open ? false : true;
        yield return new WaitForSeconds(0.1f);
        DoorAnimator.SetBool("IsOpen", !isbool);
        float time = state == State.Open ? 0.9f : 0.1f;
        yield return new WaitForSeconds(time);
        mLeftDoorCollider.enabled = isbool;
        mRightDoorCollider.enabled = isbool;
        DoorLock.SetActive(isbool);
        DoorState = state;
    }

    IEnumerator RoomDoorsLockOpen()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            if (Room.EnemyCount == 0)
            {
                for (int i = 0; i < Room.Doors.Count; i++)
                {
                    Room.Doors[i].GetComponentInChildren<Door>().IsLock = false;
                    Room.Doors[i].GetComponentInChildren<Door>().DoorLock.SetActive(false);
                }
                StopCoroutine(RoomDoorsLockOpen());
            }
        }
    }

    #endregion
}