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
            if (PlayerPrefs.HasKey(Extensions.Prefs.TotalClaimedReward))
            {
                if(Extensions.GET_SET_CurrentDay(false) >= 5 && !string.Equals(Extensions.GetCurrentDayByDate(),Extensions.GET_SET_CurrentRewardDayByDateTime(false)))
                    Extensions.SetDefaultDailyRewardConfigs();


                
                if (!string.Equals(Extensions.GetCurrentDayByDate(), 
                        Extensions.GET_SET_CurrentRewardDayByDateTime(false)))
                {
                    Extensions.GET_SET_CurrentDay(true, Extensions.GET_SET_CurrentDay(false) + 1);
                    Extensions.GET_SET_CurrentRewardTaken(true, 0);
                    Extensions.GET_SET_CurrentRewardDayByDateTime(true, Extensions.GetCurrentDayByDate());
                }
                
                if(Extensions.GET_SET_CurrentRewardTaken(false) == 0)
                    UI_System.Instance.SwitchScreens(dailyLoginScreen);
                
            }else
                UI_System.Instance.SwitchScreens(dailyLoginScreen);
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
            public const string Level = "Level";
            // Daily Reward
            public const string TotalClaimedReward = "TotalClaimedReward";
            public const string CurrentDay = "CurrentDay";
            public const string CurrentDayByDate = "CurrentDayByDate";
            public const string LastClaimedDayByDate = "LastClaimedDayByDate";
            public const string CurrentRewardTaken = "CurrentRewardTaken";
            
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

        public static void SetDefaultDailyRewardConfigs()
        {
            GET_SET_TotalClaimedReward(true);
            GET_SET_CurrentDay(true);
            GET_SET_LastClaimedDayByDate(true);
            GET_SET_CurrentRewardDayByDateTime(true, GetCurrentDayByDate());
            GET_SET_CurrentRewardTaken(true);
            PlayerPrefs.Save();
        }
        public static int GET_SET_TotalClaimedReward(bool set, int value = 0)
        {
            Debug.Log("TotalClaimedReward : " + PlayerPrefs.GetInt(Prefs.TotalClaimedReward,0) + "");
            if (set)
            {
                PlayerPrefs.SetInt(Prefs.TotalClaimedReward, value);
                return -1;
            }
            return PlayerPrefs.GetInt(Prefs.TotalClaimedReward,0);
        }

        public static int GET_SET_CurrentDay(bool set, int value = 1)
        {
            Debug.Log("CurrentDay : " + PlayerPrefs.GetInt(Prefs.CurrentDay,1) + "");
            if (set)
            {
                PlayerPrefs.SetInt(Prefs.CurrentDay, value);
                return -1;
            }
            return PlayerPrefs.GetInt(Prefs.CurrentDay,1);
        }
        public static string GET_SET_CurrentRewardDayByDateTime(bool set, string value = "")
        {
            Debug.Log("CurrentDayByDate : " + PlayerPrefs.GetString(Prefs.CurrentDayByDate,"") + "");
            if (set)
            {
                PlayerPrefs.SetString(Prefs.CurrentDayByDate, value);
                return "";
            }
            
            return PlayerPrefs.GetString(Prefs.CurrentDayByDate,"");
        }

        public static string GET_SET_LastClaimedDayByDate(bool set, string value = "")
        {
            Debug.Log("LastClaimedDayByDate : " + PlayerPrefs.GetString(Prefs.LastClaimedDayByDate,"") + "");
            if (set)
            {
                PlayerPrefs.SetString(Prefs.LastClaimedDayByDate, value);
                return "";
            }
            
            return PlayerPrefs.GetString(Prefs.LastClaimedDayByDate,"");
        }
        public static bool IsGetRewardAvailableToday()
        {
            Debug.Log("IsGetRewardAvailableToday : " + !string.Equals(GET_SET_CurrentRewardDayByDateTime(false), GET_SET_LastClaimedDayByDate(false)) + "");
            return !string.Equals(GET_SET_CurrentRewardDayByDateTime(false), GET_SET_LastClaimedDayByDate(false));
        }
        public static int GET_SET_CurrentRewardTaken(bool set, int value = 0)
        {
            Debug.Log("CurrentRewardTaken : " + PlayerPrefs.GetInt(Prefs.CurrentRewardTaken,0) + "");
            if (set)
            {
                PlayerPrefs.SetInt(Prefs.CurrentRewardTaken, value);
                return -1;
            }
            return PlayerPrefs.GetInt(Prefs.CurrentRewardTaken,0);
        }

        public static void SetCurrentDayRewardTaken()
        {
            GET_SET_TotalClaimedReward(true, GET_SET_TotalClaimedReward(false) + 1);
            GET_SET_LastClaimedDayByDate(true, GetCurrentDayByDate());
            GET_SET_CurrentRewardDayByDateTime(true, GetCurrentDayByDate());
            GET_SET_CurrentRewardTaken(true, 1);
            PlayerPrefs.Save();
        }

        public static string GetCurrentDayByDate()
        {
            //Debug.Log("GetCurrentDayByDate : " + DateTime.Now.ToShortDateString() + "");
            //return DateTime.Now.AddDays(0).ToShortDateString();
            return DateTime.Now.ToShortDateString();
        }

        public static void SetFreeCoinLastClaimedDay()
        {
            PlayerPrefs.SetString(Prefs.FreeCoinLastClaimedDay, GetCurrentDayByDate());
            PlayerPrefs.Save();
        }
        
    }
}
