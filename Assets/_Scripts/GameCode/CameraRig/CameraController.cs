using System.Collections;
using GameCode.Tutorial;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GameCode.CameraRig
{
    public class CameraController
    {
        private readonly CameraView _view;
        private readonly ITutorialModel _tutorialModel;

        public CameraController(CameraView view, ITutorialModel tutorialModel)
        {
            _view = view;
            _tutorialModel = tutorialModel;

            view.UpdateAsObservable()
                .Subscribe(_ => OnUpdate())
                .AddTo(view);
        }

        private void OnUpdate()
        {
            var yInput = Input.GetAxis("Vertical");

            if (Mathf.Abs(yInput) > 0)
            {
                if (_tutorialModel.ShouldShowTooltip.Value)
                {
                    MainThreadDispatcher.StartCoroutine(DisableTooltip());
                }

                var yPosition = _view.Position.y;
                yPosition += yInput * _view.Speed * Time.deltaTime;
                yPosition = Mathf.Clamp(yPosition, _view.CameraOffset.Down.y, _view.CameraOffset.Up.y);
                
                _view.Position = new Vector2(0, yPosition);
            }
            SetOrthographicSize();
        }

        private void SetOrthographicSize()
        {
            float currentAspect = (float)Screen.width / Screen.height;

            float minSize = 4f;
            float orthographicSize;
            if (currentAspect > .5f) // .5f -> 13.4 orthographic size by default
            {
                orthographicSize = 13.4f - Mathf.Abs((currentAspect - .5f) * 2f);
            }
            else
            {
                orthographicSize = (_view.ReferenceScreenHeight / currentAspect) / 200;
            }

            if (orthographicSize < minSize)
            {
                orthographicSize = minSize;
            }
            _view.Camera.orthographicSize = orthographicSize;
        }

        private IEnumerator DisableTooltip()
        {
            yield return new WaitForSeconds(_view.TooltipDelay);
            _tutorialModel.ShouldShowTooltip.Value = false;
        }
    }
}
