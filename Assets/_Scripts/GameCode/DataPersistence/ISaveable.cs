namespace GameCode.DataPersistence
{
    public interface ISaveable
    {
        void OnSave(ref GameData data);
    } 
}
