using UnityEngine;
using UnityEngine.Events;

namespace Ludocore.RuntimeBackup
{
    public class LifecycleTrigger : MonoBehaviour
    {
        public UnityEvent onStart;

        void Start()
        {
            onStart.Invoke();
        }
    }
}
