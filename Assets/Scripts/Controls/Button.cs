using UnityEngine;
using DG.Tweening;
using TMPro;

namespace Card
{
    public class Button : BaseButton
    {

        [SerializeField] protected Transform _container; 
        [SerializeField] protected SpriteRenderer Renderer; 
        [SerializeField] protected TextMeshPro Text; 

        [SerializeField] private Vector3 _interactionScale = new(-0.1f, -0.1f, 0); 
        private Vector3 _startScale;
        private Vector3 _startPosition;

        private Sequence _showHideSequence;

        private Tween _showHideTween;

        private bool _enable = true; 
        public bool Locked { set; get; }

        private void Awake() 
        {
            _startScale = _container.localScale;
            _startPosition = _container.localPosition;
        }

        
        public Sequence Show(bool animated, float delay = 0f) 
        {
            _showHideSequence?.Kill();
            _showHideSequence = DOTween.Sequence();

            if (!animated)
            {
                _enable = true;

                _container.localScale = Vector3.one;

                return _showHideSequence;
            }

            _showHideSequence
                .SetDelay(delay)
                .Append(_container.DOScale(Vector3.one, 0.35f))
                .AppendCallback(() => { _enable = true; });

            return _showHideSequence;
        }

        
        private void OnMouseDown()
        {
            if (!_enable || Locked)
                return;

            _container.localScale = _startScale + _interactionScale;
        }

        
        protected override void OnMouseUpAsButton()
        {
            if (!_enable || Locked) 

                return;

            base.OnMouseUpAsButton();
        }

           
            private void OnMouseUp()
            {
                if (!_enable || Locked)
                    return;

                _container.localScale = _startScale; 
                _container.localPosition = _startPosition;
            }

           
            public void SetTextValue(string value) 
            {
                Text.text = value;
            }

           
            public Sequence LockButton(bool value, bool isAnimated)
            {
                Locked = value;

                var alpha = value ? 0.3f : 1.0f;

                if (!isAnimated)
                {
                    if (Renderer != null)
                        Renderer.SetAlpha(alpha);

                    if (Text != null)
                        Text.alpha = alpha;

                    return default;
                }

                var sequence = DOTween.Sequence(); 

                if (Renderer != null)
                    sequence.Join(Renderer.DOFade(alpha, 0.3f).SetEase(Ease.Linear));

                if (Text != null)
                    sequence.Join(Text.DOFade(alpha, 0.3f).SetEase(Ease.Linear));

                return sequence;
            }

            
            public Sequence Hide(bool animated)
            {
                _enable = false;

                _showHideSequence?.Kill();
                _showHideSequence = DOTween.Sequence();

                if (!animated)
                {
                    _container.localScale = Vector3.zero;
                    return _showHideSequence;
                }

                _showHideSequence.Append(_container.DOScale(Vector3.zero, 0.35f));

                return _showHideSequence;
            }
        }
    }
