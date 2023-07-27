using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace S_Durlanik.UI
{
    public class UI_System : MonoBehaviour 
    {
        #region Variables
        public UI_Screen startScreen;
        public UI_Screen playScreen;
        public List<TextMeshProUGUI> goldAmountTexts;
        
        public static event Action OnSwitchedScreen;

        public Image fader;
        public float fadeInDuration = 1f;
        public float fadeOutDuration = 1f;

        
        private Component[] _screens = Array.Empty<Component>();

        private UI_Screen _previousScreen;
        public UI_Screen PreviousScreen => _previousScreen;

        private UI_Screen _currentScreen;
        public UI_Screen CurrentScreen => _currentScreen;
        
        public static UI_System Instance { get; private set; }

        #endregion


        #region Main Methods
    	// Use this for initialization
        private void Awake()
        {
            if(Instance)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start () 
        {
            _screens = GetComponentsInChildren<UI_Screen>(true);
            InitializeScreens();

            if(startScreen)
            {
                SwitchScreens(startScreen);
            }

            if(fader)
            {
                fader.gameObject.SetActive(true);
            }
            FadeIn();
    	}
        #endregion



        #region Helper Methods
        public bool _screenSwitching = false;
        public void SwitchScreens(UI_Screen aScreen)
        {
            if(aScreen)
            {
                if (_screenSwitching) return;
                _screenSwitching = true;
                if(_currentScreen)
                {
                    if (_currentScreen.name != "Main_Screen")
                    {
                        _currentScreen.CloseScreen();
                    }
                    
                    if (aScreen.name == "Play_Screen")
                    {
                        startScreen.gameObject.SetActive(false);
                    }

                    _previousScreen = _currentScreen;
                }

                _currentScreen = aScreen;
                _currentScreen.gameObject.SetActive(true);
                _currentScreen.StartScreen();

                Debug.Log("Current Screen: " + _currentScreen.name);
                OnSwitchedScreen?.Invoke();
                StartCoroutine(WaitToSwitchScreens(_currentScreen));
            }
        }
        
        IEnumerator WaitToSwitchScreens(UI_Screen aScreen)
        {
            while (aScreen.GetComponent<CanvasGroup>().alpha < 1)
            {
                print("Current Screen alpha and name: "+aScreen.name +" " + aScreen.GetComponent<CanvasGroup>().alpha);
                yield return new WaitForSeconds(.05f);
            }
            _screenSwitching = false;
        }

        public void FadeIn()
        {
            if(fader)
            {
                fader.CrossFadeAlpha(0f, fadeInDuration, false);
            }
        }

        public void FadeOut()
        {
            if(fader)
            {
                fader.CrossFadeAlpha(1f, fadeOutDuration, false);
            }
        }

        public void GoToPreviousScreen()
        {
            if(_previousScreen)
            {
                SwitchScreens(_previousScreen);
            }
        } 
        public void GoToMainScreen()
        {
            if(startScreen)
            {
                SwitchScreens(startScreen);
            }
        }

        public void GoToPlayScreen()
        {
            if(playScreen)
            {
                SwitchScreens(playScreen);
            }
        }

        public void LoadScene(int sceneIndex)
        {
            StartCoroutine(WaitToLoadScene(sceneIndex));
        }

        private IEnumerator WaitToLoadScene(int sceneIndex)
        {
            yield return null;
        }

        private void InitializeScreens()
        {
            foreach(var screen in _screens)
            {
                screen.gameObject.SetActive(true);
            }
        }
        #endregion
    }
}
