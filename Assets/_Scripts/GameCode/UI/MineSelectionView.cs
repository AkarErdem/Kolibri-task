using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameCode.UI
{
    public class MineSelectionView : MonoBehaviour
    {
        [SerializeField] private Transform _mineSelectionElementsParent;
        
        public Transform MineSelectionElementsParent => _mineSelectionElementsParent;

        public List<MineSelectionElementView> MineSelectionElementViews => 
            _mineSelectionElementsParent.GetComponentsInChildren<MineSelectionElementView>().ToList();
    }
}
