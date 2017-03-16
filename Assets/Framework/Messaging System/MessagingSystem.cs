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

            private Stack<string> _addTypeStack = new Stack<string>();
            private Stack<MessageHandlerDelegate> _addHandlerStack =
                new Stack<MessageHandlerDelegate>();
            private Stack<string> _removeTypeStack = new Stack<string>();
            private Stack<MessageHandlerDelegate> _removeHandlerStack =
                new Stack<MessageHandlerDelegate>();

            private static bool _trigger;
            private const float _MAX_QUEUE_PROCESSING_TIME = 0.03334f; // 30 FPS
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
                    Debug.Log("There is a message!\n");

                    if (timer > _MAX_QUEUE_PROCESSING_TIME)
                        return;

                    Debug.Log("after if\n");

                    BaseMessage msg = _messageQueue.Peek();
                    if (TriggerMessage(msg))
                        _messageQueue.Dequeue();

                    Debug.Log("after trigger\n");               

                    timer += Time.unscaledDeltaTime;
                }
            }

            /// <summary>
            /// Calls handler functions
            /// </summary>
            /// <param name="msg">Message</param>
            /// <returns>Could the message be handled by the listener?</returns>
            private bool TriggerMessage(BaseMessage msg)
            {
                _trigger = true;
                string msgName = msg.name;

                if (!_listenerDict.ContainsKey(msgName))
                {
                    CustomLogger.LogWarningFormat("Message \"{0}\" is not registered!\n", msgName);
                    _trigger = false;
                    return false;
                }

                // Add listeners
                while (_addTypeStack.Count > 0)
                    _listenerDict[_addTypeStack.Pop()].Add(_addHandlerStack.Pop());

                // Remove listeners
                while (_removeTypeStack.Count > 0)
                    _listenerDict[_removeTypeStack.Pop()].Remove(_removeHandlerStack.Pop());

                // Iterate the handler functions
                for (int i = 0; i < _listenerDict[msgName].Count; ++i)
                {
                    Debug.Log("Message is processing...\n");
                    if (_listenerDict[msgName][i](msg))
                    {
                        _trigger = false;
                        return true;
                    }
                }

                _trigger = false;
                return false;
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

                // If this was called from TriggerMessage add the listener later
                if (_trigger)
                {
                    _addTypeStack.Push(msgName);
                    _addHandlerStack.Push(handler);
                    return true;
                }

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

                // If this was called from TriggerMessage remove the listener later
                if (_trigger)
                {
                    _removeTypeStack.Push(msgName);
                    _removeHandlerStack.Push(handler);
                    return true;
                }

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