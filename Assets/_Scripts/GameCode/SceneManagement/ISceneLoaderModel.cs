using System.Collections;
using UniRx;

namespace GameCode.SceneManagement
{
    public interface ISceneLoaderModel
    {
        IReactiveProperty<bool> IsLoading { get; }
        void LoadScene(string sceneName);
        void ReloadScene();
    }
}
