using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCode.UI
{
    public class HudView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _cashAmount;
        [SerializeField] private TMP_Text _mineName;
        [SerializeField] private GameObject _tooltip;
        [SerializeField] private MineSelectionView _mineSelectionView;
        [SerializeField] private Button _mapButton;
        [SerializeField] private Image _loadingScreen;

        public string MineName
        {
            set => _mineName.SetText($"{value}");
        }

        public double CashAmount
        {
            set => _cashAmount.SetText($"{value:F0} <sprite=0>");
        }

        public bool TooltipVisible
        {
            set => _tooltip.gameObject.SetActive(value);
        }
        
        public MineSelectionView MineSelectionView => _mineSelectionView;
        
        public Button MapButton => _mapButton;

        public Image LoadingScreen => _loadingScreen;
    }
}
