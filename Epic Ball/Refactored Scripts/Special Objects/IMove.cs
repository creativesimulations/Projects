using UnityEngine;

namespace EpicBall
{
    public interface IMove
    {
        void Chase(GameObject player);
        void StopChasing();
    }
}
