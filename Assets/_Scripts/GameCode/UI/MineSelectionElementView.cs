using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GameCode.UI
{
    public class MineSelectionElementView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private Button _switchMineButton;
        
        public string Name
        {
            set => _name.SetText($"{value}");
        }
        public string Description
        {
            set => _description.SetText($"{value}");
        }
        public Button SwitchMineButton => _switchMineButton;
    }
}
