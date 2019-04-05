public class CharacterCommand
{
    public void SetCommand(Command command, Character character)
    {
        command.Execute(character, command);
    }
}