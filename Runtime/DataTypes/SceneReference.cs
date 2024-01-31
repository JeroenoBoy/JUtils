using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JUtils
{
    /// <summary>
    /// Reference a scene via its asset instead of its name. It can also give suggestions based on it being null / not in build
    /// </summary>
    /// <example><code lang="CSharp">
    /// namespace Example
    /// {
    ///     public class Game : ScriptableObject
    ///     {
    ///         [SerializeField] private SceneReference _scene;
    ///
    ///         public IEnumerator LoadGameAsync(Game currentGame)
    ///         {
    ///             yield return currentGame._scene.UnloadSceneAsync();
    ///             yield return _scene.LoadSceneAsync(LoadSceneMode.Additive);
    ///         }
    ///     }
    /// }
    /// </code></example>>
    [Serializable]
    public partial struct SceneReference
    {
        [SerializeField] private string _sceneName;
        [SerializeField] private string _scenePath;

        public string sceneName => _sceneName;
        public string scenePath => _scenePath;

        public int buildIndex => SceneUtility.GetBuildIndexByScenePath(_scenePath);


        public void LoadScene(LoadSceneMode mode = LoadSceneMode.Single)
        {
            SceneManager.LoadScene(buildIndex, mode);
        }


        public AsyncOperation LoadSceneAsync(LoadSceneMode mode = LoadSceneMode.Single)
        {
            return SceneManager.LoadSceneAsync(buildIndex, mode);
        }


        public AsyncOperation UnloadSceneAsync()
        {
            return SceneManager.UnloadSceneAsync(buildIndex);
        }
    }
}