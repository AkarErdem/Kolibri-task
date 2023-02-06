using UniRx;

namespace GameCode.DataPersistence
{
    public class DataPersistenceController
    {
        private readonly ISaveModel _model;
        
        public DataPersistenceController(ISaveModel model, CompositeDisposable disposable)
        {
            _model = model;
            
            Observable.OnceApplicationQuit()
                .Subscribe(_ => SaveGame())
                .AddTo(disposable);

            // Game loading starts here
            LoadGame();
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

