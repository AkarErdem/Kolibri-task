using GameCode.Init;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UniRx;

namespace GameCode.SceneManagement
{
    public class SceneLoaderModel : ISceneLoaderModel
    {
        private readonly GameConfig _gameConfig;

        public IReactiveProperty<bool> IsLoading { get; }
        
        public SceneLoaderModel(GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
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

            var time = Time.time;
            var asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            //Don't let the Scene activate until you allow it to
            asyncLoad.allowSceneActivation = false;

            // Wait until the timer ends
            while (Time.time - time < _gameConfig.SceneLoadCooldownTime)
            {
                yield return null;
            }

            // Wait until the asynchronous scene fully loads
            asyncLoad.allowSceneActivation = true;
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            IsLoading.Value = false;
        }
    }
}
