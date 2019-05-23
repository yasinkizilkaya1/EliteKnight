using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    #region Constants

    private const string TAG_CHARACTER = "Player";
    private const string TAG_TILE_MAP_WALL = "wall";
    private const string TAG_TILE_MAP_GROUND = "Tilemap_Ground";
    private const int CHEST_WIDHT = 4;
    private const int DOOR_WIDHT = 3;

    #endregion

    #region Fields

    private TileBase[] mTileWallArray;
    private TileBase[] mTileGroundArray;
    public Tilemap TileMapGround;
    public Tilemap TileMapWall;

    public UIManager UIManager;
    public room room;

    public List<Door> Doors;
    public List<GameObject> Enemys;

    public GameObject ChestObject;
    public GameObject Door;
    private GameObject mObje;

    public Vector2Int size;
    private Vector3Int vector3;
    private Vector3Int[] positions;
    public Vector3 BossTransform;

    public int EnemyCount;
    public int BossCount;
    public int ZombieCount;
    public int PursueEnemyCount;
    public int TowerExplodCount;
    public int TowerModeratorCount;
    public int TowerStandartCount;
    public int ChestCount;
    public int BoxCount;

    public int UpDoorCount;
    public int DownDoorCount;
    public int RightDoorCount;
    public int LeftDoorCount;

    public List<int> DoorUpDownPlace;
    public List<int> DoorUpPlace;
    public List<int> DoorDownPlace;
    public List<int> DoorRightLeftPlace;
    public List<int> DoorRightPlace;
    public List<int> DoorLeftPlace;

    public bool IsEnemyCreate;

    #endregion

    #region Property

    enum Type
    {
        Zombie,
        PursueEnemy,
        TowerExplod,
        TowerModerator,
        TowerStandart,
        Boss,
        Chest
    }

    #endregion

    #region Unity Methods

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (BossCount == 1 && BossTransform.x == 0 && BossTransform.y == 0 && BossTransform.z == 0)
        {
            foreach (Door door in Doors)
            {
                if (door.IsRoomLogin)
                {
                    BossLocationFind(door);
                }
            }
        }
    }

    #endregion

    #region Private Methods

    private void Init()
    {
        EnemyCountCalculate();
        ChestCount = RandomValue(1, 4);
        TileMapWall = GameObject.FindWithTag(TAG_TILE_MAP_WALL).GetComponent<Tilemap>();
        TileMapGround = GameObject.FindWithTag(TAG_TILE_MAP_GROUND).GetComponent<Tilemap>();
        PlaceCreate();
        StartCoroutine(EnemyAndDoorControl());
        EnemyCount = BossCount + ZombieCount + PursueEnemyCount + TowerExplodCount + TowerModeratorCount + TowerStandartCount;
    }

    private void PlaceCreate()
    {
        positions = new Vector3Int[size.x * size.y];
        mTileGroundArray = new TileBase[positions.Length];
        mTileWallArray = new TileBase[positions.Length];

        vector3 = new Vector3Int((int)transform.position.x, (int)transform.position.y, 1);

        for (int x = 0; x < size.y; x++)
        {
            for (int y = 0; y < size.x; y++)
            {
                Tile tile = x % 2 == 0 ? room.tileGround : room.tileGround2;
                Vector3Int position = new Vector3Int(vector3.x + y, vector3.y + x, 1);
                TileMapGround.SetTile(position, tile);
            }
        }

        CreateDoors();
        CreateBorders();
    }

    private void CreateBorders()
    {
        WallCreate(0, UpDoorCount, size.x, DoorUpPlace, DoorUpDownPlace, size.y, true);
        WallCreate(UpDoorCount, DownDoorCount, size.x, DoorDownPlace, DoorUpDownPlace, -1, true);
        WallCreate(0, RightDoorCount, size.y, DoorRightPlace, DoorRightLeftPlace, size.x, false);
        WallCreate(RightDoorCount, LeftDoorCount, size.y, DoorLeftPlace, DoorRightLeftPlace, -1, false);
    }

    private void CreateDoors()
    {
        CreateDoor(UpDoorCount, size.x, true, 0);
        CreateDoor(DownDoorCount, size.x, true, -size.y - 1);
        CreateDoor(RightDoorCount, size.y, false, 0);
        CreateDoor(LeftDoorCount, size.y, false, -size.x - 1);
    }

    private void CreateDoor(int count, int span, bool isHorizontal, int number)
    {
        GameObject RoomObject = gameObject;

        if (count > 0)
        {
            int Department = span / count;
            int Location = Department / 2;

            if (isHorizontal == false)
            {
                for (int x = 0; x < DOOR_WIDHT; x++)
                {
                    BorderDraw(x, vector3.x + size.x + number, vector3.y + Location + x - 1, mTileGroundArray, TileMapGround, room.tileGround);
                    DoorRightLeftPlace.Add(Location + x);
                }

                RoomObject = Instantiate(Door, transform);
                RoomObject.transform.position = new Vector3(vector3.x + size.x + number + 0.5f, vector3.y + Location + 0.5f, 2);
                RoomObject.transform.Rotate(0, 0, -90);
                RoomObject.GetComponent<Door>().Room = gameObject.GetComponent<Room>();
            }
            else
            {
                for (int x = 0; x < DOOR_WIDHT; x++)
                {
                    BorderDraw(x, vector3.x + Location + x - 1, vector3.y + size.y + number, mTileGroundArray, TileMapGround, room.tileGround);
                    DoorUpDownPlace.Add(Location + x);
                }

                RoomObject = Instantiate(Door, transform);
                RoomObject.transform.position = new Vector3(vector3.x + Location + 0.5f, vector3.y + size.y + number + 0.5f, 2);
                RoomObject.GetComponent<Door>().Room = gameObject.GetComponent<Room>();

            }

            Doors.Add(RoomObject.GetComponent<Door>());
        }
    }

    private void WallCreate(int startvalue, int finishvalue, int bordervalue, List<int> array, List<int> doorarray, int transformY, bool isupdown)
    {
        for (int i = startvalue; i < finishvalue + startvalue; i++)
        {
            array.Add(doorarray[i]);
        }

        for (int i = 0; i < bordervalue + 2; i++)
        {
            if (array.Contains(i) == false)
            {
                if (isupdown == false)
                {
                    BorderDraw(i, vector3.x + transformY, vector3.y + i - 1, mTileWallArray, TileMapWall, room.tileWall);
                }
                else
                {
                    BorderDraw(i, vector3.x + i - 1, vector3.y + transformY, mTileWallArray, TileMapWall, room.tileWall);
                }
            }
        }
    }

    private void BorderDraw(int i, int locationX, int locationY, TileBase[] tilebase, Tilemap tilemap, Tile tile)
    {
        positions[i] = new Vector3Int(locationX, locationY, 1);
        tilebase[i] = tile;
        tilemap.SetTiles(positions, tilebase);
    }

    public void ObjectsCreate()
    {
        if (BossCount > 0)
        {
            ObjeCreate(BossCount, Enemys[5], Type.Boss);
            UIManager.BossHealthBarSlider.gameObject.SetActive(true);
        }
        else
        {
            ObjeCreate(ZombieCount, Enemys[0], Type.Zombie);
            ObjeCreate(PursueEnemyCount, Enemys[1], Type.PursueEnemy);
            ObjeCreate(TowerExplodCount, Enemys[2], Type.TowerExplod);
            ObjeCreate(TowerModeratorCount, Enemys[3], Type.TowerModerator);
            ObjeCreate(TowerStandartCount, Enemys[4], Type.TowerStandart);
        }
        ObjeCreate(ChestCount, ChestObject, Type.Chest);
    }

    private void EnemyCountCalculate()
    {
        int TowerCount = 0;
        bool IsChallenge = 1 < RandomValue(1, 2) ? true : false;
        EnemyCount = size.x / 3;

        if (IsChallenge)
        {
            TowerExplodCount = RandomValue(0, 2);
            TowerModeratorCount = RandomValue(0, 2);
            TowerStandartCount = RandomValue(0, 2);
            TowerCount = TowerExplodCount + TowerModeratorCount + TowerStandartCount;
            ZombieCount = RandomValue(0, (EnemyCount - TowerCount) / 2);
            PursueEnemyCount = RandomValue(0, (EnemyCount - (ZombieCount + TowerCount)) / 2);
        }
        else
        {
            ZombieCount = RandomValue(0, EnemyCount / 2);
            PursueEnemyCount = RandomValue(0, (EnemyCount - ZombieCount) / 2);
        }
    }

    private void ObjeCreate(int finshValue, GameObject Object, Type enemyType)
    {
        GameObject roomObject = this.gameObject;

        for (int i = 0; i < finshValue; i++)
        {
            roomObject = Instantiate(Object, transform);
            roomObject.transform.position = RandomLocationFind(4);

            switch (enemyType)
            {
                case Type.Zombie:
                    roomObject.GetComponentInChildren<Zombies>().RoomEqual(this);
                    break;
                case Type.PursueEnemy:
                    roomObject.GetComponentInChildren<WarriorEnemy>().RoomEqual(this);
                    break;
                case Type.TowerExplod:
                    roomObject.GetComponentInChildren<TowerWeapon>().RoomEqual(this);
                    break;
                case Type.TowerModerator:
                    roomObject.GetComponentInChildren<TowerWeapon>().RoomEqual(this);
                    break;
                case Type.TowerStandart:
                    roomObject.GetComponentInChildren<TowerWeapon>().RoomEqual(this);
                    break;
                case Type.Chest:
                    break;
                case Type.Boss:
                    roomObject.transform.position = BossTransform;
                    roomObject.GetComponentInChildren<SpaceShip>().RoomEqual(this);
                    break;
            }
        }
    }

    private Vector3 RandomLocationFind(int objewidth)
    {
        int transformX = RandomValue(objewidth, size.x - objewidth);
        int transformY = RandomValue(objewidth, size.y - objewidth);

        Vector3 location = new Vector3(transform.position.x + transformX, transform.position.y + transformY, 1);
        return location;
    }

    private void BossLocationFind(Door door)
    {
        float transformX = 0.0f;
        float transformY = 0.0f;

        if (size.x > size.y)
        {
            transformX = RoomDoorWhereFind(door, size.x);
            transformY = size.y / 2;
        }
        else
        {
            transformX = size.x / 2;
            transformY = RoomDoorWhereFind(door, size.y);
        }

        BossTransform = new Vector3(transform.position.x + transformX, transform.position.y + transformY, 1);
    }

    private float RoomDoorWhereFind(Door door, int value)
    {
        float Value = 0.0f;

        if (door.transform.position.y > this.transform.position.y && door.transform.position.x > this.transform.position.x)
        {
            Value = value / 4;
        }
        else if (door.transform.position.y <= this.transform.position.y && door.transform.position.x > this.transform.position.x)
        {
            Value = value - (value / 4);
        }
        return Value;
    }

    private int RandomValue(int min, int max)
    {
        int randomNamber = Random.Range(min, max);
        return randomNamber;
    }

    #endregion

    #region Public Methods

    public void MakeEqual(Vector2Int _size, Vector3Int _vector3, int up, int down, int right, int left)
    {
        size = _size;
        vector3 = _vector3;
        UpDoorCount = up;
        DownDoorCount = down;
        RightDoorCount = right;
        LeftDoorCount = left;
    }

    #endregion

    #region IEnumerators

    IEnumerator EnemyAndDoorControl()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);

            if (IsEnemyCreate == true)
            {
                yield return new WaitForSeconds(0.2f);
                IsEnemyCreate = false;
                ObjectsCreate();
            }

            if (EnemyCount == 0)
            {
                foreach (Door door in Doors)
                {
                    door.IsLock = false;
                    door.DoorLock.SetActive(false);
                }
            }

            if (EnemyCount == 0 && IsEnemyCreate == true)
            {

                StopCoroutine(EnemyAndDoorControl());
            }
        }
    }

    #endregion
}