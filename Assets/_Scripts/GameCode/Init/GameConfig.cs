using System.Collections.Generic;
using GameCode.DataPersistence;
using GameCode.Mineshaft;
using GameCode.Worker;
using UnityEngine;

namespace GameCode.Init
{
    [CreateAssetMenu(menuName = "Game Config")]
    public class GameConfig : ScriptableObject
    {
        [Header("Configuration")]
        [SerializeField] private List<MineConfig> _mineConfigs;
        private int _activeMineConfigIndex;
        
        [Header("Save Data")]
        [SerializeField] private bool _encryptDataFile;
        [SerializeField] private string _dataFileName;
        
        public List<MineConfig> MineConfigs => new(_mineConfigs);
        public bool EncryptDataFile => _encryptDataFile;
        
        public string DataFileName => _dataFileName;
        public string DataDirPath => Application.persistentDataPath;
        
        public MineData StartingMineData => _mineConfigs[_activeMineConfigIndex].StartingMineData;
        
        public IMineshaftConfig MineshaftConfig => _mineConfigs[_activeMineConfigIndex].MineshaftConfig;
        public IWorkerConfig MineshaftWorkerConfig => _mineConfigs[_activeMineConfigIndex].MineshaftWorkerConfig;
        public IWorkerConfig ElevatorWorkerConfig => _mineConfigs[_activeMineConfigIndex].ElevatorWorkerConfig;
        public IWorkerConfig WarehouseWorkerConfig => _mineConfigs[_activeMineConfigIndex].WarehouseWorkerConfig;
        
        public float ActorUpgradePriceIncrement => _mineConfigs[_activeMineConfigIndex].ActorUpgradePriceIncrement;
        public float ActorUpgradeSkillIncrement => _mineConfigs[_activeMineConfigIndex].ActorUpgradeSkillIncrement;
        public float ActorPriceIncrementPerShaft => _mineConfigs[_activeMineConfigIndex].ActorPriceIncrementPerShaft;
        public float ActorSkillIncrementPerShaft => _mineConfigs[_activeMineConfigIndex].ActorSkillIncrementPerShaft;
        
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [EditorCools.Button(name: "Show Data Directory Path")]
        private void ShowDataDirPath() => UnityEditor.EditorUtility.RevealInFinder($"{DataDirPath}");
    }

    [System.Serializable]
    public class MineConfig
    {
        [Header("Info")] 
        public string MineName;
        public string MineDescription;
        
        [Header("Config")]
        public MineshaftConfig MineshaftConfig;
        public WorkerConfig MineshaftWorkerConfig;
        public WorkerConfig ElevatorWorkerConfig;
        public WorkerConfig WarehouseWorkerConfig;

        [Header("Actor Increment")]
        public float ActorUpgradePriceIncrement;
        public float ActorUpgradeSkillIncrement;

        public float ActorPriceIncrementPerShaft;
        public float ActorSkillIncrementPerShaft;

        [Header("Starting Data")] 
        public MineData StartingMineData;
    }
}
