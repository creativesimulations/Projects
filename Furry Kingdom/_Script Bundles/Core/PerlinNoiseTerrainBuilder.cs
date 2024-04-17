using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Furry
{

    public class PerlinNoiseTerrainBuilder : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _lands;
        [SerializeField] private Vector3 _levelSize = new Vector3(0,0,0);
        [SerializeField] private Vector2 _gridOffsets = new Vector2(0, 0);
        [SerializeField, Range(1,30)] private int _mountainLimit = 1;

        private int[,,] _grid;
        private List<Vector3> _landPosList = new List<Vector3>();
        private List<GameObject> _landObjectsList = new List<GameObject>();

        private Vector3 _playerSpawnOffset = new Vector3(0,1,0);

        private void Awake()
        {
            _grid = new int[(int)_levelSize.x, (int)_levelSize.y, (int)_levelSize.z]; // The size should be random for large, medium and small sizes. There should be a specific number of top land pieces.
        }
        void Start()
        {
            LoopThroughGrid();
        }


        private void SpawnLands(Vector3 pos)
        {
            _landPosList.Add(pos);
            GameObject land = Instantiate(_lands[Random.Range(0, _lands.Count)], pos, Quaternion.identity);
            _landObjectsList.Add(land);
            land.transform.SetParent(transform);
        }

        private void LoopThroughGrid()
        {
                // width - starts from the left
                for (int x = 0; x < _grid.GetLength(0); x++)
                {
                    // depth - starts from the back
                    for (int z = 0; z < _grid.GetLength(2); z++)
                    {
                        Vector3 landPos = new Vector3(x * _gridOffsets.x, generateNoise(x, z, _levelSize.y) * _gridOffsets.y, z * _gridOffsets.x);
                    SpawnLands(landPos);
                }
                }
            SpawnPlayers();
        }
        private float generateNoise(int x, int z, float detailScale)
        {
            float xNoise = (x + transform.position.x) / detailScale;
            float zNoise = (z + transform.position.y) / detailScale;
            return Mathf.PerlinNoise(xNoise, zNoise);
        }
        private GameObject DecideLandPiece()
        {
            return null; //Should return the type of land to place from the serialized list _lands.
        }

        private void SpawnPlayers()
        {
            for (int i = 0; i < 100; i++)
            {
                Debug.Log("Player " + i + " should be spawned in random places.");
                PlayerSpawnLocation();
            }
        }
        private Vector3 PlayerSpawnLocation()
        {
            Vector3 playerSpawnPoint = _landPosList[Random.Range(0, _landPosList.Count)] + _playerSpawnOffset;
            if (IsObstacleFree(playerSpawnPoint))
            {
                return playerSpawnPoint;
            }
            else
            {
                return PlayerSpawnLocation(); // will this work???
            }
        }
        private bool IsObstacleFree(Vector3 location)
        {
            // Check that the spot is clear of obstacles.
            return true;
        }
    }

}