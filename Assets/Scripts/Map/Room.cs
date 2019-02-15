using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    #region Constants

    private const string TAG_CHARACTER = "Player";
    private const string TAG_TILEMAP_WALL = "wall";
    private const string TAG_TILEMAP_GROUND = "Tilemap_Ground";
    private const int CHEST_WIDHT = 4;
    private const int DOOR_WIDHT = 3;

    #endregion

    #region Fields

    private TileBase[] tileWallArray;
    private TileBase[] tileGroundArray;
    public Tilemap tilemapGround;
    public Tilemap tilemapWall;

    public room room;

    public List<GameObject> DoorList;
    public List<GameObject> EnemyList;

    public BoxCollider2D boxCollider;

    public GameObject ChestObject;
    public GameObject Door;
    private GameObject mObje;

    private Vector2Int size;
    private Vector3Int vector3;
    private Vector3Int[] positions;

    public int EnemyCount;
    public int ZombieCount;
    public int PursueEnemyCount;
    public int TowerExplodCount;
    public int TowerModeratorCount;
    public int TowerStandartCount;
    public int ChestCount;
    public int BoxCount;

    public bool IsCreate;

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

    #endregion

    #region Property

    enum EnemyType
    {
        Zombie,
        PursueEnemy,
        TowerExplod,
        TowerModerator,
        TowerStandart
    }

    #endregion

    #region Unity Methods

    private void Start()
    {
        Init();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TAG_CHARACTER))
        {
            if (EnemyCount > 0 && IsCreate)
            {
                EnemysCreate();

                for (int i = 0; i < DoorList.Count; i++)
                {
                    DoorList[i].GetComponent<Door>().DoorClose();
                    DoorList[i].GetComponent<Door>().IsInside = true;
                }
                IsCreate = false;
            }

            if (EnemyCount == 0)
            {
                for (int i = 0; i < DoorList.Count; i++)
                {
                    DoorList[i].GetComponent<Door>().IsLock = false;
                    DoorList[i].GetComponent<Door>().IsInside = false;
                    DoorList[i].GetComponent<Door>().DoorLock.SetActive(false);
                }
            }
        }
    }

    #endregion

    #region Private Methods

    private void Init()
    {
        IsCreate = true;
        ZombieCount = RandomValue(1, 8);
        //PursueEnemyCount = RandomValue(1, 8);
        TowerExplodCount = RandomValue(0, 2);
        TowerModeratorCount = RandomValue(0, 2);
        TowerStandartCount = RandomValue(0, 2);
        tilemapWall = GameObject.FindWithTag(TAG_TILEMAP_WALL).GetComponent<Tilemap>();
        tilemapGround = GameObject.FindWithTag(TAG_TILEMAP_GROUND).GetComponent<Tilemap>();
        EnemyCount = ZombieCount + PursueEnemyCount + TowerExplodCount + TowerModeratorCount + TowerStandartCount;
        PlaceCreate();
    }

    private void PlaceCreate()
    {
        positions = new Vector3Int[size.x * size.y];
        tileGroundArray = new TileBase[positions.Length];
        tileWallArray = new TileBase[positions.Length];

        vector3 = new Vector3Int((int)transform.position.x, (int)transform.position.y, 1);

        boxCollider.size = new Vector2(size.x, size.y);
        boxCollider.offset = new Vector2(size.x / 2, size.y / 2);

        for (int x = 0; x < size.y; x++)
        {
            for (int y = 0; y < size.x; y++)
            {
                Tile tile = x % 2 == 0 ? room.tileGround : room.tileGround2;
                Vector3Int position = new Vector3Int(vector3.x + y, vector3.y + x, 1);
                tilemapGround.SetTile(position, tile);
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
        if (count > 0)
        {
            int Department = span / count;
            int Location = Department / 2;

            if (isHorizontal == false)
            {
                for (int x = 0; x < DOOR_WIDHT; x++)
                {
                    BorderDraw(x, vector3.x + size.x + number, vector3.y + Location + x - 1, tileGroundArray, tilemapGround, room.tileGround);
                    DoorRightLeftPlace.Add(Location + x);
                }

                mObje = Instantiate(Door, new Vector3(vector3.x + size.x + number + 0.5f, vector3.y + Location + 0.5f, 2), Quaternion.identity) as GameObject;
                mObje.transform.Rotate(0, 0, -90);
            }
            else
            {
                for (int x = 0; x < DOOR_WIDHT; x++)
                {
                    BorderDraw(x, vector3.x + Location + x - 1, vector3.y + size.y + number, tileGroundArray, tilemapGround, room.tileGround);
                    DoorUpDownPlace.Add(Location + x);
                }

                mObje = Instantiate(Door, new Vector3(vector3.x + Location + 0.5f, vector3.y + size.y + number + 0.5f, 2), Quaternion.identity) as GameObject;
            }

            DoorList.Add(mObje);
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
                    BorderDraw(i, vector3.x + transformY, vector3.y + i - 1, tileWallArray, tilemapWall, room.tileWall);
                }
                else
                {
                    BorderDraw(i, vector3.x + i - 1, vector3.y + transformY, tileWallArray, tilemapWall, room.tileWall);
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

    private void EnemysCreate()
    {
        ObjeCreate(ZombieCount, EnemyList[0], EnemyType.Zombie);
        ObjeCreate(PursueEnemyCount, EnemyList[1], EnemyType.PursueEnemy);
        ObjeCreate(TowerExplodCount, EnemyList[2], EnemyType.TowerExplod);
        ObjeCreate(TowerModeratorCount, EnemyList[3], EnemyType.TowerModerator);
        ObjeCreate(TowerStandartCount, EnemyList[4], EnemyType.TowerStandart);
    }

    private void ObjeCreate(int finshValue, GameObject Object, EnemyType enemyType)
    {
        for (int i = 0; i < finshValue; i++)
        {
            GameObject gameObject = Instantiate(Object, RandomLocationFind(4), Quaternion.identity) as GameObject;

            switch (enemyType)
            {
                case EnemyType.Zombie:
                    gameObject.GetComponentInChildren<Zombies>().RoomEqual(this.gameObject);
                    break;
                case EnemyType.PursueEnemy:
                    gameObject.GetComponentInChildren<WarriorEnemy>().RoomEqual(this.gameObject);
                    break;
                case EnemyType.TowerExplod:
                    gameObject.GetComponentInChildren<TowerWeapon>().RoomEqual(this.gameObject);
                    break;
                case EnemyType.TowerModerator:
                    gameObject.GetComponentInChildren<TowerWeapon>().RoomEqual(this.gameObject);
                    break;
                case EnemyType.TowerStandart:
                    gameObject.GetComponentInChildren<TowerWeapon>().RoomEqual(this.gameObject);
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
}