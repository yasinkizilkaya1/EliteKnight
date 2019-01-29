using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Place : MonoBehaviour
{
    #region Constants

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
    public Tile tileground;
    public Tile tileground2;
    public Tile tilewall;

    public GameObject ChestObject;

    public Vector2Int size;
    private Vector3Int vector3;
    private Vector3Int[] positions;

    public List<Vector3Int> DoorLocations;

    public int ZombieCount;
    public int EnemyCount;
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

    #endregion

    #region Unity Methods

    private void Start()
    {
        Init();
    }

    public void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    #endregion

    #region Private Methods

    private void Init()
    {
        tilemapGround = GameObject.FindWithTag(TAG_TILEMAP_GROUND).GetComponent<Tilemap>();
        tilemapWall = GameObject.FindWithTag(TAG_TILEMAP_WALL).GetComponent<Tilemap>();
        PlaceCreate();
    }

    private void PlaceCreate()
    {
        positions = new Vector3Int[size.x * size.y];
        tileGroundArray = new TileBase[positions.Length];
        tileWallArray = new TileBase[positions.Length];

        vector3 = new Vector3Int((int)transform.position.x, (int)transform.position.y, 1);

        for (int x = 0; x < size.y; x++)
        {
            for (int i = 0; i < size.x; i++)
            {
                positions[i] = new Vector3Int(vector3.x + i, vector3.y + x, 1);
                int a = x % 2 == 0 ? 0 : 1;
                tileGroundArray[i] = i % 2 == a ? tileground : tileground2;
            }
            tilemapGround.SetTiles(positions, tileGroundArray);
        }
        // ObjectCreate(ChestObject,1,6,2);
        BorderCreate();
    }

    private void BorderCreate()
    {
        LocationSetDoor(UpDoorCount, size.x, true, 0);
        LocationSetDoor(DownDoorCount, size.x, true, -size.y - 1);
        LocationSetDoor(RightDoorCount, size.y, false, 0);
        LocationSetDoor(LeftDoorCount, size.y, false, -size.x - 1);

        WallCreate(0, UpDoorCount, size.x, DoorUpPlace, DoorUpDownPlace, size.y, true);
        WallCreate(UpDoorCount, DownDoorCount, size.x, DoorDownPlace, DoorUpDownPlace, -1, true);
        WallCreate(0, RightDoorCount, size.y, DoorRightPlace, DoorRightLeftPlace, size.x, false);
        WallCreate(RightDoorCount, LeftDoorCount, size.y, DoorLeftPlace, DoorRightLeftPlace, -1, false);
    }

    private void LocationSetDoor(int count, int span, bool isLocationX, int number)
    {
        if (count > 0)
        {
            int Department = span / count;
            int Location = Department / 2;

            for (int i = 0; i < count; i++)
            {
                if (isLocationX == false)
                {
                    for (int x = 0; x < DOOR_WIDHT; x++)
                    {
                        BorderDraw(i, vector3.x + size.x + number, vector3.y + Location + x - 1, tileGroundArray, tilemapGround, tileground);
                        DoorRightLeftPlace.Add(Location + x);
                    }
                }
                else
                {
                    for (int x = 0; x < DOOR_WIDHT; x++)
                    {
                        BorderDraw(i, vector3.x + Location + x - 1, vector3.y + size.y + number, tileGroundArray, tilemapGround, tileground);
                        DoorUpDownPlace.Add(Location + x);
                    }
                }
            }
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
                    BorderDraw(i, vector3.x + transformY, vector3.y + i - 1, tileWallArray, tilemapWall, tilewall);
                }
                else
                {
                    BorderDraw(i, vector3.x + i - 1, vector3.y + transformY, tileWallArray, tilemapWall, tilewall);
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

    private void ObjectCreate(GameObject createobje, int minvalue, int maxvalue, int objewidth)
    {
        int count = RandomValue(minvalue, maxvalue);

        for (int i = 0; i < count; i++)
        {
            Instantiate(createobje, RandomLocationFind(objewidth), Quaternion.identity);
        }
    }

    private Vector3 RandomLocationFind(int objewidth)
    {
        int transformX = RandomValue(objewidth, size.x - objewidth);
        int transformY = RandomValue(objewidth, size.y - objewidth);

        Vector3 location = new Vector3(transform.position.x + transformX, transform.position.y + transformY);
        return location;
    }

    private int RandomValue(int min, int max)
    {
        int randomNamber = Random.Range(min, max);
        return randomNamber;
    }

    #endregion

    #region Public Methods

    public void MakeEqual(Vector2Int _size, Vector3Int _vector3, List<Vector3Int> vector3list, int up, int down, int right, int left)
    {
        UpDoorCount = up;
        DownDoorCount = down;
        RightDoorCount = right;
        LeftDoorCount = left;
        size = _size;
        vector3 = _vector3;
    }

    #endregion
}