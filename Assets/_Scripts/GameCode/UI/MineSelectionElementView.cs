using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCode.UI
{
    public class MineSelectionElementView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private Button _goToMineButton;
        
        public TMP_Text Name => _name;
        public TMP_Text Description => _description;
        public Button GoToMineButton => _goToMineButton;
    }
}
