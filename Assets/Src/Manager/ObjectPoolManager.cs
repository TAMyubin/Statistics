using UnityEngine;
using System.Collections.Generic;
public class ObjectPoolManager
{
    private static ObjectPoolManager _instance;
    public static ObjectPoolManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ObjectPoolManager();
            }
            return _instance;
        }
    }

    private GameObject _poolRoot;

    private ObjectPoolManager()
    {
        _poolRoot = new GameObject("PoolRoot");
        GameObject.DontDestroyOnLoad(_poolRoot);
    }

    private Dictionary<int, Queue<GameObject>> _objectPool = new Dictionary<int, Queue<GameObject>>();

    public void ClearAllObject()
    {
        foreach (int prefabID in _objectPool.Keys)
        {
            foreach (GameObject obj in _objectPool[prefabID])
            {
                GameObject.Destroy(obj);
            }
        }
        _objectPool.Clear();
    }

    public void ClearPrefabeObject(GameObject prefab)
    {
        int prefabID = prefab.GetInstanceID();
        if (_objectPool.ContainsKey(prefabID))
        {
            foreach (GameObject obj in _objectPool[prefabID])
            {
                GameObject.Destroy(obj);
            }
            _objectPool.Remove(prefabID);
        }
    }

    public GameObject GetPrefabObject(GameObject prefab,Transform parent = null)
    {
        int prefabID = prefab.GetInstanceID();
        if (_objectPool.ContainsKey(prefabID) && _objectPool[prefabID].Count > 0)
        {
            GameObject ret = _objectPool[prefabID].Dequeue();
            ret.transform.SetParent(null, false);
            ret.SetActive(true);
            return ret;
        }
        else
        {
            GameObject ret = GameObject.Instantiate(prefab,parent);
            ret.SetActive(true);
            return ret;
        }
    }

    public void RecoverPrefabObject(GameObject prefab, GameObject obj)
    {
        RecoverPrefabObject(prefab.GetInstanceID(), obj);
    }

    public void RecoverPrefabObject(int prefabID, GameObject obj)
    {
        if (!obj) return;
        if (!_objectPool.ContainsKey(prefabID))
        {
            _objectPool.Add(prefabID, new Queue<GameObject>());
        }
        obj.SetActive(false);
        obj.transform.SetParent(_poolRoot.transform);
        // obj.transform.localPosition = Vector3.zero;
        _objectPool[prefabID].Enqueue(obj);
    }
}