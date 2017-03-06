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
            private Stack<string> _typeStack = new Stack<string>();
            private Stack<MessageHandlerDelegate> _handlerStack = 
                new Stack<MessageHandlerDelegate>();
            private bool _isTriggered;

            private const float _MAX_QUEUE_PROCESSING_TIME = 0.03334f;
            #endregion

            #region Properties
            public static MessagingSystem Instance
            {
                get { return ((MessagingSystem)_Instance); }
            }
            #endregion

            private void Update()
            {
                float timer = 0f;

                // Check for Messages that were send while TriggerMessage was active
                while (_typeStack.Count > 0)
                    _listenerDict[_typeStack.Pop()].Add(_handlerStack.Pop());

                // Iterate through all messages or return early if it takes too long
                while (_messageQueue.Count > 0)
                {
                    if (_MAX_QUEUE_PROCESSING_TIME > 0f)
                        if (timer > _MAX_QUEUE_PROCESSING_TIME)
                            return;

                    BaseMessage msg = _messageQueue.Dequeue();
                    if (!TriggerMessage(msg))
                        CustomLogger.LogWarningFormat("Error when processing message: {0}", msg.name);

                    if (_MAX_QUEUE_PROCESSING_TIME > 0f)
                        timer += Time.deltaTime;
                }
            }

            /// <summary>
            /// Calls handlers of the listeners
            /// </summary>
            /// <param name="msg">Message</param>
            /// <returns>Could the message be handled by the listener?</returns>
            private bool TriggerMessage(BaseMessage msg)
            {
                _isTriggered = true;
                string msgName = msg.name;

                if (!_listenerDict.ContainsKey(msgName))
                {
                    CustomLogger.LogFormat("MessagingSystem: Message \"{0}\" has no listeners!", msgName);
                    _isTriggered = false;
                    return false;
                }

                // Iterate through the handlers of the listeners
                for (int i = 0; i < _listenerDict[msgName].Count; ++i)
                {
                    if (_listenerDict[msgName][i](msg))
                    {
                        _isTriggered = false;
                        return true;
                    }
                }

                _isTriggered = false;
                return true;
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
                    CustomLogger.LogWarning("AttachListener failed due to no message specified!\n");
                    return false;
                }

                string msgName = type.Name;

                if (!_listenerDict.ContainsKey(msgName))
                    _listenerDict.Add(msgName, new List<MessageHandlerDelegate>());

                bool contains = _listenerDict[msgName].Contains(handler);

                // If this was called from TriggerMessage add the listener later
                if (_isTriggered)
                {
                    if (!contains)
                    {
                        _typeStack.Push(msgName);
                        _handlerStack.Push(handler);
                        return true;
                    }
                    else
                        return false;
                }

                if (contains)
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
                    CustomLogger.LogWarning("DetachListener failed due to no message specified!\n");
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
                    CustomLogger.LogWarningFormat("{0} is not registered!\n", msg.name);

                _messageQueue.Enqueue(msg);
            }

            /// <summary>
            /// Delegate for listeners
            /// </summary>
            /// <param name="message">Message</param>
            /// <returns>Was the message handled?</returns>
            public delegate bool MessageHandlerDelegate(BaseMessage message);

            void OnDestroy()
            {
                _alive = false;
            }

        }
    }
}