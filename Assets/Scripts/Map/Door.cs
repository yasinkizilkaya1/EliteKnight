using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    #region Constants

    private const string TAG_CHARCTER = "Player";
    private const float DOOR_WEIGHT = 3.1f;
    private const int DOOR_SPEED = 10;

    #endregion

    #region Property

    enum State
    {
        Open,
        Close
    }

    enum Orientation
    {
        Horizontal,
        Vertical
    }

    #endregion

    #region Fields

    public GameObject RightDoor;
    public GameObject LeftDoor;
    public GameObject DoorLock;

    public int EnemyCount;

    public bool IsLock;

    private Orientation mOriontation;
    private State mState;

    private float mDoorChangePositionValue;

    private Collider2D mLeftDoorCollider;
    private Collider2D mRightDoorCollider;

    public Animator DoorAnimator;

    #endregion

    #region Unity Method

    public void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        if (EnemyCount == 0)
        {
            IsLock = false;
            DoorLock.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TAG_CHARCTER))
        {
            if (IsLock == false)
            {
                DoorOpen();
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
        switch (mOriontation)
        {
            case Orientation.Horizontal:
                mDoorChangePositionValue = DOOR_WEIGHT;
                break;
            case Orientation.Vertical:
                mDoorChangePositionValue = DOOR_WEIGHT * -1;
                break;
            default:
                mDoorChangePositionValue = DOOR_WEIGHT;
                break;
        }

        switch (state)
        {
            case State.Open:
                if (mOriontation == Orientation.Horizontal)
                    StartCoroutine(DoorEvent(DOOR_SPEED, Orientation.Horizontal, State.Open));
                else
                    StartCoroutine(DoorEvent(DOOR_SPEED, Orientation.Vertical, State.Open));
                break;
            case State.Close:
                if (mOriontation == Orientation.Horizontal)
                    StartCoroutine(DoorEvent(DOOR_SPEED, Orientation.Horizontal, State.Close));
                else
                    StartCoroutine(DoorEvent(DOOR_SPEED, Orientation.Vertical, State.Close));
                break;
        }
    }

    #endregion

    #region Public Method

    public void EqualDatas(bool horizontal, int enemyCount)
    {
        EnemyCount = enemyCount;
        mOriontation = horizontal ? Orientation.Horizontal : Orientation.Vertical;
    }

    public void DoorOpen()
    {
        if (mState != State.Open)
        {
            ChangeDoorState(State.Open);
            IsLock = false;
        }
        else
        {
            IsLock = false;
        }
    }

    public void DoorClose()
    {
        ChangeDoorState(State.Close);
        IsLock = true;
    }

    #endregion

    #region IEnumerator Method

    IEnumerator DoorEvent(int DoorSpeed, Orientation orientation, State state)
    {
        bool isbool = state == State.Open ? false : true;
        float time = state == State.Open ? 0f : 1f;
        mRightDoorCollider.enabled = isbool;
        mLeftDoorCollider.enabled = isbool;
        mState = state;
        DoorAnimator.SetBool("IsOpen", !isbool);
        yield return new WaitForSeconds(time);
        DoorLock.SetActive(isbool);
    }

    #endregion
}