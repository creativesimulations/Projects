using UnityEngine;
namespace EpicBall
{

    public class SplashScreenBall : MonoBehaviour
    {
        [Tooltip("The list of all of the player skins.")]
        [SerializeField] GameObject[] _skins;
        // Start is called before the first frame update
        void Start()
        {
            SetSkin();
            BallSkinController.ChoseSkin += SetSkin;
        }

        /// <summary>
        /// Sets the chosen skin on the player bal in the background.
        /// </summary>
        private void SetSkin()
        {
            if (_skins.Length > 0)
            {
                GameObject _currentSkin = GetComponentInChildren<Skin>().gameObject;
                Vector3 currentPosition = _currentSkin.transform.position;
                Destroy(_currentSkin);
                _currentSkin = Instantiate(_skins[PlayerPrefsController.GetChosenSkin()], currentPosition, Quaternion.identity, gameObject.transform);

            }
            else
            {
                ExceptionManager.instance.SendEmptyContainerMessage("_skins", GetType().ToString(), name);
            }
        }

        private void OnDisable()
        {
            BallSkinController.ChoseSkin -= SetSkin;
        }
    }

}
