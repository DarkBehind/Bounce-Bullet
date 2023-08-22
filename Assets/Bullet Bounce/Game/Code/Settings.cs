using System.Collections;
using System.Collections.Generic;
using S_Durlanik.Sound;
using UnityEngine;
using UnityEngine.UI;

namespace S_Durlanik
{
    public class Settings : MonoBehaviour
    {
        public Slider musicSlider;
        public Slider sfxSlider;
        public Image musicOnOffImage;
        public Image sfxOnOffImage;
        [SerializeField] private List<Sprite> musicOnOffSprites;
        [SerializeField] private List<Sprite> sfxOnOffSprites;

        private void Start()
        {
            SoundManager.Instance.musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", .5f);
            SoundManager.Instance.sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", .5f);
            
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", .5f);
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", .5f);
        }
        
        public void MusicSlider_OnValueChanged()
        {
            SoundManager.Instance.musicSource.volume = musicSlider.value;
            PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
            ChangeSprite(true);
        }
        
        public void SFXSlider_OnValueChanged()
        {
            SoundManager.Instance.sfxSource.volume = sfxSlider.value;
            PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
            ChangeSprite(false);
        }

        void ChangeSprite(bool isMusic)
        {
            if(isMusic)
                if(musicSlider.value <= 0)
                    musicOnOffImage.sprite = GetSprite(true, true);
                else
                    musicOnOffImage.sprite = GetSprite(true, false);
            else
                if(sfxSlider.value <= 0)
                    sfxOnOffImage.sprite = GetSprite(false, true);
                else
                    sfxOnOffImage.sprite = GetSprite(false, false);
                
        }

        Sprite GetSprite(bool isMusic, bool isOff)
        {
            if(isMusic)
                if(isOff)
                    return musicOnOffSprites[0];
                else
                    return musicOnOffSprites[1];
            else
                if(isOff)
                    return sfxOnOffSprites[0];
                else
                    return sfxOnOffSprites[1];
        }
    }
}
