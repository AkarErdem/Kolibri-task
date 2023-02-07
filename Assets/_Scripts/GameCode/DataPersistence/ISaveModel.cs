namespace GameCode.DataPersistence
{
    public interface ISaveModel
    {
        public GameData GameData { get; }
        void RegisterSaveable(ISaveable saveable);
        void SaveGame();
        void LoadGame();
    }
}
