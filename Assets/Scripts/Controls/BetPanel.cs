using System;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace Card
{
  
    public class BetPanel : MonoBehaviour 
    {
        [SerializeField] private SpriteRenderer _background; 
        [SerializeField] private TextMeshPro _valueText;

        [SerializeField] private Button _leftArrow;
        [SerializeField] private Button _rightArrow; 

        
        public event Action<int> Changed;

        
        private int _value;

      
        private int _step;

     
        public int Value 
        {
            get => _value;
            set
            {
                _value = value;  
                _valueText.text = $"{_value}";

                Changed?.Invoke(_value);
            }
        }

       
        private void Awake()
        {
            _leftArrow.Click += LeftArrowOnClick;
            _rightArrow.Click += RightArrowOnClick;
        }

       
        public void Init(int step)
        {
            _step = step;
            Value = _step;

            _leftArrow.SetTextValue($"-{_step}"); 
            _rightArrow.SetTextValue($"+{_step}");
        }

        
        public Sequence Show(bool isAnimated = false)
        {
            if (isAnimated)
            {
                return DOTween.Sequence()
                    .Append(_background.DOFade(1.0f, 0.3f).SetEase(Ease.Linear))
                    .Join(_valueText.DOFade(1.0f, 0.3f).SetEase(Ease.Linear))
                    .Join(_leftArrow.Show(true))
                    .Join(_rightArrow.Show(true));
            }

            _background.SetAlpha(1.0f);
            _valueText.alpha = 1.0f;
            _leftArrow.Show(false);
            _rightArrow.Show(false);

            return default;
        }

        
        public void UpdateButtons(int currency)
        {
            _leftArrow.LockButton(Value - _step <= 0, false);
            _rightArrow.LockButton(Value + _step > currency, false);
        }

        /// <summary>
        /// Нажата правая стрелка, увеличена ставка
        /// </summary>
        private void RightArrowOnClick() => Value += _step;// нажади увеличили значание 

        /// <summary>
        /// Нажата левая стрелка, уменьшена ставкиа
        /// </summary>
        private void LeftArrowOnClick() => Value -= _step;// нажали уменьшили знач

        /// <summary>
        /// Заблокировать кнопки
        /// </summary>
        public Sequence LockButton(bool value, bool isAnimated)// здесь блакиру.тмя сразу все кнопки, когда переходим в геймплэй
        {
            return DOTween.Sequence()
                .Join(_leftArrow.LockButton(value, isAnimated))
                .Join(_rightArrow.LockButton(value, isAnimated));
        }

        /// <summary>
        /// Скрыть эелемент
        /// </summary>
        /// <param name="isAnimated">Анимацией ли</param>
        /// <returns>Анимация сокрытия</returns>
        public Sequence Hide(bool isAnimated = false)
        {
            if (isAnimated)
            {
                return DOTween.Sequence()
                    .Join(_valueText.DOFade(0.0f, 0.3f).SetEase(Ease.Linear))
                    .Join(_leftArrow.Hide(true))
                    .Join(_rightArrow.Hide(true))
                    .Append(_background.DOFade(0.0f, 0.3f).SetEase(Ease.Linear));
            }

            _background.SetAlpha(0.0f);
            _valueText.alpha = 0.0f;
            _leftArrow.Hide(false);
            _rightArrow.Hide(false);

            return default;
        }

        public void Dispose()// хжесь уничтожение полностью
        {
            _leftArrow.Click -= LeftArrowOnClick;
            _rightArrow.Click -= RightArrowOnClick;

            Changed = null;
        }
    }
}