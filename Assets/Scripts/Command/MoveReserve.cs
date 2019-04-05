using UnityEngine;

public class MoveReserve : Command
{
    public override void Execute(Character character, Command command)
    {
        character.transform.Translate(-character.Speed * Time.deltaTime, 0, 0);
    }
}