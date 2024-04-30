using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;

namespace Furry
{

public static class Utilities
    {
        
        public static Vector3 TestNewLocation(Vector3 location, float range)
        {
            NavMeshHit hit;
            bool positionFound = NavMesh.SamplePosition(location, out hit, range, NavMesh.AllAreas);

            if (positionFound)
            {
                return hit.position;
            }
            else
            {
                return Vector3.zero;
            }
        }

    }
       //     Debug.DrawRay(hit.position, Vector3.up, Color.red, 2);

}