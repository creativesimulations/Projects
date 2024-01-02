using Core;
using UnityEngine;

namespace EpicBall
{

    public class PushToPool : MonoBehaviour
    {
        /// <summary>
        /// This script is placed on a particle effect to be used in a callback to put the particle game object back into a pool.
        /// </summary>

        private Transform parent;
        private ObjectPooler _pooler;

        private void Start()
        {
            ParticleSystem.MainModule main = GetComponent<ParticleSystem>().main;
            main.stopAction = ParticleSystemStopAction.Callback;
            parent = gameObject.transform.parent;
            _pooler = Singleton.instance.GetComponent<ObjectPooler>();
        }

        private void OnParticleSystemStopped()
        {
            if (_pooler != null)
            {
                _pooler.PushToPool(gameObject, true, parent);
            }
            else
            {
                ExceptionManager.instance.SendMissingComponentMessage("ObjectPooler", GetType().ToString(), name);
            }
        }
    }
}
