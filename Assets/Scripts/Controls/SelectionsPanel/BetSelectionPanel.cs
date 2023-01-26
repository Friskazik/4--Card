using UnityEngine;
using DG.Tweening;
using TMPro;

namespace Card
{
   
    public class BetSelectionPanel : BaseSelectionPanel<BaseSelectionData>
    {
        
        [SerializeField] private TextMeshPro _selectionText;

       
        public override int CurrentIndex
        {
            get => Index;
            set
            {
                Index = value;

                var data = GetData(Index);
                _selectionText.text = data.Name;

                Name = data.Name;
                IndexOnChanged(Name);
            }
        }

        
        public override Sequence Show(bool isAnimated = false)
        {
            return DOTween.Sequence()
                .Join(base.Show(isAnimated))
                .Join(EnableSelectionText(true, isAnimated));
        }

       
        private Tween EnableSelectionText(bool value, bool isAnimated)
        {
            var alpha = value ? 1.0f : 0.0f;

            if (!isAnimated)
            {
                _selectionText.alpha = alpha;
                return default;
            }

            return _selectionText.DOFade(alpha, 0.3f).SetEase(Ease.Linear);
        }

        
        public override Sequence Hide(bool isAnimated = false)
        {
            return DOTween.Sequence()
                .Join(base.Hide(isAnimated))
                .Join(EnableSelectionText(false, isAnimated));
        }
    }
}