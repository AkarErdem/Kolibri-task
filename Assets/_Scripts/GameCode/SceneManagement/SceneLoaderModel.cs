using UniRx;
using UnityEngine.SceneManagement;

namespace GameCode.SceneManagement
{
    public class SceneLoaderModel : ISceneLoaderModel
    {
        public IReactiveProperty<bool> IsLoading { get; }
        
        public SceneLoaderModel()
        {
            IsLoading = new ReactiveProperty<bool>(false);
        }
        
        public void LoadScene(string sceneName)
        {
            throw new System.NotImplementedException();
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
