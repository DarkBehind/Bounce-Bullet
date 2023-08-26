using System;
using System.Collections;
using System.Collections.Generic;
using S_Durlanik.Game;
using S_Durlanik.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace S_Durlanik.UI
{
    public class GameOverUI : PopupScreen
    {
        [SerializeField] private GameObject gameOverPart1;
        [SerializeField] private GameObject gameOverPart2;
        [SerializeField] private int[] bulletPrices = {100, 350};

        [SerializeField] private int bulletAmount = 1;
        [SerializeField] private Button buyBulletButton;
        [SerializeField] private TextMeshProUGUI bulletPriceText;
        int _failedTime = 0;
        bool _clicked = false;
        private void OnEnable()
        {
            LevelManager.OnLevelFailed += OnLevelFailed;
        }

        private void OnDisable()
        {
            LevelManager.OnLevelFailed -= OnLevelFailed;
        }

        private void OnLevelFailed()
        {
            if (_failedTime >= bulletPrices.Length)
            {
                gameOverPart1.SetActive(false);
                gameOverPart2.SetActive(true);
            }
            if(_failedTime < bulletPrices.Length)
                bulletPriceText.text = bulletPrices[_failedTime] + " (+"+bulletAmount+" Bullet)";
            
            if(_failedTime < bulletPrices.Length && InventoryManager.Instance.UserGold.stackAmount <= bulletPrices[_failedTime])
                buyBulletButton.interactable = false;
            else
                buyBulletButton.interactable = true;
            
            StartScreen();
        }

        public void OnBuyBulletButton()
        {
            if(_clicked) return;
            _clicked = true;
            InventoryManager.Instance.CheckGoldAndRemove(bulletPrices[_failedTime], () =>
            {
                AddBulletAndContinueGame();
                Debug.Log("Bullet has been bought");
            }, () =>
            {
                _clicked = false;
                Debug.Log("Not enough gold");
            });
            
        }

        public void OnWatchAdsButton()
        {
            if(_clicked) return;
            _clicked = true;
            ADS.Instance.rewarded.LoadRewardedAd(null, () =>
            {
                AddBulletAndContinueGame();
            });
        }
        void AddBulletAndContinueGame()
        {
            LevelManager.Instance.AddShotCount(bulletAmount);
            LevelManager.Instance.ChangeGameStatus(isGameRunning: true);
            LevelManager.Instance.playTopBar.UpdateBulletCountText();
            _failedTime++;
            CloseScreen(() =>
            {
                _clicked = false;
                gameOverPart1.SetActive(true);
                gameOverPart2.SetActive(false);
            });
        }

        public void OnNoThanksButton()
        {
            gameOverPart1.SetActive(false);
            gameOverPart2.SetActive(true);
        }
        
        public void ResetFailedTime()
        {
            _failedTime = 0;
        }

        public void OnTryAgain()
        {
            if (_clicked) return;
            _clicked = true;
            //ADS.Instance.interstitialAds.ShowAd();
            LevelManager.Instance.RestartLevel();
            CloseScreen(() =>
            {
                _clicked = false;
                gameOverPart1.SetActive(true);
                gameOverPart2.SetActive(false);
                ResetFailedTime();
            });
        }

        public void OnMenu()
        {
            if(_clicked) return;
            _clicked = true;
            LevelManager.Instance.ReturnToMenu();
            CloseScreen(()=>
            {
                _clicked = false;
                gameOverPart1.SetActive(true);
                gameOverPart2.SetActive(false);
                ResetFailedTime();
            });
        }
    }
}
