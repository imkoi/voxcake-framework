using System;
using UnityEngine;

namespace VoxCake.Framework
{
    public class View : MonoBehaviour
    {
        protected event Action Created;
        protected event Action Destroyed;
        
        private void Start()
        {
            var viewContext = GetComponentInParent<Context>();
            viewContext.AddView(this);

            if (Created != null)
            {
                Created.Invoke();
            }
        }

        private void OnDestroy()
        {
            var viewContext = GetComponentInParent<Context>();
            viewContext.RemoveView(this);

            if (Destroyed != null)
            {
                Destroyed.Invoke();
            }
        }
    }
}