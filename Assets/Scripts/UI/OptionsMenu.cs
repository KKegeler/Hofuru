using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : Window {

    public Slider soundSlider;
    public Slider musicSlider;

	public void Back()
    {
        manager.Open(0);
    }

    public void muteSound()
    {
        AudioListener.volume = soundSlider.value;
    }
}
