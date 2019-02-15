using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    #region Constants

    private const string TAG_CHARCTER = "Player";

    #endregion

    #region Fields

    private Room mRoom;

    private Collider2D mLeftDoorCollider;
    private Collider2D mRightDoorCollider;

    private State mState;

    public GameObject RightDoor;
    public GameObject LeftDoor;
    public GameObject DoorLock;

    public bool IsLock;
    public bool IsInside;
    public bool IsOpen;

    public int EnemyCount;

    public Animator DoorAnimator;

    #endregion

    #region Property

    enum State
    {
        Open,
        Close
    }

    #endregion

    #region Unity Method

    public void Awake()
    {
        Initialize();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TAG_CHARCTER))
        {
            if (IsLock == false && IsInside == false)
            {
                ChangeDoorState(State.Open);
            }
        }
    }

    #endregion

    #region Private Method

    private void Initialize()
    {
        mRightDoorCollider = RightDoor.GetComponent<Collider2D>();
        mLeftDoorCollider = LeftDoor.GetComponent<Collider2D>();
        mState = State.Close;
    }

    private void ChangeDoorState(State state)
    {
        StartCoroutine(DoorEvent(state));
    }

    #endregion

    #region Public Method

    public void DoorClose()
    {
        ChangeDoorState(State.Close);
    }

    #endregion

    #region IEnumerator Method

    IEnumerator DoorEvent(State state)
    {
        mState = state;
        bool isbool = mState == State.Open ? false : true;
        DoorAnimator.SetBool("IsOpen", !isbool);
        yield return new WaitForSeconds(1f);
        mLeftDoorCollider.enabled = isbool;
        mRightDoorCollider.enabled = isbool;
        DoorLock.SetActive(isbool);
        IsLock = isbool;
    }

    #endregion
}