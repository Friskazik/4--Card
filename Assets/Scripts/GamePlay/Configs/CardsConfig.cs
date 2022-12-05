using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Card
{
    
    public class CardsConfig : ScriptableObject 
    {
        
        public List<Model> Cards;

        
        private const string FolderPath = "Assets/Resources/ScriptableObjects";

        private static CardsConfig _instance;

       
        public static CardsConfig Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                
                _instance = Resources.Load<CardsConfig>(  
                    "ScriptableObjects/CardsConfig");

                if (_instance == null)
                {
                    
                    _instance = CreateInstance<CardsConfig>();
                    Debug.LogError("CardsConfig not found.");
                }
                else
                    return _instance;

#if UNITY_EDITOR
                if (!Directory.Exists(FolderPath))
                    Directory.CreateDirectory(FolderPath);

                UnityEditor.AssetDatabase.CreateAsset(_instance, $"{FolderPath}/CardsConfig.asset");
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.AssetDatabase.Refresh();

#endif
                return _instance;
            }
        }

     
        public List<ColorSelectionData> GetColorSelectionData()
        {
            var dict = new Dictionary<string, ColorSelectionData>();

            foreach (var card in Cards)
            {
                var data = card.ColorSelectionData;
                if (dict.ContainsKey(data.Name))
                    continue;

                dict.Add(data.Name, data);
            }

            return dict.Values.ToList();
        }

        
        public List<SuitSelectionData> GetSuitSelectionData()
        {
            var dict = new Dictionary<string, SuitSelectionData>();  

            foreach (var card in Cards)
            {
                var data = card.SuitSelectionData; 
                if (dict.ContainsKey(data.Name))
                    continue;

                dict.Add(data.Name, data);
            }

            return dict.Values.ToList(); 
        }


        [Serializable]
        public class Model 
        {
            
            public Sprite Sprite;

            
            public ColorSelectionData ColorSelectionData;

           
            public SuitSelectionData SuitSelectionData;
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Game/CardsConfig")]
        private static void Show()
        {
            UnityEditor.Selection.activeObject = Instance;
        }
#endif
    }
}