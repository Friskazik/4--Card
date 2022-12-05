using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Card
{
   
    public abstract class BaseSelectionPanel<T> : MonoBehaviour where T : BaseSelectionData 
    {
        [SerializeField] private SpriteRenderer _background;

        [SerializeField] private Button _leftArrow;
        [SerializeField] private Button _rightArrow;

        
        public event Action<string> Changed;

        
        protected int Index;

        
        private List<T> _data;

        
        public string Name { protected set; get; }

       
        public abstract int CurrentIndex { get; set; }

        
        private void Awake()
        {
            _leftArrow.Click += LeftArrowOnClick;
            _rightArrow.Click += RightArrowOnClick;
        }

        
        public void Init(List<T> data)
        {
            _data = data;
            CurrentIndex = 0;
        }

        public virtual Sequence Show(bool isAnimated = false)
        {
            if (isAnimated)
            {
                return DOTween.Sequence()
                    .Append(_background.DOFade(1.0f, 0.3f).SetEase(Ease.Linear))
                    .Join(_leftArrow.Show(true))
                    .Join(_rightArrow.Show(true));
            }

            _background.SetAlpha(1.0f);
            _leftArrow.Show(false);
            _rightArrow.Show(false);

            return default;
        }

        
        private void RightArrowOnClick() => CurrentIndex = _data.Count <= CurrentIndex + 1 ? 0 : ++Index;
        
        private void LeftArrowOnClick() => CurrentIndex = CurrentIndex - 1 < 0 ? _data.Count - 1 : --Index;

       
        protected virtual void IndexOnChanged(string value) => Changed?.Invoke(value);

       
        protected T GetData(int index) => index > _data.Count || index < 0 ? null : _data[index];

        
        public Sequence LockButton(bool value, bool isAnimated)
        {
            return DOTween.Sequence()
                .Join(_leftArrow.LockButton(value, isAnimated))
                .Join(_rightArrow.LockButton(value, isAnimated));
        }

        
        public virtual Sequence Hide(bool isAnimated = false)
        {
            if (isAnimated)
            {
                return DOTween.Sequence()
                    .Join(_leftArrow.Hide(true))
                    .Join(_rightArrow.Hide(true))
                    .Append(_background.DOFade(0.0f, 0.3f).SetEase(Ease.Linear));
            }

            _background.SetAlpha(0.0f);
            _leftArrow.Hide(false);
            _rightArrow.Hide(false);

            return default;
        }

       
        public virtual void Dispose()
        {
            _leftArrow.Click -= LeftArrowOnClick;
            _rightArrow.Click -= RightArrowOnClick;

            Changed = null;
        }
    }
}