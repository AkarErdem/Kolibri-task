using GameCode.DataPersistence;
using GameCode.Finance;
using GameCode.GameArea;
using GameCode.Init;
using GameCode.Worker;
using UniRx;
using UnityEngine;

namespace GameCode.Mineshaft
{
    public class MineshaftModel : IAreaModel
    {
        private const double BasePrice = 60;
        private readonly GameConfig _config;
        private readonly FinanceModel _financeModel;
        
        public int MineshaftNumber { get; }
        public IReadOnlyReactiveProperty<bool> CanUpgrade { get; }
        private readonly IReactiveProperty<double> _upgradePrice;
        public IReadOnlyReactiveProperty<double> UpgradePrice => _upgradePrice;
        private readonly IReactiveProperty<int> _level;
        public IReadOnlyReactiveProperty<int> Level => _level;
        private readonly IReactiveProperty<double> _stashAmount;
        public IReactiveProperty<double> StashAmount => _stashAmount;
        public double NextShaftPrice { get; }
        public IReadOnlyReactiveProperty<bool> CanBuyNextShaft { get; }
            
        public MineshaftModel(MineshaftCreationData creationData, int mineshaftNumber, GameConfig config, FinanceModel financeModel, CompositeDisposable disposable)
        {
            MineshaftNumber = mineshaftNumber;
            
            _config = config;
            
            _financeModel = financeModel;
            
            _level = new ReactiveProperty<int>(creationData.Level);
            
            _stashAmount = new ReactiveProperty<double>(creationData.StashAmount);
            
            SkillMultiplier = Mathf.Pow(_config.ActorSkillIncrementPerShaft, MineshaftNumber) * Mathf.Pow(config.ActorUpgradeSkillIncrement, _level.Value - 1);
            
            _upgradePrice = new ReactiveProperty<double>(BasePrice * Mathf.Pow(config.ActorPriceIncrementPerShaft, MineshaftNumber - 1)
                                                                   * Mathf.Pow(_config.ActorUpgradePriceIncrement, _level.Value - 1));
            
            NextShaftPrice = config.MineshaftConfig.BaseMineshaftCost * Mathf.Pow(config.MineshaftConfig.MineshaftCostIncrement, MineshaftNumber - 1);
            
            CanUpgrade = _financeModel.Money
                .Select(money => money >= _upgradePrice.Value)
                .ToReadOnlyReactiveProperty()
                .AddTo(disposable);
            
            CanBuyNextShaft = _financeModel.Money
                .Select(money => money >= NextShaftPrice)
                .ToReadOnlyReactiveProperty()
                .AddTo(disposable);
        }
        
        public void Upgrade()
        {
            if (_financeModel.Money.Value < _upgradePrice.Value)
                return;

            var price = _upgradePrice.Value;
            SkillMultiplier *= _config.ActorUpgradeSkillIncrement;
            _upgradePrice.Value *= _config.ActorUpgradePriceIncrement;
            _financeModel.DrawResource(price);
            _level.Value++;
        }
        
        public void BuyNextShaft()
        {
            if (_financeModel.Money.Value < NextShaftPrice)
                return;
            _financeModel.DrawResource(NextShaftPrice);
        }
        
        public double SkillMultiplier { get; set; }
        
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
        
        public void OnSave(ref GameData data) { } // Factory handles the mineshaft save
    }
    
    [System.Serializable]
    public struct MineshaftCreationData
    {
        public int Level;
        public double StashAmount;
        public WorkerCreationData Worker;
    }
}
