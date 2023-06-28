using System;
using System.Collections;
using System.Collections.Generic;
using S_Durlanik.Game;
using S_Durlanik.UI;
using UnityEngine;

namespace S_Durlanik.UI
{
    public class GameOverUI : PopupScreen
    {
        [SerializeField] private GameObject gameOverPart1;
        [SerializeField] private GameObject gameOverPart2;
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
            StartScreen();
        }

        public void OnBuyBulletButton()
        {
            Debug.Log("Bullet has been bought");
            LevelManager.Instance.AddShotCount(1);
            LevelManager.Instance.ChangeGameStatus(isGameRunning: true);
            LevelManager.Instance.playTopBar.UpdateBulletCountText();
            CloseScreen();
        }

        public void OnWatchAdsButton()
        {
            Debug.Log("Ads have been watched");
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
