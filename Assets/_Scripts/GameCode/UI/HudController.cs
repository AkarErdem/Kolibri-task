using GameCode.Finance;
using GameCode.Tutorial;
using UniRx;

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

            int mineIndex = 0;
            foreach (var mineSelectionElementView in _view.MineSelectionView.MineSelectionElementViews)
            {
                mineSelectionElementView.GoToMineButton.
                    OnClickAsObservable().
                    Subscribe(_ =>
                    {
                        SwitchMine(mineIndex);
                    })
                    .AddTo(disposable);
                mineIndex++;
            }
        }
        
        private void SwitchMine(int mineIndex)
        {
            _model.SwitchMine(mineIndex);
        }

        private void UpdateTooltipVisibility(bool shouldShowTooltip)
        {
            _view.TooltipVisible = shouldShowTooltip;
        }
    }
}