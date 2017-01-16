using UnityEngine;
using UnityEngine.UI;
using Framework;
using Framework.Messaging;

public class ScoreText : MonoBehaviour
{
    #region Variablen
    private Text _scoreText;
    #endregion

    private void Start()
    {
        MessagingSystem.Instance.AttachListener(typeof(ScoreTextMessage), ScoreTextHandler);
        _scoreText = GetComponent<Text>();
    }

    #region Handler
    public bool ScoreTextHandler(BaseMessage msg)
    {
        ScoreTextMessage castMsg = (ScoreTextMessage)msg;
        _scoreText.text = castMsg.scoreText;

        return true;
    }
    #endregion

    private void OnDestroy()
    {
        if (MessagingSystem.IsAlive)
            MessagingSystem.Instance.DetachListener(typeof(ScoreTextMessage), ScoreTextHandler);
    }

}