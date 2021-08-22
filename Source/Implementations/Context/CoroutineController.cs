using System.Collections;
using UnityEngine;

namespace VoxCake.Framework
{
    internal class CoroutineController : ICoroutineController
    {
        private readonly Context _context;

        public CoroutineController(Context context)
        {
            _context = context;
        }

        public Coroutine StartCoroutine(IEnumerator enumerator)
        {
            return _context.StartCoroutine(enumerator);
        }

        public void StopCoroutine(Coroutine coroutine)
        {
            _context.StopCoroutine(coroutine);
        }
    }
}