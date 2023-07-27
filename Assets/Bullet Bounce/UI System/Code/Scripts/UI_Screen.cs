using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace S_Durlanik.UI
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CanvasGroup))]
    public class UI_Screen : MonoBehaviour 
    {
        #region Variables
        public Selectable startSelectable;
        public bool screenSwitching = false;
        public static event Action OnScreenStart;
        public static event Action OnScreenClose ;

        private Animator _animator;
        #endregion


        #region Main Methods
    	// Use this for initialization
        private void Start () 
        {
            _animator = GetComponent<Animator>();

            if(startSelectable)
            {
                EventSystem.current.SetSelectedGameObject(startSelectable.gameObject);
            }
    	}
        #endregion


        #region Helper Methods
        public virtual void StartScreen(Action onCompleted = null)
        {
            if (screenSwitching) return;
            OnScreenStart?.Invoke();

            HandleAnimator("show");
            StartCoroutine(WaitToSwitchScreens(this, false, onCompleted));
        }


        public virtual void CloseScreen(Action onCompleted = null)
        {
            if (screenSwitching) return;
            OnScreenClose?.Invoke();
            HandleAnimator("hide");
            StartCoroutine(WaitToSwitchScreens(this, true,onCompleted));
        }


        public void HandleAnimator(string aTrigger)
        {
            if(_animator)
            {
                _animator.SetTrigger(aTrigger);
            }
        }
        
        IEnumerator WaitToSwitchScreens(UI_Screen aScreen, bool isClosing,Action onCompleted = null)
        {
            screenSwitching = true;
            float alpha = isClosing ? 0 : 1;
            if (isClosing)
            {
                while(aScreen.GetComponent<CanvasGroup>().alpha > alpha)
                {
                    print("Current Screen alpha and name: "+aScreen.name +" " + aScreen.GetComponent<CanvasGroup>().alpha);
                    yield return new WaitForSeconds(.05f);
                }
            }
            else
            {
                while(aScreen.GetComponent<CanvasGroup>().alpha < alpha)
                {
                    print("Current Screen alpha and name: "+aScreen.name +" " + aScreen.GetComponent<CanvasGroup>().alpha);
                    yield return new WaitForSeconds(.05f);
                }
            }
            
            screenSwitching = false;
            onCompleted?.Invoke();
        }
        #endregion
    }
}
