using System.Collections;
using System.Collections.Generic;
using VoxCake.Framework.Utilities;

namespace VoxCake.Framework
{
    public class SceneUtility
    {
        private const string LOAD_SCENE_FAIL_MESSAGE =
            "Failed to load scene \"{0}\", because its null or empty!";
        private const string UNLOAD_SCENES_FAIL_MESSAGE =
            "Failed to unload scenes, because scenes collection is null!";
        
        public static IEnumerator LoadScene(string sceneName, IEnumerable<string> unloadSceneNames = null)
        {
            var shouldUnloadScenes = unloadSceneNames != null;
            var shouldLoadScene = !string.IsNullOrEmpty(sceneName);

            if (shouldLoadScene)
            {
                if (shouldUnloadScenes)
                {
                    yield return UnloadScenes(unloadSceneNames);
                }

                yield return SceneLoadingUtility.LoadScene(sceneName);
            }
            else
            {
                throw new FrameworkException(string.Format(LOAD_SCENE_FAIL_MESSAGE, sceneName));
            }
        }

        public static IEnumerator UnloadScenes(IEnumerable<string> unloadSceneNames)
        {
            var shouldUnloadScenes = unloadSceneNames != null;
            
            if (shouldUnloadScenes)
            {
                yield return SceneLoadingUtility.UnloadScenes(unloadSceneNames);
            }
            else
            {
                throw new FrameworkException(UNLOAD_SCENES_FAIL_MESSAGE);
            }
        }
    }
}