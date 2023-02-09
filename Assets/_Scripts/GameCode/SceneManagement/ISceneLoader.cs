using UniRx;

namespace GameCode.SceneManagement
{
    public interface ISceneLoader
    {
        IReactiveProperty<bool> IsLoading { get; }
        void LoadScene(string sceneName);
        void ReloadScene();
    }
}
