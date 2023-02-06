using UnityEngine;

namespace GameCode.CameraRig
{
    [ExecuteInEditMode]
    public class CameraView : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _transform;
        [SerializeField] private Vector2 _verticalLimit;
        [SerializeField] private float _verticalOffsetPerPixel;
        [SerializeField] private float _speed;
        [SerializeField] private float _tooltipDelay;
        [SerializeField] private float referenceScreenHeight;
        [SerializeField] private float referenceScreenWidth;
        
        public float Speed => _speed;
        public float TooltipDelay => _tooltipDelay;
        public Vector2 Position
        {
            get => _transform.position;
            set => _transform.position = value;
        }
        public Vector2 VerticalLimit
        {
            get
            {
                var screenWidth = Screen.width;
                if (screenWidth > referenceScreenWidth)
                {
                    var _verticalLimitY = (screenWidth - referenceScreenWidth) / _verticalOffsetPerPixel;
                    return new Vector2(_verticalLimit.x, _verticalLimitY);
                }
                return _verticalLimit;
            }
        }
        private void OnGUI()
        {
            float currentAspect = (float) Screen.width / Screen.height;
            _camera.orthographicSize = (referenceScreenHeight / currentAspect) / 200;
        }
    }
}