using UnityEngine;

namespace EpicBall
{
    public class GlobalConstants : MonoBehaviour
    {

        /// <summary>
        /// Music and Sound options.
        /// </summary>
        public const string MUSIC_VOLUME = "musicVolume";
        public const string SOUND_VOLUME = "soundVolume";
        public const string MUSIC_VOLUME_KEY = "musicVolumeKey";
        public const string SOUND_VOLUME_KEY = "soundVolumeKey";

        /// <summary>
        /// Whether the controls are right or left handed.
        /// </summary>
        public const string CONTROL_DIRECTION = "ControlDirection";
        public const string CONTROLLER_KEY = "ControllerKey";

        /// <summary>
        /// Scoring and awards.
        /// </summary>
        public const string LEVELS_COMPLETED = "Levels Completed";
        public const string LEVELS_ACTIVE = "Levels Active";
        public const string HIGH_SCORE = "HighScore";
        public const string PREVIOUS_HIGH_SCORE = "Previous HighScore";
        public const string GOLD_GEM_PRIZE = "Gold Gem Prize";
        public const string PURPLE_GEM_PRIZE = "Purple Gem Prize";
        public const string DESTROY_BLOCK_PRIZE = "Destroy Block Prize";
        public const string GOLD_MEDAL = "Gold Medal";
        public const string SILVER_MEDAL = "Silver Medal";
        public const string BRONZE_MEDAL = "Bronze Medal";
        public const string EASY_LEVELS_STAR = "Easy Level Stars";
        public const string HARD_LEVELS_STAR = "Hard Level Stars";
        public const string SKINS_STAR = "SkinsStar";
        public const string LEADERBOARD_STAR = "Leaderboard Star";

        /// <summary>
        /// Skins - Different skins for the player ball.
        /// </summary>
        public const string CHOSEN_PLAYER_KEY = "Chosen Player Key";
        public const string SKINS_AVAILABLE = "Skins Available";
        public const string BALL_MATRIX = "Matrix";
        public const string BALL_FIREBALL = "Fireball";
        public const string BALL_DIE = "Die";
        public const string BALL_BILLIARD = "Billiard";
        public const string BALL_EYE = "Eye";
        public const string BALL_BEACH = "Beach";
        public const string BALL_SPARK = "Spark";
        public const string BALL_BUCKY = "Bucky";
        public const string BALL_WOOD = "Wood";
        public const string BALL_WHEEL = "Wheel";
        public const string BALL_BUBBLE = "Bubble";
        public const string BALL_SPIKE = "Spike";
        public const string BALL_PENGUIN = "Penguin";
        public const string BALL_YOYO = "Yoyo";
        public const string BALL_BANANA = "Banana";
        public const string BALL_SOCCER = "Soccer";
        public const string BALL_SPIRIT = "Spirit";
        public const string BALL_CHICKEN = "Chicken";
        public const int SKINS_AMOUNT = 18;

        /// <summary>
        /// Tutorials - So we know which tutorials to load if they haven't been shown yet.
        /// </summary>
        public const string TUT_PRECISION = "Precision";
        public const string TUT_GOLD_GEMS = "Gold Gems";
        public const string TUT_JUMP = "Jump";
        public const string TUT_GOAL = "Goal";
        public const string TUT_FALL = "Fall";
        public const string TUT_BLOCKS = "Blocks";
        public const string TUT_CONTROLS = "Controls";
        public const string TUT_CONSUMERS = "Consumers";
        public const string TUT_DESTROYERS = "Destroyers";
        public const string TUT_JUMP_SPEED = "Jump Speed";
        public const string TUT_HOLES = "Holes";
        public const string TUT_CHASERS = "Chasers";
        public const string TUT_TELEBLOCKS = "Teleblocks";
        public const string TUT_TELEPORTS = "Teleports";
        public const string TUT_GLASS = "Glass";
        public const string TUT_TNT = "TNT";
        public const string TUT_GIANT = "Giant";

        /// <summary>
        /// UI scenes.
        /// </summary>
        public const string PRE_SPLASH = "Pre Splash";
        public const string SPLASH_SCREEN = "Splash Screen";
        public const string MAIN_MENU = "Main Menu";
        public const string OPTIONS_MENU = "Options Menu";
        public const string BALL_SKINS = "Ball Skins";
        public const string PAUSE_MENU = "Pause Menu";
        public const string HARD_LEVEL_NOTICE = "Open Hard Levels";
        public const string LEVEL_TITLE = "Level Title";
        public const string TIPS_MENU = "Tips Menu";
        public const string EASY_LEVEL_MENU = "Easy Level Menu";
        public const string HARD_LEVEL_MENU = "Hard Level Menu";
        public const string FINALE = "Finale";

        /// <summary>
        /// Various other constants.
        /// </summary>
        public const string PLAYER = "Player";
        public const string CAMERA_TARGET = "CameraTarget";
        public const string EASY_LEVEL = "Easy Level";
        public const string HARD_LEVEL = "Hard Level";
        public const string TIME = "Time";
    }
}