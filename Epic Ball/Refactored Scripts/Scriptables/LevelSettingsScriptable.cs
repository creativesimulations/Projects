using System;
using UnityEngine;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace EpicBall
{

    [CreateAssetMenu(fileName = "LevelSettings", menuName = "EpicBallScriptables/LevelSettings")]

    public class LevelSettingsScriptable : ScriptableObject
    {
        [Header("Setup parameters for a given level.")]
        [Tooltip("The name of the level.")]
        public string _levelName;
        [Tooltip("The number of the level.")]
        public int _levelNum;
        [Tooltip("The least time it should take to complete the level to be awarded a gold medal.")]
        public int _goldTime;
        [Tooltip("The least time it should take to complete the level to be awarded a silver medal.")]
        public int _silverTime;
        public enum SceneType { UI, EasyLevel, HardLevel };
        [Tooltip("Whether the scene is easy, hard or a UI scene.")]
        public SceneType _sceneType;

        [SerializeField] public bool _isActive;
        [SerializeField] public bool _isCompleted;
        [SerializeField] public int _bestTime;
        [SerializeField] public int _bestScore;
        [SerializeField] public bool _goldGemCollected;
        [SerializeField] public string _medal;

        public void SetScriptableData(bool active, bool complete, int time, int score, bool goldGemCollected, string medal)
        {
            _isActive = active;
            _isCompleted = complete;
            _bestTime = time;
            _bestScore = score;
            _goldGemCollected = goldGemCollected;
            _medal = medal;
        }
    }

}
