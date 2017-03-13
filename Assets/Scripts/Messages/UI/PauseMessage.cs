using Framework.Messaging;

public class PauseMessage : BaseMessage 
{
	public readonly bool isPaused;

	public PauseMessage(bool value)
	{
		isPaused = value;
	}
	
}
