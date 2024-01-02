using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

namespace Contagion
{

    public class ContagionController : MonoBehaviour
    {
        [SerializeField] private RPGCharacterAnims.CameraController cameraFollower;

        [SerializeField] private GameObject planeToSpawnOn;
        private Mesh planeMesh;
        [SerializeField] public Transform[] moveableAreas;
        [SerializeField] private float areaRadius;
        [SerializeField] private int initialSpawn;
        [SerializeField] private float objectSize;
        public List<GameObject> wellCharacterList = new();
        public List<GameObject> illCharacterList = new();
        public List<GameObject> immuneCharacterList = new();
        private bool startedTimer;

        private float originalOffset;

        public delegate void OnCount();
        public static OnCount onCount;
        public delegate void OnUpdateIll();
        public static OnUpdateIll onUpdateIll;
        public delegate void OnUpdateImmune();
        public static OnUpdateImmune onUpdateImmune;
        public delegate void OnUpdateDead();
        public static OnUpdateDead onUpdateDead;


        void Start()
        {
            originalOffset = cameraFollower.cameraTargetOffsetY;

            planeMesh = planeToSpawnOn.GetComponent<MeshFilter>().mesh;
            EventManager.InitialSpawn(EventManager.objectPooler.character1, initialSpawn, EventManager.objectPooler.character1Parent, false);
            EventManager.InitialSpawn(EventManager.objectPooler.character2, initialSpawn / 4, EventManager.objectPooler.character2Parent, true);
            EventManager.InitialSpawn(EventManager.objectPooler.character3, initialSpawn / 4, EventManager.objectPooler.character3Parent, true);
            EventManager.InitialSpawn(EventManager.objectPooler.character4, initialSpawn / 4, EventManager.objectPooler.character4Parent, true);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                InfectChar();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                Refocus();
            }
        }

        private void Refocus()
        {
            cameraFollower.cameraTarget.transform.position = Vector3.zero;
            cameraFollower.cameraTargetOffsetY = originalOffset;
        }

        public Vector3 GetARandomPosOnPlane()
        {
            Bounds bounds = planeMesh.bounds;

            float minX = planeToSpawnOn.transform.position.x - planeToSpawnOn.transform.localScale.x * bounds.size.x * 0.5f;
            float minZ = planeToSpawnOn.transform.position.z - planeToSpawnOn.transform.localScale.z * bounds.size.z * 0.5f;

            Vector3 newVec = new Vector3(Random.Range(minX, -minX), transform.position.y, Random.Range(minZ, -minZ));
            return newVec;
        }

        public Vector3 GetPointInCirle(int areaToMoveIn)
        {
            Transform transformAreaToMoveIn = moveableAreas[areaToMoveIn];
            Vector3 positionRaw = Random.insideUnitSphere * areaRadius;
            Vector3 point = new Vector3(transformAreaToMoveIn.position.x + positionRaw.x, 1, transformAreaToMoveIn.position.z + positionRaw.z);
            return point;
        }

        private void InfectChar()
        {
            int charNumber = Random.Range(0, wellCharacterList.Count);
            GameObject charToGetSick = wellCharacterList[charNumber];
            Vector3 tpo = charToGetSick.transform.position;
            if (!startedTimer)
            {
                cameraFollower.cameraTarget.transform.position = tpo;
                cameraFollower.cameraTargetOffsetY = 0;
                onCount();
                startedTimer = true;
            }
            charToGetSick.GetComponent<Contagion>().WellToIll(GetARandomPosOnPlane());
        }

        public void SwitchToWell(GameObject gOToHeal, Vector3 position, Quaternion rotation, Vector3 goal, bool free, int areaNum, float speed, bool survived)
        {
            EventManager.DespawnObject(gOToHeal);

            GameObject newChar = EventManager.SpawnObject(EventManager.objectPooler.character1, true, EventManager.objectPooler.character1Parent, false);
            newChar.transform.SetPositionAndRotation(position, rotation);
            newChar.SetActive(true);
            newChar.GetComponent<Contagion>().SetStats(free, areaNum, this, goal, speed, survived);
        }

        public void SwitchToIll(GameObject gOToInfect, Vector3 position, Quaternion rotation, Vector3 goal, bool free, int areaNum, float speed, bool survived)
        {
            EventManager.DespawnObject(gOToInfect);

            GameObject newChar = EventManager.SpawnObject(EventManager.objectPooler.character2, true, EventManager.objectPooler.character2Parent, false);
            newChar.transform.SetPositionAndRotation(position, rotation);
            newChar.SetActive(true);
            newChar.GetComponent<Contagion>().SetStats(free, areaNum, this, goal, speed, survived);
            onUpdateIll();
        }

        public void SwitchToImmune(GameObject gOToImmunize, Vector3 position, Quaternion rotation, Vector3 goal, bool free, int areaNum, float speed)
        {
            EventManager.DespawnObject(gOToImmunize);

            GameObject newChar = EventManager.SpawnObject(EventManager.objectPooler.character3, true, EventManager.objectPooler.character3Parent, false);
            newChar.transform.SetPositionAndRotation(position, rotation);
            newChar.SetActive(true);
            newChar.GetComponent<Contagion>().SetStats(free, areaNum, this, goal, speed, true);
            onUpdateImmune();
        }

        public void SwitchToDead(GameObject gOToKill, Vector3 position, Quaternion rotation, Vector3 goal)
        {
            EventManager.DespawnObject(gOToKill);

            GameObject newChar = EventManager.SpawnObject(EventManager.objectPooler.character4, true, EventManager.objectPooler.character4Parent, false);
            newChar.transform.SetPositionAndRotation(position, rotation);
            newChar.SetActive(true);
            onUpdateDead();
        }
    }
}