using UnityEngine;

public class MoveLeft : Command
{
    public override void Execute()
    {
        Character.transform.Translate(0, Character.Speed * Time.deltaTime, 0);
        Character.CharacterWay = 2;
    }
}