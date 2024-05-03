using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.AI.Navigation;
using UnityEngine;

namespace Furry
{
    [RequireComponent(typeof(LevelTerrainTiles))]

    public class ProceduralLevelGenerator : MonoBehaviour
    {
        public static event Action OnLevelGenerated;
        public static event Action<List<GameObject>> OnTilesSet;

        [Header("Level fields.")]
        [Tooltip("Maximum amount of tiles in level.")]
        [SerializeField] private int _terrainTileAmount = 100;
        [Tooltip("Maximum height of tiles in level.")]
        [SerializeField] private int _maxLevelHeight = 12;
        [Tooltip("Maximum width of tiles in level on both the x and y axis.")]
        [SerializeField] private int _maxLevelHorizontal;
        [Tooltip("Maximum amount of water tiles in level.")]
        [SerializeField, Range(0, 1000)] private int _waterTiles = 20;
        [Tooltip("width and height of offests for tileplacement.")]
        [SerializeField] private Vector2 _terrainOffsets = new Vector2(3f, .3f);
        [Tooltip("Position where level should begin to be built from.")]
        [SerializeField] private Vector3 _seedPosition;

        private int _spawnedWater = 0;
        private LevelTerrainTiles _terrainTiles;
        private Vector3 _currentPos;
        private Vector3[] _generationDirections;
        private HashSet<Vector2> _levelGrid = new HashSet<Vector2>();
        private List<GameObject> _terrainObjects = new List<GameObject>();
        private CancellationTokenSource _nextPosCTS = new CancellationTokenSource();

        private void Awake()
        {
            if (_seedPosition == null)
            {
                _seedPosition = transform.position;
            }
            _currentPos = _seedPosition;
            _terrainTiles = GetComponent<LevelTerrainTiles>();

            _generationDirections = new Vector3[] {
              new Vector3(_terrainOffsets.x, 0, 0), new Vector3(_terrainOffsets.x, _terrainOffsets.y, 0), new Vector3(_terrainOffsets.x, -_terrainOffsets.y, 0)
            , new Vector3(-_terrainOffsets.x, 0, 0), new Vector3(-_terrainOffsets.x, _terrainOffsets.y, 0), new Vector3(-_terrainOffsets.x, -_terrainOffsets.y, 0)
            , new Vector3(0, 0, _terrainOffsets.x), new Vector3(0, _terrainOffsets.y, _terrainOffsets.x), new Vector3(0, -_terrainOffsets.y, _terrainOffsets.x)
            , new Vector3(0, 0, -_terrainOffsets.x), new Vector3(0, _terrainOffsets.y, -_terrainOffsets.x), new Vector3(0, -_terrainOffsets.y, -_terrainOffsets.x) };
        }
        void Start()
        {
            BuildLevel();
        }
        /// <summary>
        /// Build the entire level asyncronously.
        /// </summary>
        private async void BuildLevel()
        {
            SpawnLand(_currentPos, _terrainTiles.ChooseTerrainTile(_currentPos.y));
            for (int i = 1; i < _terrainTileAmount; i++)
            {
                await NextPosition(_nextPosCTS.Token);
            }
            await OutlineWater(_nextPosCTS.Token);
            await BakeNavMeshes();
            OnTilesSet?.Invoke(_terrainObjects);
            OnLevelGenerated?.Invoke();
        }

        /// <summary>
        /// Task to get the next position to place a tile.
        /// </summary>
        /// <param name="ct"></param> Cancellation token
        /// <returns></returns>
        private async Task NextPosition(CancellationToken ct)
        {
            bool foundAvailableArea = false;
            int iterations = 0;
            int tries = 0;
            Vector3 potentialPos;
            while (!foundAvailableArea && !ct.IsCancellationRequested)
            {
                if (tries > 20)
                {
                    _nextPosCTS.Cancel();
                }
                if (iterations < 3)
                {
                    potentialPos = _currentPos + _generationDirections[UnityEngine.Random.Range(0, 11)];
                }
                else
                {
                    iterations = 0;
                    _currentPos = _terrainObjects[UnityEngine.Random.Range(0, _terrainObjects.Count)].transform.position;
                    potentialPos = _currentPos + _generationDirections[UnityEngine.Random.Range(0, 11)];
                    tries++;
                }
                if (PositionAvailable(potentialPos))
                {
                    tries = 0;
                    SpawnLand(potentialPos, _terrainTiles.ChooseTerrainTile(potentialPos.y));
                    foundAvailableArea = true;
                }
                iterations++;
            }
            await Task.Yield();
        }

        /// <summary>
        /// Task to outline the terratin tiles with water tiles.
        /// </summary>
        /// <param name="ct"></param> Cancellation token.
        /// <returns></returns>
        private async Task OutlineWater(CancellationToken ct)
        {
            for (int i = 0; i < _waterTiles; i++)
            {
                if (_spawnedWater > _waterTiles)
                {
                    _nextPosCTS.Cancel();
                }
                WaterPosAvailable(CalculateWaterPlacement(_terrainObjects[i].transform.position, _generationDirections[0]));
                WaterPosAvailable(CalculateWaterPlacement(_terrainObjects[i].transform.position, _generationDirections[3]));
                WaterPosAvailable(CalculateWaterPlacement(_terrainObjects[i].transform.position, _generationDirections[6]));
                WaterPosAvailable(CalculateWaterPlacement(_terrainObjects[i].transform.position, _generationDirections[9]));
            }
            await Task.Yield();
        }

        /// <summary>
        /// If position is available, spawns a water tile.
        /// </summary>
        /// <param name="pos"></param> position to check.
        private void WaterPosAvailable(Vector3 pos)
        {
            if (PositionAvailable(pos))
            {
                SpawnLand(pos, (_terrainTiles.RandomWaterTile()));
                _spawnedWater++;
            }
        }

        /// <summary>
        /// Returns true if the position is available.
        /// </summary>
        /// <param name="nextPos"></param> Potisiton
        /// <returns></returns>
        private bool PositionAvailable(Vector3 nextPos)
        {
            Vector2 checkPos = new Vector2(nextPos.x, nextPos.z);
            if (!_levelGrid.Contains(checkPos))
            {
                if ((nextPos.y <= _maxLevelHeight && nextPos.y >= 0)
                    && (nextPos.x <= _maxLevelHorizontal && nextPos.x >= -_maxLevelHorizontal)
                    && (nextPos.z <= _maxLevelHorizontal && nextPos.z >= -_maxLevelHorizontal))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets Vector2 from Vector3 and adds it to a HashSet.
        /// </summary>
        /// <param name="pos"></param>
        private void AddVectorsToLists(Vector3 pos)
        {
            Vector2 XYPosition = new Vector2(pos.x, pos.z);
            _levelGrid.Add(XYPosition);
        }
        private void SpawnLand(Vector3 pos, GameObject tile)
        {
            GameObject land = Instantiate(tile, pos, Quaternion.Euler(0, (UnityEngine.Random.Range(0, 3) * 90), 0));
            land.transform.SetParent(transform);
            _terrainObjects.Add(land);
            AddVectorsToLists(pos);
            _currentPos = pos;
        }

        /// <summary>
        /// Task to bake navmesh surfaces
        /// </summary>
        /// <returns></returns>
        private async Task BakeNavMeshes()
        {
            NavMeshSurface[] navmeshSurfaces = GetComponents<NavMeshSurface>();
            foreach (var navMesh in navmeshSurfaces)
            {
                navMesh.BuildNavMesh();
            }
            await Task.Yield();
        }

        /// <summary>
        /// Calculates where the next water tile should be placed.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="addValue"></param>
        /// <returns></returns>
        private Vector3 CalculateWaterPlacement(Vector3 origin, Vector3 addValue)
        {
            return new Vector3(origin.x + addValue.x, 12, origin.z + addValue.z);
        }

        private void OnDisable()
        {
            _nextPosCTS.Cancel();
        }
    }
}