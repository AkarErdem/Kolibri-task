using GameCode.DataPersistence;
using UniRx;
using UnityEngine;

namespace GameCode.Finance
{
    public class FinanceModel : ISaveable
    {
        private readonly IReactiveProperty<double> _money;
        public IReadOnlyReactiveProperty<double> Money => _money;

        public FinanceModel(FinanceCreationData creationData, ISaveModel saveModel)
        {
            _money = new ReactiveProperty<double>(creationData.Money);
            saveModel.RegisterSaveable(this);
        }

        public void AddResource(double amount)
        {
            if (amount < 0)
            {
                Debug.LogWarning("Received negative amount to add to inventory!");
                return;
            }

            _money.Value += amount;
        }

        public double DrawResource(double amount)
        {
            var result = 0d;
            if (_money.Value <= amount)
            {
                result = _money.Value;
                _money.Value = 0;
            }
            else
            {
                result = amount;
                _money.Value -= amount;
            }
            
            return result;
        }

        public void OnSave(ref GameData data)
        {
            data.ActiveMineData.FinanceCreationData = 
                new FinanceCreationData()
                {
                    Money = _money.Value
                };
        }
    }

    [System.Serializable]
    public struct FinanceCreationData
    {
        public double Money;
    }
}
