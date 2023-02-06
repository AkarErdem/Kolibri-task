using GameCode.DataPersistence;
using GameCode.Finance;
using GameCode.GameArea;
using GameCode.Init;
using GameCode.Worker;
using UniRx;
using UnityEngine;
 
namespace GameCode.Elevator
{
    public class ElevatorModel : IAreaModel
    {
        private const double BasePrice = 60;
        private readonly GameConfig _config;
        private readonly FinanceModel _financeModel;
        private readonly IReactiveProperty<double> _upgradePrice;
        private readonly IReactiveProperty<int> _level;
        
        public double SkillMultiplier { get; set; }
        public IReadOnlyReactiveProperty<bool> CanUpgrade { get; }
        public IReadOnlyReactiveProperty<double> UpgradePrice => _upgradePrice;
        public IReadOnlyReactiveProperty<int> Level => _level;
        
        public readonly IReactiveProperty<double> _stashAmount;
        public IReactiveProperty<double> StashAmount => _stashAmount;
        
        public ElevatorModel(ElevatorCreationData creationData, GameConfig config, FinanceModel financeModel, CompositeDisposable disposable, ISaveModel saveModel)
        {
            _config = config;
            _financeModel = financeModel;

            _level = new ReactiveProperty<int>(creationData.Level);
            _stashAmount = new ReactiveProperty<double>(creationData.StashAmount);
            
            SkillMultiplier = Mathf.Pow(_config.ActorSkillIncrementPerShaft, 1) * Mathf.Pow(config.ActorUpgradeSkillIncrement, _level.Value - 1);
            _upgradePrice = new ReactiveProperty<double>(BasePrice * Mathf.Pow(_config.ActorUpgradePriceIncrement, _level.Value - 1));
            CanUpgrade = _financeModel.Money
                .Select(money => money >= _upgradePrice.Value)
                .ToReadOnlyReactiveProperty()
                .AddTo(disposable);
            
            saveModel.RegisterSaveable(this);
        }

        public void Upgrade()
        {
            if (_financeModel.Money.Value < _upgradePrice.Value) return;

            SkillMultiplier *= _config.ActorUpgradeSkillIncrement;
            var upgradePrice = _upgradePrice.Value;
            _upgradePrice.Value *= _config.ActorUpgradePriceIncrement;
            _financeModel.DrawResource(upgradePrice);
            _level.Value++;
        }
        
        public double DrawResource(double amount)
        {
            var result = 0d;
            if (StashAmount.Value <= amount)
            {
                result = StashAmount.Value;
                StashAmount.Value = 0;
            }
            else
            {
                result = amount;
                StashAmount.Value -= amount;
            }

            return result;
        }

        public void OnSave(ref GameData data)
        {
            data.ActiveMineData.ElevatorCreationData.Level = Level.Value;
            data.ActiveMineData.ElevatorCreationData.StashAmount = StashAmount.Value;
            Debug.Log("Elevator saved");
        }
    }
    
    [System.Serializable]
    public struct ElevatorCreationData
    {
        public int Level;
        public double StashAmount;
        public WorkerCreationData Worker;
    }
}
