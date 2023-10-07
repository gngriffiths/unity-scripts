using System.Collections.Generic;
using UnityEngine;

// Attach to a GameObject with a GameObjectPool component.

public class GameObjectPoolManager : MonoBehaviour
{
    List<GameObject> activatedGameObjects;

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

    void ReturnAllToPool()
    {
        foreach (var gameObject in activatedGameObjects)
        {
            gameObjectPool.ReturnObjectToPool(gameObject);
        }
        activatedGameObjects.Clear();
    }

    private void RemovePoint()
    {
        if (activatedGameObjects.Count == 0)
            return;

        gameObjectPool.ReturnObjectToPool(activatedGameObjects[0]);
        activatedGameObjects.RemoveAt(0);
    }
}

