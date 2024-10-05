using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Util
{
    /// <summary>
    /// used to destroy unity objects from other threads. for example used by editormaterial finalizers
    /// </summary>
    public class MainThreadDestroyer : MonoBehaviour
    {
        private static readonly List<UnityEngine.Object> objects = new List<UnityEngine.Object>();

        public static void AddForDestruction(UnityEngine.Object obj)
        {
            lock (objects)
            {
                objects.Add(obj);
            }
        }

        private void Update()
        {
            lock (objects)
            {
                foreach (var obj in objects)
                {
                    Destroy(obj);
                }
            }
        }
    }
}
