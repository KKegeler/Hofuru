using UnityEngine;
using UnityEngine.UI;
using Framework.Audio;

public class OptionsMenu : Window
{

    public Slider sfxSlider;
    public Slider musicSlider;

    public Slider masterSlider;

    public Text masterText;
    public Text musicText;
    public Text sfxText;
    public Text qualityText;

    void Start()
    {
        masterSlider.value = AudioManager.Instance.MasterVolume;
        UpdateQualityLabel();
        UpdateVolumeLabel();
    }

    public void Back()
    {
        manager.Open(0);
    }

    public void setMasterVolume()
    {
        AudioManager.Instance.MasterVolume = masterSlider.value;
        UpdateVolumeLabel();
    }

    public void setMusicVolume()
    {
        AudioManager.Instance.MusicVolume = musicSlider.value;
        UpdateVolumeLabel();
    }

    public void setSFXVolume()
    {
        AudioManager.Instance.SfxVolume = sfxSlider.value;
        UpdateVolumeLabel();
    }

    private void UpdateVolumeLabel()
    {
        masterText.text = string.Concat("MasterVolume \t- ",
            (AudioManager.Instance.MasterVolume * 100).ToString("f2"), "%");
        musicText.text = string.Concat("MusicVolume \t- ",
            (AudioManager.Instance.MusicVolume * 100).ToString("f2"), "%");
        sfxText.text = string.Concat("SFXVolume \t\t- ",
            (AudioManager.Instance.SfxVolume * 100).ToString("f2"), "%");
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
        qualityText.text = string.Concat("Quality Level - ", qualityName);
    }
}
