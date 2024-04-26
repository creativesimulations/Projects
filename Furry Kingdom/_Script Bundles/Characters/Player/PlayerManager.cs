using Furry;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.InputSystem;
using System;

public class PlayerManager : MonoBehaviour
{
    public static event Action<string> OnJoin;

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private List<LayerMask> _playerLayerMask;
    private int _numPlayers;

    public static List<PlayerInput> Players {  get; private set; }

    private CancellationTokenSource _cts = new CancellationTokenSource();

    private void Awake()
    {
        Players = new List<PlayerInput>();
    }

    void Start()
    {
        MainMenu.OnChoosePlayerNum += SetNumPlayers;
        ProceduralLevelGenerator.OnTilesSet += PlacePlayers;
    }
    private void SetNumPlayers(int num)
    {
        _numPlayers = num;
    }
    private async void PlacePlayers(List<GameObject> tiles)
    {
        CancellationToken ct = _cts.Token;
        for (int i = 1; i <= _numPlayers; i++)
        {
            GameObject playerPrefab = _playerPrefab;
            bool placed = false;
            while (!placed && !ct.IsCancellationRequested)
            {
                TileManager tM = tiles[UnityEngine.Random.Range(0, tiles.Count)].GetComponent<TileManager>();
                if (tM != null && tM.HasAnimal() == false)
                {
                    SpawnPlayer(playerPrefab, tM.gameObject.transform.position, i);
                    placed = true;
                }
                await Task.Yield();
            }
        }
    }
    private void SpawnPlayer(GameObject playerToSpawn, Vector3 pos, int playerNum)
    {
        Vector3 playerPos = Utilities.TestNewLocation(pos, 200);
        Instantiate(playerToSpawn, playerPos, Quaternion.identity, transform);
    }

    void OnPlayerJoined(PlayerInput playerInput)
    {
        Players.Add(playerInput);
    }
    void OnPlayerLeft(PlayerInput playerInput)
    {
        Players.Remove(playerInput);
    }
}
