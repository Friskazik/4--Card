using UnityEngine;
using DG.Tweening;

namespace Card
{
   
    public class CardManager : MonoBehaviour
    {
        
        [SerializeField] private GameObject _back;

       
        [SerializeField] private GameObject _model;

        
        [SerializeField] private SpriteRenderer _icon;

       
        public CardsConfig.Model Data { private set; get; } 

        
        public void Init(CardsConfig.Model model)
        {
            _icon.sprite = model.Sprite;

            Data = model;
        }

        
        public Tween Rotate(Vector3 value, float duration, Ease ease) =>
            transform.DOLocalRotate(value, duration).SetEase(ease);

       
        public Tween Move(Vector3[] points, float duration, Ease ease) =>
            transform.DOPath(points, duration).SetEase(ease); 

        
        public Sequence Move(Transform point, float duration, Ease ease) 
        {
            return DOTween.Sequence()
                .Join(transform.DOMove(point.position, duration).SetEase(ease))
                .Join(transform.DOScale(point.localScale, duration).SetEase(ease));
        }

       
        public void SetLayer(int layer) 
        {
            _back.layer = layer;
            _model.layer = layer;
            _icon.gameObject.layer = layer;
            gameObject.layer = layer;
        }
    }
}