﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Camera Shake
/// </summary>
public class CameraShake : MonoBehaviour
{
    #region Variables
    private static CameraShake _instance;
    
    [SerializeField]
    private float _shakeTime = 0;
    [SerializeField]
    private float _shakeAmount = 0;
    [SerializeField]
    private float _shakeSpeed = 0;

    private Transform _tf;
    #endregion

    #region Properties
    public static CameraShake Instance  
    {
        get { return _instance; }
    }
    #endregion

    private void Awake() 
    {
        if (!_instance)
            _instance = this;
        else if (_instance != this)
            gameObject.SetActive(false);
    }

    private void Start()
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
            pos = _tf.localPosition;
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
            pos = _tf.localPosition;
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