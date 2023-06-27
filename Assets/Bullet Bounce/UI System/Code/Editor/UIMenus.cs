using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace S_Durlanik.UI
{
    public class UIMenus : MonoBehaviour 
    {
        [MenuItem("S-Durlanik/UI Tools/Create UI Group")]
        public static void CreateUIGroup()
        {
            var uiGroup = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Bullet Bounce/Prefab/UI/UI_GRP.prefab");
            if(uiGroup)
            {
                var createdGroup = (GameObject)Instantiate(uiGroup);
                createdGroup.name = "UI_GRP";
            }
            else
            {
                EditorUtility.DisplayDialog("UI Tools Warning", "Cannot find UI Group Prefab!", "OK");
            }
        }
    }
}
