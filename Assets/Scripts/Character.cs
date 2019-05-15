using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    #region Constants

    private const string mCARD_DATA_BILL = ".asset";
    private const string mCARD_DATA_PATH = "Assets/Data/Characters/";
    private const string mTAG_ITEM = "Item";

    #endregion

    #region Fields

    public CharacterData characterData;
    public GameManager gameManager;
    public UIManager UIManager;
    public Knife knife;
    public Gun Gun;

    public int CurrentHP;
    public int MaxHP;
    public int CurrentDefence;
    public int Speed;
    public int Power;
    public int CharacterWay;
    public int run;
    public int Energy;
    public int MaxDefance;
    public int DeadEnemyCount;
    private int mDefaultSpeed;
    private int mRunSpeed;
    public int SelectionWeaponId;

    public bool IsNewGun;
    public bool isDead;
    public bool IsTire;

    public float shooting;

    private Vector3 mMousePositionVector;
    public GameObject BodyObject;
    public GameObject RightWeaponObject;
    public List<Gun> Guns;
    private List<Key> mKeys;

    private MoveForward mMoveForward;
    private MoveReserve mMoveReserve;
    private MoveRight mMoveRight;
    private MoveLeft mMoveLeft;
    private FireWeapon mFireWeapon;

    #endregion

    #region Unity Methods

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (gameManager != null)
        {
            Moving(gameManager);
            Run(gameManager);
            CharacterTurn(gameManager);
            Attack();
            InventoryOpenAndClose();

            if (Gun.CurrentAmmo == 0)
            {
                UIManager.AmmoBar.Animator.SetBool("Shoot", Input.GetKeyDown(mKeys[6].CurrentKey));
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0f && Guns.Count > 1)
            {
                if (SelectionWeaponId < Guns.Count - 1)
                {
                    SelectionWeaponId++;
                }
                else
                {
                    SelectionWeaponId = 0;
                }
                gameManager.GunSlot.GunChange(Guns[SelectionWeaponId], this);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f && Guns.Count > 1)
            {
                if (SelectionWeaponId > 0)
                {
                    SelectionWeaponId--;
                }
                else
                {
                    SelectionWeaponId = Guns.Count - 1;
                }
                gameManager.GunSlot.GunChange(Guns[SelectionWeaponId], this);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(mTAG_ITEM))
        {
            if (collider.GetComponent<Gun>())
            {
                collider.gameObject.SetActive(false);
                gameManager.IsUpdateChests = true;
                gameManager.GunSlot.GunAdd(collider.gameObject, RightWeaponObject);
                gameManager.Inventory.ItemAdd(collider.GetComponent<Gun>().Weapon, false);
            }
            else
            {
                Destroy(collider.gameObject);
                gameManager.Inventory.ItemAdd(collider.GetComponent<ItemData>().Item, true);
            }
        }
    }

    #endregion

    #region Private Methods

    private void Init()
    {
        UIManager = gameManager.UIManager;
        mKeys = gameManager.KeySetting.Keys;

        IsNewGun = true;
        characterData = gameManager.CharacterData;
        name = characterData.Name;
        CurrentHP = characterData.Health;
        MaxHP = CurrentHP;
        Energy = characterData.Energy;
        CurrentDefence = characterData.Defence;
        Power = characterData.Power;
        Speed = characterData.Speed;
        mDefaultSpeed = characterData.Speed;
        mRunSpeed = characterData.RunSpeed;
        MaxDefance = characterData.Defence;
        InventoryStandartGunAdd();

        mMoveForward = new MoveForward();
        mMoveReserve = new MoveReserve();
        mMoveRight = new MoveRight();
        mMoveLeft = new MoveLeft();
        mFireWeapon = new FireWeapon();
        mMoveForward.Character = this;
        mMoveReserve.Character = this;
        mMoveRight.Character = this;
        mMoveLeft.Character = this;
        mFireWeapon.Character = this;
    }

    private void Moving(GameManager GameManager)
    {
        if (GameManager.IsPause == false)
        {
            if (Input.GetKey(mKeys[0].CurrentKey))
            {
                mMoveForward.Execute();
            }

            if (Input.GetKey(mKeys[1].CurrentKey))
            {
                mMoveReserve.Execute();
            }

            if (Input.GetKey(mKeys[2].CurrentKey))
            {
                mMoveRight.Execute();
            }

            if (Input.GetKey(mKeys[3].CurrentKey))
            {
                mMoveLeft.Execute();
            }

            if (Input.GetKey(mKeys[0].CurrentKey) == false &&
                Input.GetKey(mKeys[1].CurrentKey) == false &&
                Input.GetKey(mKeys[2].CurrentKey) == false &&
                Input.GetKey(mKeys[3].CurrentKey) == false)
            {
                CharacterWay = 0;
            }
        }
    }

    private void CharacterTurn(GameManager gameManager)
    {
        if (gameManager.IsPause == false)
        {
            mMousePositionVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mMousePositionVector.z = transform.position.z;
            BodyObject.transform.right = (mMousePositionVector - transform.position);
        }
    }

    private void Run(GameManager gameManager)
    {
        if (gameManager.IsPause == false)
        {
            if (Input.GetKey(mKeys[4].CurrentKey))
            {
                if (Energy > 0)
                {
                    Speed = mRunSpeed;
                    Energy -= characterData.EnergyIncreaseAmmount;
                    run = 1;
                }
                else
                {
                    Speed = mDefaultSpeed;
                    IsTire = true;
                    run = 0;
                }
            }
            else
            {
                Speed = mDefaultSpeed;
                run = 0;

                if (characterData.MaxEnergy == Energy)
                {
                    IsTire = false;
                }
                else
                {
                    Energy += characterData.EnergyIncreaseAmmount;
                }
            }
        }
    }

    private void Attack()
    {
        if (Input.GetKeyDown(mKeys[6].CurrentKey) && gameManager.IsPause == false)
        {
            if (Gun == null && RightWeaponObject == null)
            {
                knife.IsAttack = true;
            }
            else
            {
                mFireWeapon.Execute();
            }
        }
        else if (Input.GetKeyDown(mKeys[6].CurrentKey) == false && Gun == null)
        {
            knife.IsAttack = false;
        }
    }

    private void InventoryOpenAndClose()
    {
        if (Input.GetKeyDown(mKeys[7].CurrentKey))
        {
            if (gameManager.Inventory.gameObject.activeInHierarchy == true)
            {
                gameManager.Inventory.gameObject.SetActive(false);
                gameManager.GameSetting(GameManager.GameSettings.Continue);
            }
            else
            {
                gameManager.Inventory.gameObject.SetActive(true);
                gameManager.GameSetting(GameManager.GameSettings.Stop);
            }
        }
    }

    private void InventoryStandartGunAdd()
    {
        gameManager.Inventory.ItemAdd(Gun.Weapon, false);
    }

    #endregion

    #region Public Method

    public void HealthDisCount(int value)
    {
        int remainingDamage = 0;

        if (CurrentDefence > 0)
        {
            if (value > CurrentDefence)
            {
                remainingDamage = value - CurrentDefence;
                CurrentDefence = 0;
            }
            else
            {
                CurrentDefence -= value;
            }
        }
        else if (CurrentHP > 0)
        {
            if (value > CurrentHP)
            {
                Destroy(gameObject);
                isDead = true;
            }
            else
            {
                CurrentHP -= value;
            }

            if (remainingDamage != 0)
            {
                CurrentHP -= remainingDamage;
            }
        }
    }

    public void SlowDown(bool inside, int power, float healthDecreaseTime)
    {
        if (shooting > 0)
        {
            if (inside == true)
            {
                Speed = 2;

                if (shooting > 0)
                {
                    shooting -= Time.deltaTime;

                    if (shooting <= 0)
                    {
                        HealthDisCount(power);
                        shooting = healthDecreaseTime;
                    }
                }
            }
            else if (Input.GetKey(gameManager.KeySetting.Keys[4].CurrentKey) == false && inside == false)
            {
                Speed = characterData.Speed;
            }
        }
        else
        {
            shooting = healthDecreaseTime;
        }
    }

    #endregion
}