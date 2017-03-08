namespace Framework
{
    /// <summary>
    /// Base class for Messages
    /// </summary>
    public class BaseMessage
    {
        #region Variables
        public string name;
        #endregion

        #region Constructors
        public BaseMessage()
        {
            name = GetType().Name;
        }
        #endregion

    }
}