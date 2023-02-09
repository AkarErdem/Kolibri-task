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
        [SerializeField] private float _sceneLoadCooldownTime;
        [SerializeField] private int _mineConfigIndex;
        
        [Header("Save Data")]
        [SerializeField] private bool _encryptDataFile;
        [SerializeField] private string _encryptionCodeWord;
        [SerializeField] private string _dataFileName;

        public int MineConfigIndex
        {
            get => _mineConfigIndex;
            set => _mineConfigIndex = value;
        }

        public MineData StartingMineData => _mineConfigs[_mineConfigIndex].StartingMineData;
        public List<MineConfig> MineConfigs => _mineConfigs;
        
        public float SceneLoadCooldownTime => _sceneLoadCooldownTime;

        public bool EncryptDataFile => _encryptDataFile;
        public string EncryptionCodeWord => _encryptionCodeWord;
        public string DataFileName
        {
            get 
            {
                if (_encryptDataFile)
                    return $"{_dataFileName}_encrypted";
                return _dataFileName;
            }
        }
        public string DataDirPath => Application.persistentDataPath;
        
        public string MineName => _mineConfigs[_mineConfigIndex].MineName;

        public IMineshaftConfig MineshaftConfig => _mineConfigs[_mineConfigIndex].MineshaftConfig;
        public IWorkerConfig MineshaftWorkerConfig => _mineConfigs[_mineConfigIndex].MineshaftWorkerConfig;
        public IWorkerConfig ElevatorWorkerConfig => _mineConfigs[_mineConfigIndex].ElevatorWorkerConfig;
        public IWorkerConfig WarehouseWorkerConfig => _mineConfigs[_mineConfigIndex].WarehouseWorkerConfig;
        
        public float ActorUpgradePriceIncrement => _mineConfigs[_mineConfigIndex].ActorUpgradePriceIncrement;
        public float ActorUpgradeSkillIncrement => _mineConfigs[_mineConfigIndex].ActorUpgradeSkillIncrement;
        public float ActorPriceIncrementPerShaft => _mineConfigs[_mineConfigIndex].ActorPriceIncrementPerShaft;
        public float ActorSkillIncrementPerShaft => _mineConfigs[_mineConfigIndex].ActorSkillIncrementPerShaft;


        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [EditorCools.Button(name: "Set Mine Names")]
        private void SetMineNames()
        {
            for (int i = 0; i < _mineConfigs.Count; i++)
            {
                _mineConfigs[i].MineName = $"Mine {i+1}";
            }
        }

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
