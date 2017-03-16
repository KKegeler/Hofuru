﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Framework.Log;

namespace Framework
{
    namespace Audio
    {
        /// <summary>
        /// Plays music and SFX
        /// </summary>
        public class AudioManager : MonoBehaviour
        {
            #region Variables
            private static AudioManager _instance;

            [Range(0f, 1f)]
            private static float _masterVolume = 1f;
            [Range(0f, 1f)]
            private static float _musicVolume = 1f;
            [Range(0f, 1f)]
            private static float _sfxVolume = 1f;

            private const float _MIN_FADE_TIME = 1f;
            private const float _MAX_FADE_TIME = 5f;
            private const float _DEFAULT_FADE_TIME = -1f;

            [Header("Sound")]
            [SerializeField]
            private List<AudioClip> _music = new List<AudioClip>();
            [SerializeField]
            private List<AudioClip> _sfx = new List<AudioClip>();

            [Header("Fade")]
            [SerializeField]
            [Range(_MIN_FADE_TIME, _MAX_FADE_TIME)]
            private float _fadeTime = _MIN_FADE_TIME;

            private AudioSource _mainSource;
            private AudioSource _sfxSource;
            #endregion

            #region Properties
            public static AudioManager Instance
            {
                get { return _instance; }
            }

            public float MasterVolume
            {
                get { return _masterVolume; }
                set
                {
                    _masterVolume = value;
                    _mainSource.volume = _masterVolume * _musicVolume;
                    _sfxSource.volume = _masterVolume * _sfxVolume;
                }
            }

            public float MusicVolume
            {
                get { return _musicVolume; }
                set { _musicVolume = value; }
            }

            public float SfxVolume
            {
                get { return _sfxVolume; }
                set { _sfxVolume = value; }
            }
            #endregion

            private void Awake()
            {
                if (!_instance)
                    _instance = this;
                else if (_instance != this)
                    Destroy(gameObject);

                DontDestroyOnLoad(gameObject);

                List<AudioSource> sources = new List<AudioSource>(GetComponents<AudioSource>());

                while (sources.Count < 2)
                    sources.Add(gameObject.AddComponent<AudioSource>());

                _mainSource = sources[0];
                _sfxSource = sources[1];
                _mainSource.loop = true;

                MusicVolume = _mainSource.volume;
                SfxVolume = _sfxSource.volume;
            }

            #region SFX
            /// <summary>
            /// Plays SFX on other source
            /// </summary>
            /// <param name="clipIndex">Clip to play by index</param>
            /// <param name="other">Audio source</param>
            public void PlaySfx(int clipIndex, AudioSource other)
            {
                if (clipIndex < 0 || clipIndex >= _sfx.Count)
                {
                    CustomLogger.LogWarningFormat("Index {0} is out of range!\n",
                        clipIndex);
                    return;
                }

                AudioClip clip = _sfx[clipIndex];
                other.PlayOneShot(clip, MasterVolume * SfxVolume);
            }

            /// <summary>
            /// Plays Sfx
            /// </summary>
            /// <param name="clipIndex">Clip to play by index</param>
            public void PlaySfx(int clipIndex)
            {
                if (clipIndex < 0 || clipIndex >= _sfx.Count)
                {
                    CustomLogger.LogWarningFormat("Index {0} is out of range!\n",
                        clipIndex);
                    return;
                }

                AudioClip clip = _sfx[clipIndex];
                _sfxSource.PlayOneShot(clip, MasterVolume * SfxVolume);
            }

            /// <summary>
            /// Plays Sfx on other Source
            /// </summary>
            /// <param name="clip">Clip to play</param>
            /// <param name="other">Audio source</param>
            public void PlaySfx(AudioClip clip, AudioSource other)
            {
                other.PlayOneShot(clip, MasterVolume * SfxVolume);
            }

            /// <summary>
            /// Plays Sfx
            /// </summary>
            /// <param name="clip">Clip to play</param>
            public void PlaySfx(AudioClip clip)
            {
                _sfxSource.PlayOneShot(clip, MasterVolume * SfxVolume);
            }
            #endregion

            #region Music
            /// <summary>
            /// Plays music
            /// </summary>
            /// <param name="clipIndex">Clip to play by index</param>
            public void PlayMusic(int clipIndex)
            {
                if (clipIndex < 0 || clipIndex >= _music.Count)
                {
                    CustomLogger.LogWarningFormat("Index {0} is out of range!\n",
                        clipIndex);
                    return;
                }

                AudioClip clip = _music[Mathf.Clamp(clipIndex, 0, _music.Count - 1)];
                _mainSource.clip = clip;
                _mainSource.Play();
            }

            /// <summary>
            /// Plays music
            /// </summary>
            /// <param name="clip">Clip to play</param>
            public void PlayMusic(AudioClip clip)
            {
                _mainSource.clip = clip;
                _mainSource.Play();
            }

            /// <summary>
            /// Plays random music
            /// </summary>
            public void PlayRandomMusic()
            {
                AudioClip clip = _music[Random.Range(0, _music.Count)];
                _mainSource.clip = clip;
                _mainSource.Play();
            }
            #endregion

            /// <summary>
            /// Fades music in
            /// </summary>
            /// <param name="time">Time in seconds to fade in</param>
            public void FadeIn(float time = _DEFAULT_FADE_TIME)
            {
                if (time != _DEFAULT_FADE_TIME)
                    _fadeTime = Mathf.Clamp(time, _MIN_FADE_TIME, _MAX_FADE_TIME);

                StopAllCoroutines();
                StartCoroutine(FadeInRoutine());
            }

            /// <summary>
            /// Fades music out
            /// </summary>
            /// <param name="time">Time in seconds to fade out</param>
            public void FadeOut(float time = _DEFAULT_FADE_TIME)
            {
                if (time != _DEFAULT_FADE_TIME)
                    _fadeTime = Mathf.Clamp(time, _MIN_FADE_TIME, _MAX_FADE_TIME);

                StopAllCoroutines();
                StartCoroutine(FadeOutRoutine());
            }

            private IEnumerator FadeInRoutine()
            {
                float oldVolume = MasterVolume;
                MasterVolume = 0;

                while (MasterVolume < oldVolume)
                {
                    MasterVolume += Time.deltaTime / _fadeTime * oldVolume * MusicVolume;
                    yield return new WaitForEndOfFrame();
                }
            }

            private IEnumerator FadeOutRoutine()
            {
                float oldVolume = MasterVolume;

                while (MasterVolume > 0)
                {
                    MasterVolume -= Time.deltaTime / _fadeTime * oldVolume * MusicVolume;
                    yield return new WaitForEndOfFrame();
                }
            }

        }
    }
}