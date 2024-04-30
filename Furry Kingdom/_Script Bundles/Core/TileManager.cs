using MalbersAnimations.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Furry
{

    public class TileManager : MonoBehaviour
    {

        [SerializeField] private List<GameObject> _animalsToSpawn;
        [SerializeField, Range(0, 1)] private float _animalSpawnPercentage = 0.4f;
        [SerializeField] private GameObject _flag;
        [SerializeField] private GameObject _tileAnimal;

        private MeshRenderer _renderer;

        private void Awake()
        {
            ProceduralLevelGenerator.OnLevelGenerated += Spawn;
            _renderer = _flag.GetComponent<MeshRenderer>();
        }
        private void Spawn()
        {
            if (_tileAnimal == null && _animalsToSpawn.Count > 0 && Random.value < _animalSpawnPercentage)
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
            _tileAnimal = Instantiate(animalToSpawn, pos, Quaternion.identity, transform.GetChild(0));
        }
        public void ChangeFlag(Material flagMat)
        {
            Material[] newMaterials = _renderer.materials;
            newMaterials[0] = flagMat;
            _renderer.materials = newMaterials;
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
        private void KillAnimal()
        {
            Destroy(_tileAnimal);
            _tileAnimal = null;
            // set a timer on game manager to spawn a new animal on a random tile.
        }
    }
}