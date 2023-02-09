using GameCode.CameraRig;
using GameCode.DataPersistence;
using GameCode.Elevator;
using GameCode.Finance;
using GameCode.Mineshaft;
using GameCode.SceneManagement;
using GameCode.Tutorial;
using GameCode.UI;
using GameCode.Warehouse;
using UniRx;
using UnityEngine;

namespace GameCode.Init
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private GameConfig _gameConfig;
        
        [SerializeField] private HudView _hudView;
        [SerializeField] private CameraView _cameraView;
        [SerializeField] private ElevatorView _elevatorView;
        [SerializeField] private WarehouseView _warehouseView;

        private void Start()
        {
            // Disposable
            var disposable = new CompositeDisposable().AddTo(this);

            // Tutorial
            var tutorialModel = new TutorialModel();

            // SceneHandler
            var sceneHandlerModel = new SceneLoaderModel();

            // Camera
            new CameraController(_cameraView, tutorialModel);

            // File Data Handler
            var fileDataHandler = new FileDataHandlerModel(_gameConfig);

            // Data Persistence
            var saveModel = new DataPersistenceModel(_gameConfig, fileDataHandler);
            new DataPersistenceController(_gameConfig, saveModel, sceneHandlerModel, disposable);
            
            // Receive GameData -> ActiveMineData
            var activeMineData = saveModel.GameData.ActiveMineData;
            
            // Finance
            var financeModel = new FinanceModel(activeMineData.FinanceCreationData, saveModel);
            
            // Mineshaft
            var mineshaftCollectionModel = new MineshaftMvcCollection();
            var mineshaftFactory = new MineshaftFactory(mineshaftCollectionModel, financeModel, _gameConfig, disposable, saveModel);
            mineshaftFactory.CreateMineshaft(activeMineData.MineshaftCreationData);

            // Elevator
            var elevatorModel = new ElevatorModel(activeMineData.ElevatorCreationData, _gameConfig, financeModel, disposable, saveModel);
            new ElevatorController(activeMineData.ElevatorCreationData, _elevatorView, elevatorModel, mineshaftCollectionModel, _gameConfig, disposable, saveModel);
            
            // Warehouse
            var warehouseModel = new WarehouseModel(activeMineData.WarehouseCreationData, _gameConfig, saveModel, financeModel, disposable);
            new WarehouseController(activeMineData.WarehouseCreationData, _warehouseView, warehouseModel, elevatorModel, _gameConfig, disposable, saveModel);

            // Hud
            var hudModel = new HudModel(sceneHandlerModel, saveModel);
            new HudController(hudModel, _hudView, _gameConfig, financeModel, tutorialModel, sceneHandlerModel, disposable);
        }
    }
}