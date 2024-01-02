using UnityEngine;

namespace EpicBall
{
    public interface IBump
    {
        void PlayBumpClip(AudioClip[] clipsToPlay);
        void PlaySpecialActionClip(AudioClip clipToPlay);
        void PlayAfterDestroy(AudioClip clipToPlay);
        public bool GetClipPlayed();
        public void SetClipPlayed(bool state);

    }
}
