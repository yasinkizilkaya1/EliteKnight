using UnityEngine;

public class MoveReserve : Command
{
    public override void Execute()
    {
        Character.transform.Translate(-Character.Speed * Time.deltaTime, 0, 0);
        Character.CharacterWay = 1;
    }
}