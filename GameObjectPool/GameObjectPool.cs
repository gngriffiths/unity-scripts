using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] int poolSize;

    private Queue<GameObject> pool;

    void Awake()
    {
        // Initialize the object pool
        pool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, this.transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    // Use this function to get an object from the pool
    public GameObject GetObjectFromPool()
    {
        if (pool.Count == 0)
        {
            // Automatica pool size increase: If the pool is empty, create a new object, add it to the pool, and return it the calling method. 
            GameObject obj = Instantiate(prefab);
            return obj;
        }
        else
        {
            // If the pool has objects, dequeue and return it the calling method.
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
    }

    // Use this function to return an object to the pool
    public void ReturnObjectToPool(GameObject obj)
    {
        obj.transform.parent = transform;
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}