using System.Collections.Generic;
using UnityEngine;

// Attach to a GameObject with a GameObjectPool component.

public class GameObjectPoolManager : MonoBehaviour
{
    List<GameObject> activatedGameObjects = new List<GameObject>();

    GameObjectPool gameObjectPool
    {
        get
        {
            if (_gameObjectPool == null)
                _gameObjectPool = GetComponentInChildren<GameObjectPool>();
            return _gameObjectPool;
        }
    }
    GameObjectPool _gameObjectPool;

    public GameObject GetObjectFromPool()
    {
        GameObject obj = gameObjectPool.GetObjectFromPool();
        activatedGameObjects.Add(obj);
        return obj;

    }

    public void ReturnObjectToPool(GameObject gameObject)
    {
        // Get gameObject from acivatedGameObjects.
        var go = activatedGameObjects.Contains(gameObject);
        if (go)
        {
            gameObjectPool.ReturnObjectToPool(gameObject);
            activatedGameObjects.Remove(gameObject);
        }
    }

    public void ReturnAllToPool()
    {
        foreach (var gameObject in activatedGameObjects)
        {
            gameObjectPool.ReturnObjectToPool(gameObject);
        }
        activatedGameObjects.Clear();
    }

}

