using System.Collections;
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

        public void ReloadScene()
        {
            LoadScene(SceneManager.GetActiveScene().name);
        }

        public void LoadScene(string sceneName)
        {
            MainThreadDispatcher.StartCoroutine(LoadSceneAsync(sceneName));
        }

        /// <summary>
        /// The Application loads the Scene in the background as the current Scene runs.
        /// This is particularly good for creating loading screens.
        /// </summary>
        private IEnumerator LoadSceneAsync(string sceneName)
        {
            if (IsLoading.Value)
            {
                yield break;
            }

            IsLoading.Value = true;
            var asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            IsLoading.Value = false;
        }
    }
}
