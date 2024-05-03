using UnityEngine;

namespace Furry
{
    [RequireComponent(typeof(BoxCollider))]
    public class DerenderSensor : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out RendererToggle rT))
            {
                rT.ActivateRenderers();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out RendererToggle rT))
            {
                rT.DeActivateRenderers();
            }
        }
    }

}