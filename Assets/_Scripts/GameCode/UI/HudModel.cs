using DG.Tweening;
using GameCode.DataPersistence;
using GameCode.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace GameCode.UI
{
    public class HudModel
    {
        private ISaveModel _saveModel;
        private ISceneLoaderModel _sceneLoaderModel;
        private int _mineConfigIndex;
        private Vector3 mineSelectionPanelStartingPosition;
        
        public HudModel(ISceneLoaderModel sceneLoaderModel, ISaveModel saveModel)
        {
            this._saveModel = saveModel;
            this._sceneLoaderModel = sceneLoaderModel;
            saveModel.AfterOnSaveCalled += SaveModel_AfterOnSaveCalled;
        }

        public void UpdateMineSelectionVisibility(Transform mineSelectionTransform, 
            Transform mineSelectionPanelTransform, Image mineSelectionBackgroundImage, 
            bool shouldShowMineSelection)
        {
            if (mineSelectionPanelStartingPosition == Vector3.zero)
                mineSelectionPanelStartingPosition = mineSelectionPanelTransform.localPosition;

            var tweenTime = .5f;
            var destination = Vector3.zero;
            var finalAlpha = 0.4f;

            if (!shouldShowMineSelection)
            {
                finalAlpha = 0;
                destination = mineSelectionPanelStartingPosition;
            }

            if (shouldShowMineSelection)
                UpdateTransformVisibility(mineSelectionTransform, true);

            DOTween.ToAlpha(() => mineSelectionBackgroundImage.color, x => mineSelectionBackgroundImage.color = x,
                finalAlpha, tweenTime).SetEase(Ease.OutQuad);

            mineSelectionPanelTransform.DOLocalMove(destination, tweenTime).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                UpdateTransformVisibility(mineSelectionTransform, shouldShowMineSelection);
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
            transform.gameObject.SetActive(value);
        }
        private void SaveModel_AfterOnSaveCalled(GameData gameData)
        {
            _saveModel.AfterOnSaveCalled -= SaveModel_AfterOnSaveCalled;
            gameData.Data.UserPreferencesData.ActiveMineIndex = _mineConfigIndex;
        }
    }
}
