using GameCode.Finance;
using GameCode.Init;
using GameCode.SceneManagement;
using GameCode.Tutorial;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GameCode.UI
{
    public class HudController
    {
        private readonly HudView _view;
        private readonly HudModel _model;

        public HudController(HudModel model, HudView view, GameConfig gameConfig, FinanceModel financeModel, 
            ITutorialModel tutorialModel, ISceneLoaderModel sceneLoaderModel, CompositeDisposable disposable)
        {
            _model = model;
            _view = view;

            view.MineName = gameConfig.MineName;

            financeModel.Money
                .Subscribe(money => view.CashAmount = money)
                .AddTo(disposable);

            tutorialModel.ShouldShowTooltip
                .Subscribe(UpdateTooltipVisibility)
                .AddTo(disposable);

            sceneLoaderModel.IsLoading
                .Subscribe(UpdateLoadingScreenVisibility)
                .AddTo(disposable);

            view.MapButton
                .OnClickAsObservable()
                .Subscribe(_ => UpdateMineSelectionVisibility(true))
                .AddTo(disposable);

            view.MineSelectionView.CloseButton
                .OnClickAsObservable()
                .Subscribe(_ => UpdateMineSelectionVisibility(false))
                .AddTo(disposable);

            view.MineSelectionView.MineSelectionBackground
                .OnPointerDownAsObservable()
                .Subscribe(_ => UpdateMineSelectionVisibility(false))
                .AddTo(disposable);

            for (int i = 0; i < gameConfig.MineConfigs.Count; i++)
            {
                MineConfig mineConfig = gameConfig.MineConfigs[i];

                var mineIndex = i;
                var mineSelectionElementView = Object.Instantiate(view.MineSelectionView.MineSelectionElementViewPrefab, view.MineSelectionView.ElementsParent);
                mineSelectionElementView.Name = mineConfig.MineName;
                mineSelectionElementView.Description = mineConfig.MineDescription;
                mineSelectionElementView.SwitchMineButton.interactable = gameConfig.MineConfigIndex != i;
                mineSelectionElementView.SwitchMineButton
                    .OnClickAsObservable()
                    .Subscribe(_ =>
                    {
                        UpdateMineSelectionVisibility(false);
                        SwitchMine(mineIndex);
                    })
                    .AddTo(disposable);
            }
        }
        
        private void SwitchMine(int mineIndex)
        {
            _model.SwitchMine(mineIndex);
        }
        
        private void UpdateMineSelectionVisibility(bool shouldShowMineSelection)
        {
            _model.UpdateMineSelectionVisibility(_view.MineSelectionView.transform, _view.MineSelectionView.MineSelectionPanel,
                _view.MineSelectionView.MineSelectionBackground, shouldShowMineSelection);
        }

        private void UpdateTooltipVisibility(bool shouldShowTooltip)
        {
            _view.TooltipVisible = shouldShowTooltip;
        }

        private void UpdateLoadingScreenVisibility(bool shouldShowLoadingScreen)
        {
            _view.LoadingScreen.enabled = shouldShowLoadingScreen;
        }

        
    }
}
