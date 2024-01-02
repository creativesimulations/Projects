using UnityEngine;

namespace EpicBall
{
    public interface IExplodable
    {
        void Explode(float _power, Vector3 explosionPos, float _radius, float _pushUp);
    }
}
