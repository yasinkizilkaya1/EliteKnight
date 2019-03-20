using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Map : MonoBehaviour
{
    #region Constants

    private const int MIN_VALUE = 25;
    private const int MAX_VALUE = 50;
    private const int PLACE_GAP = 30;
    private const int MAP_PLACE_MINVALUE = 6;
    private const int MAP_PLACE_MAXVALUE = 17;
    private const int MAZE_WIDTH = 3;

    #endregion

    #region Field

    public List<Vector2Int> RoomSizeList;
    public List<Vector2Int> DoorLocation;
    public List<Vector3Int> RoomCoordinatesList;
    private List<Vector3Int> mBridgeLocation;
    private List<Vector3Int> mWallLocation;

    public GameObject RoomObject;

    private Vector3Int mVector3Int;
    private Vector2Int mVector2Int;
    private Vector3Int mOldTransform;

    public int RoomCoun;
    private int mUpDoor;
    private int mDownDoor;
    private int mRightDoor;
    private int mLeftDoor;

    private int mTransformX;
    private int mTransformY;
    private int mRoomX;
    private int mRoomY;

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
        RoomCoun = RandomValue(MAP_PLACE_MINVALUE, MAP_PLACE_MAXVALUE);
        PlacesFind();
        MapCreate();
    }

    private void MapCreate()
    {
        GameObject Map = gameObject;

        for (int i = 0; i < RoomCoun; i++)
        {
            if (i != 0)
            {
                UpDoor.Add(mUpDoor);
                DownDoor.Add(mDownDoor);
                RightDoor.Add(mLeftDoor);
                LeftDoor.Add(mRightDoor);
            }

            DoorPlacesFind(i);

            Map = Instantiate(RoomObject, transform);
            Map.transform.position = RoomCoordinatesList[i];
            Map.GetComponent<Room>().MakeEqual(RoomSizeList[i], RoomCoordinatesList[i], mUpDoor, mDownDoor, mRightDoor, mLeftDoor);

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
            mTransformX = RandomValue(MIN_VALUE, MAX_VALUE);
            mTransformY = RandomValue(MIN_VALUE, MAX_VALUE);
            mVector2Int = new Vector2Int(mTransformX, mTransformY);
            RoomSizeList.Add(mVector2Int);
            PlaceTransform(i, mTransformX, mTransformY);
        }
    }

    private void BrigeCreate(int i)
    {
        if (RoomCoordinatesList[i].y < RoomCoordinatesList[i + 1].y)
        {
            int FirstDoorLocationX = RoomSizeList[i].x / 6;
            int SecondDoorLocationX = RoomSizeList[i + 1].x / 6;

            Vector2Int FirstDoor = new Vector2Int(RoomCoordinatesList[i].x + FirstDoorLocationX, RoomCoordinatesList[i].y + RoomSizeList[i].y + 1);
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
            int FirstDoorLocationY = RoomSizeList[i].y / 6;
            int SecondDoorLocationY = RoomSizeList[i + 1].y / 6;

            Vector2Int FirstDoor = new Vector2Int(RoomCoordinatesList[i].x + RoomSizeList[i].x + 1, RoomCoordinatesList[i].y + FirstDoorLocationY);
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

        for (int t = 0; t < mBridgeLocation.Count; t++)
        {
            BridgeTilemap.SetTile(mBridgeLocation[t], BridgeTile);
        }

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
                mRoomY += (transformY + PLACE_GAP) * up;
            }
            else
            {
                up++;
                mRoomY -= (-transformY - PLACE_GAP) * down;
            }
        }
        else
        {
            if (ispositive)
            {
                left++;
                mRoomX += (transformX + PLACE_GAP) * right;
            }
            else
            {
                right++;
                mRoomX -= (transformX + PLACE_GAP) * -left;
            }
        }

        if (i == 0)
        {
            mRoomX = 0;
            mRoomY = 0;
            mVector3Int = new Vector3Int(0, 0, 1);
        }
        else
        {
            mVector3Int = new Vector3Int(mRoomX, mRoomY, 1);
        }
        RoomCoordinatesList.Add(mVector3Int);
    }

    private void DoorPlacesFind(int i)
    {
        mUpDoor = 0;
        mDownDoor = 0;
        mRightDoor = 0;
        mLeftDoor = 0;

        if (i == 0)
        {
            mDownDoor = RoomCoordinatesList[i + 1].y < RoomCoordinatesList[i].y ? MAZE_WIDTH : 0;
            mUpDoor = RoomCoordinatesList[i + 1].y > RoomCoordinatesList[i].y ? MAZE_WIDTH : 0;
            mLeftDoor = RoomCoordinatesList[i + 1].x < RoomCoordinatesList[i].x ? MAZE_WIDTH : 0;
            mRightDoor = RoomCoordinatesList[i + 1].x > RoomCoordinatesList[i].x ? MAZE_WIDTH : 0;
        }
        else
        {
            mOldTransform = i == RoomCoun - 1 ? RoomCoordinatesList[i - 1] : RoomCoordinatesList[i + 1];

            mDownDoor = RoomCoordinatesList[i - 1].y < RoomCoordinatesList[i].y && RoomCoordinatesList[i - 1].y != RoomCoordinatesList[i].y ? MAZE_WIDTH : 0;
            mUpDoor = mOldTransform.y > RoomCoordinatesList[i].y && mOldTransform.y != RoomCoordinatesList[i].y ? MAZE_WIDTH : 0;
            mLeftDoor = RoomCoordinatesList[i - 1].x < RoomCoordinatesList[i].x && RoomCoordinatesList[i - 1].x != RoomCoordinatesList[i].x ? MAZE_WIDTH : 0;
            mRightDoor = mOldTransform.x > RoomCoordinatesList[i].x && mOldTransform.x != RoomCoordinatesList[i].x ? MAZE_WIDTH : 0;
        }
    }

    private int RandomValue(int min, int max)
    {
        int number = Random.Range(min, max);
        return number;
    }

    #endregion
}