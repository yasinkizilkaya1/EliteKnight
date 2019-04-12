using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    #region Constants

    private const string TAG_CHARCTER = "Character";

    #endregion

    #region Fields

    public Room Room;

    private Collider2D mLeftDoorCollider;
    private Collider2D mRightDoorCollider;

    public State DoorState;

    public GameObject RightDoor;
    public GameObject LeftDoor;
    public GameObject DoorLock;

    public bool IsChristen;
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
            if (DoorState == State.Close && IsChristen == true && IsLock == false)
            {
                ChangeDoorState(State.Open);
                IsChristen = false;
            }

            if (DoorState == State.Open && Room.EnemyCount > 0 && IsChristen == true)
            {
                for (int i = 0; i < Room.DoorList.Count; i++)
                {
                    Room.DoorList[i].GetComponentInChildren<Door>().ChangeDoorState(Door.State.Close);
                    Room.DoorList[i].GetComponentInChildren<Door>().IsChristen = false;
                    Room.DoorList[i].GetComponentInChildren<Door>().IsLock = true;
                }
                Room.IsCreate = true;
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
        IsChristen = true;
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
        DoorState = state;
        bool isbool = DoorState == State.Open ? false : true;
        yield return new WaitForSeconds(0.2f);
        DoorAnimator.SetBool("IsOpen", !isbool);
        float time = state == State.Open ? 0.9f : 0.2f;
        yield return new WaitForSeconds(time);
        mLeftDoorCollider.enabled = isbool;
        mRightDoorCollider.enabled = isbool;
        DoorLock.SetActive(isbool);
    }

    IEnumerator RoomDoorsLockOpen()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            if (Room.EnemyCount == 0)
            {
                for (int i = 0; i < Room.DoorList.Count; i++)
                {
                    Room.DoorList[i].GetComponentInChildren<Door>().IsLock = false;
                    Room.DoorList[i].GetComponentInChildren<Door>().IsChristen = true;
                    Room.DoorList[i].GetComponentInChildren<Door>().DoorLock.SetActive(false);
                }
                StopCoroutine(RoomDoorsLockOpen());
            }
        }
    }

    #endregion
}