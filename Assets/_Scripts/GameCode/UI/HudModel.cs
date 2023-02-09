using DG.Tweening;
using GameCode.DataPersistence;
using GameCode.Init;
using GameCode.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace GameCode.UI
{
    public class HudModel
    {
        private readonly ISaveModel _saveModel;
        private readonly ISceneLoaderModel _sceneLoaderModel;
        private readonly GameConfig _gameConfig;
        private Vector3 mineSelectionPanelStartingPosition;
        private int _mineConfigIndex;

        public HudModel(GameConfig gameConfig, ISceneLoaderModel sceneLoaderModel, ISaveModel saveModel)
        {
            this._gameConfig = gameConfig;
            this._saveModel = saveModel;
            this._sceneLoaderModel = sceneLoaderModel;

            this._mineConfigIndex = gameConfig.MineConfigIndex;
            saveModel.SaveGame();
            saveModel.AfterOnSaveCalled += SaveModel_AfterOnSaveCalled;
        }

        public void UpdateMineSelectionVisibility(Transform mineSelectionTransform, 
            Transform mineSelectionPanelTransform, Image mineSelectionBackgroundImage, 
            bool shouldShow)
        {
            if (mineSelectionPanelStartingPosition == Vector3.zero)
                mineSelectionPanelStartingPosition = mineSelectionPanelTransform.localPosition;

            var tweenTime = .5f;
            var destination = Vector3.zero;
            var finalAlpha = 0.4f;

            if (shouldShow)
            {
                UpdateTransformVisibility(mineSelectionTransform, true);
            }
            else
            {
                finalAlpha = 0;
                destination = mineSelectionPanelStartingPosition;
            }

            DOTween.ToAlpha(
                () => mineSelectionBackgroundImage.color,
                x => mineSelectionBackgroundImage.color = x,
                finalAlpha, tweenTime)
                .SetEase(Ease.OutQuad);

            mineSelectionPanelTransform
                .DOLocalMove(destination, tweenTime)
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    UpdateTransformVisibility(mineSelectionTransform, shouldShow);
                });
        }

        public void UpdateLoadingScreenVisibility(Image loadingScreen, bool shouldShow)
        {
            var tweenTime = _gameConfig.SceneLoadCooldownTime;
            var destination = Vector3.zero;
            var finalAlpha = 0;

            if (shouldShow)
            {
                loadingScreen.color = new Color(loadingScreen.color.r, loadingScreen.color.g, loadingScreen.color.b, 0);
                finalAlpha = 1;
            }
            else
            {
                loadingScreen.color = new Color(loadingScreen.color.r, loadingScreen.color.g, loadingScreen.color.b, 1);
                finalAlpha = 0;
            }

            UpdateTransformVisibility(loadingScreen.transform, true);

            DOTween.ToAlpha(
                () => loadingScreen.color,
                x => loadingScreen.color = x,
                finalAlpha, tweenTime)
                .SetEase(Ease.OutQuad);

            loadingScreen.transform
                .DOLocalMove(destination, tweenTime)
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    UpdateTransformVisibility(loadingScreen.transform, shouldShow);
                });
        }

        public void SwitchMine(int mineIndex)
        {
            _mineConfigIndex = mineIndex;
            _saveModel.SaveGame();
            _sceneLoaderModel.ReloadScene();
        }

        private void UpdateTransformVisibility(Transform transform, bool value)
        {
            if (transform == null) return;
            transform.gameObject.SetActive(value);
        }

        private void SaveModel_AfterOnSaveCalled(GameData gameData)
        {
            _saveModel.AfterOnSaveCalled -= SaveModel_AfterOnSaveCalled;
            gameData.Data.UserPreferencesData.ActiveMineIndex = _mineConfigIndex;
        }
    }
}
