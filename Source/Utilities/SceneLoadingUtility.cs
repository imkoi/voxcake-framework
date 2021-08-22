using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace VoxCake.Framework.Utilities
{
    internal static class SceneLoadingUtility
    {
        private const string UNLOAD_SCENE_FAIL_MESSAGE =
            "Failed to unload scene \"{0}\", because its null or empty!";
        
        internal static IEnumerator LoadScene(string sceneName)
        {
            var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            yield return operation;

            var scene = SceneManager.GetSceneByName(sceneName);
            SceneManager.SetActiveScene(scene);
        }

        internal static IEnumerator UnloadScenes(IEnumerable<string> unloadScenes)
        {
            foreach (var unloadScene in unloadScenes)
            {
                yield return UnloadScene(unloadScene);
            }
        }

        private static IEnumerator UnloadScene(string sceneName)
        {
            var isSceneUnloadable = !string.IsNullOrEmpty(sceneName);

            if (isSceneUnloadable)
            {
                var scene = SceneManager.GetSceneByName(sceneName);
                var operation = SceneManager.UnloadSceneAsync(scene);

                yield return operation;
            }
            else
            {
                throw new FrameworkException(string.Format(UNLOAD_SCENE_FAIL_MESSAGE, sceneName));
            }
        }
    }
}