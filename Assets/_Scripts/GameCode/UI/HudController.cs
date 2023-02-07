using GameCode.Finance;
using GameCode.Tutorial;
using UniRx;
using UniRx.Triggers;

namespace GameCode.UI
{
    public class HudController
    {
        private readonly HudView _view;
        private readonly HudModel _model;

        public HudController(HudModel model, HudView view, FinanceModel financeModel, ITutorialModel tutorialModel,
            CompositeDisposable disposable)
        {
            _model = model;
            _view = view;
            
            financeModel.Money
                .Subscribe(money => view.CashAmount = money)
                .AddTo(disposable);
            
            tutorialModel.ShouldShowTooltip
                .Subscribe(UpdateTooltipVisibility)
                .AddTo(disposable);

            for (var i = 0; i < _view.MineSelectionView.MineSelectionElementViews.Count; i++)
            {
                var GoToMineButton = _view.MineSelectionView.MineSelectionElementViews[i].SwitchMineButton;
                GoToMineButton
                    .OnClickAsObservable()
                    .Subscribe(_ =>
                    {
                        UpdateMineSelectionVisibility(false);
                        SwitchMine(i);
                    })
                    .AddTo(disposable);
            }

            view.MapButton
                .OnClickAsObservable()
                .Subscribe(_ => UpdateMineSelectionVisibility(true))
                .AddTo(disposable);

            view.MineSelectionView.Background
                .OnPointerDownAsObservable()
                .Subscribe(_ => UpdateMineSelectionVisibility(false))
                .AddTo(disposable);
        }
        
        private void SwitchMine(int mineIndex)
        {
            _model.SwitchMine(mineIndex);
        }
        
        private void UpdateMineSelectionVisibility(bool shouldShowMineSelection)
        {
            _view.MineSelectionView.gameObject.SetActive(shouldShowMineSelection);
        }
        
        private void UpdateTooltipVisibility(bool shouldShowTooltip)
        {
            _view.TooltipVisible = shouldShowTooltip;
        }
    }
}
