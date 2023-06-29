using System;
using System.Collections;
using System.Collections.Generic;
using S_Durlanik.UI;
using UnityEngine;

namespace S_Durlanik.Game
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }
        public static event Action OnLevelStarted;
        public static event Action OnLevelCompleted;
        public static event Action OnLevelFailed;

        public PlayTopBar playTopBar;

        public Transform levelsParent;
        [HideInInspector] public int maxShotCount;
        [HideInInspector] public int enemyCount;

        private bool _isGameRunning;
        Level _currentLevelObject;
        private void OnEnable()
        {
            Enemy.OnEnemyDestroyed += OnEnemyDestroyed;
            GunController.OnBulletFired += OnBulletFired;
        }

        private void OnDisable()
        {
            Enemy.OnEnemyDestroyed -= OnEnemyDestroyed;
            GunController.OnBulletFired -= OnBulletFired;
        }



        private void OnEnemyDestroyed(Enemy enemy)
        {
            enemyCount--;
            if (enemyCount <= 0)
            {
                OnLevelCompleted?.Invoke();
                Debug.Log("Level Completed!");
            }
        }

        private void OnBulletFired()
        {
            if (maxShotCount <= 0)
            {
                return;
            }

            maxShotCount--;
            playTopBar.UpdateBulletCountText();
        }

        private void Awake()
        {
            Instance = this;
        }


        private void CheckLevelFailed()
        {
            if (!_isGameRunning)
            {
                return;
            }

            if (maxShotCount <= 0 && enemyCount > 0)
            {
                if (FindObjectOfType<Bullet>())
                {
                    return;
                }

                OnLevelFailed?.Invoke();
                Debug.Log("Level Failed!");
                _isGameRunning = false;
            }
        }


        public static int GetCurrentLevel()
        {
            return PlayerPrefs.GetInt(Extensions.Prefs.Level, 1);
        }
        public void SetCurrentLevel(int levelNumber)
        {
            PlayerPrefs.SetInt(Extensions.Prefs.Level, levelNumber);
        }

        public void LoadLevel(int levelNumber)
        {
            if(_currentLevelObject)
                Destroy(_currentLevelObject.gameObject);
            Level levelToLoad = Instantiate(Resources.Load<Level>("Levels/Level" + levelNumber), levelsParent);

            if (levelToLoad)
            {
                OnLevelStarted?.Invoke();
                _isGameRunning = true;
                _currentLevelObject = levelToLoad;
                InvokeRepeating(nameof(CheckLevelFailed), 1, 1f);
            }
            else
            {
                Debug.LogError("Level not found!");
            }

            SetLevelProperties(levelToLoad);
        }

        public void RestartLevel()
        {
            UI_System.Instance.GoToPlayScreen();
            LoadLevel(GetCurrentLevel());
        }

        public void ReturnToMenu()
        {
            UI_System.Instance.GoToMainScreen();
        }
        
        public void LoadNextLevel()
        {
            SetCurrentLevel(GetCurrentLevel() + 1);
            LoadLevel(GetCurrentLevel());
        }
        private void SetLevelProperties(Level level)
        {
            maxShotCount = level.maxShotCount;
            enemyCount = level.enemyCount;
            
            playTopBar.UpdateBulletCountText();
        }

        public void AddShotCount(int amountToAdd)
        {
            maxShotCount += amountToAdd;
        }

        public void ChangeGameStatus(bool isGameRunning)
        {
            _isGameRunning = isGameRunning;
        }
    }
}
