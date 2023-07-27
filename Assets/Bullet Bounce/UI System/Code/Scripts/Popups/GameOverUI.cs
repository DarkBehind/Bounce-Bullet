using System;
using System.Collections;
using System.Collections.Generic;
using S_Durlanik.Game;
using S_Durlanik.UI;
using TMPro;
using UnityEngine;

namespace S_Durlanik.UI
{
    public class GameOverUI : PopupScreen
    {
        [SerializeField] private GameObject gameOverPart1;
        [SerializeField] private GameObject gameOverPart2;
        [SerializeField] private int bulletPrice = 100;
        [SerializeField] private int bulletAmount = 1;
        [SerializeField] private TextMeshProUGUI bulletPriceText;
        bool _clicked = false;
        private void OnEnable()
        {
            LevelManager.OnLevelFailed += OnLevelFailed;
            bulletPriceText.text = bulletPrice + " (+"+bulletAmount+" Bullet)";
        }

        private void OnDisable()
        {
            LevelManager.OnLevelFailed -= OnLevelFailed;
        }

        private void OnLevelFailed()
        {
            StartScreen();
        }

        public void OnBuyBulletButton()
        {
            if(_clicked) return;
            _clicked = true;
            InventoryManager.Instance.CheckGoldAndRemove(bulletPrice, () =>
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

        public void OnTryAgain()
        {
            if (_clicked) return;
            _clicked = true;
            LevelManager.Instance.RestartLevel();
            CloseScreen(() =>
            {
                _clicked = false;
                gameOverPart1.SetActive(true);
                gameOverPart2.SetActive(false);
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
            });
        }
    }
}
