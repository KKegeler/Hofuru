using UnityEngine;
using System.Collections.Generic;
using Framework.Log;

namespace Framework
{
    namespace Messaging
    {
        /// <summary>
        /// Global Messaging-System
        /// </summary>
        public class MessagingSystem : SingletonAsComponent<MessagingSystem>
        {
            #region Variables
            private Dictionary<string, List<MessageHandlerDelegate>> _listenerDict
                = new Dictionary<string, List<MessageHandlerDelegate>>();
            private Queue<BaseMessage> _messageQueue = new Queue<BaseMessage>();

            private const float _MAX_QUEUE_PROCESSING_TIME = 0.01667f; // 60 FPS
            #endregion

            #region Properties
            public static MessagingSystem Instance
            {
                get { return (MessagingSystem)_Instance; }
            }
            #endregion

            private void Update()
            {
                float timer = 0f;

                // Iterate the messages or return early if it takes too long
                while (_messageQueue.Count > 0)
                {
                    if (timer > _MAX_QUEUE_PROCESSING_TIME)
                        return;

                    BaseMessage msg = _messageQueue.Dequeue();
                    TriggerMessage(msg);

                    timer += Time.deltaTime;
                }
            }

            /// <summary>
            /// Calls handler functions
            /// </summary>
            /// <param name="msg">Message</param>
            /// <returns>Could the message be handled by the listener?</returns>
            private void TriggerMessage(BaseMessage msg)
            {
                string msgName = msg.name;

                if (!_listenerDict.ContainsKey(msgName))
                {
                    CustomLogger.LogWarningFormat("Message \"{0}\" is not registered!\n", msgName);
                    return;
                }

                // Iterate the handler functions
                for (int i = 0; i < _listenerDict[msgName].Count; ++i)
                    if (_listenerDict[msgName][i](msg))
                        return;

                return;
            }

            /// <summary>
            /// Adds a listener
            /// </summary>
            /// <param name="type">Message type</param>
            /// <param name="handler">Handler</param>
            /// <returns>Was the listener added successfully?</returns>
            public bool AttachListener(System.Type type, MessageHandlerDelegate handler)
            {
                if (type == null)
                {
                    CustomLogger.LogWarning("AttachListener failed! Message was null!\n");
                    return false;
                }

                string msgName = type.Name;

                if (!_listenerDict.ContainsKey(msgName))
                    _listenerDict.Add(msgName, new List<MessageHandlerDelegate>());

                if (_listenerDict[msgName].Contains(handler))
                    return false;

                _listenerDict[msgName].Add(handler);
                return true;
            }

            /// <summary>
            /// Removes a listener
            /// </summary>
            /// <param name="type">Message type</param>
            /// <param name="handler">Handler</param>
            /// <returns>Was the listener removed successfully?</returns>
            public bool DetachListener(System.Type type, MessageHandlerDelegate handler)
            {
                if (type == null)
                {
                    CustomLogger.LogWarning("DetachListener failed! Message was null!\n");
                    return false;
                }

                string msgName = type.Name;

                if (!_listenerDict.ContainsKey(msgName))
                    return false;

                if (!_listenerDict[msgName].Contains(handler))
                    return false;

                _listenerDict[msgName].Remove(handler);
                return true;
            }

            /// <summary>
            /// Sends a message
            /// </summary>
            /// <param name="msg">Message</param>
            public void QueueMessage(BaseMessage msg)
            {
                if (!_listenerDict.ContainsKey(msg.name))
                {
                    CustomLogger.LogWarningFormat("{0} is not registered!\n", msg.name);
                    return;
                }

                _messageQueue.Enqueue(msg);
            }

            /// <summary>
            /// Delegate for listeners
            /// </summary>
            /// <param name="message">Message</param>
            /// <returns>Was the message handled?</returns>
            public delegate bool MessageHandlerDelegate(BaseMessage message);

        }
    }
}