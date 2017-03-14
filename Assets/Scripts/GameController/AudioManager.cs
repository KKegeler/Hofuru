using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Plays music and SFX
/// </summary>
public class AudioManager : MonoBehaviour
{
    #region Variables
    private static AudioManager _instance;

    [SerializeField]
    private List<AudioClip> _music = new List<AudioClip>();
    [SerializeField]
    private List<AudioClip> _sfx = new List<AudioClip>();
    [SerializeField] [Range(1f, 5f)]
    private float _fadeTime = 1f;
    [Range(0f, 1f)]
    private float _volume;

    private AudioSource _source;
    #endregion

    #region Properties
    public static AudioManager Instance
    {
        get { return _instance; }
    }

    public float Volume
    {
        get { return _volume; }
        set
        {
            _volume = value;
            _source.volume = _volume;
        }
    }
    #endregion

    private void Awake()
    {
        if (!_instance)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _source = GetComponent<AudioSource>();
        _source.loop = true;

        Volume = _source.volume;
    }

    /// <summary>
    /// Plays SFX
    /// </summary>
    /// <param name="clipIndex">Clip to play by index</param>
    /// <param name="otherSource">Audio source</param>
    public void PlaySfx(int clipIndex, AudioSource otherSource)
    {
        AudioClip clip = _sfx[Mathf.Clamp(clipIndex, 0, _sfx.Count)];
        otherSource.PlayOneShot(clip, Volume);
    }

    /// <summary>
    /// Plays music
    /// </summary>
    /// <param name="clipIndex">Clip to play by index</param>
    public void PlayMusic(int clipIndex)
    {
        AudioClip clip = _music[Mathf.Clamp(clipIndex, 0, _music.Count - 1)];
        _source.clip = clip;
        _source.Play();
    }

    /// <summary>
    /// Plays random music
    /// </summary>
    public void PlayRandomMusic()
    {
        AudioClip clip = _music[Random.Range(0, _music.Count)];
        _source.clip = clip;
        _source.Play();
    }

    /// <summary>
    /// Fades music in
    /// </summary>
    /// <param name="time">Time in seconds to fade in</param>
    public void FadeIn(float time = -1)
    {
        if (time != -1)
            _fadeTime = Mathf.Clamp(time, 1f, 5f);

        StopAllCoroutines();
        StartCoroutine(FadeInRoutine());
    }

    /// <summary>
    /// Fades music out
    /// </summary>
    /// <param name="time">Time in seconds to fade out</param>
    public void FadeOut(float time = -1)
    {
        if (time != -1)
            _fadeTime = Mathf.Clamp(time, 1f, 5f);

        StopAllCoroutines();
        StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeInRoutine()
    {
        float oldVolume = Volume;
        Volume = 0;

        while (Volume < oldVolume)
        {
            Volume += Time.deltaTime / _fadeTime * oldVolume;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator FadeOutRoutine()
    {
        float oldVolume = Volume;

        while (Volume > 0)
        {
            Volume -= Time.deltaTime / _fadeTime * oldVolume;
            yield return new WaitForEndOfFrame();
        }
    }

}