using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCode.UI
{
    public class HudView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _cashAmount;
        [SerializeField] private GameObject _tooltip;
        [SerializeField] private MineSelectionView _mineSelectionView;
        [SerializeField] private Button _mapButton;
        
        public double CashAmount
        {
            set => _cashAmount.SetText($"Money: {value:F0}");
        }

        public bool TooltipVisible
        {
            set => _tooltip.gameObject.SetActive(value);
        }
        
        public MineSelectionView MineSelectionView => _mineSelectionView;
        
        public Button MapButton => _mapButton;
    }
}
