using UnityEngine;
using UnityEngine.UI;

namespace GameCode.UI
{
    public class LoadingScreenView : MonoBehaviour
    {
        [SerializeField] public Image _loadingImage;

        public Image LoadingImage => _loadingImage;
    }
}
