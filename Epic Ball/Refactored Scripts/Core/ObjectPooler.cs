using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class ObjectPooler : MonoBehaviour
    {
        [Header("Items to pool")]
        [Tooltip("Set up particle effects to pool. The particle effects will be stored in the parent container for repeat use.")]
        [SerializeField] public GameObject consumeParticle = null;
        [SerializeField] public Transform consumeParticleParent;
        [SerializeField] public GameObject tntParticle = null;
        [SerializeField] public Transform tntParticleParent;
        [SerializeField] public GameObject teleportParticle = null;
        [SerializeField] public Transform teleportParticleParent;
        [SerializeField] public GameObject gemParticle = null;
        [SerializeField] public Transform gemParticleParent;
        [SerializeField] public GameObject glassBoxParticle = null;
        [SerializeField] public Transform glassBoxParticleParent;
        [SerializeField] public GameObject glassCylinderParticle = null;
        [SerializeField] public Transform glassCylinderParticleParent;
        [SerializeField] public GameObject glassSphereParticle = null;
        [SerializeField] public Transform glassSphereParticleParent;
        [SerializeField] public GameObject destroyerParticle = null;
        [SerializeField] public Transform destroyerParticleParent;
        [SerializeField] public GameObject jumpParticle = null;
        [SerializeField] public Transform jumpParticleParent;
        [Tooltip("Enter the number of objects to spawn to activate the initial spawning.")]
        [SerializeField] private int numberOfExtraObjects;

        private Dictionary<string, Queue<GameObject>> container = new Dictionary<string, Queue<GameObject>>();
        private Dictionary<string, GameObject> prefabContainer = new Dictionary<string, GameObject>();

        private void Awake()
        {
            AddToPool(consumeParticle, numberOfExtraObjects, consumeParticleParent, true);
            AddToPool(tntParticle, numberOfExtraObjects, tntParticleParent, true);
            AddToPool(teleportParticle, numberOfExtraObjects, teleportParticleParent, true);
            AddToPool(gemParticle, numberOfExtraObjects, gemParticleParent, true);
            AddToPool(glassBoxParticle, numberOfExtraObjects, glassBoxParticleParent, true);
            AddToPool(glassCylinderParticle, numberOfExtraObjects, glassCylinderParticleParent, true);
            AddToPool(glassSphereParticle, numberOfExtraObjects, glassSphereParticleParent, true);
            AddToPool(destroyerParticle, numberOfExtraObjects, destroyerParticleParent, true);
            AddToPool(jumpParticle, numberOfExtraObjects, jumpParticleParent, true);
        }

        /// <summary>
        /// Add objects to a pool
        /// </summary>
        /// <param name="prefab"></param> The prefab to be pooled.
        /// <param name="count"></param> The amount of copies to put in the pool.
        /// <param name="parent"></param> The parent where the pooled objects will be stored.
        /// <param name="disable"></param> If it should be disabled when it is pooled.
        /// <returns></returns>
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
                    PushToPool(obj, true, parent);
                }
            }
            return true;
        }

        /// <summary>
        /// Retrieve an object from the pool by its prefab gameobject.
        /// </summary>
        /// <param name="prefab"></param> The prefab gameobject to retrieve.
        /// <param name="forceInstantiate"></param> If a new gameobject should be created.
        /// <param name="instantiateIfNone"></param> If a new gameobject should be created if there are no objects in the pool.
        /// <param name="container"></param> Which parent the object should be placved in.
        /// <param name="SetActive"></param> If the object should be active when it is retrieved.
        /// <returns></returns>
        public GameObject PopFromPool(GameObject prefab, bool forceInstantiate = false, bool instantiateIfNone = false, Transform container = null, bool SetActive = true)
        {
            if (prefab == null)
            {
                return null;
            }
            return PopFromPool(prefab.name, forceInstantiate, instantiateIfNone, container, SetActive);
        }

        /// <summary>
        /// Retrieve an object from the pool by its prefab name.
        /// </summary>
        /// <param name="prefabName"></param> The prefab name of the object to retrieve.
        /// <param name="forceInstantiate"></param> If a new gameobject should be created.
        /// <param name="instantiateIfNone"></param> If a new gameobject should be created if there are no objects in the pool.
        /// <param name="container"></param> Which parent the object should be placved in.
        /// <param name="SetActive"></param> If the object should be active when it is retrieved.
        /// <returns></returns>
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

        /// <summary>
        /// This code is run if you choose to create a new object when retrieving from the pool.
        /// </summary>
        /// <param name="prefabName"></param> The prefab name of the object to retrieve.
        /// <param name="container"></param> Which parent the object should be placved in.
        /// <param name="SetActive"></param> If the object should be active when it is retrieved.
        /// <returns></returns>
        private GameObject CreateObject(string prefabName, Transform container, bool SetActive)
        {
            GameObject obj = (GameObject)Object.Instantiate(prefabContainer[prefabName]);
            obj.name = prefabName;
            obj.transform.parent = container;
            return obj;
        }

        /// <summary>
        /// Puts an object back into the pool.
        /// </summary>
        /// <param name="obj"></param> The game object to be put in the pool.
        /// <param name="retainObject"></param> If the object should be destroyed.
        /// <param name="parent"></param> The parent to return the game object to.
        public void PushToPool(GameObject obj, bool retainObject = true, Transform parent = null)
        {
            if (obj == null) { return; }
            if (retainObject == false)
            {
                Destroy(obj);
                return;
            }
            if (parent != null)
            {
                obj.transform.parent = parent;
            }
            Queue<GameObject> queue = FindInContainer(obj.name);
            queue.Enqueue(obj);
            obj.SetActive(false);
        }

        /// <summary>
        /// Removes or destroys an object from the pool.
        /// </summary>
        /// <param name="prefab"></param> The game object to be removed.
        /// <param name="destroyObject"></param> If it should be destroyed.
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
                    Destroy(obj);
                }
            }
        }

        /// <summary>
        /// Removes a pool from the list of pools.
        /// </summary>
        public void ReleasePool()
        {
            foreach (var kvp in this.container)
            {
                Queue<GameObject> queue = kvp.Value;
                while (queue.Count > 0)
                {
                    GameObject obj = queue.Dequeue();
                    Destroy(obj);
                }
            }
            this.container = null;
            this.container = new Dictionary<string, Queue<GameObject>>();
            this.prefabContainer.Clear();
            this.prefabContainer = null;
            this.prefabContainer = new Dictionary<string, GameObject>();
        }

        /// <summary>
        /// Returns an object from the pools by the game object's name.
        /// </summary>
        /// <param name="prefabName"></param> The name of the game object to be found.
        /// <returns></returns>
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
