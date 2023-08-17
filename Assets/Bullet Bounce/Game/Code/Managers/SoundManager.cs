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
            foreach (string sfx in SFX.GetNames(typeof(SFX)))
            {
                _sfxSources.Add(Resources.Load<AudioClip>("SFX/"+sfx));
            }
        }

        public void PlaySFXOneTime(SFX sfx)
        {
            AudioClip clip = _sfxSources.Find(x => x.name == sfx.ToString());
            if(clip)
                sfxSource.PlayOneShot(clip);
        }

        #region Button Sound

        public void PlayButtonSound()
        {
            PlaySFXOneTime(SFX.Button_Sound_01);
        }

        #endregion
    }

    public enum SFX
    {
        Button_Sound_01
    }
}
