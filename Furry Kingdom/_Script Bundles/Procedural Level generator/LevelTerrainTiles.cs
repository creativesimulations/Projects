using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Furry
{

public class LevelTerrainTiles : MonoBehaviour
{

    [SerializeField] private List<GameObject> _waterTerrains = new List<GameObject>();
    [SerializeField] private float _maxWaterHeight = .1f;
    [SerializeField] private List<GameObject> _grassTerrains = new List<GameObject>();
    [SerializeField] private float _maxGrassHeight = .4f;
    [SerializeField] private List<GameObject> _desertTerrains = new List<GameObject>();
    [SerializeField] private float _maxDesertHeight = .7f;
    [SerializeField] private List<GameObject> _forestTerrains = new List<GameObject>();
    [SerializeField] private float _maxForestHeight = 1f;
    [SerializeField] private List<GameObject> _mountainTerrains = new List<GameObject>();

        private Dictionary<string, List<GameObject>> _tilesDict = new Dictionary<string, List<GameObject>>();

        private void Awake()
        {
            _tilesDict.Add("water", _waterTerrains);
            _tilesDict.Add("desert", _desertTerrains);
            _tilesDict.Add("grass", _grassTerrains);
            _tilesDict.Add("forest", _forestTerrains);
            _tilesDict.Add("mountain", _mountainTerrains);
        }

        public GameObject ChooseTerrainTile(float height)
        {
            if (height > _maxForestHeight)
            {
                return _tilesDict["mountain"][Random.Range(0, _mountainTerrains.Count)];
            }
            else if (height > _maxDesertHeight)
            {
                return _tilesDict["forest"][Random.Range(0, _forestTerrains.Count)];
            }
            else if (height > _maxGrassHeight)
            {
                return _tilesDict["desert"][Random.Range(0, _desertTerrains.Count)];
            }
            else 
            {
                return _tilesDict["grass"][Random.Range(0, _grassTerrains.Count)];
            }
        }
        public GameObject RandomWaterTile()
        {
            return _tilesDict["water"][Random.Range(0, _waterTerrains.Count)];
        }
    }

}