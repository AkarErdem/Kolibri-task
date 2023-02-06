using GameCode.GameArea;
using UniRx;

namespace GameCode.Worker
{
    public class WorkerModel
    {
        private readonly IWorkerConfig _config;

        public WorkerState State;
        public readonly IReactiveProperty<double> CarryingAmount;
        public float Speed => _config.Speed;
        public IReadOnlyReactiveProperty<double> CarryingCapacity { get; }
        public float JobTime => _config.GetJobTime(State);

        public WorkerModel(WorkerCreationData creationData, IAreaModel areaModel, IWorkerConfig config, CompositeDisposable disposable)
        {
            _config = config;
            CarryingAmount = new ReactiveProperty<double>(creationData.CarryingAmount);
            CarryingCapacity = areaModel.Level.Select(_ => _config.Skill * areaModel.SkillMultiplier)
                .ToReadOnlyReactiveProperty()
                .AddTo(disposable);
        }
    }
    
    [System.Serializable]
    public struct WorkerCreationData
    {
        public double CarryingAmount;
    }
}
