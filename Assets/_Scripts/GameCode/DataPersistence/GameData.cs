using System.Collections.Generic;
using GameCode.Elevator;
using GameCode.Finance;
using GameCode.Init;
using GameCode.Mineshaft;
using GameCode.Utilities;
using GameCode.Warehouse;
using UnityEngine;

namespace GameCode.DataPersistence
{
    [System.Serializable]
    public class GameData
    {
        private readonly GameConfig _gameConfig;
        private Data _data;
        
        public Data Data => _data;
        
        public int ActiveMineIndex
        {
            get => _data.UserPreferencesData.ActiveMineIndex;
            set => _data.UserPreferencesData.ActiveMineIndex = value;
        }
        
        public MineData ActiveMineData
        {
            get
            {
                if (ActiveMineIndex >= _data.MineDataList.Count)
                    ActiveMineIndex = 0;
                return _data.MineDataList[ActiveMineIndex];
            }
        }
        
        public GameData(GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
            CreateData();
        }
        
        public GameData(GameConfig gameConfig, Data data)
        {
            _data = data;
            _gameConfig = gameConfig;

            // If given data is invalid
            if (data == null || data.MineDataList == null || data.MineDataList.Count == 0)
            {
                CreateData();
            }
            // If new mines added to the game
            else if (data.MineDataList.Count > _gameConfig.MineConfigs.Count)
            {
                int startIndex = _data.MineDataList.Count;
                for (var i = startIndex; i < _gameConfig.MineConfigs.Count; i++)
                {
                    _data.MineDataList.Add(CreateMineData(i));
                }
            }
        }
        
        /// <summary>
        /// Create default data
        /// </summary>
        private void CreateData()
        {
            var activeMineIndex = 0;
            var mineDataList = new List<MineData>();
            for (var i = 0; i < _gameConfig.MineConfigs.Count; i++)
            {
                mineDataList.Add(CreateMineData(i));
            }
            _data = new Data()
            {
                MineDataList = mineDataList,
                UserPreferencesData = new UserPreferencesData() { ActiveMineIndex = activeMineIndex }
            };
        }
        
        /// <summary>
        /// Create a deep copy of mine data via game config
        /// </summary>
        private MineData CreateMineData(int mineConfigIndex)
        {
            return _gameConfig.MineConfigs[mineConfigIndex].StartingMineData.Copy();
        }
    }
    
    [System.Serializable]
    public class Data
    {
        public List<MineData> MineDataList { get; set; }
        public UserPreferencesData UserPreferencesData { get; set; }
    }
    [System.Serializable]
    public class MineData
    {
        public Vector2 FirstMineshaftPosition;
        public FinanceCreationData FinanceCreationData;
        public WarehouseCreationData WarehouseCreationData;
        public ElevatorCreationData ElevatorCreationData;
        public List<MineshaftCreationData> MineshaftCreationData;
    }
    [System.Serializable]
    public class UserPreferencesData
    {
        public int ActiveMineIndex;
    }
}
