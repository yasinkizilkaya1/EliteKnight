using UnityEngine;

public class MoveRight : Command
{
    public override void Execute(Character character, Command command)
    {
        character.transform.Translate(0, -character.Speed * Time.deltaTime, 0);
    }
}