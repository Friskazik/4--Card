using System;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace Card
{
   
    public class IntLabel : MonoBehaviour
    {
     
        public event Action<int> Changed;

        [SerializeField] private SpriteRenderer _background;
        [SerializeField] private TextMeshPro _progressText;

        private const string CurrencyKey = "CurrencyData";

        private Sequence _enableSequence;

        private int _value;

       
        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                _progressText.text = $"{value}";

                PlayerPrefs.SetInt(CurrencyKey, _value); 

                OnValueChanged(value);
            }
        }

        /// <summary>
        /// Загрузка данныъ
        /// </summary>
        public void Init(int startCurrency)
        {
            //TODO если захочешь сохранку - удали нижнюю строку
            PlayerPrefs.DeleteKey(CurrencyKey);
            Value = PlayerPrefs.HasKey(CurrencyKey) ? PlayerPrefs.GetInt(CurrencyKey) : startCurrency;
        }

        /// <summary>
        /// Валюта была изменена
        /// </summary>
        private void OnValueChanged(int value)
        {
            Changed?.Invoke(value);
        }

        /// <summary>
        /// Включить/Отключить валюту
        /// </summary>
        public Sequence Enable(bool value, bool animated, float time = 0.35f)
        {
            _enableSequence?.Kill();

            var alpha = value ? 1.0f : 0.0f;

            if (!animated)
            {
                _background.SetAlpha(alpha);
                _progressText.alpha = alpha;

                return default;
            }

            _enableSequence = DOTween.Sequence()
                .Join(_background.DOFade(alpha, time).SetEase(Ease.InQuad))
                .Join(_progressText.DOFade(alpha, time).SetEase(Ease.InQuad));

            return _enableSequence;
        }

        /// <summary>
        /// Уничтожить элемент
        /// </summary>
        public void Dispose()
        {
            Changed = null;
        }
    }
}