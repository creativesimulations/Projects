using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using TMPro;
using Scriptables;

public class TeamController : MonoBehaviour
{
    [Header("Setup for this team")]
    [Tooltip("Team number. All projectiles, upgrades and such for this team will receive the same number during play so they will know which team they are related to.")]
    [SerializeField] public Team team = Team.Team1;
    public enum Team // Team enum
    {
        Team1 = 1,
        Team2 = 2,
        Team3 = 3,
        Team4 = 4
    };

    [TextArea(1, 10)]
    [Tooltip("Private info on this team.")]
    public string description = "";
    [Tooltip("Prefab of weapon to be used to spawn more when upgrade is collected to get another weapon.")]
    public GameObject weaponToSpawn;
    [Tooltip("Parent where new weapons will spawn.")]
    public Transform weaponParent;
    [Tooltip("Put a Weapon Scriptable Object here.")]
    public WeaponScriptableObject WSO;

    [Tooltip("The transforms for the areas in which the weapons will aim.")]
    [SerializeField] public Transform[] weaponTargetAreas;
    [Tooltip("The precision of the weapon shots to hit their target point.")]
    public float targetAreaRadius = 5f;

    [Tooltip("The transforms for the areas in which the weapons will spawn.")]
    [SerializeField] public Transform[] weaponSpawnAreas;
    [Tooltip("The radius in which to spawn new weapons in the weapon target areas.")]
    public float weaponSpawnRadius = 5f;

    private Color teamColor;
    private string teamColorName;
    private string messageForGeneralNotice;

    [HideInInspector] public int teamNum;

    private void Awake()
    {
        //Set team number and color
        SetTeam();
        // Add Upgrade method to Onupgrade delegate so the method is run whenever Onpgrade is called
        EventManager.onUpgrade += Upgrade;
    }

    // Set team number and color for texts
    private void SetTeam()
    {
        switch (team)
        {
            case Team.Team1:
                teamNum = (int)Team.Team1;
                teamColor = Color.blue;
                teamColorName = "Blue";
                break;
            case Team.Team2:
                teamNum = (int)Team.Team2;
                teamColor = Color.red;
                teamColorName = "Red";
                break;
            case Team.Team3:
                teamNum = (int)Team.Team3;
                teamColor = Color.yellow;
                teamColorName = "Yellow";
                break;
            case Team.Team4:
                teamNum = (int)Team.Team4;
                teamColor = Color.green;
                teamColorName = "Green";
                break;
        }

    }
    // Run delegate in Event Manager to upgrade team stats
    public void Upgrade(int upgradeNum, float amountToChange, int team)
    {
        if (team == teamNum)
        {
            // Display a text message on the screen
            GameObject notice = EventManager.SpawnObject(EventManager.objectPooler.generalNotices, true, EventManager.objectPooler.generalNoticesParent);
            switch (upgradeNum)
            {
                case 0: //  fire speed
                        messageForGeneralNotice = (teamColorName + " team just upgraded their firing speed by " + amountToChange + "!");
                        WSO.timeBetweenShots -= amountToChange;
                    break;
                case 1:  //  explosive Radius
                        WSO.explosionRadius += amountToChange;
                        messageForGeneralNotice = (teamColorName + " team now has "+ amountToChange +" more explosivene range!");
                    break;
                case 2:  //  upwards Force
                         //  explosive Force
                        WSO.explosionForce += amountToChange;
                        WSO.explosionUpwardsForce += amountToChange;
                        messageForGeneralNotice = (teamColorName + " team increased their explosive forces by " + amountToChange + "!");
                    break;
                case 3:  //  New Weapon
                        messageForGeneralNotice = (teamColorName + " team added another cannon!");
                        SpawnNewCannon();
                    break;
            }
        notice.GetComponent<GeneralNotice>().Setup(teamColor, messageForGeneralNotice); 
        }
    }

    private void SpawnNewCannon()
    {
        Vector3 spawnPoint = GetSpawnPoint();
        if (spawnPoint == Vector3.zero)
        {
            SpawnNewCannon();
            return;
        }
        else
        {
        Quaternion spawnRotation = new Quaternion();
        //  spawnRotation.eulerAngles = new Vector3(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));  For 3D random rotation
        spawnRotation.eulerAngles = new Vector3(0, Random.Range(0.0f, 360.0f), 0);
        GameObject newWeapon = Instantiate(weaponToSpawn, spawnPoint, spawnRotation, weaponParent);
        newWeapon.GetComponentInChildren<Cannon>().WSO = WSO;
        }
    }

    Vector3 GetSpawnPoint()
    {
        Transform transformToSpawn = weaponSpawnAreas[Random.Range(0, weaponSpawnAreas.Length)];
        float startTime = Time.realtimeSinceStartup;
        bool test = false;
            Vector3 spawnPositionRaw = Random.insideUnitCircle * weaponSpawnRadius;
            Vector3 spawnPoint = new Vector3(transformToSpawn.position.x + spawnPositionRaw.x, transformToSpawn.position.y + spawnPositionRaw.y, transformToSpawn.position.z + spawnPositionRaw.z);
            test = !Physics.CheckSphere(spawnPoint, weaponSpawnRadius);
            if (Time.realtimeSinceStartup - startTime > 0.1f)
            {
                return Vector3.zero;
            }
        return spawnPoint;
    }

    private void BlowSmoke()
    {
        // Get the pooled smoke particle to play from the Object Pooler
        GameObject particle = EventManager.SpawnObject(EventManager.objectPooler.weaponSpawnParticle, false, EventManager.objectPooler.weaponSpawnParticleParent);
        particle.transform.position = transform.position;

        // Deactive the smoke particle after it is finished playing
        StartCoroutine(EventManager.DelayDespawnObject(particle, null, 2.2f));
    }

    private void OnTriggerExit(Collider other) {
        other.transform.SetParent(null, true);
    }

}
