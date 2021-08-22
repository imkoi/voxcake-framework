using System.Collections;
using UnityEngine;

namespace VoxCake.Framework
{
    public interface ICoroutineController
    {
        Coroutine StartCoroutine(IEnumerator enumerator);
        void StopCoroutine(Coroutine coroutine);
    }
}