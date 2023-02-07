using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GameCode.UI
{
    public class MineSelectionView : MonoBehaviour
    {
        [SerializeField] private Transform _elementsParent;
        [SerializeField] private Image _background;
        
        public Transform ElementsParent => _elementsParent;
        public Image Background => _background;

        public List<MineSelectionElementView> MineSelectionElementViews => 
            _elementsParent.GetComponentsInChildren<MineSelectionElementView>().ToList();
    }
}
