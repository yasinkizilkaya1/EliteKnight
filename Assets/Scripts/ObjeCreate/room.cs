using UnityEngine.Tilemaps;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New Room", menuName = "Data/Room")]
public class room : Entity
{
    public Tile tileGround;
    public Tile tileGround2;
    public Tile tileWall;
}