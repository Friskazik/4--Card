using System;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace Card
{
   
    public class InfoPopup : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _background;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private Button _closeButton;
        [SerializeField] private GlowElement _glowElement;
        [SerializeField] private TextMeshPro _yourBetText;
        [SerializeField] private TextMeshPro _yourWinning;

        [SerializeField] private float _backgroundAlpha;

      
        public event Action Completed;

        
        public Transform CardPoint;

        private void Awake()
        {
            _glowElement.Init();

            _closeButton.Click += CloseButtonOnClick;
        }

      
        public Sequence Show(bool isAnimated)
        {
            EnableCollider(true);

            _yourBetText.text = string.Empty;
            _yourWinning.text = string.Empty;

            if (!isAnimated)
            {
                _background.SetAlpha(_backgroundAlpha);
                _yourWinning.alpha = 1.0f;
                _yourBetText.alpha = 1.0f;

                return default;
            }

            return DOTween.Sequence()
                .Append(_background.DOFade(_backgroundAlpha, 0.3f).SetEase(Ease.Linear))
                .Append(_yourWinning.DOFade(1.0f, 0.3f).SetEase(Ease.Linear))
                .Join(_yourBetText.DOFade(1.0f, 0.3f).SetEase(Ease.Linear));
        }

        
        public void UpdateText(string yourBetText, string yourWinnerText)
        {
            _yourBetText.text = $"Your Bet   {yourBetText}";
            _yourWinning.text = $"Your winner    {yourWinnerText}";
        }

        
        public Sequence ShowGlow() => _glowElement.Show();

      
        public Sequence ShowCloseButton() => _closeButton.Show(true);

   
        public void EnableCollider(bool value) => _collider.enabled = value;

       
        private void CloseButtonOnClick()
        {
            Completed?.Invoke();
        }

      
        public Sequence Hide(bool isAnimated)
        {
            if (!isAnimated)
            {
                _background.SetAlpha(0.0f);
                _yourWinning.alpha = 0.0f;
                _yourBetText.alpha = 0.0f;
                _glowElement.Hide();
                _closeButton.Hide(false);

                EnableCollider(false);

                return default;
            }

            return DOTween.Sequence()
                .Append(_yourWinning.DOFade(0.0f, 0.3f).SetEase(Ease.Linear))
                .Join(_yourBetText.DOFade(0.0f, 0.3f).SetEase(Ease.Linear))
                .Join(_closeButton.Hide(true))
                .AppendCallback(_glowElement.Hide)
                .Append(_background.DOFade(0.0f, 0.3f).SetEase(Ease.Linear))
                .AppendCallback(() => EnableCollider(false));
        }
    }
}