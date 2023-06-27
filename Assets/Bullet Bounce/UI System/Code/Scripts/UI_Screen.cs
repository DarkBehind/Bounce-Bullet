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
        public virtual void StartScreen()
        {
            OnScreenStart?.Invoke();

            HandleAnimator("show");
        }


        public virtual void CloseScreen()
        {
            OnScreenClose?.Invoke();

            HandleAnimator("hide");
        }


        private void HandleAnimator(string aTrigger)
        {
            if(_animator)
            {
                _animator.SetTrigger(aTrigger);
            }
        }
        #endregion
    }
}
