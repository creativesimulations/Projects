using Furry;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Furry
{

public class AnimalSpawner : MonoBehaviour
{
        [SerializeField] private List<GameObject> _animalsToSpawn;
        [SerializeField, Range(0,1)] private float spawnPercentage = 0.5f;
        [SerializeField] private GameObject _flag;
        [SerializeField] private GameObject _animal;

        private void Awake()
    {
        ProceduralLevelGenerator.OnLevelGenerated += SpawnAnimal;
    }
    private void SpawnAnimal()
        {
            if (_animalsToSpawn.Count > 0 && Random.value < spawnPercentage)
                {
                    Vector3 pos = LevelBuildingUtilities.TestNewLocation(transform.position, 2000);
                _animal = Instantiate(_animalsToSpawn[Random.Range(0,_animalsToSpawn.Count)], pos, Quaternion.identity);
                }
    }
}
}