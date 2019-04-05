using UnityEngine;

public class Move : Command
{
    public enum MoveType
    {
        Forward,
        Reserve,
        Right,
        Left
    }

    private MoveType mMoveType;

    public void GetType(MoveType moveType)
    {
        mMoveType = moveType;
    }

    public override void Execute(Character character, Command command)
    {
        switch (mMoveType)
        {
            case MoveType.Forward:
                character.transform.Translate(character.Speed * Time.deltaTime, 0, 0);
                break;
            case MoveType.Reserve:
                character.transform.Translate(-character.Speed * Time.deltaTime, 0, 0);
                break;
            case MoveType.Right:
                character.transform.Translate(0, -character.Speed * Time.deltaTime, 0);
                break;
            case MoveType.Left:
                character.transform.Translate(0, character.Speed * Time.deltaTime, 0);
                break;
        }
    }
}