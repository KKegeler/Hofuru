using UnityEngine;
using System.Collections;

/// <summary>
/// Camera Shake
/// </summary>
public class CameraShake : MonoBehaviour
{
    #region Variables
    private static CameraShake _instance;
    
    [SerializeField]
    private float _shakeTime;
    [SerializeField]
    private float _shakeAmount;
    [SerializeField]
    private float _shakeSpeed;

    private Transform _tf;
    #endregion

    void Start()
    {
        _tf = transform;
    }

    #region Shake
    /// <summary>
    /// Shakes the camera
    /// </summary>
    public void ShakeCamera()
    {
        StartCoroutine(Shake());
    }

    /// <summary>
    /// Shakes the camera
    /// </summary>
    /// <param name="time">Shake time</param>
    /// <param name="amount">Shake amount</param>
    /// <param name="speed">Shake speed</param>
    public void ShakeCamera(float time, float amount, float speed)
    {
        StartCoroutine(Shake(time, amount, speed));
    }
    #endregion

    private IEnumerator Shake()
    {
        Vector3 pos = _tf.localPosition;

        float timer = 0.0f;

        while (timer < _shakeTime)
        {
            Vector3 randPoint = pos +
                Random.insideUnitSphere * _shakeAmount;

            _tf.localPosition = Vector3.Lerp(_tf.localPosition, 
                randPoint, Time.deltaTime * _shakeSpeed);

            yield return null;

            timer += Time.deltaTime;
        }

        _tf.localPosition = pos;
    }

    private IEnumerator Shake(float time, float amount, float speed)
    {
        Vector3 pos = _tf.localPosition;

        float timer = 0.0f;

        while (timer < time)
        {
            Vector3 randPoint = pos +
                Random.insideUnitSphere * amount;

            _tf.localPosition = Vector3.Lerp(_tf.localPosition,
                randPoint, Time.deltaTime * speed);

            yield return null;

            timer += Time.deltaTime;
        }

        _tf.localPosition = pos;
    }

}