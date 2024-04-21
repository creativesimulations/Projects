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

        [SerializeField] private int _terrainTileAmount = 100;
        [SerializeField] private int _maxLevelHeight = 12;
        [SerializeField] private int _maxLevelHorizontal;
        [SerializeField] private Vector2 _terrainOffsets = new Vector2(3f, .3f);
        [SerializeField] private Vector3 _seedPosition;

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
            await FillWater(_nextPosCTS.Token);

            NavMeshSurface[] _navmeshSurfaces = GetComponents<NavMeshSurface>();
            LevelBuildingUtilities.BakeNavMeshes(_navmeshSurfaces);
            // choose two random tiles and spawn the players
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
                if (tries > 10)
                {
                    _nextPosCTS.Cancel();
                }
                if (iterations < 2)
                {
                    potentialPos = _currentPos + _generationDirections[UnityEngine.Random.Range(0, 11)];
                }
                else
                {
                    iterations = 0;
                    _currentPos = _terrainObjects[UnityEngine.Random.Range(0, _terrainObjects.Count)].transform.position + _generationDirections[UnityEngine.Random.Range(0, 11)];
                    potentialPos = _currentPos + _generationDirections[UnityEngine.Random.Range(0, 11)];
                    tries++;
                }
                if (PositionAvailable(potentialPos))
                {
                    SpawnLand(potentialPos, _terrainTiles.ChooseTerrainTile(potentialPos.y));
                    foundAvailableArea = true;
                }
                iterations++;
                await Task.Yield();
            }
        }
        private async Task FillWater(CancellationToken ct)
        {
            for (int i = 0; i < _terrainObjects.Count; i++)
            {
                Vector3 positiveX = new Vector3(_terrainObjects[i].transform.position.x + _generationDirections[0].x, 0, _terrainObjects[i].transform.position.z + +_generationDirections[0].z);
                Vector3 negativeX = new Vector3(_terrainObjects[i].transform.position.x + _generationDirections[3].x, 0, _terrainObjects[i].transform.position.z + +_generationDirections[3].z);
                Vector3 positiveZ = new Vector3(_terrainObjects[i].transform.position.x + _generationDirections[6].x, 0, _terrainObjects[i].transform.position.z + +_generationDirections[6].z);
                Vector3 negativeZ = new Vector3(_terrainObjects[i].transform.position.x + _generationDirections[9].x, 0, _terrainObjects[i].transform.position.z + +_generationDirections[9].z);
                WaterPosAvailable(positiveX);
                WaterPosAvailable(negativeX);
                WaterPosAvailable(positiveZ);
                WaterPosAvailable(negativeZ);
                await Task.Yield();
            }
        }
        private void WaterPosAvailable(Vector3 pos)
        {
            if (PositionAvailable(pos))
            {
                GameObject land = Instantiate(_terrainTiles.ChooseTerrainTile(0), pos, Quaternion.Euler(0, (UnityEngine.Random.Range(0, 3) * 90), 0));
                land.transform.SetParent(transform);
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
        private void OnDisable()
        {
            _nextPosCTS.Cancel();
        }
    }
}