using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {

    public int maxShurikenCount;
    public float maxTimefreezeTime;
    public Slider freezeTimeSlider;
    public Image sliderFillImage;
    public Text hud_teleportCountText;
    public float minTimeNeededToFreeze;
    public int maxTeleportCount;

    private float curTimefreezeTime;
    private int curShurikenCount;
    private int curTeleportCount;
    private float curHealth;
    private Color originalFreezeTimeSliderColor;


    private Text hud_ShurikenAnzeige;

    // Use this for initialization
    void Start() {
        this.curShurikenCount = this.maxShurikenCount;
        this.curTimefreezeTime = this.maxTimefreezeTime;
        this.curTeleportCount = this.maxTeleportCount;
        this.hud_ShurikenAnzeige = GameObjectBank.Instance.hud_ShurikenCounter;
        this.originalFreezeTimeSliderColor = sliderFillImage.color;
        hud_ShurikenAnzeige.text = this.curShurikenCount.ToString();
        hud_teleportCountText.text = this.curTeleportCount.ToString();
    }

    public int GetCurrentShurikenCount() {
        return this.curShurikenCount;
    }

    public float GetCurrentTimefreezeTime() {
        return curTimefreezeTime;
    }

    public int GetCurrentTeleportCount() {
        return curTeleportCount;
    }

    public void AddSchuriken(int amount) {
        if (curShurikenCount + amount > maxShurikenCount)
            curShurikenCount = maxShurikenCount;
        else
            this.curShurikenCount += amount;

        hud_ShurikenAnzeige.text = this.curShurikenCount.ToString();
    }

    public void AddTeleport(int amount) {
        if (curTeleportCount + amount > maxTeleportCount)
            curTeleportCount = maxTeleportCount;
        else
            this.curShurikenCount += amount;

        hud_teleportCountText.text = this.curTeleportCount.ToString();
    }

    public void DecreaseCurrTeleportCount() {
        if (this.curTeleportCount > 0) {
            this.curTeleportCount--;
            hud_teleportCountText.text = curTeleportCount.ToString();
        }
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
        if (curTimefreezeTime < minTimeNeededToFreeze) {
            sliderFillImage.color = Color.red;        }
    }

    public void InCreaseCurrentFreezeTime(float amount) {
        curTimefreezeTime += amount;
        freezeTimeSlider.value = curTimefreezeTime / maxTimefreezeTime;
        if (curTimefreezeTime >= minTimeNeededToFreeze) {
            sliderFillImage.color = originalFreezeTimeSliderColor;
        }
    }
}

