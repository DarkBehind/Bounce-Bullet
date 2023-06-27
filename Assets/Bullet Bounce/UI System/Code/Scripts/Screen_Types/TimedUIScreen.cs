using System.Collections;
using System.Collections.Generic;
using S_Durlanik.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace S_Durlanik.UI
{
    public class TimedUIScreen : UI_Screen 
    {
        #region Variables
        [Header("Timed Screen Properties")]
        public float screenTime = 2f;
        public UnityEvent onTimeCompleted = new UnityEvent();

        private float _startTime;
        #endregion

        #region Helper Methods
        public override void StartScreen()
        {
            base.StartScreen();

            _startTime = Time.time;
            StartCoroutine(WaitForTime());
        }

        private IEnumerator WaitForTime()
        {
            yield return new WaitForSeconds(screenTime);

            onTimeCompleted?.Invoke();
        }
        #endregion
    }
}
