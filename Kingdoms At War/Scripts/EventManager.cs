using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace Core
{
    public class EventManager : MonoBehaviour
    {

        private Dictionary<string, UnityEvent> eventDictionary;

        public static EventManager eventManager;
        public static ObjectPooler objectPooler;
        public static ObjectSpawner objectSpawner;

        // Delegates
        public delegate void OnUpgrade(int upgradeNum, float amountToChange, int teamNumber);
        public static OnUpgrade onUpgrade;
        
        public delegate void OnSetTeam();
        public static OnSetTeam onSetTeam;

        public delegate void OnPause();
        public static OnPause onPause;

        public delegate void OnStart();
        public static OnStart onStart;

        private void Awake()
        {
            objectPooler = GetComponentInParent<ObjectPooler>();
        }
        /* 
    void Update()
    {
            if (Input.GetKeyDown(KeyCode.P))
            {
                onPause();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                onStart();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                onPause();
                GameObject notice = SpawnObject(objectPooler.generalNotices, true, objectPooler.generalNoticesParent);
                notice.GetComponent<GeneralNotice>().Setup(Color.red, "The Red team won by annihilating the Blue team!");
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                onPause();
                GameObject notice = SpawnObject(objectPooler.generalNotices, true, objectPooler.generalNoticesParent);
                notice.GetComponent<GeneralNotice>().Setup(Color.red, "The Blue team won by annihilating the Red team!");
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                onUpgrade(3, 1, 1);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                onUpgrade(3, 1, 2);
            }
        }
 */

        public static EventManager Instance
        {
            get {
                if (!eventManager)
                {
                    eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                    if (!eventManager)
                    {
                        Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
                    }
                    else
                    {
                        eventManager.Init();
                    }
                }

                return eventManager;
            }
        }

        void Init()
        {
            if (eventDictionary == null)
            {
                eventDictionary = new Dictionary<string, UnityEvent>();
            }
        }

        public static void StartEventListening(string eventName, UnityAction listener)
        {
            UnityEvent thisEvent = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                Instance.eventDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopEventListening(string eventName, UnityAction listener)
        {
            if (eventManager == null) return;
            UnityEvent thisEvent = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        public static void TriggerEvent(string eventName)
        {
            UnityEvent thisEvent = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke();
            }
        }



        // Static Spawning Methods
        // To pop objects from their original parents and enable them
        public static GameObject SpawnObject(GameObject gOToDespawn, bool instantiateNewIfNone = true, Transform parent = null, bool SetActive = true)
        {
            var newObject = objectPooler.PopFromPool(gOToDespawn.name, false, instantiateNewIfNone, parent, SetActive);
            return newObject;
        }

        // To return objects to their original parent and disable them
        public static void DespawnObject(GameObject gOToDespawn, Transform parent = null)
        {
            objectPooler.PushToPool(ref gOToDespawn, true, parent);
        }

        // To spawn first objects that are assigned a parent
        public static void InitialSpawn (GameObject objectToSpawn, int numOfObjects, Transform parent = null, bool disable = true)
        {
            objectPooler.AddToPool(objectToSpawn, numOfObjects, parent, disable);
        }

        // To return objects to their original parent and disable them after a delay
        public static IEnumerator DelayDespawnObject(GameObject gOToDespawn, Transform parent = null, float delay = 1)
        {
            yield return new WaitForSeconds(delay);
            DespawnObject(gOToDespawn, parent);
        }

        // Static Delegate Methods

        

    }
}