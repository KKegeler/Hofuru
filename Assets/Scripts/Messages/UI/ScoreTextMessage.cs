using Framework;

public class ScoreTextMessage : BaseMessage
{
    public readonly string scoreText;

    public ScoreTextMessage(float scoreVal)
    {
        scoreText = scoreVal.ToString();
    }

}