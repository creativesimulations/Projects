using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Furry
{

    public class GenerateLevel : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _lands;
        private int[,,] _grid;

        void Start()
        {
            _grid = new int[10, 6, 10]; // The size should be random for large, medium and small sizes. There should be a specific number of top land pieces.
        }

        private void LoopThroughGrid()
        {
            // height - starts on the bottom row
            for (int y = 0; y < _grid.GetLength(1); y++)
            {
                // width - starts from the left
                for (int x = 0; x < _grid.GetLength(0); x++)
                {
                    // depth - starts from the back
                    for (int z = 0; z < _grid.GetLength(2); z++)
                    {
                        Debug.Log(_grid[y, x, z]);
                        // CheckGrid();
                    }
                }
            }
        }

        private void CheckGrid()
        {
            // check position in grid if land should be put. If so instantiate DecideLandPiece();
        }

        private GameObject DecideLandPiece()
        {
            return null; //Should return the type of land to place from the serialized list _lands.
        }
    }

}