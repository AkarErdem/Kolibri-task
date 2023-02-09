using GameCode.Init;
using GameCode.SceneManagement;
using UniRx;
using UnityEngine;

namespace GameCode.DataPersistence
{
    public class DataPersistenceController
    {
        private readonly ISaveModel _model;
        private readonly ISceneLoaderModel _sceneLoaderModel;

        public DataPersistenceController(GameConfig gameConfig, ISaveModel model, ISceneLoaderModel sceneLoaderModel, CompositeDisposable disposable)
        {
            _model = model;
            _sceneLoaderModel = sceneLoaderModel;
            
            Observable.OnceApplicationQuit()
                .Subscribe(_ => SaveGame())
                .AddTo(disposable);

            // Game loading starts here
            LoadGame();

            // Check the active mine index
            int activeMineIndex = model.GameData.ActiveMineIndex;
            if (activeMineIndex < 0)
                activeMineIndex = 0;
            else if (activeMineIndex >= gameConfig.MineConfigs.Count)
                activeMineIndex = gameConfig.MineConfigs.Count - 1;
            gameConfig.MineConfigIndex = activeMineIndex;
            model.GameData.ActiveMineIndex = activeMineIndex;
        }

        public void SaveGame()
        {
            _model.SaveGame();
        }

        public void LoadGame()
        {
            _model.LoadGame();
        }
    }
}

