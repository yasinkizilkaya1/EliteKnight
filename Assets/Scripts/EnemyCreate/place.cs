using UnityEngine.Tilemaps;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New place", menuName = "Data/place")]
public class place : Entity
{
    public Tile tileground;
    public Tile tileground2;
    public Tile tilewall;
    public Tile tileDoor;
}