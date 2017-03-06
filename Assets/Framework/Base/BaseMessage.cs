namespace Framework
{
    /// <summary>
    /// Base class for Messages
    /// </summary>
    public class BaseMessage
    {
        public string name;

        public BaseMessage()
        {
            name = GetType().Name;
        }

    }
}