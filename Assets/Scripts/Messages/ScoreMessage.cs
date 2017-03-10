using Framework.Messaging;

public class ScoreMessage : BaseMessage
{
    public readonly float score;

    public ScoreMessage(float scoreVal)
    {
        score = scoreVal;
    }

}