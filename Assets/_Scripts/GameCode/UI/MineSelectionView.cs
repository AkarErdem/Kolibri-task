using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GameCode.UI
{
    public class MineSelectionView : MonoBehaviour
    {
        [SerializeField] private Transform _mineSelectionPanel;
        [SerializeField] private Transform _elementsParent;
        [SerializeField] private Image _mineSelectionBackground;
        [SerializeField] private Button _closeButton;
        [SerializeField] private MineSelectionElementView mineSelectionElementViewPrefab;

        public Transform MineSelectionPanel => _mineSelectionPanel;
        public Transform ElementsParent => _elementsParent;
        public Image MineSelectionBackground => _mineSelectionBackground;
        public Button CloseButton => _closeButton;

        public MineSelectionElementView MineSelectionElementViewPrefab => mineSelectionElementViewPrefab;
    }
}
