using System;
using System.Collections.Generic;
using UnityEngine;

namespace EpicBall
{
    [Serializable]
    [CreateAssetMenu(fileName = "LevelsList", menuName = "EpicBallScriptables/LevelsList")]
    public class LevelsScriptable : ScriptableObject
    {
        [Header("Insert all level settings scriptable objects and scenes of one type here. Such as realting to UI, or easy levels or hard levels.")]
        [Tooltip("A List of scriptable level settings in the game.")]
        [SerializeField] public List<LevelSettingsScriptable> _levelSettings;
    }

}
