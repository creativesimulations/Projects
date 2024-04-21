using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;

namespace Furry
{

public static class LevelBuildingUtilities
{
    public static void BakeNavMeshes(NavMeshSurface[] navmeshSurfaces)
    {
        foreach (var navMesh in navmeshSurfaces)
        {
            navMesh.BuildNavMesh();
        }
    }


        public static Vector3 TestNewLocation(Vector3 location, float range)
        {
            NavMeshHit hit;
            NavMesh.SamplePosition(location, out hit, range, NavMesh.AllAreas);
            return hit.position;
        }

    }

}