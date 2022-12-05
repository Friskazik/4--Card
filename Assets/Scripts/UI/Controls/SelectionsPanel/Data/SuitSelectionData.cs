using System;
using UnityEngine;

namespace Card
{
    /// <summary>
    /// Данные выбора масти
    /// </summary>
    [Serializable]
    public class SuitSelectionData : BaseSelectionData
    {
        /// <summary>
        /// Спрайт масти
        /// </summary>
        public Sprite Sprite;
        
        /// <summary>
        /// Коэффицицент при угадывании
        /// </summary>
        public float Factor;
    }
}