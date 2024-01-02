using System.Collections;
using UnityEngine;
using Core;
using Scriptables;

public class Cannon : MonoBehaviour
{
    [Header("Put a Weapon Scriptable Object here")]
    public WeaponScriptableObject WSO;
    [Tooltip("The barrel")]
    [SerializeField] public GameObject barrel;
    [Tooltip("The wheels")]
    [SerializeField] public GameObject wheels;
    [Tooltip("Position where the cannonball should spawn to be shot from")]
    public Transform cannonballSpawnVector;
    [Tooltip("A Team Controller must be added to the cannon's parent")]
    private TeamController TC;
    [Tooltip("Parent of projectiles to be used by this team. This parent is in the ObjectPooler")]
    private Transform projectileSpawnParent;
    [Tooltip("Parts to separate when hit")]
    [SerializeField] private GameObject[] cannonParts;
    [Tooltip("Random audio clips to play when firing")]
    [SerializeField] AudioClip[] clips;

    private AudioSource audioSource;
    [HideInInspector] public int team;
    private Vector3 target;
    private bool rotating;
    [SerializeField] private bool paused = false;
    private float timeUntilFiring;
    private float timeBetweenShots;

    [Tooltip("Time it takes to rotate the weapon")]
    [SerializeField] private float aimSpeed = 1.0f;
    private float turnTime;



    private void Start()
    {
        // Subscribe to the Pause events.
        EventManager.onPause += Pause;
        EventManager.onStart += UnPause;
        // Get the Team Controller in the parent
        TC = GetComponentInParent<TeamController>();
        // set team number from team controller in parent
        team = TC.teamNum;
        timeBetweenShots = WSO.timeBetweenShots + Random.Range(.3f, -.3f);
        audioSource = GetComponent<AudioSource>();

        // Set projectile pool parent
        projectileSpawnParent = EventManager.objectPooler.cannonBallParent;

        // create initial projectiles
        if (WSO.projectileInitialSpawnCount > 0)
        {
            EventManager.InitialSpawn(WSO.projectileToSpawn, WSO.projectileInitialSpawnCount, projectileSpawnParent);
        }
    }

    void Update()
    {
        if (!paused)
        {
            timeUntilFiring += Time.deltaTime;
            //if rotating is finished then start the contdown to the next shot
            if (timeUntilFiring >= timeBetweenShots)
            {
                if (rotating == false)
                {
                    StartCoroutine(Aim(aimSpeed));
                }
            }
        }
    }

    private IEnumerator Aim(float durationToAim)
    {
        // set to rotating


        // get the target to shoot at
        target = GetLandPoint();

        // get the anglet to rotate to
        float upAngle = UnityEngine.Random.Range(WSO.bottomAngle, WSO.topAngle);

        //switch the rotation to opposite (because for some reason it rotates the opposite way to begin with)
       // Vector3 wantedCannonAngle = new Vector3(-90 + angle, 0, 0);
        rotating = true;

        // TURN
        float targetPlaneAngle = vector3AngleOnPlane(target, transform.position, -transform.up, transform.forward);
        Vector3 newRotation = new Vector3 (0, targetPlaneAngle, 0);
        transform.Rotate(newRotation, Space.Self);

        // UP/Down

        /* 
        if (pitch != null)
        {
            float angleX = vector3AngleOnPlane(target, pitch.position, -transform.right, transform.forward);
            Vector3 rotationX = new Vector3(angleX, 0, 0);
            pitch.localRotation = Quaternion.Euler(rotationX);
        } */
        
        // OR

        //  float upAngle = Vector3.Angle(transform.position - target.position, barrel.transform.up);
        
        Vector3 upRotation = new Vector3(-90 - upAngle, 0, 0);
        barrel.transform.localRotation = Quaternion.Euler(upRotation);
      //  barrel.transform.Rotate(upRotation, Space.Self);

        float vector3AngleOnPlane(Vector3 from, Vector3 to, Vector3 planeNormal, Vector3 toZeroangle)
        {
            Vector3 projectedVector = Vector3.ProjectOnPlane(from - to, planeNormal);
            float projectedVectorAngle = Vector3.SignedAngle(projectedVector, toZeroangle, planeNormal);

            return projectedVectorAngle;
        }

        while (turnTime < durationToAim)
        {


            turnTime += Time.deltaTime;

            yield return null;
        }

        // reset turntime
        turnTime = 0f;
        rotating = false;
        FireCannon(target, upAngle);
        timeUntilFiring = 0;
    }

    private Vector3 GetLandPoint()
    {
        // Get areas to shoot at from the Team Controller
        Transform areaToLandIn = TC.weaponTargetAreas[UnityEngine.Random.Range(0, TC.weaponTargetAreas.Length)];
        Vector3 spotToLandInArea = UnityEngine.Random.insideUnitSphere * TC.targetAreaRadius;
        Vector3 pointToHit = new Vector3(areaToLandIn.position.x + spotToLandInArea.x, areaToLandIn.position.y, areaToLandIn.position.z + spotToLandInArea.z);

        //return the exact point to shoot at
        return pointToHit;
    }

    private void FireCannon(Vector3 point, float angle)
    {
        PlayAudioClip();
        // Get the smoke particle and play it
        BlowSmoke();
        var velocity = BallisticVelocity(point, angle);

        // Get the cannon ball from the Team Controller to activate from the Object Pooler
        GameObject cannonBall = EventManager.objectPooler.PopFromPool(WSO.projectileToSpawn.name, false, true, projectileSpawnParent);

        // Set the cannon ball at the exit of the barrel
        cannonBall.transform.position = cannonballSpawnVector.position;

        // Get cannonballexplode script on cannonball
        BallExplode BES = cannonBall.GetComponent<BallExplode>();


        // Set its team number
        BES.team = team;
        // Set its Weapon Scriptable Object
        BES.WSO = WSO;

        // Give the velocity to the cannon ball
        cannonBall.GetComponent<Rigidbody>().velocity = velocity;
    }

    private void BlowSmoke()
    {
        // Get the pooled smoke particle to play from the Object Pooler
        GameObject particle = EventManager.SpawnObject(EventManager.objectPooler.cannonSmokeParticle, false, EventManager.objectPooler.cannonSmokeParticleParent);
        particle.transform.position = new Vector3(barrel.transform.position.x, (barrel.transform.position.y + .5f), barrel.transform.position.z);

        // Deactive the smoke particle after it is finished playing
        StartCoroutine(EventManager.DelayDespawnObject(particle, null, 2.2f));
    }

    private Vector3 BallisticVelocity(Vector3 destination, float angle)
    {
        Vector3 dir = destination - barrel.transform.position; // get Target Direction
        float height = dir.y; // get height difference
        dir.y = 0; // retain only the horizontal difference
        float dist = dir.magnitude; // get horizontal direction
        float a = angle * Mathf.Deg2Rad; // Convert angle to radians
        dir.y = dist * Mathf.Tan(a); // set dir to the elevation angle.
        dist += height / Mathf.Tan(a); // Correction for small height differences

        // Calculate the velocity magnitude
        float velocity = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        return velocity * dir.normalized; // Return a normalized vector.
    }

    // Detatch children so object falls apart if it is hit by a "projectile"
    public void OnHit(int otherTeam)
    {
        if (otherTeam != team)
        {
            StopAllCoroutines();
            foreach (GameObject child in cannonParts)
            {
                child.transform.SetParent(null);
                child.AddComponent<Rigidbody>();
            }
            EventManager.onPause -= Pause;
            Destroy(gameObject);
        }
    }

    private void PlayAudioClip()
    {
        if (clips != null)
        {
            AudioClip clipToPlay;
            int arrayIndex = UnityEngine.Random.Range(0, clips.Length);
            clipToPlay = clips[arrayIndex];
            audioSource.PlayOneShot(clipToPlay);
        }
    }

    private void Pause()
    {
        if (!paused)
        {
            paused = true;
        }
    }
    private void UnPause()
    {
        if (paused)
        {
            paused = false;
        }
    }
}
