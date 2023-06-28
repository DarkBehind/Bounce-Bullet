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
            InventoryManager.Instance.CheckGoldAndRemove(bulletPrice, () =>
            {
                AddBulletAndContinueGame();
                Debug.Log("Bullet has been bought");
            });
            
        }

        public void OnWatchAdsButton()
        {
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
            CloseScreen();
        }

        public void OnNoThanksButton()
        {
            gameOverPart1.SetActive(false);
            gameOverPart2.SetActive(true);
        }

        public void OnTryAgain()
        {
            LevelManager.Instance.RestartLevel();
        }

        public void OnMenu()
        {
            LevelManager.Instance.ReturnToMenu();
        }
    }
}
