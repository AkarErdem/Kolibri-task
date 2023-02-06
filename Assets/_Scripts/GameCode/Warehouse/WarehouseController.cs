using GameCode.DataPersistence;
using GameCode.Elevator;
using GameCode.Init;
using GameCode.Worker;
using UniRx;

namespace GameCode.Warehouse
{
    public class WarehouseController : ISaveable
    {
        private readonly WarehouseModel _model;
        private readonly WorkerModel _workerModel;

        public WarehouseController(WarehouseCreationData creationData, WarehouseView view, WarehouseModel model, ElevatorModel elevatorModel,
            GameConfig config, CompositeDisposable disposable, ISaveModel saveModel)
        {
            _model = model;

            _workerModel = new WorkerModel(creationData.Worker, model, config.WarehouseWorkerConfig, disposable);
            new WarehouseWorkerController(view, model, _workerModel, elevatorModel, disposable);

            model.CanUpgrade
                .Subscribe(canUpgrade => view.AreaUiCanvasView.UpgradeButton.interactable = canUpgrade)
                .AddTo(disposable);

            view.AreaUiCanvasView.UpgradeButton.OnClickAsObservable()
                .Subscribe(_ => Upgrade())
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
            data.ActiveMineData.WarehouseCreationData.Worker =
                new WorkerCreationData()
                {
                    CarryingAmount = _workerModel.CarryingAmount.Value
                };
        }
    }
}
