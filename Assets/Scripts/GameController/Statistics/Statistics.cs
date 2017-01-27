using UnityEngine;
using Framework;
using Framework.Messaging;

/// <summary>
/// Verwaltet Statistiken des Spielers
/// </summary>
public class Statistics : MonoBehaviour
{
    #region Variablen
    public static Statistics Instance;

    [SerializeField]
    private float _score;
    [SerializeField]
    private float _time;
    #endregion

    #region Properties
    public float Score
    {
        get { return _score; }
        set
        {
            _score = value;
            MessagingSystem.Instance.QueueMessage(new ScoreTextMessage(_score));
        }
    }
    #endregion

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        if (Instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        MessagingSystem.Instance.AttachListener(typeof(ScoreMessage), ScoreHandler);
    }

    private void Update()
    {
        _time += Time.deltaTime;
    }

    #region Handler
    public bool ScoreHandler(BaseMessage msg)
    {
        ScoreMessage castMsg = (ScoreMessage)msg;
        Score += castMsg.score;

        return true;
    }
    #endregion

    private void OnDestroy()
    {
        if (MessagingSystem.IsAlive)
            MessagingSystem.Instance.DetachListener(typeof(ScoreMessage), ScoreHandler);
    }

}