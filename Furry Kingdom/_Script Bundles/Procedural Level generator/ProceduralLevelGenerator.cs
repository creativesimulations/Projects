using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.PlayerSettings;

namespace Furry
{
    [RequireComponent(typeof(LevelTerrainTiles))]

    public class ProceduralLevelGenerator : MonoBehaviour
    {
        public static event Action OnLevelGenerated;
        public static event Action<List<GameObject>> OnTilesSet;

        [SerializeField] private int _terrainTileAmount = 100;
        [SerializeField] private int _maxLevelHeight = 12;
        [SerializeField] private int _maxLevelHorizontal;
        [SerializeField, Range(0,1000)] private int _waterTiles = 20;
        [SerializeField] private Vector2 _terrainOffsets = new Vector2(3f, .3f);
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
        private void WaterPosAvailable(Vector3 pos)
        {
            if (PositionAvailable(pos))
            {
                SpawnLand(pos, (_terrainTiles.RandomWaterTile()));
                _spawnedWater++;
            }
        }
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
        private async Task BakeNavMeshes()
        {
            NavMeshSurface[] navmeshSurfaces = GetComponents<NavMeshSurface>();
            foreach (var navMesh in navmeshSurfaces)
            {
                navMesh.BuildNavMesh();
            }
            await Task.Yield();
        }
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