using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TreeEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Furry
{
    public class RandomHeightTerrainGenerator : MonoBehaviour
    {
            [SerializeField] private GameObject terrainPrefab;
            [SerializeField] private int levelWidth = 10;
            [SerializeField] private int levelDepth = 10;
            [SerializeField] private int levelHeight = 10;

        GameObject[,] terrains;
        private void Start()
            {
                terrains = new GameObject[levelWidth, levelDepth];
                GenerateLevel();
            }
        private void GenerateLevel()
        {

            // Step 1: Generate terrains with random heights
            for (int z = 0; z < levelDepth; z++)
            {
                for (int x = 0; x < levelWidth; x++)
                {
                    float randomHeight = Random.Range(0.3f, levelHeight * 0.3f);
                    Vector3 position = new Vector3(x * 3, randomHeight / 2, z * 3); // Adjust position based on height
                    GameObject terrain = Instantiate(terrainPrefab, position, Quaternion.identity);
                    terrain.transform.parent = transform;
                    terrains[x, z] = terrain;
                }
            }
            EnsureContinuity();
        }
        private void EnsureContinuity()
        {
            List <GameObject> checkedObjects = new List <GameObject>();
            // Step 2: Adjust positions to ensure reachability
            for (int z = 0; z < levelDepth; z++)
            {
                for (int x = 0; x < levelWidth; x++)
                {
                    GameObject currentTerrain = terrains[x, z];
                    if (currentTerrain != null)
                    {
                        // Check adjacent terrains
                        if (x > 0 && z > 0)
                        {
                                float currentHeight = currentTerrain.transform.position.y;
                            GameObject leftTerrain = terrains[x - 1, z];
                            if (leftTerrain != null && !checkedObjects.Contains(leftTerrain))
                            {
                                float leftHeight = leftTerrain.transform.position.y;

                                if (leftHeight > currentHeight + 0.3f)
                                {
                                    leftTerrain.transform.position = new Vector3(leftTerrain.transform.position.x, currentHeight + 0.3f, leftTerrain.transform.position.z);
                                }
                                    else if (leftHeight < currentHeight + 0.3f)
                                {
                                    leftTerrain.transform.position = new Vector3(leftTerrain.transform.position.x, currentHeight - 0.3f, leftTerrain.transform.position.z);
                                }
                                return;
                            }
                            GameObject topTerrain = terrains[x, z - 1];
                            if (topTerrain != null && !checkedObjects.Contains(topTerrain))
                            {
                                float topHeight = topTerrain.transform.position.y;

                                // Ensure the current terrain is reachable by the player
                                if (topHeight > currentHeight + 0.3f)
                                {
                                    topTerrain.transform.position = new Vector3(topTerrain.transform.position.x, currentHeight + 0.3f, topTerrain.transform.position.z);
                                }
                                else if (topHeight < currentHeight + 0.3f)
                                {
                                    topTerrain.transform.position = new Vector3(topTerrain.transform.position.x, currentHeight - 0.3f, topTerrain.transform.position.z);
                                }
                                return;
                            }
                        }
                    }
                }
            }
        }

    }

}