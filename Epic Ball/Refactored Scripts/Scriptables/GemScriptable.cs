using UnityEngine;

namespace EpicBall
{

    [CreateAssetMenu(fileName = "Gems", menuName = "EpicBallScriptables/Gems")]
    public class GemScriptable : ScriptableObject
    {
        [Header("Setup parameters for gems.")]
        [Tooltip("If the gem is a gold gem.")]
        [SerializeField] public bool _isGoldGem;
        [Tooltip("The particle effect to play when the gem is collected.")]
        [SerializeField] public GameObject _particleSystem;
        [Tooltip("The audio clip to play when the gem is collected.")]
        [SerializeField] public AudioClip _audioClip;
    }
}
