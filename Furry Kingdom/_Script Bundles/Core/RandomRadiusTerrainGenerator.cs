using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TreeEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEditor.FilePathAttribute;

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

        private HashSet<Vector2> _landPosList = new HashSet<Vector2>();
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

        private void CreateTerrain()
        {
            SpawnLand(currentPos);
            Vector2 XYPosition = new Vector2(currentPos.x, currentPos.z);
            _landPosList.Add(XYPosition);
            for (int i = 1; i < _levelSize; i++)
            {
                NextPosition(_nextPosCTS.Token, currentPos);
            }
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
                        if (_landPosList.Contains(XYPosition) == false)
                        {
                            Debug.Log("not found");
                            _landPosList.Add(XYPosition);
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
                        if (_landPosList.Contains(XYPosition) == false)
                        {
                            Debug.Log("not found");
                            _landPosList.Add(XYPosition);
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
                        if (_landPosList.Contains(XYPosition) == false)
                        {
                            Debug.Log("not found");
                            _landPosList.Add(XYPosition);
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
                        if (_landPosList.Contains(XYPosition) == false)
                        {
                            Debug.Log("not found");
                            _landPosList.Add(XYPosition);
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


        /*
        private Vector3 NextPosition(Vector3 currentPos)
        {
            bool foundAvailableArea = false;
            Vector3 nextPos = new Vector3(0,0,0);
            int iterations = 0;

            while (!foundAvailableArea && iterations < 12)
            {
                Debug.Log("Iteration # " + iterations);
                iterations++;
                bool XAxis = Random.value > 0.5f;
                bool positive = Random.value > 0.5f;
                bool even = Random.value > 0.5f;
                bool high = Random.value > 0.5f;

                // The new position will be on the X axis;
                if (XAxis)
                {
                    // The new position will be positive on the X axis;
                    if (positive)
                    {
                        // The new position will be even on the Y axis;
                        if (even)
                        {
                            nextPos = new Vector3(currentPos.x + _terrainOffsets.x, currentPos.y, currentPos.z);
                        }
                        // The new position will be higher on the Y axis;
                        else if (high)
                        {
                            nextPos = new Vector3(currentPos.x + _terrainOffsets.x, currentPos.y + _terrainOffsets.y, currentPos.z);
                        }
                        // The new position will be lower on the Y axis;
                        else
                        {
                            nextPos = new Vector3(currentPos.x + _terrainOffsets.x, currentPos.y - _terrainOffsets.y, currentPos.z);
                        }
                    }
                    // The new position will be negative on the X axis;
                    else
                    {
                        // The new position will be even on the Y axis;
                        if (even)
                        {
                            nextPos = new Vector3(currentPos.x - _terrainOffsets.x, currentPos.y, currentPos.z);
                        }
                        // The new position will be higher on the Y axis;
                        else if (high)
                        {
                            nextPos = new Vector3(currentPos.x - _terrainOffsets.x, currentPos.y + _terrainOffsets.y, currentPos.z);
                        }
                        // The new position will be lower on the Y axis;
                        else
                        {
                            nextPos = new Vector3(currentPos.x - _terrainOffsets.x, currentPos.y - _terrainOffsets.y, currentPos.z);
                        }
                    }
                }
                // The new position will be on the Z axis;
                else
                {
                    // The new position will be positive on the Z axis;
                    if (positive)
                    {
                        // The new position will be even on the Y axis;
                        if (even)
                        {
                            nextPos = new Vector3(currentPos.x, currentPos.y, currentPos.z + _terrainOffsets.x);
                        }
                        // The new position will be higher on the Y axis;
                        else if (high)
                        {
                            nextPos = new Vector3(currentPos.x, currentPos.y + _terrainOffsets.y, currentPos.z + _terrainOffsets.x);
                        }
                        // The new position will be lower on the Y axis;
                        else
                        {
                            nextPos = new Vector3(currentPos.x, currentPos.y - _terrainOffsets.y, currentPos.z + _terrainOffsets.x);
                        }
                    }
                    // The new position will be negative on the Z axis;
                    else
                    {
                        // The new position will be even on the Y axis;
                        if (currentPos.y >= _maxLevelHeight || currentPos.y <= 0 || even)
                        {
                            nextPos = new Vector3(currentPos.x, currentPos.y, currentPos.z - _terrainOffsets.x);
                        }
                        // The new position will be higher on the Y axis;
                        else if (currentPos.y < _maxLevelHeight && high)
                        {
                            nextPos = new Vector3(currentPos.x, currentPos.y + _terrainOffsets.y, currentPos.z - _terrainOffsets.x);
                        }
                        // The new position will be lower on the Y axis;
                        else if (currentPos.y > _maxLevelHeight)
                        {
                            nextPos = new Vector3(currentPos.x, currentPos.y - _terrainOffsets.y, currentPos.z - _terrainOffsets.x);
                        }
                        else
                        {
                            nextPos = NextPosition(currentPos);
                        }
                    }
                }
                Vector2 XYPosition =  new Vector2(nextPos.x, nextPos.z);
                if (_landPosList.Contains(XYPosition))
                {
                    nextPos = NextPosition(currentPos);
                }
                else
                {
                    foundAvailableArea = true;
                }
            }
            Debug.Log("nextPos = " + nextPos);
            return nextPos;
        }
        */


        private void SpawnLand(Vector3 pos)
        {
            Vector2 currentPos = new Vector2(pos.x, pos.z);
            _landPosList.Add(currentPos);
            GameObject land = Instantiate(_terrains[Random.Range(0, _terrains.Count)], pos, Quaternion.identity);
            _landObjectsList.Add(land);
            land.transform.SetParent(transform);
        }

        private void OnDisable()
        {
            _nextPosCTS.Cancel();
        }
    }

}