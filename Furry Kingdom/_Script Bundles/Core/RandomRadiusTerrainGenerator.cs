using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TreeEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEditor.FilePathAttribute;
using static UnityEditor.PlayerSettings;

namespace Furry
{

    public class RandomRadiusTerrainGenerator : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _terrains;
        [SerializeField] private int _levelSize = 100;
        [SerializeField] private int _maxLevelHeight = 6;
        [SerializeField] private Vector2 _terrainOffsets = new Vector2(3f, .3f);
        [SerializeField] private Vector3 _seedPosition;
        Vector3 currentPos;

        private List<Vector2> _landPosList2 = new List<Vector2>();
        private List<Vector3> _landPosList3 = new List<Vector3>();
        private List<GameObject> _landObjectsList = new List<GameObject>();
        private Vector3 _playerSpawnOffset = new Vector3(0, 1, 0);
        private CancellationTokenSource _nextPosCTS = new CancellationTokenSource();

        private void Awake()
        {
            if (_seedPosition == null)
            {
                _seedPosition = transform.position;
            }
            currentPos = _seedPosition;
        }
        void Start()
        {
            CreateTerrain();
        }

        private async void CreateTerrain()
        {
            SpawnLand(currentPos);
            for (int i = 1; i < _levelSize; i++)
            {
                await NextPosition(_nextPosCTS.Token);
            }
        }

        private void AddVectorsToLists(Vector3 pos)
        {
            _landPosList3.Add(pos);
            Vector2 XYPosition = new Vector2(pos.x, pos.z);
            _landPosList2.Add(XYPosition);
        }

        private Vector3 GetLocationSwitch(Vector3 origin)
        {
            int direction = Random.Range(0, 16);
            Vector3 nextPos;
            switch (direction)
            {
                case 0:
                    nextPos = new Vector3(origin.x + _terrainOffsets.x, origin.y, origin.z);
                    return nextPos;
                case 1:
                    nextPos = new Vector3(origin.x + _terrainOffsets.x, origin.y + _terrainOffsets.y, origin.z);
                    return WithinHeightBounds(origin, nextPos);
                case 12:
                    nextPos = new Vector3(origin.x + _terrainOffsets.x, origin.y + _terrainOffsets.y, origin.z);
                    return WithinHeightBounds(origin, nextPos);
                case 2:
                    nextPos = new Vector3(origin.x + _terrainOffsets.x, origin.y - _terrainOffsets.y, origin.z);
                    return WithinHeightBounds(origin, nextPos);
                case 3:
                    nextPos = new Vector3(origin.x - _terrainOffsets.x, origin.y, origin.z);
                    return nextPos;
                case 4:
                    nextPos = new Vector3(origin.x - _terrainOffsets.x, origin.y + _terrainOffsets.y, origin.z);
                        return WithinHeightBounds(origin, nextPos);
                case 13:
                    nextPos = new Vector3(origin.x - _terrainOffsets.x, origin.y + _terrainOffsets.y, origin.z);
                    return WithinHeightBounds(origin, nextPos);
                case 5:
                    nextPos = new Vector3(origin.x - _terrainOffsets.x, origin.y - _terrainOffsets.y, origin.z);
                    return WithinHeightBounds(origin, nextPos);
                case 6:
                    nextPos = new Vector3(origin.x, origin.y, origin.z + _terrainOffsets.x);
                    return nextPos;
                case 7:
                    nextPos = new Vector3(origin.x, origin.y + _terrainOffsets.y, origin.z + _terrainOffsets.x);
                    return WithinHeightBounds(origin, nextPos);
                case 14:
                    nextPos = new Vector3(origin.x, origin.y + _terrainOffsets.y, origin.z + _terrainOffsets.x);
                    return WithinHeightBounds(origin, nextPos);
                case 8:
                    nextPos = new Vector3(origin.x, origin.y - _terrainOffsets.y, origin.z + _terrainOffsets.x);
                    return WithinHeightBounds(origin, nextPos);
                case 9:
                    nextPos = new Vector3(origin.x, origin.y, origin.z - _terrainOffsets.x);
                    return nextPos;
                case 10:
                    nextPos = new Vector3(origin.x, origin.y + _terrainOffsets.y, origin.z - _terrainOffsets.x);
                    return WithinHeightBounds(origin, nextPos);
                case 15:
                    nextPos = new Vector3(origin.x, origin.y + _terrainOffsets.y, origin.z - _terrainOffsets.x);
                    return WithinHeightBounds(origin, nextPos);
                case 11:
                    nextPos = new Vector3(origin.x, origin.y - _terrainOffsets.y, origin.z - _terrainOffsets.x);
                    return WithinHeightBounds(origin, nextPos);
                default:
                    return origin;
            }

        }

        private bool CheckPos(Vector3 posToCheck)
        {
            Vector2 checkPos = new Vector2(posToCheck.x, posToCheck.z);
            return _landPosList2.Contains(checkPos);
        }
        private Vector3 WithinHeightBounds(Vector3 origin, Vector3 potentialPos)
        {
            if (potentialPos.y <= _maxLevelHeight && potentialPos.y >= 0)
            {
                return potentialPos;
            }
            return origin;
        }
        
        private async Task NextPosition(CancellationToken ct)
        {
            bool foundAvailableArea = false;
            int iterations = 0;
            Vector3 potentialPos;
            while (!foundAvailableArea && !ct.IsCancellationRequested)
            {
                if (iterations > 6)
                {
                    int i = Random.Range(0, _landPosList3.Count);
                    iterations = 0;
                    potentialPos = GetLocationSwitch(_landPosList3[i]);
                }
                else
                {
                    potentialPos = GetLocationSwitch(currentPos);
                }

                if (!CheckPos(potentialPos))
                {
                    SpawnLand(potentialPos);
                    foundAvailableArea = true;
                }
                iterations++;
                await Task.Yield();
            }
        }
        

        private void SpawnLand(Vector3 pos)
        {
            GameObject land = Instantiate(_terrains[Random.Range(0, _terrains.Count)], pos, Quaternion.identity);
            _landObjectsList.Add(land);
            land.transform.SetParent(transform);
            currentPos = pos;
            AddVectorsToLists(pos);
        }

        private void OnDisable()
        {
            _nextPosCTS.Cancel();
        }
        private async void NextPosition(CancellationToken ct, Vector3 currentPos)
        {
            bool foundAvailableArea = false;
            Vector3 nextPos = new Vector3(0, 0, 0);
            while (!foundAvailableArea && !ct.IsCancellationRequested)
            {
                bool XAxis = Random.value > 0.5f;
                bool positive = Random.value > 0.5f;
                bool even = Random.value > 0.5f;
                bool high = Random.value > 0.5f;

                // The new position will be on the X axis;
                if (XAxis)
                {
                    Debug.Log("X Axis");
                    // The new position will be positive on the X axis;
                    if (positive)
                    {
                        Vector2 XYPosition = new Vector2(nextPos.x, nextPos.z);
                        Debug.Log("x positive");
                        if (_landPosList2.Contains(XYPosition) == false)
                        {
                            Debug.Log("not found");
                            _landPosList2.Add(XYPosition);
                            // The new position will be even on the Y axis;
                            if (even)
                            {
                                Debug.Log("even");
                                nextPos = new Vector3(currentPos.x + _terrainOffsets.x, currentPos.y, currentPos.z);
                            }
                            // The new position will be higher on the Y axis;
                            else if (high)
                            {
                                Debug.Log("high");
                                nextPos = new Vector3(currentPos.x + _terrainOffsets.x, currentPos.y + _terrainOffsets.y, currentPos.z);
                            }
                            // The new position will be lower on the Y axis;
                            else
                            {
                                Debug.Log("low");
                                nextPos = new Vector3(currentPos.x + _terrainOffsets.x, currentPos.y - _terrainOffsets.y, currentPos.z);
                            }
                            Debug.Log("low");
                            foundAvailableArea = true;
                        }
                    }
                    // The new position will be negative on the X axis;
                    else
                    {
                        Vector2 XYPosition = new Vector2(nextPos.x, nextPos.z);
                        Debug.Log("x negative");
                        if (_landPosList2.Contains(XYPosition) == false)
                        {
                            Debug.Log("not found");
                            _landPosList2.Add(XYPosition);
                            // The new position will be even on the Y axis;
                            if (even)
                            {
                                Debug.Log("even");
                                nextPos = new Vector3(currentPos.x - _terrainOffsets.x, currentPos.y, currentPos.z);
                            }
                            // The new position will be higher on the Y axis;
                            else if (high)
                            {
                                Debug.Log("high");
                                nextPos = new Vector3(currentPos.x - _terrainOffsets.x, currentPos.y + _terrainOffsets.y, currentPos.z);
                            }
                            // The new position will be lower on the Y axis;
                            else
                            {
                                Debug.Log("low");
                                nextPos = new Vector3(currentPos.x - _terrainOffsets.x, currentPos.y - _terrainOffsets.y, currentPos.z);
                            }
                            foundAvailableArea = true;
                        }
                    }
                }
                // The new position will be on the Z axis;
                else
                {
                    Debug.Log("z axis");
                    // The new position will be positive on the Z axis;
                    if (positive)
                    {
                        Vector2 XYPosition = new Vector2(nextPos.x, nextPos.z);
                        Debug.Log("z positive");
                        if (_landPosList2.Contains(XYPosition) == false)
                        {
                            Debug.Log("not found");
                            _landPosList2.Add(XYPosition);
                            // The new position will be even on the Y axis;
                            if (even)
                            {
                                Debug.Log("even");
                                nextPos = new Vector3(currentPos.x, currentPos.y, currentPos.z + _terrainOffsets.x);
                            }
                            // The new position will be higher on the Y axis;
                            else if (high)
                            {
                                Debug.Log("high");
                                nextPos = new Vector3(currentPos.x, currentPos.y + _terrainOffsets.y, currentPos.z + _terrainOffsets.x);
                            }
                            // The new position will be lower on the Y axis;
                            else
                            {
                                Debug.Log("low");
                                nextPos = new Vector3(currentPos.x, currentPos.y - _terrainOffsets.y, currentPos.z + _terrainOffsets.x);
                            }
                            foundAvailableArea = true;
                        }
                    }
                    // The new position will be negative on the Z axis;
                    else
                    {
                        Vector2 XYPosition = new Vector2(nextPos.x, nextPos.z);
                        Debug.Log("z negative");
                        if (_landPosList2.Contains(XYPosition) == false)
                        {
                            Debug.Log("not found");
                            _landPosList2.Add(XYPosition);
                            // The new position will be even on the Y axis;
                            if (currentPos.y >= _maxLevelHeight || currentPos.y <= 0 || even)
                            {
                                Debug.Log("even");
                                nextPos = new Vector3(currentPos.x, currentPos.y, currentPos.z - _terrainOffsets.x);
                            }
                            // The new position will be higher on the Y axis;
                            else if (currentPos.y < _maxLevelHeight && high)
                            {
                                Debug.Log("high");
                                nextPos = new Vector3(currentPos.x, currentPos.y + _terrainOffsets.y, currentPos.z - _terrainOffsets.x);
                            }
                            // The new position will be lower on the Y axis;
                            else if (currentPos.y > _maxLevelHeight)
                            {
                                Debug.Log("low");
                                nextPos = new Vector3(currentPos.x, currentPos.y - _terrainOffsets.y, currentPos.z - _terrainOffsets.x);
                            }
                            foundAvailableArea = true;
                        }
                    }
                }
                await Task.Yield();
            }
            Debug.Log("nextPos = " + nextPos);
            currentPos = nextPos;
            SpawnLand(currentPos);
        }

    }

}