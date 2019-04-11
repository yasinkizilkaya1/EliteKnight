using UnityEngine;

public class MoveRight : Command
{
    public override void Execute()
    {
        Character.transform.Translate(0, -Character.Speed * Time.deltaTime, 0);
        Character.CharacterWay = 3;
    }
}