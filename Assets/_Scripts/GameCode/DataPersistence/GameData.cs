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
        private readonly Data _data;
        private readonly GameConfig _gameConfig;
        
        public Data Data => _data;
        public MineData ActiveMineData => _data.MineDataList[0]; // _data.MineDataList[_data.ActiveMineIndex];

        public GameData(GameConfig gameConfig, Data data)
        {
            _data = data;
            _gameConfig = gameConfig;
        }
        
        public GameData(GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
            
            var activeMineIndex = 0;
            var mineDataList = new List<MineData>();
            for (var i = 0; i < gameConfig.MineConfigs.Count; i++)
            {
                mineDataList.Add(CreateMineData(i));
            }
            _data = new Data()
            {
                MineDataList = mineDataList,
                UserPreferencesData = new UserPreferencesData() { ActiveMineIndex = activeMineIndex }
            };
        }

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
