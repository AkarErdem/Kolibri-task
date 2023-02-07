using System.Collections.Generic;
using UnityEngine;
using GameCode.Init;

namespace GameCode.DataPersistence
{
    public class DataPersistenceModel : ISaveModel
    {
        private readonly GameConfig _gameConfig;
        private readonly IFileDataHandler _fileDataHandler;
        
        private GameData _gameData;
        private List<ISaveable> _saveableObjects;

        public GameData GameData => _gameData;

        public DataPersistenceModel(GameConfig gameConfig, IFileDataHandler fileDataHandler)
        {
            _saveableObjects = new List<ISaveable>();
            
            this._gameConfig = gameConfig;
            this._fileDataHandler = fileDataHandler;
        }

        /// <summary>
        /// Create a new GameData instance
        /// </summary>
        public void NewGame()
        {
            this._gameData = new GameData(_gameConfig);
        }
        
        /// <summary>
        /// Load any saved data from file using data handler
        /// </summary>
        public void LoadGame()
        {
            // Load any saved data from a file using data handler
            this._gameData = new GameData(_gameConfig, _fileDataHandler.Load());
            
            // If no data can be loaded, initialize to a new game
            if (this._gameData == null || this._gameData.Data == null)
            {
                Debug.LogWarning("No data was found. Initializing data to defaults.");
                NewGame();
            }
        }
        
        /// <summary>
        /// Save the current state of the game
        /// </summary>
        public void SaveGame()
        {
            // Pass the data to other scripts so they can update it
            foreach (var dataPersistenceObj in _saveableObjects)
            {
                dataPersistenceObj.OnSave(ref this._gameData);
            }
            // Save that data to a file using the data handler
            _fileDataHandler.Save(this._gameData.Data);
        }

        /// <summary>
        /// Register the saveable object
        /// </summary>
        public void RegisterSaveable(ISaveable saveable)
        {
            _saveableObjects.Add(saveable);
        }
    }
}
