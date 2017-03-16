using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class OptionsMenu : Window {

    public Slider sfxSlider;
    public Slider musicSlider;

    public Slider masterSlider;

    public Text masterText;
    public Text musicText;
    public Text sfxText;
    public Text qualityText;

    public GameObject audioObject;

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
        //EventSystem.current.SetSelectedGameObject(null);
        manager.Open(0);
        
    }

    public void setMasterVolume()
	{
        audioObject.GetComponent<Audio.AudioManager>().MasterVolume = masterSlider.value;
		//AudioListener.volume = musicSlider.value;
		UpdateVolumeLabel();
	}

    public void setMusicVolume()
    {
        audioObject.GetComponent<Audio.AudioManager>().MusicVolume = musicSlider.value;
        UpdateVolumeLabel();
    }

    public void setSFXVolume()
    {
        audioObject.GetComponent<Audio.AudioManager>().SfxVolume = sfxSlider.value;
        UpdateVolumeLabel();
    }

    private void UpdateVolumeLabel()
    {
         masterText.text = "MasterVolume - " + (audioObject.GetComponent<Audio.AudioManager>().MasterVolume * 100).ToString("f2") + "%";
         musicText.text = "MusicVolume - " + (audioObject.GetComponent<Audio.AudioManager>().MusicVolume * 100).ToString("f2") + "%";
         sfxText.text = "SFXVolume - " + (audioObject.GetComponent<Audio.AudioManager>().SfxVolume * 100).ToString("f2") + "%";
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
