using System.Collections.Generic;
using System.Linq;
using GameCode.DataPersistence;
using GameCode.Finance;
using GameCode.Init;
using GameCode.Worker;
using UniRx;
using UnityEngine;

namespace GameCode.Mineshaft
{
    public class MineshaftFactory : IMineshaftFactory, ISaveable
    {
        private readonly MineshaftMvcCollection _mvcCollection;
        private readonly FinanceModel _financeModel;
        private readonly GameConfig _config;
        private readonly CompositeDisposable _disposable;

        public MineshaftFactory(MineshaftMvcCollection mvcCollection, FinanceModel financeModel, GameConfig config, CompositeDisposable disposable, ISaveModel saveModel)
        {
            _mvcCollection = mvcCollection;
            _financeModel = financeModel;
            _config = config;
            _disposable = disposable;
            saveModel.RegisterSaveable(this);
        }
        
        // Old implementation: void CreateMineshaft(int mineshaftNumber, int mineshaftLevel, Vector2 position)
        public void CreateMineshaft(MineshaftCreationData creationData)
        {
            var mineshaftNumber = 1;
            var mineshaftPosition = _config.StartingMineData.FirstMineshaftPosition;
            if (_mvcCollection.GetCount() > 0) // If this is not the first created mineshaft
            {
                var lastPair = _mvcCollection.GetViews().LastOrDefault();
                mineshaftNumber = lastPair.Key + 1;
                mineshaftPosition = lastPair.Value.NextShaftView.NextShaftPosition;
                lastPair.Value.NextShaftView.Visible = false;
            }

            var mineshaftModel = new MineshaftModel(creationData, mineshaftNumber, _config, _financeModel, _disposable);
            var mineshaftView = Object.Instantiate(_config.MineshaftConfig.MineshaftPrefab, mineshaftPosition, Quaternion.identity);
            var mineshaftController = new MineshaftController(creationData, mineshaftView, mineshaftModel, this, _config, _disposable);

            _mvcCollection.Register(mineshaftNumber, mineshaftModel, mineshaftView, mineshaftController);
        }
        
        /// <summary>
        /// Create multiple mineshafts
        /// </summary>
        public void CreateMineshaft(IEnumerable<MineshaftCreationData> argsList)
        {
            foreach (var args in argsList)
            {
                CreateMineshaft(args);
            }
        }

        public void OnSave(ref GameData data)
        {
            var mineshaftCreationDataList = new List<MineshaftCreationData>();
            
            foreach (var mineshaftModelPair in _mvcCollection.GetModels())
            {
                int key = mineshaftModelPair.Key;
                var mineshaftModel = mineshaftModelPair.Value;
                mineshaftCreationDataList.Add(new MineshaftCreationData()
                {
                    Level = mineshaftModel.Level.Value,
                    StashAmount = mineshaftModel.StashAmount.Value,
                    Worker = new WorkerCreationData()
                    {
                        CarryingAmount = _mvcCollection.GetController(key).WorkerModel.CarryingAmount.Value
                    }
                });
            }
            
            data.ActiveMineData.MineshaftCreationData = new List<MineshaftCreationData>(mineshaftCreationDataList);
        }
    }
}

