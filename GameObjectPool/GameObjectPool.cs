using System.Collections.Generic;
using UnityEngine;
using VContainer;

public abstract class GameObjectPool : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] int poolSize;

    private IObjectResolver resolver;
    private List<GameObject> activatedGameObjects = new List<GameObject>();
    private Queue<GameObject> pool;

    [Inject]
    public void Construct(IObjectResolver resolver)
    {
        this.resolver = resolver;
    }

    void Awake()
    {
        // Initialize the object pool
        pool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            //GameObject obj = Instantiate(prefab, this.transform);
            GameObject obj = CreateNewObject();
            pool.Enqueue(obj);
        }
    }

    GameObject CreateNewObject()
    {
        GameObject obj = Instantiate(prefab, this.transform);
        obj.SetActive(false);
        // Inject dependencies into each MonoBehaviour component of the GameObject.
        foreach (var component in obj.GetComponents<MonoBehaviour>())
        {
            resolver.Inject(component);
        }
        return obj;
    }

    // Use this function to get an object from the pool
    public GameObject GetObjectFromPool()
    {
        GameObject obj;

        if (pool.Count == 0)
        {
            // Automatica pool size increase: If the pool is empty, create a new object, add it to the pool, and return it the calling method. 
            obj = Instantiate(prefab);
        }
        else
        {
            // If the pool has objects, dequeue and return it the calling method.
            obj = pool.Dequeue();
            obj.SetActive(true);
        }

        activatedGameObjects.Add(obj);
        return obj;
    }

    // Use this function to return an object to the pool
    public void ReturnObjectToPool(GameObject obj)
    {
        bool go = activatedGameObjects.Contains(obj);
        if (!go)
            return;

        obj.transform.parent = transform;
        obj.SetActive(false);
        pool.Enqueue(obj);
        activatedGameObjects.Remove(obj);
    }

    public void ReturnObjectsToPool(List<GameObject> objs)
    {
        foreach (var obj in objs)
        {
            ReturnObjectToPool(obj);
        }
    }

    public void ReturnAllToPool()
    {
        foreach (var obj in activatedGameObjects)
        {
            obj.transform.parent = transform;
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
        activatedGameObjects.Clear();
    }
}