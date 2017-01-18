using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {

    public int maxShurikenCount;
    public float maxTimefreezeTime;
    public Slider freezeTimeSlider;

    private float curTimefreezeTime;
    private int curShurikenCount;
    private float curHealth;

    private Text hud_ShurikenAnzeige;

    // Use this for initialization
    void Start() {
        this.curShurikenCount = this.maxShurikenCount;
        this.curTimefreezeTime = this.maxTimefreezeTime;
        this.hud_ShurikenAnzeige = GameObjectBank.instance.hud_ShurikenCounter;
        hud_ShurikenAnzeige.text = this.curShurikenCount.ToString();
    }

    public int GetCurrentShurikenCount() {
        return this.curShurikenCount;
    }

    public float GetCurrentTimefreezeTime() {
        return curTimefreezeTime;
    }

    public void AddSchuriken(int amount) {
        this.curShurikenCount += amount;
        hud_ShurikenAnzeige.text = this.curShurikenCount.ToString();
    }

    public void DecreaseCurrSchurikenCount() {
        if (this.curShurikenCount > 0) {
            this.curShurikenCount--;
            hud_ShurikenAnzeige.text = this.curShurikenCount.ToString();
        }
    }

    public void ReduceCurrentFreezeTime(float amount) {
        curTimefreezeTime -= amount;
        freezeTimeSlider.value = curTimefreezeTime / maxTimefreezeTime;
    }

    public void InCreaseCurrentFreezeTime(float amount) {
        curTimefreezeTime += amount;
        freezeTimeSlider.value = curTimefreezeTime / maxTimefreezeTime;
    }
}
