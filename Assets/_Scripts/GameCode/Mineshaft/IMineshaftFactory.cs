namespace GameCode.Mineshaft
{
    public interface IMineshaftFactory
    {
        // Old implementation
        // void CreateMineshaft(int mineshaftNumber, int mineshaftLevel, Vector2 position);

        void CreateMineshaft(MineshaftCreationData creationData);
    }
}

