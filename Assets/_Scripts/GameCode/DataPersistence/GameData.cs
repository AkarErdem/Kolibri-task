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

        public MineData ActiveMineData => _data.MineDataList[ActiveMineIndex];


        public GameData(GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
            CreateData();
        }
        
        public GameData(GameConfig gameConfig, Data data)
        {
            _data = data;
            _gameConfig = gameConfig;

            // If no data has found
            if (_data == null || _data.MineDataList == null || _data.MineDataList.Count == 0)
            {
                Debug.Log("No save data has found. Creating a new one.");
                CreateData();
            }
            // If new mines added to the game
            else if (_gameConfig.MineConfigs.Count > _data.MineDataList.Count)
            {
                Debug.Log("New mines added to the game.\n Save data is updating.");
                int startIndex = _data.MineDataList.Count;
                for (var i = startIndex; i < _gameConfig.MineConfigs.Count; i++)
                {
                    _data.MineDataList.Add(CreateMineData(i));
                }
            }
            // If save file has more mine than it should be
            else if (_data.MineDataList.Count > _gameConfig.MineConfigs.Count)
            {
                Debug.Log($"Save data has more mine than it should be. Removing the extra mines.");

                int startIndex = _data.MineDataList.Count - 1;
                for (var i = startIndex; i >= _gameConfig.MineConfigs.Count; i--)
                {
                    _data.MineDataList.RemoveAt(i);
                }
            }
        }
        
        /// <summary>
        /// Create default data
        /// </summary>
        private void CreateData()
        {
            var mineDataList = new List<MineData>();
            for (var i = 0; i < _gameConfig.MineConfigs.Count; i++)
            {
                mineDataList.Add(CreateMineData(i));
            }
            _data = new Data()
            {
                MineDataList = mineDataList,
                UserPreferencesData = new UserPreferencesData() { ActiveMineIndex = 0 }
            };
        }
        
        /// <summary>
        /// Create a deep copy of mine data using starting mine data from game config
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
