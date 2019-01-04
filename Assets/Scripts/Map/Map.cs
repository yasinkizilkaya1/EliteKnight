using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    private const int minNum = 15;
    private const int maxNum = 50;

    public Tile tileground;
    public Tile tileground2;
    public Tile tilewall;
    public Tile tileDoor;
    public GameObject ChestObject;

    public Tilemap tilemapGround;
    public Tilemap tilemapWall;
    private Vector3Int vector3;
    private Vector3Int vector3Int;

    public Map map;

    public int chestCount;
    public int RandomNumber;
    private int numberX;
    private int numberY;

    private Vector2Int size;
    private Vector3Int[] positions;
    private TileBase[] tileGroundArray;
    private TileBase[] tileWallArray;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        size = new Vector2Int(EqualNumbers(minNum, maxNum), EqualNumbers(minNum, maxNum));
        Test();
    }

    public void Test()
    {
        positions = new Vector3Int[size.x * size.y];
        tileGroundArray = new TileBase[positions.Length];
        tileWallArray = new TileBase[positions.Length];

        Vector2Int vector2 = new Vector2Int((int)transform.position.x, (int)transform.position.y);

        for (int x = 0; x < size.y; x++)
        {
            for (int i = 0; i < size.x; i++)
            {
                positions[i] = new Vector3Int(vector2.x + i, vector2.y + x, 1);
                int a = x % 2 == 0 ? 0 : 1;
                tileGroundArray[i] = i % 2 == a ? tileground : tileground2;
            }
            tilemapGround.SetTiles(positions, tileGroundArray);
        }

        for (int i = 0; i < size.x; i++)
        {
            BorderCreate(i, vector2.x + i, vector2.y + size.y); //Up Border
            BorderCreate(i, vector2.x + i, vector2.y - 1);     //Down Border
        }

        for (int i = 0; i < size.y; i++)
        {
            BorderCreate(i, vector2.x + size.x, vector2.y + i); //Right Border
            BorderCreate(i, vector2.x - 1, vector2.y + i);    //Left Border

            BorderCreate(i, vector2.x + size.x, vector2.y + size.y);
            BorderCreate(i, vector2.x + -1, vector2.y + -1);
            BorderCreate(i, vector2.x - 1, vector2.y + size.y);
            BorderCreate(i, vector2.x + size.x, vector2.y - 1);
        }
    }

    private void BorderCreate(int i, int locationX, int locationY)
    {
        positions[i] = new Vector3Int(locationX, locationY, 1);
        tileWallArray[i] = tilewall;
        tilemapWall.SetTiles(positions, tileWallArray);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Test();
        }
    }

    public int EqualNumbers(int min, int max)
    {
        int number = Random.Range(min, max);
        return number;
    }

    private Vector3 RandomLocationFind(Vector2Int current)
    {
        numberX = EqualNumbers(-RandomNumber + 2, RandomNumber - 2);
        numberY = EqualNumbers(-RandomNumber + 2, RandomNumber - 2);

        Vector3 location = new Vector3(size.x - numberX, size.y - numberY);
        return location;
    }

    private void ChestCreate(int ChestCount)
    {
        for (int i = 0; i < ChestCount; i++)
        {
            Instantiate(ChestObject, RandomLocationFind(size), Quaternion.identity);
        }
    }
}