using UnityEngine;

namespace EpicBall
{
    public interface ITeleportable
    {
        void Teleport();
        void Teleport(Vector3 teleDestination);
    }
}
