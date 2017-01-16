using UnityEngine;
using System.Collections.Generic;

namespace Framework
{
    namespace Messaging
    {
        /// <summary>
        /// Globales Messaging-System
        /// </summary>
        public class MessagingSystem : SingletonAsComponent<MessagingSystem>
        {
            #region Variablen
            private Dictionary<string, List<MessageHandlerDelegate>> _listenerDict
                = new Dictionary<string, List<MessageHandlerDelegate>>();
            private Queue<BaseMessage> _messageQueue = new Queue<BaseMessage>();
            private Stack<string> _typeStack = new Stack<string>();
            private Stack<MessageHandlerDelegate> _handlerStack = new Stack<MessageHandlerDelegate>();
            private const float _MAX_QUEUE_PROCESSING_TIME = 0.03334f;
            private bool _isTriggered;
            #endregion

            #region Properties
            public static MessagingSystem Instance
            {
                get { return ((MessagingSystem)_Instance); }
                set { _Instance = value; }
            }
            #endregion

            private void Update()
            {
                float timer = 0f;

                while (_typeStack.Count > 0)
                    _listenerDict[_typeStack.Pop()].Add(_handlerStack.Pop());

                while (_messageQueue.Count > 0)
                {
                    if (_MAX_QUEUE_PROCESSING_TIME > 0f)
                        if (timer > _MAX_QUEUE_PROCESSING_TIME)
                            return;

                    BaseMessage msg = _messageQueue.Dequeue();
                    if (!TriggerMessage(msg))
                        Debug.LogWarningFormat("Error when processing message: {0}", msg.name);

                    if (_MAX_QUEUE_PROCESSING_TIME > 0f)
                        timer += Time.deltaTime;
                }
            }

            /// <summary>
            /// Ruft die Handler-Funktion der Listener auf
            /// </summary>
            /// <param name="msg">Message Klasse</param>
            /// <returns>Konnte die Nachricht verarbeitet werden?</returns>
            private bool TriggerMessage(BaseMessage msg)
            {
                _isTriggered = true;
                string msgName = msg.name;

                if (!_listenerDict.ContainsKey(msgName))
                {
                    Debug.LogFormat("MessagingSystem: Message \"{0}\" has no listeners!", msgName);
                    _isTriggered = false;
                    return false;
                }

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
            /// Fügt einen Listener hinzu
            /// </summary>
            /// <param name="type">Typ der Nachricht</param>
            /// <param name="handler">Handlerfunktion des Listeners</param>
            /// <returns>Wurde der Listener erfolgreich hinzugefügt?</returns>
            public bool AttachListener(System.Type type, MessageHandlerDelegate handler)
            {
                if (type == null)
                {
                    Debug.LogWarning("AttachListener failed due to no message specified!\n");
                    return false;
                }

                string msgName = type.Name;

                if (!_listenerDict.ContainsKey(msgName))
                    _listenerDict.Add(msgName, new List<MessageHandlerDelegate>());

                bool contains = _listenerDict[msgName].Contains(handler);

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
            /// Entfernt einen Listener
            /// </summary>
            /// <param name="type">Typ der Nachricht</param>
            /// <param name="handler">Handlerfunktion des Listeners</param>
            /// <returns>Wurde der Listener erfolgreich entfernt?</returns>
            public bool DetachListener(System.Type type, MessageHandlerDelegate handler)
            {
                if (type == null)
                {
                    Debug.LogWarning("DetachListener failed due to no message specified!\n");
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
            /// Sendet eine Nachricht
            /// </summary>
            /// <param name="msg">Nachricht</param>
            /// <returns>Ist ein Eintrag für diese Nachricht vorhanden?</returns>
            public void QueueMessage(BaseMessage msg)
            {
                if (!_listenerDict.ContainsKey(msg.name))
                    Debug.LogWarningFormat("{0} is not registered!\n", msg.name);

                _messageQueue.Enqueue(msg);
            }

            /// <summary>
            /// Delegate Funktion, die Listener implementieren
            /// </summary>
            /// <param name="message">Neue Nachricht</param>
            /// <returns>Konnte die Nachricht bearbeitet werden?</returns>
            public delegate bool MessageHandlerDelegate(BaseMessage message);

        }
    }
}