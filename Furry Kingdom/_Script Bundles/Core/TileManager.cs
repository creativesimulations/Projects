using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Furry
{

    public class TileManager : MonoBehaviour
    {

        [SerializeField] private List<GameObject> _animalsToSpawn;
        [SerializeField, Range(0, 1)] private float spawnPercentage = 0.5f;
        [SerializeField] private GameObject _flag;
        [SerializeField] private GameObject _tileAnimal;

        private bool[] _inSight = new bool[2];

        private void Awake()
        {
            ProceduralLevelGenerator.OnLevelGenerated += Spawn;
        }
        private void Spawn()
        {
            if (_tileAnimal == null && _animalsToSpawn.Count > 0 && Random.value < spawnPercentage)
            {
                SpawnAnimal(_animalsToSpawn[Random.Range(0, _animalsToSpawn.Count)]);
            }
            else
            {
                StartCoroutine(WaitToSpawn());
            }
        }
        private IEnumerator WaitToSpawn()
        {
            var secondsTillSpawn = new WaitForSecondsRealtime(Random.Range(10,20));

            yield return secondsTillSpawn;
        }
                private void SpawnAnimal(GameObject animalToSpawn)
        {
            Vector3 pos = Utilities.TestNewLocation(transform.position, 200);
            _tileAnimal = Instantiate(animalToSpawn, pos, Quaternion.identity);
        }

        public bool HasAnimal()
        {
            bool value = false;
            if (_tileAnimal != null)
            {
                value = true;
            }
            return value;
        }

        public void ActivateAnimal()
        {
            if (_inSight[0] == true)
            {
                _inSight[1] = true;
            }
            else
            {
                _inSight[0] = true;
                if (_tileAnimal != null)
                {
                    _tileAnimal.SetActive(true);
                }
            }
        }
        public void DeactivateAnimal()
        {
            if (_inSight[1] == true)
            {
                _inSight[1] = false;
            }
            else
            {
                _inSight[0] = false;
                if (_tileAnimal != null)
                {
                    _tileAnimal.SetActive(false);
                }
            }
        }
        private void KillAnimal()
        {
            Destroy(_tileAnimal);
            _tileAnimal = null;
            // set a timer on game manager to spawn a new animal on a random tile.
        }
    }
}