using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Furry
{ 
public class MainMenu : MonoBehaviour
{
        public static event Action<int> OnChoosePlayerNum;

        private Canvas _canvas;
        private void Awake()
        {
            _canvas = GetComponentInChildren<Canvas>();
        }
        void Start()
    {
    }

    void Update()
    {
        
    }
        public void ChoosePlayer(int numPlayers)
        {
            OnChoosePlayerNum?.Invoke(numPlayers);
            DisableCanvas();
            LoadLevel();
         //   StartCoroutine(FadeCanvas());
        }


        /*
        private IEnumerator FadeCanvas()
        {
            while (_canvasGroup.alpha > 0)
            {
                _canvasGroup.alpha -= .1f;
                yield return new WaitForEndOfFrame();
            }
            LoadLevel();
        }
        */

        private void DisableCanvas()
        {
            _canvas.gameObject.SetActive(false);
        }
        private void EnableCanvas()
        {
            _canvas.gameObject.SetActive(true);
        }
        private void LoadLevel()
        {
            SceneManager.LoadSceneAsync("GameLevel", LoadSceneMode.Additive);
        }
}

}