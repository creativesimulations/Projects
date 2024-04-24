using Furry;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using static UnityEditor.FilePathAttribute;
using UnityEngine.AI;
using Cinemachine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _playerPrefabs;
    [SerializeField] private List<LayerMask> _playerLayerMask;

    private Player[] _players;
    private int _maxPlayers = 2;

    private CancellationTokenSource _cts = new CancellationTokenSource();

    void Start()
    {
        ProceduralLevelGenerator.OnTilesSet += PlacePlayers;
    }

    private async void PlacePlayers(List<GameObject> tiles)
    {
        CancellationToken ct = _cts.Token;
        for (int i = 0; i < _maxPlayers; i++)
        {
            GameObject playerPrefab = _playerPrefabs[i].gameObject;
            bool placed = false;
            while (!placed && !ct.IsCancellationRequested)
            {
                TileManager tM = tiles[Random.Range(0, tiles.Count)].GetComponent<TileManager>();
                if (tM != null && tM.HasAnimal() == false)
                {
                    SpawnPlayer(playerPrefab, tM.gameObject.transform.position);
                    placed = true;
                }
                await Task.Yield();
            }
        }
    }
    private void SpawnPlayer(GameObject playerToSpawn, Vector3 pos)
    {
        Vector3 playerPos = Utilities.TestNewLocation(pos, 200);
        Instantiate(playerToSpawn, playerPos, Quaternion.identity, transform);
    }
}
