using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class ObjectPooler : MonoBehaviour
    {
        [Header("Items to pool")]
        [Tooltip("Set up items to pool if there is no other script to spawn them. You need to tick the correlated box and enter the number of extra objects to spawn to activate the spawning.")]
        [SerializeField] public bool spawnExplosionParticle;
        [SerializeField] public GameObject explosionParticle = null;
        [SerializeField] public Transform explosionParticleParent;
        [SerializeField] public bool spawnbulletParticle;
        [SerializeField] public GameObject bulletParticle = null;
        [SerializeField] public Transform bulletParticleParent;
        [SerializeField] public bool spawncannonSmokeParticle;
        [SerializeField] public GameObject cannonSmokeParticle = null;
        [SerializeField] public Transform cannonSmokeParticleParent;
        [SerializeField] public Transform cannonBallParent;
        [SerializeField] public bool spawncannonBallExplosionParticle;
        [SerializeField] public GameObject cannonBallExplosionParticle = null;
        [SerializeField] public Transform cannonBallExplosionParticleParent;
        [SerializeField] public bool spawnmissleParticle;
        [SerializeField] public GameObject missleParticle = null;
        [SerializeField] public Transform missleParticleParent;
        [SerializeField] public bool spawnfireParticle;
        [SerializeField] public GameObject fireParticle = null;
        [SerializeField] public Transform fireParticleParent;
        [SerializeField] public bool spawnsplashParticle;
        [SerializeField] public GameObject splashParticle = null;
        [SerializeField] public Transform splashParticleParent;
        [SerializeField] public bool spawnArrowParticle;
        [SerializeField] public GameObject arrowParticle = null;
        [SerializeField] public Transform arrowParent;
        [SerializeField] public bool spawnWeaponSpawnParticle;
        [SerializeField] public GameObject weaponSpawnParticle = null;
        [SerializeField] public Transform weaponSpawnParticleParent;
        [SerializeField] public bool spawnCharacter1;
        [SerializeField] public GameObject character1 = null;
        [SerializeField] public Transform character1Parent;
        [SerializeField] public bool spawnCharacter2;
        [SerializeField] public GameObject character2 = null;
        [SerializeField] public Transform character2Parent;
        [SerializeField] public bool spawnCharacter3;
        [SerializeField] public GameObject character3 = null;
        [SerializeField] public Transform character3Parent;
        [SerializeField] public bool spawnCharacter4;
        [SerializeField] public GameObject character4 = null;
        [SerializeField] public Transform character4Parent;
        [Tooltip("You need to enter the number of objects to spawn to activate the initial spawning.")]
        [SerializeField] private int numberOfExtraObjects;

        [SerializeField] public bool spawngeneralNotices;
        [SerializeField] public GameObject generalNotices = null;
        [SerializeField] public Transform generalNoticesParent;

        [Tooltip("You need to enter the number of notices to spawn to activate the initial spawning.")]
        [SerializeField] private int numberOfNotices;
        // SerializeField] private int numberOfShips;

        private Dictionary<string, Queue<GameObject>> container = new Dictionary<string, Queue<GameObject>>();
        private Dictionary<string, GameObject> prefabContainer = new Dictionary<string, GameObject>();

        private void Awake()
        {
            if (spawnExplosionParticle)
            {
                AddToPool(explosionParticle, numberOfExtraObjects, explosionParticleParent);
            }
            if (spawnbulletParticle)
            {
            AddToPool(bulletParticle, numberOfExtraObjects, bulletParticleParent);
            }
            if (spawncannonSmokeParticle)
            {
            AddToPool(cannonSmokeParticle, numberOfExtraObjects, cannonSmokeParticleParent);
            }
            if (spawncannonBallExplosionParticle)
            {
            AddToPool(cannonBallExplosionParticle, numberOfExtraObjects, cannonBallExplosionParticleParent);
            }
            if (spawnmissleParticle)
            {
            AddToPool(missleParticle, numberOfExtraObjects, missleParticleParent);
            }
            if (spawnfireParticle)
            {
            AddToPool(fireParticle, numberOfExtraObjects, fireParticleParent);
            }
            if (spawnsplashParticle)
            {
            AddToPool(splashParticle, numberOfExtraObjects, splashParticleParent);
            }
            if (spawnArrowParticle)
            {
            AddToPool(arrowParticle, numberOfExtraObjects, arrowParent);
            }


            if (spawngeneralNotices)
            {
                AddToPool(generalNotices, numberOfNotices, generalNoticesParent);
            }
        }

        // Add objects to a pool
        public bool AddToPool(GameObject prefab, int count, Transform parent = null, bool disable = true)
        {
            if (prefab == null || count <= 0) { return false; }
            string name = prefab.name;
            if (this.prefabContainer.ContainsKey(name) == false)
            {
                this.prefabContainer.Add(name, prefab);
            }
            if (this.prefabContainer[name] == null)
            {
                this.prefabContainer[name] = prefab;
            }
            for (int i = 0; i < count; i++)
            {
                GameObject obj = PopFromPool(name, true, false, parent);
                if (disable)
                {
                    PushToPool(ref obj, true, parent);
                }
            }
            return true;
        }

        // Get the object from pool by gameobject
        public GameObject PopFromPool(GameObject prefab, bool forceInstantiate = false, bool instantiateIfNone = false, Transform container = null, bool SetActive = true)
        {
            if (prefab == null)
            {
                return null;
            }
            return PopFromPool(prefab.name, forceInstantiate, instantiateIfNone, container, SetActive);
        }

        // Get object from pool by the object name
        public GameObject PopFromPool(string prefabName, bool forceInstantiate = false, bool instantiateIfNone = false, Transform container = null, bool SetActive = true)
        {
            if (prefabName == null || this.prefabContainer.ContainsKey(prefabName) == false)
            {
                return null;
            }
            if (forceInstantiate == true)
            {
                return CreateObject(prefabName, container, SetActive);
            }
            GameObject obj = null;
            Queue<GameObject> queue = FindInContainer(prefabName);
            if (queue.Count > 0)
            {
                obj = queue.Dequeue();
                obj.transform.parent = container;
                obj.SetActive(SetActive);
            }
            if (obj == null && instantiateIfNone == true)
            {
                return CreateObject(prefabName, container, SetActive);
            }
            return obj;
        }

        // This code is run if you choose to create a new object in case it doesn't exist in the pool
        private GameObject CreateObject(string prefabName, Transform container, bool SetActive)
        {
            GameObject obj = (GameObject)Object.Instantiate(prefabContainer[prefabName]);
            obj.name = prefabName;
            obj.transform.parent = container;
            return obj;
        }

        // return an object to a pool
        public void PushToPool(ref GameObject obj, bool retainObject = true, Transform parent = null)
        {
            if (obj == null) { return; }
            if (retainObject == false)
            {
                Object.Destroy(obj);
                obj = null;
                return;
            }
            if (parent != null)
            {
                obj.transform.parent = parent;
            }
            Queue<GameObject> queue = FindInContainer(obj.name);
            queue.Enqueue(obj);
            obj.SetActive(false);
            obj = null;
        }

        // Remove an object from a pool or also destroy it
        public void ReleaseItems(GameObject prefab, bool destroyObject = false)
        {
            if (prefab == null) { return; }
            Queue<GameObject> queue = FindInContainer(prefab.name);
            if (queue == null) { return; }
            while (queue.Count > 0)
            {
                GameObject obj = queue.Dequeue();
                if (destroyObject == true)
                {
                    Object.Destroy(obj);
                }
            }
        }

        // Remove a pool from the list of pools
        public void ReleasePool()
        {
            foreach (var kvp in this.container)
            {
                Queue<GameObject> queue = kvp.Value;
                while (queue.Count > 0)
                {
                    GameObject obj = queue.Dequeue();
                    Object.Destroy(obj);
                }
            }
            this.container = null;
            this.container = new Dictionary<string, Queue<GameObject>>();
            this.prefabContainer.Clear();
            this.prefabContainer = null;
            this.prefabContainer = new Dictionary<string, GameObject>();
        }

        // Find an object in the pools by object name
        private Queue<GameObject> FindInContainer(string prefabName)
        {
            if (this.container.ContainsKey(prefabName) == false)
            {
                this.container.Add(prefabName, new Queue<GameObject>());
            }
            return this.container[prefabName];
        }
    }
}
