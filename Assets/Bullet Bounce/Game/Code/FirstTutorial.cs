using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using S_Durlanik.Game;
using TMPro;
using UnityEngine;

namespace S_Durlanik
{
    public class FirstTutorial : MonoBehaviour
    {
        public Animator firstTutorialAnimator;
        public TextMeshPro firstTutorialText;
        public SpriteMask spriteMask;
        public RectTransform bulletAim;
        public Animator bulletAimAnimator;
        void Start()
        {
            if (!PlayerPrefs.HasKey(Extensions.Prefs.FirstTutorial))
            {
                LevelManager.OnLevelStarted += OnLevelStarted;
                Debug.Log("First Tutorial");
            }else
            {
                gameObject.SetActive(false);
            }
            

        }
        void OnLevelStarted()
        {
            if(this)
                StartCoroutine(TutorialCoroutine());
            LevelManager.OnLevelStarted -= OnLevelStarted;
            
        }
        
        IEnumerator TutorialCoroutine()
        {
            firstTutorialAnimator.SetTrigger("Show");
            
            
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player)
            {
                spriteMask.transform.DOMove(player.transform.position,2f);
            }

            yield return new WaitForSeconds(2f);
            
            firstTutorialAnimator.SetTrigger("ShowAim");
            yield return new WaitForSeconds(13.5f);
            
            GameObject platform = GameObject.Find("Platform_Default");
            if (platform)
            {
                spriteMask.transform.DOMove(platform.transform.position,2f);
            }
            yield return new WaitForSeconds(2);
            firstTutorialText.text = "You can shoot enemies by bouncing bullets!";
            firstTutorialAnimator.SetTrigger("Bounce");
            yield return new WaitForSeconds(5.5f);
            firstTutorialAnimator.SetTrigger("Hide");
            yield return new WaitForSeconds(1.2f);
            
            GameObject bulletCount = GameObject.Find("BulletCount");
            
            bulletAim.gameObject.SetActive(true);
            bulletAim.DOMove(bulletCount.transform.position + new Vector3(105f,0,0),2f);
            yield return new WaitForSeconds(2.5f);
            
            bulletAimAnimator.SetTrigger("Hide");
            yield return new WaitForSeconds(5.2f);
            bulletAim.gameObject.SetActive(false);

            
            Extensions.SetFirstTutorial();
            gameObject.SetActive(false);
        }
        
        void Update()
        {
        
        }
    }
}
