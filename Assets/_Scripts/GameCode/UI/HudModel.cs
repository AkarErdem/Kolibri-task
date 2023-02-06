using GameCode.DataPersistence;
using GameCode.Init;
using GameCode.SceneManagement;

namespace GameCode.UI
{
    public class HudModel
    {
        private GameConfig _gameConfig;
        private SceneLoaderModel _sceneLoaderModel;
        private ISaveModel _saveModel;
        
        public HudModel(GameConfig gameConfig, SceneLoaderModel sceneLoaderModel, ISaveModel saveModel)
        {
            this._gameConfig = gameConfig;
            this._saveModel = saveModel;
            this._sceneLoaderModel = sceneLoaderModel;
        }

        public void SwitchMine(int mineIndex)
        {
            _saveModel.SaveGame();
            _sceneLoaderModel.ReloadScene();
        }
    }
}
