using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

namespace Card
{
   
    public class CardsController : MonoBehaviour
    {
        
        [SerializeField] private List<Transform> _points;
        
        
        [SerializeField] private CardManager _prefab;

        
        public event Action<CardManager> CardSelected; 

       
        private readonly List<CardManager> _cards = new(); 
        
       
        private readonly Vector3 _openRotate = new(0, 180, 0); 

       
        private Camera _camera; 

       
        private bool _isActive; 

       
        private void Awake()
        {
            _camera = Camera.main;
        }

       
        public void Init(List<CardsConfig.Model> data) 
        {
            for (var i = 0; i < _points.Count; i++)
            {
                var card = GetCardManager(_points[i]);
                card.Init(data[i]);

                _cards.Add(card);
            }
        }

       
        private CardManager GetCardManager(Transform parent)
        {
            var card = Instantiate(_prefab); 
            var tr = card.transform;
            tr.parent = parent;
            tr.localPosition = Vector3.zero;
            tr.eulerAngles = Vector3.zero;
            tr.localScale = Vector3.one;

            return card;
        }

        
        public async UniTask Mix()
        {
            var startMix = true;

            DOTween.Sequence()
                .Append(ShowCards())
                .Append(StartMix())
                .AppendCallback(() => startMix = false);

            await UniTask.WaitWhile(() => startMix);

            _isActive = true;
        }

        
        private Sequence ShowCards()
        {
            
            var sequence = DOTween.Sequence();
            foreach (var card in _cards)
                sequence.Append(card.Rotate(_openRotate, 0.25f, Ease.Linear));

           
            sequence.AppendInterval(2f);

           
            for (var i = _cards.Count - 1; i >= 0; i--)
                sequence.Append(_cards[i].Rotate(Vector3.zero, 0.25f, Ease.Linear));

            return sequence;
        }

       
        private Sequence StartMix()
        {
            var sequence = DOTween.Sequence();

           
            var iterations = Random.Range(1, 6);
            for (var i = 0; i < iterations; i++)
                sequence.Append(Move());

            return sequence;
        }

        
        private Sequence Move()
        {
            var sequence = DOTween.Sequence();
            var randomIndex = Random.Range(1, _cards.Count);

            var points = GetPos(_points[0].position, _points[randomIndex].position, 1);
            sequence.Join(_cards[0].Move(points, 0.5f, Ease.Linear)); 

            points = GetPos(_points[randomIndex].position, _points[0].position, -1);
            sequence.Join(_cards[randomIndex].Move(points, 0.5f, Ease.Linear));

            var startIndex = randomIndex == 1 ? 2 : 1;
            var endIndex = randomIndex == 3 ? 2 : 3;

            points = GetPos(_points[startIndex].position, _points[endIndex].position, 1.5f);
            sequence.Join(_cards[startIndex].Move(points, 0.5f, Ease.Linear));

            points = GetPos(_points[endIndex].position, _points[startIndex].position, -1.5f);
            sequence.Join(_cards[endIndex].Move(points, 0.5f, Ease.Linear));

            return sequence;
        }

       
        private Vector3[] GetPos(Vector3 startPos, Vector3 endPos, float offset)
        {
            var middlePoint = new Vector3
            {
                x = (startPos.x + endPos.x) / 2,
                y = startPos.y + offset,
                z = startPos.z
            };

            return new[] {startPos, middlePoint, endPos};
        }

        
        private void Update()
        {
           
            if (!_isActive)
                return;

            
            if (!Input.GetMouseButtonDown(0))
                return;

           
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

           
            if (!Physics.Raycast(ray, out var hit))
                return;

           
            var card = hit.collider.GetComponent<CardManager>();
            if (card == null)
                return;

            CardSelected?.Invoke(card);
            _isActive = false;
        }

        
        public void Complete()
        {
            foreach (var card in _cards)
            {
                card.SetLayer(LayerManager.Default);
                
                var tr = card.transform;
                tr.localRotation = Quaternion.identity;
                tr.localScale = Vector3.one;
                tr.localPosition = Vector3.zero;
            }
        }

        
        public void Dispose()
        {
            CardSelected = null;

            foreach (var card in _cards)
                DestroyImmediate(card.gameObject);

            _cards.Clear();
        }
    }
}