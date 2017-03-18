using UnityEngine;
using System.Collections;

public class TimeController : MonoBehaviour {

    public AnimationCurve animCurve;
    private float originTimeScale;
    private float originFixedDeltaTime;
    private IEnumerator freezeRoutine;
    //private CameraController camController;

    // Use this for initialization
    void Start() {
        //this.camController = GameObjectBank.Instance.gameController.GetComponent<CameraController>();
        originTimeScale = Time.timeScale;
        originFixedDeltaTime = Time.fixedDeltaTime;
    }

    /// <summary>
    /// Versetzt Spiel in Zeitlupeneffekt oder kompletten Stopp. 
    /// </summary>
    /// <param name="duration">Zeit bis minValue (=timeScale) erreicht werden soll</param>
    /// <param name="minValue">minimalster timeScale, der erreicht werden soll</param>
    /// <param name="steps">Anzahl der Abstufungen</param>
    public void FreezeTime(float duration, float minValue, int steps) {
        this.freezeRoutine = FreezeRoutine(duration, minValue, steps);
        StartCoroutine(this.freezeRoutine);
    }

    public void SetOriginTime() {
        StopCoroutine(this.freezeRoutine);
        Time.timeScale = originTimeScale;
        Time.fixedDeltaTime = originFixedDeltaTime;
    }

    private IEnumerator FreezeRoutine(float duration, float minvalue, int steps) {
        /*
        float stepTime = duration / steps;
        float reduceAmount = (originTimeScale - minvalue) / steps;
        for (int i = 0; i < steps; i++) {
            if (Time.timeScale - reduceAmount >= 0) {
                Time.timeScale -= reduceAmount;
                Time.fixedDeltaTime = originFixedDeltaTime * Time.timeScale;
                yield return new WaitForSeconds(stepTime);
            }
        }
        */

        float stepTime = duration / steps;
        animCurve.RemoveKey(animCurve.keys.Length - 1);
        animCurve.AddKey(new Keyframe(1, minvalue));
        for (int i = 0; i < steps; i++) {
           // Debug.Log(animCurve.Evaluate((float)i / (float)steps));
            Time.timeScale = animCurve.Evaluate((float) i / (float) steps);
            Time.fixedDeltaTime = originFixedDeltaTime * Time.timeScale;
            yield return new WaitForSecondsRealtime(stepTime);
        }
    }
}
