using Furry;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Furry
{


    public class Derender : MonoBehaviour
    {
        [SerializeField] private Vector3 _derenderInsideCubeSize;
        private List<GameObject> _terrainTiles;

        private void Start()
        {
            ProceduralLevelGenerator.OnTilesSet += Init;
        }
        private void Init(List<GameObject> terrainTiles)
        {
            _terrainTiles = terrainTiles;
            DeactivateTiles();
        }
        private void DeactivateTiles()
        {
            for (int t = 0; t < _terrainTiles.Count; t++)
            {
                RendererToggle rT = _terrainTiles[t].GetComponentInChildren<RendererToggle>();
                if (rT != null)
                {
                    rT.DeActivateRenderers();
                }
            }
        }
    }

}