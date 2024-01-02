using UnityEngine;

public class ParticleHolder : MonoBehaviour
{

    public ParticleSystem[] effects;
    ParticlePool particlePool;

    public void CreateParticles()
    {
        // 0 = Consume
        // 1 = TNT
        // 2 = Tele
        // 3 = Gem
        // 4 = Glass
        // 5 = Destroyemy

        particlePool = new ParticlePool(effects[0], effects[1], effects[2], effects[3], effects[4], effects[5], 10);
    }

    public void playParticle(int particleType, Vector3 particlePos, Vector3 size)
    {
        ParticleSystem particleToPlay = particlePool.getAvailabeParticle(particleType);

        if (particleToPlay != null)
        {
            if (particleToPlay.isPlaying)
            {
                particleToPlay.Stop();
            }
            var particleShape = particleToPlay.shape;
            particleShape.scale = size;
            particleToPlay.transform.position = particlePos;
            particleToPlay.Play();
        }

    }
}