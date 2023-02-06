﻿using GameCode.DataPersistence;
using GameCode.Finance;
using GameCode.GameArea;
using GameCode.Init;
using GameCode.Worker;
using UniRx;
using UnityEngine;

namespace GameCode.Warehouse
{
    public class WarehouseModel : IAreaModel
    {
        private readonly GameConfig _config;
        private readonly FinanceModel _financeModel;
        
        private const double BasePrice = 60;
        private readonly IReactiveProperty<double> _upgradePrice;
        private readonly IReactiveProperty<int> _level;
        
        public WarehouseModel(WarehouseCreationData creationData, GameConfig config, ISaveModel saveModel, FinanceModel financeModel, CompositeDisposable disposable)
        {
            _config = config;
            _financeModel = financeModel;
            
            _level = new ReactiveProperty<int>(creationData.Level);
            SkillMultiplier = Mathf.Pow(_config.ActorSkillIncrementPerShaft, 1) * Mathf.Pow(config.ActorUpgradeSkillIncrement, _level.Value - 1);
            _upgradePrice = new ReactiveProperty<double>(BasePrice * Mathf.Pow(_config.ActorUpgradePriceIncrement, _level.Value - 1));
            CanUpgrade = _financeModel.Money
                .Select(money => money >= _upgradePrice.Value)
                .ToReadOnlyReactiveProperty()
                .AddTo(disposable);
            saveModel.RegisterSaveable(this);
        }
        public double SkillMultiplier { get; set; }
        public IReadOnlyReactiveProperty<bool> CanUpgrade { get; }
        public IReadOnlyReactiveProperty<double> UpgradePrice => _upgradePrice;
        public IReadOnlyReactiveProperty<int> Level => _level;
        public void AddResource(double amount) => _financeModel.AddResource(amount);
        public void Upgrade()
        {
            if (_financeModel.Money.Value < _upgradePrice.Value) return;
            
            SkillMultiplier *= _config.ActorUpgradeSkillIncrement;
            var upgradePrice = _upgradePrice.Value;
            _upgradePrice.Value *= _config.ActorUpgradePriceIncrement;
            _financeModel.DrawResource(upgradePrice);
            _level.Value++;
        }

        public void OnSave(ref GameData data)
        {
            data.ActiveMineData.WarehouseCreationData.Level = _level.Value;
            Debug.Log("Warehouse saved");
        }
    }
    
    [System.Serializable]
    public struct WarehouseCreationData
    {
        public int Level;
        public WorkerCreationData Worker;
    }
}
