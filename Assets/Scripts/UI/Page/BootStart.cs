using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace Card
{
    
    public class BootStart : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _headerText;
        [SerializeField] private Button _playButton;
        [SerializeField] private IntLabel _currency;
        [SerializeField] private BetSelectionPanel _betSelectionPanel;
        [SerializeField] private BetPanel _betPanel;

        [Header("Types bet")] [SerializeField] private ColorSelectionPanel _colorSelectionPanel;
        [SerializeField] private SuitSelectionPanel _suitSelectionPanel;

        private CardsController _cardsController;
        private InfoPopup _infoPopup;

        private readonly Vector3 _openRotate = new(0, 180, 0);

        
        private void Awake()
        {
            _playButton.Click += PlayButtonOnClick;

            var cardsConfig = CardsConfig.Instance;

            _colorSelectionPanel.Init(cardsConfig.GetColorSelectionData());
            _suitSelectionPanel.Init(cardsConfig.GetSuitSelectionData());

            _cardsController = FindObjectOfType<CardsController>();
            _cardsController.Init(cardsConfig.Cards);
            _cardsController.CardSelected += CardOnSelected;

            _infoPopup = FindObjectOfType<InfoPopup>();
            _infoPopup.Completed += InfoPopupOnCompleted;

            var betConfig = BetConfig.Instance;

            _betSelectionPanel.Changed += BetSelectionPanelOnChanged;
            _betSelectionPanel.Init(betConfig.BetData.BetTypes);

            _currency.Init(betConfig.BetData.StartCurrency);

            _betPanel.Changed += BetPanelOnChanged;
            _betPanel.Init(betConfig.BetData.Step);

            UpdateHeader(string.Empty);

            _infoPopup.Hide(false);
        }

       
        private void BetPanelOnChanged(int value) => _betPanel.UpdateButtons(_currency.Value);

      
        private void BetSelectionPanelOnChanged(string value)
        {
            var isActiveColor = value == "Color";

            _colorSelectionPanel.gameObject.SetActive(isActiveColor);
            _suitSelectionPanel.gameObject.SetActive(!isActiveColor);
        }

       
        private async void PlayButtonOnClick()
        {
            _currency.Value -= _betPanel.Value;

            UpdateHeader("Good Luck");

            LockUI(true, false);
            await _cardsController.Mix();

            UpdateHeader("Select Card");
        }

       
        private void UpdateHeader(string value) => _headerText.text = value;

      
        private async void CardOnSelected(CardManager card)
        {
            card.SetLayer(LayerManager.Popup);

            _infoPopup.Show(true);

            var factor = GetFactor(card.Data);
            var currency = _betPanel.Value * factor;

            var startShow = true;
            DOTween.Sequence()
                .Join(_infoPopup.Show(true))
                .Join(card.Move(_infoPopup.CardPoint, 0.5f, Ease.Linear))
                .Append(_infoPopup.ShowGlow())
                .Join(card.Rotate(_openRotate, 0.5f, Ease.Linear))
                .AppendCallback(() => _infoPopup.UpdateText($"{_betPanel.Value} ({factor})", $"{currency}"))
                .AppendCallback(() => startShow = false);

            await UniTask.WaitWhile(() => startShow);

            _currency.Value += (int) currency;

            _infoPopup.ShowCloseButton();
        }

      
        private float GetFactor(CardsConfig.Model model)
        {
            return _betSelectionPanel.Name switch
            {
                "Color" when _colorSelectionPanel.Name == model.ColorSelectionData.Name => model.ColorSelectionData
                    .Factor,
                "Suit" when _suitSelectionPanel.Name == model.SuitSelectionData.Name => model.SuitSelectionData.Factor,
                _ => 0.0f
            };
        }

    
        private Sequence LockUI(bool value, bool isAnimated)
        {
            return DOTween.Sequence()
                .Join(_playButton.LockButton(value, isAnimated))
                .Join(_betSelectionPanel.LockButton(value, isAnimated))
                .Join(_betPanel.LockButton(value, isAnimated))
                .Join(_colorSelectionPanel.LockButton(value, isAnimated))
                .Join(_suitSelectionPanel.LockButton(value, isAnimated));
        }


        private void InfoPopupOnCompleted()
        {
            _cardsController.Complete();
            _infoPopup.Hide(true);
            LockUI(false, true);
        }

       
        private void OnDestroy()
        {
            _playButton.Click -= PlayButtonOnClick;

            _currency.Dispose();

            _betSelectionPanel.Changed -= BetSelectionPanelOnChanged;
            _betSelectionPanel.Dispose();

            _betPanel.Changed -= BetPanelOnChanged;
            _betPanel.Dispose();

            _cardsController.CardSelected -= CardOnSelected;
            _cardsController.Dispose();

            _infoPopup.Completed -= InfoPopupOnCompleted;

            _colorSelectionPanel.Dispose();

            _suitSelectionPanel.Dispose();
        }
    }
}