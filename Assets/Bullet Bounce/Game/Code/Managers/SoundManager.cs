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
