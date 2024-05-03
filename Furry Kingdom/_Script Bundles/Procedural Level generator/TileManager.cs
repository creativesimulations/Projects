using MalbersAnimations.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Furry
{

    public class TileManager : MonoBehaviour
    {
        [Header("Tile fields.")]
        [Tooltip("List of animals to spawn for this type of tile.")]
        [SerializeField] private List<GameObject> _animalsToSpawn;
        [Tooltip("Percentage chance to spawn an animal if tile has no current animal.")]
        [SerializeField, Range(0, 1)] private float _animalSpawnPercentage = 0.4f;
        [Tooltip("Flag on tile.")]
        [SerializeField] private GameObject _flag;

        private GameObject _tileAnimal;
        private MeshRenderer _renderer;

        private void Awake()
        {
            ProceduralLevelGenerator.OnLevelGenerated += TryToSpawn;
            _renderer = _flag.GetComponent<MeshRenderer>();
        }

        /// <summary>
        /// Checks for a chance to spawn an animal on this tile.
        /// </summary>
        private void TryToSpawn()
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

        /// <summary>
        /// Waits a desired amount of time before trying to spawn an animal.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Changes the material of the flas to the supplied material.
        /// </summary>
        /// <param name="flagMat"></param> Material to change to.
        public void ChangeFlag(Material flagMat)
        {
            Material[] newMaterials = _renderer.materials;
            newMaterials[0] = flagMat;
            _renderer.materials = newMaterials;
        }

        /// <summary>
        /// Returns true if this tile already has an animal.
        /// </summary>
        /// <returns></returns>
        public bool HasAnimal()
        {
            bool value = false;
            if (_tileAnimal != null)
            {
                value = true;
            }
            return value;
        }

        /// <summary>
        /// Destrorys the current animal on this tile.
        /// </summary>
        private void KillAnimal()
        {
            // i NEED TO ESTABLISH AN OBJECT POOLER INSTEAD OF DESTROYING THE ANIMALS.  ***
            Destroy(_tileAnimal);
            _tileAnimal = null;
            // set a timer on game manager to spawn a new animal on a random tile.
        }
    }
}