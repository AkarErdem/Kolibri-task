using UnityEngine;

namespace GameCode.CameraRig
{
    public class CameraView : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _transform;
        [SerializeField] private CameraOffset _cameraOffset;
        [SerializeField] private float _verticalOffsetPerPixel;
        [SerializeField] private float _speed;
        [SerializeField] private float _tooltipDelay;
        [SerializeField] private float _referenceScreenHeight;
        [SerializeField] private float _referenceScreenWidth;
        
        public Camera Camera => _camera;

        public float Speed => _speed;

        public float TooltipDelay => _tooltipDelay;

        public Vector2 Position
        {
            get => _transform.position;
            set => _transform.position = value;
        }

        public float ReferenceScreenHeight => _referenceScreenHeight;

        public CameraOffset CameraOffset => _cameraOffset;
    }

    [System.Serializable]
    public class CameraOffset
    {
        public Vector2 Up;
        public Vector2 Down;
    }
}
