using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace S_Durlanik.Sound
{
    public class SoundManager : MonoBehaviour
    {
        #region Instance

        public static SoundManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
                return;
            }

            Destroy(gameObject);
        }

        #endregion
        
        public AudioSource musicSource;
        public AudioSource sfxSource;
        public List<AudioClip> _sfxSources;
        private void Start()
        {
            musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", .1f);
            sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", .5f);
            
            foreach (string sfx in SFX.GetNames(typeof(SFX)))
            {
                _sfxSources.Add(Resources.Load<AudioClip>("SFX/"+sfx));
            }
            PlayMusic(Music.Main_Menu_Music_01);
        }
        
        public void PlayMusic(Music music)
        {
            AudioClip clip = Resources.Load<AudioClip>("Music/"+music);
            if(clip)
                musicSource.clip = clip;
            musicSource.Play();
        }

        // SFX
        public void PlaySFXOneTime(SFX sfx)
        {
            AudioClip clip = _sfxSources.Find(x => x.name == sfx.ToString());
            if(clip)
                sfxSource.PlayOneShot(clip);
        }

        public void PlaySFX(SFX sfx,bool onLoop = false,float delay = 0f)
        {
            AudioClip clip = _sfxSources.Find(x => x.name == sfx.ToString());
            if(clip)
                sfxSource.clip = clip;
            sfxSource.PlayDelayed(delay);
            sfxSource.loop = onLoop;
        }

        #region Button Sound

        public void PlayButtonSound()
        {
            PlaySFXOneTime(SFX.Button_Sound_01);
        }

        #endregion
    }

    public enum Music
    {
        Main_Menu_Music_01,
    }
    public enum SFX
    {
        Button_Sound_01,
        Arrow_Sound_01,
        Hurt_Sound_01,
        Hurt_Sound_02,
        Hurt_Sound_03,
        Hurt_Sound_04,
        Hurt_Sound_05,
        SFX_SpinWheel_Start_Sound_1,
        SpinWheel_Fast_Loop_Sound_1,
        SpinWheel_Slow_Loop_Sound_1
    }
}
