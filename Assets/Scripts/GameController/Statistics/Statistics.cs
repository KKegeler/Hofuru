using UnityEngine;
using Framework.Messaging;

/// <summary>
/// Manages the player's statistics
/// </summary>
public class Statistics : MonoBehaviour
{
    #region Variables
    private static Statistics _instance;
    [SerializeField]
    private float _score;
    [SerializeField]
    private float _time;
    #endregion

    #region Properties
    public static Statistics Instance
    {
        get { return _instance; }
    }

    public float Score
    {
        get { return _score; }
        set
        {
            _score = value;
            MessagingSystem.Instance.QueueMessage(new ScoreTextMessage(_score));
        }
    }

    public float FinalScore
    {
        get { return CalculateFinalScore(); }
    }
    #endregion

    private void Awake()
    {
        if (!_instance)
            _instance = this;
        if (_instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        MessagingSystem.Instance.AttachListener(typeof(ScoreMessage), ScoreHandler);
        GameManager.Instance.WakeUp();
    }

    private void Update()
    {
        _time += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R))
            DataSerializer.Reset();

        if (Input.GetKeyDown(KeyCode.T))
            DataSerializer.TestLog();
    }

    #region Handler
    public bool ScoreHandler(BaseMessage msg)
    {
        ScoreMessage castMsg = (ScoreMessage)msg;
        Score += castMsg.score;

        return true;
    }
    #endregion

    private float CalculateFinalScore()
    {
        // Do crazy math stuff here
        return _score;
    }

    private void OnDestroy()
    {
        if (MessagingSystem.IsAlive)
            MessagingSystem.Instance.DetachListener(typeof(ScoreMessage), ScoreHandler);
    }

}