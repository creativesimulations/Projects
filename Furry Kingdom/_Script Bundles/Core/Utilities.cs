using UnityEngine;
using UnityEngine.AI;

namespace Furry
{

    public static class Utilities
    {

        /// <summary>
        /// Returns a Vector3 of the nearest point on the navmesh within a range from a location. Returns Vector3.zero if one isn't found.
        /// </summary>
        /// <param name="location"></param> Location to check from.
        /// <param name="range"></param> Maximum range to check.
        /// <returns></returns>
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