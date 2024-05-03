using System.Collections.Generic;
using UnityEngine;

namespace Furry
{
    public class Derender : MonoBehaviour
    {
        [SerializeField] private Vector3 _derenderInsideCubeSize;

        private void Start()
        {
            ProceduralLevelGenerator.OnTilesSet += DerenderTiles;
        }

        /// <summary>
        /// Turns off the renderer components on all objects in the list.
        /// </summary>
        /// <param name="terrainTiles"></param> List of objects to derender.
        private void DerenderTiles(List<GameObject> terrainTiles)
        {
            for (int t = 0; t < terrainTiles.Count; t++)
            {
                RendererToggle rT = terrainTiles[t].GetComponentInChildren<RendererToggle>();
                if (rT != null)
                {
                    rT.DeActivateRenderers();
                }
            }
        }
    }

}