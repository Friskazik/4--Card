using UnityEngine;
using DG.Tweening;

namespace Card
{
    
    public class SuitSelectionPanel : BaseSelectionPanel<SuitSelectionData>
    {
        
        [SerializeField] private SpriteRenderer _selectionImage;

       
        public override int CurrentIndex
        {
            get => Index;
            set
            {
                Index = value;

                var data = GetData(Index);
                _selectionImage.sprite = data.Sprite;

                Name = data.Name;
            }
        }

        
        public override Sequence Show(bool isAnimated = false)
        {
            return DOTween.Sequence()
                .Join(base.Show(isAnimated))
                .Join(EnableSelectionImage(true, isAnimated));
        }

        
        private Tween EnableSelectionImage(bool value, bool isAnimated)
        {
            var alpha = value ? 1.0f : 0.0f;

            if (!isAnimated)
            {
                _selectionImage.SetAlpha(alpha);
                return default;
            }

            return _selectionImage.DOFade(alpha, 0.3f).SetEase(Ease.Linear);
        }

        
        public override Sequence Hide(bool isAnimated = false)
        {
            return DOTween.Sequence()
                .Join(base.Hide(isAnimated))
                .Join(EnableSelectionImage(false, isAnimated));
        }
    }
}