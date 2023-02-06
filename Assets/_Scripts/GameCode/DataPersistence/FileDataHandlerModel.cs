using System;
using System.IO;
using GameCode.Init;
using UnityEngine;
using Newtonsoft.Json;

namespace GameCode.DataPersistence
{
    public class FileDataHandlerModel : IFileDataHandler
    {
        private GameConfig _gameConfig;
        private readonly string _dataDirPath;
        private readonly string _dataFileName;

        public FileDataHandlerModel(GameConfig gameConfig)
        {
            this._gameConfig = gameConfig;
            this._dataDirPath = gameConfig.DataDirPath;
            this._dataFileName = gameConfig.DataFileName;
        }

        public Data Load()
        {
            Data data = null;
            
            // Use Path.Combine to account for different OS's having different path separators
            var fullPath = new DirectoryInfo(Path.Combine(_dataDirPath, _dataFileName)).FullName;
            
            if (File.Exists(fullPath))
            {
                try
                {
                    // Load the serialized data from the file
                    string dataToLoad = string.Empty;
                    
                    // Read the serialized data from the file
                    using(var stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }
                    
                    // Deserialize the data from Json back into the C# object
                    data = new GameData(_gameConfig, JsonConvert.DeserializeObject<Data>(dataToLoad)).Data;
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error occured when trying to load data from file: {fullPath}\n{e.Message}");
                }
            }

            return data;
        }
        
        public void Save(Data data)
        {
            // Use Path.Combine to account for different OS's having different path separators
            var fullPath = new DirectoryInfo(Path.Combine(_dataDirPath, _dataFileName)).FullName;
            
            try
            {
                // Create the directory the file will be written if it does not already exists
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                
                // Serialize the C# game data object into Json
                string dataToSave = JsonConvert.SerializeObject(data, Formatting.Indented);
                
                // Write the serialized data to the file
                using(var stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToSave);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error occured when trying to save data to file: {fullPath}\n{e.Message}");
            }
        }
    }
}
