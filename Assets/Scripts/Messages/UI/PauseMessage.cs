using Framework.Messaging;

public class PauseMessage : BaseMessage
{
    public readonly bool pause;

    public PauseMessage(bool var)
    {
        pause = var;
    }

}