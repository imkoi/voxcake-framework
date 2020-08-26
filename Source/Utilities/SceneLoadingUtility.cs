using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using VoxCake.Common.Utilities;

namespace VoxCake.Framework.Utilities
{
    internal static class SceneLoadingUtility
    {
        internal static async Task LoadScene(string sceneName, Stopwatch sw, int maxTaskFreezeMs,
            CancellationToken cancellationToken)
        {
            var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            await Awaiter.ReduceTaskFreezeAsync(sw, maxTaskFreezeMs, cancellationToken);
            
            while (!operation.isDone)
            {
                await Awaiter.WaitMs(cancellationToken);
            }

            var scene = SceneManager.GetSceneByName(sceneName);
            SceneManager.SetActiveScene(scene);
            await Awaiter.ReduceTaskFreezeAsync(sw, maxTaskFreezeMs, cancellationToken);
        }

        internal static async Task UnloadScenes(string[] unloadScenes, Stopwatch sw, int maxTaskFreezeMs,
            CancellationToken cancellationToken)
        {
            var unloadCount = unloadScenes.Length;
            for (var i = 0; i < unloadCount; i++)
            {
                await UnloadScene(unloadScenes[i], sw, maxTaskFreezeMs, cancellationToken);
            }
        }

        private static async Task UnloadScene(string sceneName, Stopwatch sw, int maxTaskFreezeMs,
            CancellationToken cancellationToken)
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            var operation = SceneManager.UnloadSceneAsync(scene);
            await Awaiter.ReduceTaskFreezeAsync(sw, maxTaskFreezeMs, cancellationToken);
            
            while (!operation.isDone)
            {
                await Awaiter.WaitMs(cancellationToken);
            }
        }
    }
}