using DG.Tweening;
using UnityEngine;

namespace Card
{
   
    public class GlowElement : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _back;
        [SerializeField] private SpriteRenderer _forward;
        [SerializeField] private ParticleSystem _fx;

        private Sequence _sequence;

        public void Init()
        {
            var endRotation = new Vector3(0, 0, -360);
            var rotationTime = 10;
            var scaleTime = 1;

            _sequence = DOTween.Sequence();
            _sequence
                .Append(_back.transform.DOLocalRotate(endRotation, rotationTime, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear))
                .Join(_forward.transform.DOLocalRotate(endRotation * -1, rotationTime, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear))
                .Join(_back.transform.DOScale(Vector3.one * 1.3f, scaleTime).SetLoops(10, LoopType.Yoyo)
                    .SetEase(Ease.Linear))
                .Join(_forward.transform.DOScale(Vector3.one * 1.3f, scaleTime).SetLoops(10, LoopType.Yoyo)
                    .SetEase(Ease.Linear))
                .SetLoops(-1)
                .Pause();

            _fx.Pause();
        }
        public void Play()
        {
            _sequence?.Play();
            _fx.Play();
        }

        public void Pause()
        {
            _sequence?.Pause();
            _fx.Pause();
        }

        public Sequence Show(bool isAnimated = true, float duration = 0.2f)
        {
            var sequence = DOTween.Sequence();

            if (isAnimated)
            {
                transform.localScale = Vector3.zero;
                sequence.Join(transform.DOScale(1, duration * 0.85f).SetEase(Ease.OutBack));
            }
            else
                transform.localScale = Vector3.one;

            sequence.AppendCallback(Play);

            return sequence;
        }

        public void Hide()
        {
            Pause();

            transform.localScale = Vector3.zero;

            _back.transform.localRotation = Quaternion.identity;
            _forward.transform.localRotation = Quaternion.identity;

            _back.transform.localScale = Vector3.one;
            _forward.transform.localScale = Vector3.one;
        }
    }
}