using UnityEngine;

namespace EpicBall
{
    public class Singleton : MonoBehaviour
    {

        public static Singleton instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                // Render the game up to 60 fps.
                Application.targetFrameRate = 60;
            }
        }

    }
}