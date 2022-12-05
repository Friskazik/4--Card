using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Card
{
   
    public class BetConfig : ScriptableObject 
    {
       
        public Model BetData; 

       
        private const string FolderPath = "Assets/Resources/ScriptableObjects";

        private static BetConfig _instance;

        
        public static BetConfig Instance 
        {
            get
            {
                if (_instance != null)
                    return _instance;

               
                _instance = Resources.Load<BetConfig>(
                    "ScriptableObjects/BetConfig");

                if (_instance == null)
                {
                   
                    _instance = CreateInstance<BetConfig>();
                    Debug.LogError("BetConfig not found.");
                }
                else
                    return _instance; 

#if UNITY_EDITOR 
                if (!Directory.Exists(FolderPath))
                    Directory.CreateDirectory(FolderPath);

                UnityEditor.AssetDatabase.CreateAsset(_instance, $"{FolderPath}/BetConfig.asset");
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.AssetDatabase.Refresh();

#endif
                return _instance;
            }
        }


        [Serializable]
        public class Model
        {
            
            public List<BaseSelectionData> BetTypes;

           
            public int StartCurrency;

           
            public int Step;
        }

        
#if UNITY_EDITOR
        [UnityEditor.MenuItem("Game/BetConfig")] 
        private static void Show()
        {
            UnityEditor.Selection.activeObject = Instance;
        }
#endif
    }
}