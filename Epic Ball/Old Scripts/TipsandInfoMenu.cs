using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TipsandInfoMenu : MonoBehaviour
{
    public void BackOnClick()
    {
        SceneManager.LoadSceneAsync(GlobalConstants.MAIN_MENU, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync (GlobalConstants.TIPS_MENU);
    }

}
