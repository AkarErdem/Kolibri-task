namespace GameCode.DataPersistence
{
    public interface ISaveModel
    {
        GameData GameData { get; }
        void RegisterSaveable(ISaveable saveable);
        void SaveGame();
        void LoadGame();
    }
}
