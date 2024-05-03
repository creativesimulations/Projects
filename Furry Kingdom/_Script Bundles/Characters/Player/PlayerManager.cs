using Furry;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.InputSystem;
using System;

public class PlayerManager : MonoBehaviour
{
    /// <summary>
    /// Called after players have been spawned.
    /// </summary>
    public static event Action<List<GameObject>> OnPlayersSet;

    [Tooltip("Player Prefabs.")]
    [SerializeField] private List<GameObject> _playerPrefabs;

    public static List<PlayerInput> _playerInputs { get; private set; }

    private int _numPlayers;
    private List<GameObject> _playerObjects = new List<GameObject>();
    private CancellationTokenSource _cts = new CancellationTokenSource();

    private void Awake()
    {
        _playerInputs = new List<PlayerInput>();
    }

    void Start()
    {
        MainMenu.OnChoosePlayerNum += SetNumPlayers;
        ProceduralLevelGenerator.OnTilesSet += PlacePlayers;
    }

    /// <summary>
    /// Sets the number of players that need to be in the game.
    /// </summary>
    /// <param name="num"></param>
    private void SetNumPlayers(int num)
    {
        _numPlayers = num;
    }

    /// <summary>
    /// Instantiates each of the player prefabs on a random tile in the game that does not have an animal spawned on it.
    /// </summary>
    /// <param name="tiles"></param> List of tiles to spawn the players on.
    private async void PlacePlayers(List<GameObject> tiles)
    {
        CancellationToken ct = _cts.Token;
        for (int i = 1; i <= _numPlayers; i++)
        {
            GameObject playerPrefab = _playerPrefabs[i - 1];
            bool placed = false;
            while (!placed && !ct.IsCancellationRequested)
            {
                TileManager tM = tiles[UnityEngine.Random.Range(0, tiles.Count)].GetComponent<TileManager>();
                if (tM != null && tM.HasAnimal() == false)
                {
                    SpawnPlayer(playerPrefab, tM.gameObject.transform.position);
                    placed = true;
                }
                await Task.Yield();
            }
        }
        OnPlayersSet?.Invoke(_playerObjects);
    }

    /// <summary>
    /// Instantiates the player at a location.
    /// </summary>
    /// <param name="playerToSpawn"></param> Prefab to spawn from.
    /// <param name="pos"></param> Location to spawn at.
    private void SpawnPlayer(GameObject playerToSpawn, Vector3 pos)
    {
        Vector3 playerPos = Utilities.TestNewLocation(pos, 200);
        GameObject newPlayer = Instantiate(playerToSpawn, playerPos, Quaternion.identity, transform);
        _playerObjects.Add(newPlayer);
    }

    /// <summary>
    /// Adds the newly joined player's input to the input list.
    /// </summary>
    /// <param name="playerInput"></param> player's input
    void OnPlayerJoined(PlayerInput playerInput)
    {
        _playerInputs.Add(playerInput);
    }

    /// <summary>
    /// Removes the newly left player's input to the input list.
    /// </summary>
    /// <param name="playerInput"></param> player's input
    void OnPlayerLeft(PlayerInput playerInput)
    {
        _playerInputs.Remove(playerInput);
    }
}
