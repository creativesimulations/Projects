using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] public Camera Camera;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

    }

    void Start()
    {
        if (Camera == null)
        {
            FindObjectOfType<Camera>();
        }
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
