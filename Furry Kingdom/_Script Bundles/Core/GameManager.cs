using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private void Awake()
    {
#if (Unity_EDITOR)
        {
            Debug.unityLogger.logEnabled = true;
        }
#else
        {
            Debug.unityLogger.logEnabled = false;
        }
#endif
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    async void SpawnPlayer()
    {

       // GameObject player = await AssetDatabase.LoadAssetAsync("Assets/Player.asset");
       // GameObject playerBody = new GameObject(playerBody);
       // await playerBody.transform.position = new Vector3(1, 5, 4);

    }
}
