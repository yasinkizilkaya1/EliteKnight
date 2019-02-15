using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Map : MonoBehaviour
{
    #region Constants

    private const int MIN_VALUE = 25;
    private const int MAX_VALUE = 50;
    private const int PLACE_MAX = 100;
    private const int MAP_PLACE_MinVALUE = 6;
    private const int MAP_PLACE_MaxVALUE = 17;
    private const int MAZE_WIDTH = 3;

    #endregion

    #region Field

    public List<Vector2Int> PlacesSizeList;
    public List<Vector2Int> DoorLocation;
    public List<Vector3Int> RoomCoordinatesList;
    private List<Vector3Int> mBridgeLocation;
    private List<Vector3Int> mWallLocation;

    public GameObject PlaceObject;

    private Vector3Int vector3Int;
    private Vector2Int vector2Int;
    private Vector3Int OldTransform;

    public int RoomCoun;
    private int upDoor;
    private int downDoor;
    private int rightDoor;
    private int leftDoor;

    private int TransformX;
    private int TransformY;
    private int placeX;
    private int placeY;

    public List<int> UpDoor;
    public List<int> DownDoor;
    public List<int> RightDoor;
    public List<int> LeftDoor;

    public Tilemap BridgeTilemap;
    public Tilemap BridgeWallTilemap;

    public Tile BridgeTile;
    public Tile BridgeWallTile;

    #endregion

    #region Unity Method

    private void Start()
    {
        Init();
    }

    #endregion

    #region Private Methods

    private void Init()
    {
        mBridgeLocation = new List<Vector3Int>();
        mWallLocation = new List<Vector3Int>();
        RoomCoun = RandomValue(MAP_PLACE_MinVALUE, MAP_PLACE_MaxVALUE);
        PlacesFind();
        MapCreate();
    }

    private void MapCreate()
    {
        for (int i = 0; i < RoomCoun; i++)
        {
            if (i != 0)
            {
                UpDoor.Add(upDoor);
                DownDoor.Add(downDoor);
                RightDoor.Add(leftDoor);
                LeftDoor.Add(rightDoor);
            }

            DoorPlacesFind(i);
            Room room = Instantiate(PlaceObject, RoomCoordinatesList[i], Quaternion.identity).GetComponent<Room>();
            room.GetComponent<Room>().MakeEqual(PlacesSizeList[i], RoomCoordinatesList[i], upDoor, downDoor, rightDoor, leftDoor);

            if (i != RoomCoun - 1)
            {
                BrigeCreate(i);
            }
        }
    }

    private void PlacesFind()
    {
        for (int i = 0; i < RoomCoun; i++)
        {
            TransformX = RandomValue(MIN_VALUE, MAX_VALUE);
            TransformY = RandomValue(MIN_VALUE, MAX_VALUE);
            vector2Int = new Vector2Int(TransformX, TransformY);
            PlacesSizeList.Add(vector2Int);
            PlaceTransform(i, TransformX, TransformY);
        }
    }

    private void BrigeCreate(int i)
    {
        if (RoomCoordinatesList[i].y < RoomCoordinatesList[i + 1].y)
        {
            int FirstDoorLocationX = PlacesSizeList[i].x / 6;
            int SecondDoorLocationX = PlacesSizeList[i + 1].x / 6;

            Vector2Int FirstDoor = new Vector2Int(RoomCoordinatesList[i].x + FirstDoorLocationX, RoomCoordinatesList[i].y + PlacesSizeList[i].y + 1);
            Vector2Int SecondDoor = new Vector2Int(RoomCoordinatesList[i + 1].x + SecondDoorLocationX, RoomCoordinatesList[i + 1].y - 1);

            DoorLocation.Add(FirstDoor);
            DoorLocation.Add(SecondDoor);

            int distanceY = Mathf.Abs(SecondDoor.y - FirstDoor.y);

            if (FirstDoor.x != SecondDoor.x)
            {
                for (int x = 0; x < distanceY / 2; x++)
                {
                    for (int y = 0; y < MAZE_WIDTH; y++)
                    {
                        mBridgeLocation.Add(new Vector3Int(FirstDoor.x + y - 1, FirstDoor.y + x, 1));
                        mBridgeLocation.Add(new Vector3Int(SecondDoor.x + y - 1, SecondDoor.y - x, 1));
                    }
                }

                int distanceX = FirstDoor.x > SecondDoor.x ? Mathf.Abs((FirstDoor.x + 2) - (SecondDoor.x - 1)) : Mathf.Abs((SecondDoor.x + 2) - (FirstDoor.x - 1));

                for (int y = 0; y < distanceX; y++)
                {
                    int increase = FirstDoor.x > SecondDoor.x ? y * -1 : y;
                    int transformX = FirstDoor.x > SecondDoor.x ? FirstDoor.x + 1 : FirstDoor.x - 1;

                    for (int a = -1; a < 2; a++)
                    {
                        mBridgeLocation.Add(new Vector3Int(transformX + increase, FirstDoor.y + distanceY / 2 + a, 1));
                    }
                }
            }
            else
            {
                for (int x = 0; x < distanceY; x++)
                {
                    mWallLocation.Add(new Vector3Int(FirstDoor.x + 2, FirstDoor.y + x, 1));
                    mWallLocation.Add(new Vector3Int(FirstDoor.x - 2, FirstDoor.y + x, 1));

                    for (int y = 0; y < MAZE_WIDTH; y++)
                    {
                        mBridgeLocation.Add(new Vector3Int(FirstDoor.x + y - 1, FirstDoor.y + x, 1));
                    }
                }
            }

            BorderBridge(true, distanceY, FirstDoor, SecondDoor);
        }

        if (RoomCoordinatesList[i].x < RoomCoordinatesList[i + 1].x)
        {
            int FirstDoorLocationY = PlacesSizeList[i].y / 6;
            int SecondDoorLocationY = PlacesSizeList[i + 1].y / 6;

            Vector2Int FirstDoor = new Vector2Int(RoomCoordinatesList[i].x + PlacesSizeList[i].x + 1, RoomCoordinatesList[i].y + FirstDoorLocationY);
            Vector2Int SecondDoor = new Vector2Int(RoomCoordinatesList[i + 1].x - 1, RoomCoordinatesList[i + 1].y + SecondDoorLocationY);

            DoorLocation.Add(FirstDoor);
            DoorLocation.Add(SecondDoor);

            int distanceX = Mathf.Abs(RoomCoordinatesList[i + 1].x - FirstDoor.x);

            if (FirstDoor.y != SecondDoor.y)
            {
                for (int x = 0; x < distanceX / 2; x++)
                {
                    for (int y = 0; y < MAZE_WIDTH; y++)
                    {
                        mBridgeLocation.Add(new Vector3Int(FirstDoor.x + x, FirstDoor.y + y - 1, 1));
                        mBridgeLocation.Add(new Vector3Int(SecondDoor.x - x, SecondDoor.y + y - 1, 1));
                    }
                }

                int distanceY = FirstDoor.y > SecondDoor.y ? Mathf.Abs((FirstDoor.y + 2) - (SecondDoor.y - 1)) : Mathf.Abs((SecondDoor.y + 2) - (FirstDoor.y - 1));

                for (int y = 0; y < distanceY; y++)
                {
                    int increase = FirstDoor.y > SecondDoor.y ? y * -1 : y;
                    int transformY = FirstDoor.y > SecondDoor.y ? FirstDoor.y + 1 : FirstDoor.y - 1;

                    for (int x = 0; x < MAZE_WIDTH; x++)
                    {
                        mBridgeLocation.Add(new Vector3Int(FirstDoor.x + distanceX / 2 + x - 2, transformY + increase, 1));
                    }
                }
            }
            else
            {
                for (int x = 0; x < distanceX; x++)
                {
                    mWallLocation.Add(new Vector3Int(FirstDoor.x + x, FirstDoor.y + 2, 1));
                    mWallLocation.Add(new Vector3Int(FirstDoor.x + x, FirstDoor.y - 2, 1));

                    for (int y = 0; y < MAZE_WIDTH; y++)
                    {
                        mBridgeLocation.Add(new Vector3Int(FirstDoor.x + x, FirstDoor.y + y - 1, 1));
                    }
                }
            }

            BorderBridge(false, distanceX, FirstDoor, SecondDoor);
        }

        for (int t = 0; t < mBridgeLocation.Count; t++)        {
            BridgeTilemap.SetTile(mBridgeLocation[t], BridgeTile);        }

        for (int k = 0; k < mWallLocation.Count; k++)
        {
            BridgeWallTilemap.SetTile(mWallLocation[k], BridgeWallTile);
        }
    }

    private void BorderBridge(bool updown, int distance, Vector2Int FirstDoor, Vector2Int SecondDoor)
    {
        int BorderDistance = (distance / 2) + 2;
        int meandistance = distance - BorderDistance;

        if (updown)
        {
            int gap = Mathf.Abs(FirstDoor.x - SecondDoor.x);
            int firstDoorTransform = FirstDoor.x > SecondDoor.x ? 2 : -2;
            int secondDoorTransform = FirstDoor.x > SecondDoor.x ? -2 : 2;

            for (int i = 0; i <= distance; i++)
            {
                if (i <= BorderDistance)
                {
                    mWallLocation.Add(new Vector3Int(FirstDoor.x + firstDoorTransform, FirstDoor.y + i, 1));
                    mWallLocation.Add(new Vector3Int(SecondDoor.x + secondDoorTransform, SecondDoor.y - i, 1));
                }

                if (i == BorderDistance)
                {
                    int value = -1;

                    if (FirstDoor.x > SecondDoor.x)
                    {
                        for (int x = 0; x < gap; x++)
                        {
                            mWallLocation.Add(new Vector3Int(FirstDoor.x + 2 + value, FirstDoor.y + BorderDistance, 1));
                            mWallLocation.Add(new Vector3Int(FirstDoor.x - 1 + value, SecondDoor.y - BorderDistance, 1));
                            value--;
                        }
                    }
                    else
                    {
                        for (int x = 0; x < gap; x++)
                        {
                            mWallLocation.Add(new Vector3Int(FirstDoor.x + 1 - value, FirstDoor.y + meandistance, 1));
                            mWallLocation.Add(new Vector3Int(FirstDoor.x - 2 - value, SecondDoor.y - meandistance, 1));
                            value--;
                        }
                    }
                }

                if (i > BorderDistance)
                {
                    mWallLocation.Add(new Vector3Int(SecondDoor.x + firstDoorTransform, FirstDoor.y + i, 1));
                    mWallLocation.Add(new Vector3Int(FirstDoor.x + secondDoorTransform, SecondDoor.y - i, 1));
                }
            }
        }
        else
        {
            int gap = Mathf.Abs(FirstDoor.y - SecondDoor.y);
            int rightDoorTransform = FirstDoor.y > SecondDoor.y ? 2 : -2;
            int leftDoorTransform = FirstDoor.y > SecondDoor.y ? -2 : 2;

            for (int i = 0; i <= distance; i++)
            {
                if (i <= BorderDistance)
                {
                    mWallLocation.Add(new Vector3Int(FirstDoor.x - 1 + i, FirstDoor.y + rightDoorTransform, 1));
                    mWallLocation.Add(new Vector3Int(SecondDoor.x - i, SecondDoor.y + leftDoorTransform, 1));
                }

                if (i == BorderDistance)
                {
                    int value = -2;

                    if (FirstDoor.y > SecondDoor.y)
                    {
                        for (int y = 0; y < gap; y++)
                        {
                            mWallLocation.Add(new Vector3Int(FirstDoor.x + BorderDistance - 1, SecondDoor.y - value, 1));
                            mWallLocation.Add(new Vector3Int(SecondDoor.x - BorderDistance, FirstDoor.y + value, 1));
                            value--;
                        }
                    }
                    else
                    {
                        for (int y = 0; y < gap; y++)
                        {
                            mWallLocation.Add(new Vector3Int(FirstDoor.x + BorderDistance - 1, SecondDoor.y + value, 1));
                            mWallLocation.Add(new Vector3Int(SecondDoor.x - BorderDistance, FirstDoor.y - value, 1));
                            value--;
                        }
                    }
                }

                if (i > BorderDistance)
                {
                    mWallLocation.Add(new Vector3Int(SecondDoor.x - i, FirstDoor.y + leftDoorTransform, 1));
                    mWallLocation.Add(new Vector3Int(FirstDoor.x - 1 + i, SecondDoor.y + rightDoorTransform, 1));
                }
            }
        }
    }

    private void PlaceTransform(int i, int transformX, int transformY)
    {
        int up = 1;
        int down = 1;
        int right = 1;
        int left = 1;
        bool isupdown = (Random.value < 0.5f);
        bool ispositive = (Random.value < 0.5f);

        if (isupdown)
        {
            if (ispositive)
            {
                down++;
                placeY += (transformY + PLACE_MAX) * up;
            }
            else
            {
                up++;
                placeY -= (-transformY - PLACE_MAX) * down;
            }
        }
        else
        {
            if (ispositive)
            {
                left++;
                placeX += (transformX + PLACE_MAX) * right;
            }
            else
            {
                right++;
                placeX -= (transformX + PLACE_MAX) * -left;
            }
        }

        if (i == 0)
        {
            placeX = 0;
            placeY = 0;
            vector3Int = new Vector3Int(0, 0, 1);
        }
        else
        {
            vector3Int = new Vector3Int(placeX, placeY, 1);
        }
        RoomCoordinatesList.Add(vector3Int);
    }

    private void DoorPlacesFind(int i)
    {
        upDoor = 0;
        downDoor = 0;
        rightDoor = 0;
        leftDoor = 0;

        if (i == 0)
        {
            downDoor = RoomCoordinatesList[i + 1].y < RoomCoordinatesList[i].y ? MAZE_WIDTH : 0;
            upDoor = RoomCoordinatesList[i + 1].y > RoomCoordinatesList[i].y ? MAZE_WIDTH : 0;
            leftDoor = RoomCoordinatesList[i + 1].x < RoomCoordinatesList[i].x ? MAZE_WIDTH : 0;
            rightDoor = RoomCoordinatesList[i + 1].x > RoomCoordinatesList[i].x ? MAZE_WIDTH : 0;
        }
        else
        {
            OldTransform = i == RoomCoun - 1 ? RoomCoordinatesList[i - 1] : RoomCoordinatesList[i + 1];

            downDoor = RoomCoordinatesList[i - 1].y < RoomCoordinatesList[i].y && RoomCoordinatesList[i - 1].y != RoomCoordinatesList[i].y ? MAZE_WIDTH : 0;
            upDoor = OldTransform.y > RoomCoordinatesList[i].y && OldTransform.y != RoomCoordinatesList[i].y ? MAZE_WIDTH : 0;
            leftDoor = RoomCoordinatesList[i - 1].x < RoomCoordinatesList[i].x && RoomCoordinatesList[i - 1].x != RoomCoordinatesList[i].x ? MAZE_WIDTH : 0;
            rightDoor = OldTransform.x > RoomCoordinatesList[i].x && OldTransform.x != RoomCoordinatesList[i].x ? MAZE_WIDTH : 0;
        }
    }

    private int RandomValue(int min, int max)
    {
        int number = Random.Range(min, max);
        return number;
    }

    #endregion
}