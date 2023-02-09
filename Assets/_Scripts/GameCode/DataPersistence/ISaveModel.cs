using System;

namespace GameCode.DataPersistence
{
    public interface ISaveModel
    {
        GameData GameData { get; }
        event Action<GameData> AfterOnSaveCalled;
        void RegisterSaveable(ISaveable saveable);
        void SaveGame();
        void LoadGame();
    }
}
