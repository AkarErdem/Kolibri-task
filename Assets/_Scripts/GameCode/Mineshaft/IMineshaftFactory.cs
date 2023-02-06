namespace GameCode.Mineshaft
{
    public interface IMineshaftFactory
    {
        void CreateMineshaft(MineshaftCreationData creationData);
        
        // Old implementation
        // void CreateMineshaft(int mineshaftNumber, int mineshaftLevel, Vector2 position);
    }
}

