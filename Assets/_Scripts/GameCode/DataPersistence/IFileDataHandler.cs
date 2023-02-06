namespace GameCode.DataPersistence
{
    public interface IFileDataHandler
    {
        Data Load();
        void Save(Data data);
    }
}
