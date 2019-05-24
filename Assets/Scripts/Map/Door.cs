using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    #region Constants

    public const string mTAG_CHARCTER = "Character";

    #endregion

    #region Fields

    public Room Room;
    public DoorInside DoorInside;
    public Animator DoorAnimator;

    public Collider2D LeftDoorCollider;
    public Collider2D RightDoorCollider;
    public Collider2D Collider2D;

    public State DoorState;

    public GameObject DoorLock;

    public bool IsLock;
    public bool IsRoomLogin;

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
        if (collision.gameObject.CompareTag(mTAG_CHARCTER))
        {
            IsRoomLogin = true;

            if (DoorState == State.Close && IsLock == false)
            {
                ChangeDoorState(State.Open);
                Collider2D.enabled = false;
                DoorInside.Collider2D.enabled = true;
            }
            else if (DoorState == State.Open && Room.EnemyCount > 0)
            {
                Collider2D.enabled = true;
                DoorInside.Collider2D.enabled = true;
                Room.IsEnemyCreate = true;

                for (int i = 0; i < Room.Doors.Count; i++)
                {
                    Room.Doors[i].ChangeDoorState(Door.State.Close);
                    Room.Doors[i].IsLock = true;
                }
            }
        }
    }

    #endregion

    #region Private Method

    private void Initialize()
    {
        DoorState = State.Close;
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
        LeftDoorCollider.enabled = isbool;
        RightDoorCollider.enabled = isbool;
        DoorLock.SetActive(isbool);
        DoorState = state;
        LeftDoorCollider.gameObject.SetActive(isbool);
        RightDoorCollider.gameObject.SetActive(isbool);
    }

    #endregion
}