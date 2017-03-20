using Framework.Pool;
using Framework.Audio;

public class BloodObject : PoolObject
{
    public override void OnObjectReuse()
    {
        AudioManager.Instance.PlaySfx("Splatter");
    }

}