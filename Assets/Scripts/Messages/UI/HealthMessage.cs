using Framework;

public class HealthMessage : BaseMessage {

    public readonly float health;

    public HealthMessage(float h)
    {
        health = h;
    }
}
