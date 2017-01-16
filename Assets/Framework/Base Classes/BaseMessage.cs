namespace Framework
{
    /// <summary>
    /// Basisklasse für Nachrichten
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