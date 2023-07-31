using System;
using System.Collections;
using System.Collections.Generic;
using S_Durlanik.UI;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace S_Durlanik.Game
{
    public class SceneManager : MonoBehaviour
    {
        

        [Header("Screens")]
        public UI_Screen dailyLoginScreen;
        public DailyFreeCoinUI dailyFreeCoinUI;

        [Header("Frame")] 
        public GameObject topWall;
        public GameObject leftWall;
        public GameObject rightWall;
        public GameObject bottomWall;

        private float _screenHeight;
        private float _screenWidth;
        private const float WallWidth = 1;
        private const float WallHeight = 2;
        
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
            
            _screenHeight = 2f * _camera.orthographicSize;
            _screenWidth = _screenHeight * _camera.aspect;
            
            topWall.transform.parent.gameObject.SetActive(true);
            Application.targetFrameRate = 60;
        }

        public void Start()
        {
           SetPosition();
           SetScale();
           Invoke(nameof(CheckDailyReward), .1f);
           dailyFreeCoinUI.CheckDailyFreeCoin();
        }

        private void SetPosition()
        {
            SetPosition("top", topWall);
            SetPosition("left", leftWall);
            SetPosition("right", rightWall);
            SetPosition("bottom", bottomWall);
        }

        private void SetScale()
        {
            SetScale("horizontal", leftWall);
            SetScale("horizontal", rightWall);
            SetScale("vertical", topWall);
            SetScale("vertical", bottomWall);
        }
        private void SetPosition(string alignment, GameObject objectToAlign)
        {
            var position = Vector2.zero;

            switch (alignment.ToLower())
            {
                case "left":
                    position = new Vector2(-_screenWidth/2 -.5f, 0f);
                    break;
                case "right":
                    position = new Vector2(_screenWidth/2 +.5f, 0f);
                    break;
                case "top":
                    position = new Vector2(0f, _screenHeight/2 +1);
                    break;
                case "bottom":
                    position = new Vector2(0f, -_screenHeight/2 );
                    break;
                default:
                    Debug.LogError("Invalid alignment parameter. Use 'left', 'right', 'top' or 'bottom'.");
                    break;
            }

            objectToAlign.transform.position = position;
        }

        private void SetScale(string aligment, GameObject objectToAlign)
        {
            var scale = Vector3.zero;

            switch (aligment.ToLower())
            {
                case "horizontal":
                    scale = new Vector3(WallWidth, _screenHeight, 1);
                    break;
                case "vertical":
                    scale = new Vector3(_screenWidth, WallHeight, 1);
                    break;
                default:
                    Debug.LogError("Invalid alignment parameter. Use 'horizontal' or 'vertical'.");
                    break;
            }
            
            objectToAlign.transform.localScale = scale;
        }
        
        public void CheckDailyReward()
        {

            if (PlayerPrefs.HasKey(Extensions.Prefs.LastClaimedDailyReward))
            {
                if (PlayerPrefs.GetInt(Extensions.Prefs.TotalClaimedDailyReward) >= 5)
                {
                    PlayerPrefs.SetInt(Extensions.Prefs.TotalClaimedDailyReward, 0);
                }

                string lastClaimedDay = PlayerPrefs.GetString(Extensions.Prefs.LastClaimedDailyReward);

                string currentDay = Extensions.GetCurrentDayByDate();

                if (!string.Equals(lastClaimedDay,currentDay))
                {
                    Extensions.IncreaseCurrentDay();
                    UI_System.Instance.SwitchScreens(dailyLoginScreen);
                }
            }
            else
            {
                PlayerPrefs.SetInt(Extensions.Prefs.TotalClaimedDailyReward, 0);
                PlayerPrefs.SetInt(Extensions.Prefs.CurrentDay, 1);
                UI_System.Instance.SwitchScreens(dailyLoginScreen);
            }
        }
    }
    
    public static class Extensions
    {
        public class Tags
        {
            public const string Wall = "Wall";
            public const string Bullet = "Bullet";
            public const string BoxObstacle = "Box_Obstacle";
            public const string DestroyableBoxObstacle = "Box_Obstacle_Destroyable";
            public const string CircleObstacle = "Circle_Obstacle";
            public const string Platform = "Platform_Rigidbody";
        }
        
        public static class Prefs
        {
            public const string AdsRemoved = "AdsRemoved";
            public const string CurrentDay = "CurrentDay";
            public const string LastClaimedDailyReward = "LastClaimedDay";
            public const string TotalClaimedDailyReward = "TotalClaimedDays";
            public const string Level = "Level";
            public static string CurrentDayRewardTaken => GetCurrentDay() + "_RewardTaken";
            
            // Free Coin
            public const string FreeCoinLastClaimedDay = "FreeCoinLastClaimedDay";
        }
        
        public static void ClearChildren(Transform transform)
        {
            foreach (Transform child in transform)
            {
                Object.Destroy(child.gameObject);
            }
        }
        public static bool IsAdsRemoved()
        {
            return PlayerPrefs.HasKey(Prefs.AdsRemoved);
        }
        
        public static int GetCurrentDay()
        {
            return PlayerPrefs.GetInt(Prefs.CurrentDay,1);
        }

        public static string GetCurrentDayByDate()
        {
            return DateTime.Now.ToShortDateString();
        }
        
        public static void IncreaseCurrentDay()
        {
            Debug.Log("Increasing current day is "+ GetCurrentDay());
            PlayerPrefs.SetInt(Prefs.CurrentDay, GetCurrentDay() + 1);
            PlayerPrefs.Save();
        }
        
        public static int HasCurrentDayRewardTaken()
        {
            return PlayerPrefs.GetInt(Prefs.CurrentDayRewardTaken, 0);
        }
        
        public static void SetCurrentDayRewardTaken()
        {
            PlayerPrefs.SetInt(Prefs.CurrentDayRewardTaken, 1);
            var totalClaimedDays = PlayerPrefs.GetInt(Prefs.TotalClaimedDailyReward);
            PlayerPrefs.SetInt(Prefs.TotalClaimedDailyReward, totalClaimedDays + 1);
        
            Debug.Log("Total claimed days: " + PlayerPrefs.GetInt(Prefs.TotalClaimedDailyReward));
            PlayerPrefs.SetString(Prefs.LastClaimedDailyReward, GetCurrentDayByDate());
            PlayerPrefs.Save();
        }
        
        public static void SetFreeCoinLastClaimedDay()
        {
            PlayerPrefs.SetString(Prefs.FreeCoinLastClaimedDay, GetCurrentDayByDate());
            PlayerPrefs.Save();
        }
        
    }
}
