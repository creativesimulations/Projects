using UnityEngine;

namespace EpicBall
{
    public class Timer : MonoBehaviour
    {

        private static float _timeStart;
        private bool _timerActive = false;
        private int _currentTime;


        void Start()
        {
            GameManager.PlayGame += StartTimer;
            GameManager.CompleteLvl += SaveTime;
        }

        void Update()
        {
            if (_timerActive)
            {
                _timeStart += Time.deltaTime;
            }
        }

        /// <summary>
        /// Sets the timer to 0 and the timer to 'active'.
        /// </summary>
        private void StartTimer()
        {
            _timeStart = 0;
            _timerActive = true;
        }

        /// <summary>
        /// Returns an int of the float timer.
        /// </summary>
        /// <returns></returns>
        public static int GetIntTime()
        {
            return (int)_timeStart;
        }

        /// <summary>
        /// Deactivates the timer and sets the '_currentTime' to '_timeStart'.  
        /// </summary>
        private void SaveTime()
        {
            _timerActive = false;
            _currentTime = GetIntTime();
        }

        /// <summary>
        /// Returns a string of the '_currentTime'.
        /// </summary>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static string StringTime(int endTime)
        {
            return
            Mathf.Floor(endTime / 60).ToString("00") + ":" + Mathf.FloorToInt(endTime % 60).ToString("00");
        }

        private void OnDisable()
        {
            GameManager.PlayGame -= StartTimer;
            GameManager.CompleteLvl -= SaveTime;
        }
    }
}