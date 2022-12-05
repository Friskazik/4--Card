using System;
using UnityEngine;

namespace Card
{
    /// <summary>
    /// ДАнные выбора цвета
    /// </summary>
    [Serializable]
    public class ColorSelectionData : BaseSelectionData
    {
        /// <summary>
        /// Цвет
        /// </summary>
        public Color Color;
        
        /// <summary>
        /// Коэффицицент при угадывании
        /// </summary>
        public float Factor;
    }
}