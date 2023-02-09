using GameCode.DataPersistence;
using GameCode.Init;
using GameCode.Mineshaft;
using GameCode.Worker;
using UnityEngine;
using UniRx;

namespace GameCode.Elevator
{
    public class ElevatorController : ISaveable
    {
        private readonly ElevatorModel _model;
        private readonly ElevatorView _view;
        private readonly WorkerModel _workerModel;

        public ElevatorController(ElevatorCreationData creationData, ElevatorView view, ElevatorModel model, MineshaftMvcCollection mineshaftMvcCollection,
             GameConfig gameConfig, CompositeDisposable disposable, ISaveModel saveModel)
        {
            _model = model;
            _view = view;

            if (creationData.Worker.LocalPosition != Vector2.zero)
                _view.WorkerView.LocalPosition = creationData.Worker.LocalPosition;
            _workerModel = new WorkerModel(creationData.Worker, model, gameConfig.ElevatorWorkerConfig, disposable);
            new ElevatorWorkerController(view, model, _workerModel, mineshaftMvcCollection, disposable);

            model.CanUpgrade
                .Subscribe(canUpgrade => view.AreaUiCanvasView.UpgradeButton.interactable = canUpgrade)
                .AddTo(disposable);

            view.AreaUiCanvasView.UpgradeButton.OnClickAsObservable()
                .Subscribe(_ => Upgrade())
                .AddTo(disposable);

            model.StashAmount.Subscribe(amount => view.StashAmount = amount.ToString("F0"))
                .AddTo(disposable);
            
            _workerModel.CarryingCapacity
                .Subscribe(capacity => view.AreaUiCanvasView.CarryingCapacity = capacity.ToString("F0"))
                .AddTo(disposable);

            model.UpgradePrice
                .Subscribe(upgradePrice => view.AreaUiCanvasView.UpgradeCost = upgradePrice.ToString("F0"))
                .AddTo(disposable);
            
            saveModel.RegisterSaveable(this);
        }

        private void Upgrade()
        {
            _model.Upgrade();
        }

        public void OnSave(ref GameData data)
        {
            data.ActiveMineData.ElevatorCreationData.Worker =
                new WorkerCreationData()
                {
                    CarryingAmount = _workerModel.CarryingAmount.Value,
                    LocalPosition = _view.WorkerView.LocalPosition
                };
        }
    }
}
