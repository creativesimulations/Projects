using UnityEngine;

namespace Furry
{

    public class RendererToggle : MonoBehaviour
    {

        private bool _isRendering = true;
        private MeshRenderer[] _renderers;

        private int _camerasRendering = 0;

        void Start()
        {
            _renderers = GetComponentsInChildren<MeshRenderer>();
        }

        /// <summary>
        /// Turns on the renderers that are on this object.
        /// </summary>
        public void ActivateRenderers()
        {
            _camerasRendering++;
            if (!_isRendering)
            {
                foreach (var renderer in _renderers)
                {
                    renderer.enabled = true;
                }
                _isRendering = true;
            }
        }

        /// <summary>
        /// Turns off the renderers that are on this object.
        /// </summary>
        public void DeActivateRenderers()
        {
            if (_camerasRendering > 0)
            {
                _camerasRendering--;
            }
            if (_isRendering && _camerasRendering == 0)
            {
                foreach (var renderer in _renderers)
                {
                    renderer.enabled = false;
                }
                _isRendering = false;
            }
        }
    }

}