using UnityEngine;
using System.Collections;

// CameraShake bei Schaden am Spieler

public class CameraShake : MonoBehaviour
{
    #region Variablen
    // Singleton
    static CameraShake _instance;
    public static CameraShake instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<CameraShake>();

            return _instance;
        }
    }

    private Transform thisTransform = null;

    public float shakeTime;
    public float shakeAmount;
    public float shakeSpeed;
    #endregion

    #region Init
    void Start()
    {
        thisTransform = GetComponent<Transform>();
    }
    #endregion

    #region ShakeCamera
    public void ShakeCamera()
    {
        StartCoroutine("Shake");
    }

    //Shake camera coroutine
    IEnumerator Shake()
    {
        Vector3 OrigPosition = thisTransform.localPosition;

        float ElapsedTime = 0.0f;

        while (ElapsedTime < shakeTime)
        {
            Vector3 RandomPoint = OrigPosition +
                Random.insideUnitSphere * shakeAmount;

            thisTransform.localPosition = Vector3.Lerp(thisTransform.localPosition, 
                RandomPoint, Time.deltaTime * shakeSpeed);

            yield return null;

            ElapsedTime += Time.deltaTime;
        }

        thisTransform.localPosition = OrigPosition;
    }
    #endregion

}