using UnityEngine;

public class MoveLeft : Command
{
    public override void Execute(Character character, Command command)
    {
        character.transform.Translate(0, character.Speed * Time.deltaTime, 0);
    }
}