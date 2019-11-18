using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool //Pool class
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    #region Singleton thing
    public static ObjectPooler Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public List<Pool> pools; //List of object pools
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools) //For every object pool
        {
            Queue<GameObject> objectPool = new Queue<GameObject>(); //Make a queue for that pool

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab); //Instantiate objects for the pool as specified by pool size
                obj.SetActive(false); //Disable the spawned objects 
                objectPool.Enqueue(obj); //Add it to queue
            }

            //Add pool to Dictionary
            poolDictionary.Add(pool.tag, objectPool);
        }
         
    }

    public GameObject SpawnPoolObject (string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag)) //Check if tag exist; if it doesn't then
        {
            Debug.LogWarning(tag + "pool doesn't exist.");
            return null; //Spawn nothing
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true); //Enable it (since it was inactive in pool)
        objectToSpawn.transform.position = position; //Set the position for the object
        objectToSpawn.transform.rotation = rotation;  //Set the rotation for the object

        poolDictionary[tag].Enqueue(objectToSpawn); //Add it back to queue

        return objectToSpawn;
    }
}
