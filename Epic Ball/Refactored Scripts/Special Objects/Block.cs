using Core;
using EpicBall;
using UnityEngine;

public class Block : MonoBehaviour, IExplodable, IBump
{
    public Rigidbody Rb {  get; set; }
    protected AudioSource audioSource;
    protected bool _startAwake;
    private bool _clipPlayed;
    private int _thresholdToPlayBump = 2;
    protected ObjectPooler objectPooler;
    private int _index;

    private void Start()
    {
        Rb.sleepThreshold = .25f;
        
        if(!_startAwake)
        {
            Rb.Sleep();
        }
    }

    /// <summary>
    /// Add force to the block when it is within the radius of a TNT block.
    /// </summary>
    /// <param name="_power"></param> The power of the explosion.
    /// <param name="explosionPos"></param> The origin position of the explosion.
    /// <param name="_radius"></param> The redisu of the explosion.
    /// <param name="_pushUp"></param> The upward force of the exlposion.
    public virtual void Explode(float _power, Vector3 explosionPos, float _radius, float _pushUp)
    {
        Rb.AddExplosionForce(_power, explosionPos, _radius, _pushUp);
    }

    /// <summary>
    /// Deactivate the object when it is consumed.
    /// </summary>
    public void Consume()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Change the position of the object to the destinaiton when it is teleported.
    /// </summary>
    /// <param name="teleDestination"></param> The teleportation destination.
    public void Teleport(Vector3 teleDestination)
    {
        transform.position = teleDestination;
    }

    /// <summary>
    /// The method for teleporting an object with no parameters.
    /// </summary>
    public void Teleport()
    {

    }

    /// <summary>
    /// Play a random bump clip.
    /// </summary>
    /// <param name="clipsToplay"></param> The array of audio clips to randomly choose from.
    public void PlayBumpClip(AudioClip[] clipsToplay)
    {
        _index = Random.Range(0, clipsToplay.Length);
            AudioClip bumpClip = clipsToplay[_index];
            audioSource.PlayOneShot(bumpClip);
    }

    /// <summary>
    /// Play a special adio clip.
    /// </summary>
    /// <param name="clipToPlay"></param> The special audio clip to play.
    public void PlaySpecialActionClip(AudioClip clipToPlay)
    {
        audioSource.PlayOneShot(clipToPlay);
    }

    /// <summary>
    /// Play an audio clip after the object is destroyed.
    /// </summary>
    /// <param name="clipToPlay"></param> The audio clip to play after the game object is destroyed.
    public void PlayAfterDestroy(AudioClip clipToPlay)
    {
        AudioSourceExtensions.PlayAfterDestroy(audioSource, clipToPlay);
    }

    /// <summary>
    /// Wake up the rigid body on this game object wen an object leaves its trigger collider.
    /// </summary>
    /// <param name="other"></param> the collider of the other game object.
    private void OnTriggerExit(Collider other)
    {
        if (Rb.IsSleeping())
        {
            Rb.WakeUp();
        }
    }

    /// <summary>
    /// Set the state of the audio bump clip if it has been played or not.
    /// </summary>
    /// <param name="state"></param>
    public void SetClipPlayed(bool state)
    {
        _clipPlayed = state;
    }

    /// <summary>
    /// Returns the state whether the bump audio clip has been played or not.
    /// </summary>
    /// <returns></returns>
    public bool GetClipPlayed()
    {
        return _clipPlayed;
    }

    /// <summary>
    /// Returns the threshold velocity at which a bump clip should be played.
    /// </summary>
    /// <returns></returns>
    protected int GetThresholdToPlayBump()
    {
        return _thresholdToPlayBump;
    }
}
