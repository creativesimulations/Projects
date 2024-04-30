using Furry;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.InputSystem;
using System;

public class PlayerManager : MonoBehaviour
{
    public static event Action<List<GameObject>> OnPlayersSet;

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private List<GameObject> _playerPrefabs;
    private int _numPlayers;

    public static List<PlayerInput> PlayerInputs {  get; private set; }
    private List<GameObject> PlayerObjects = new List<GameObject>();

    private CancellationTokenSource _cts = new CancellationTokenSource();

    private void Awake()
    {
        PlayerInputs = new List<PlayerInput>();
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
            GameObject playerPrefab = _playerPrefabs[i-1];
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
        OnPlayersSet?.Invoke(PlayerObjects);
    }
    private void SpawnPlayer(GameObject playerToSpawn, Vector3 pos, int playerNum)
    {
        Vector3 playerPos = Utilities.TestNewLocation(pos, 200);
        GameObject newPlayer = Instantiate(playerToSpawn, playerPos, Quaternion.identity, transform);
        PlayerObjects.Add(newPlayer);
    }

    void OnPlayerJoined(PlayerInput playerInput)
    {
        PlayerInputs.Add(playerInput);
    }
    void OnPlayerLeft(PlayerInput playerInput)
    {
        PlayerInputs.Remove(playerInput);
    }
}
