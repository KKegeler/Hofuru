using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : Window {

    public Slider soundSlider;
    public Slider musicSlider;

    public Text musicText;
    public Text qualityText;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        UpdateQualityLabel();
		UpdateVolumeLabel();
    }

	public void Back()
    {
        manager.Open(0);
    }

    public void setVolume()
	{
		AudioListener.volume = musicSlider.value;
		UpdateVolumeLabel();
	}

    private void UpdateVolumeLabel()
    {
         musicText.text = "MasterVolume - " + (AudioListener.volume * 100).ToString("f2") + "%";
         
    }

    public void IncreaseQuality()
	{
		QualitySettings.IncreaseLevel();
		UpdateQualityLabel();
	}

	public void DecreaseQuality()
	{
		QualitySettings.DecreaseLevel();
		UpdateQualityLabel();
	}

    private void UpdateQualityLabel()
    {
        int currentQuality = QualitySettings.GetQualityLevel();
		string qualityName = QualitySettings.names[currentQuality];
		qualityText.text = "Quality Level - " + qualityName;
    }
}
