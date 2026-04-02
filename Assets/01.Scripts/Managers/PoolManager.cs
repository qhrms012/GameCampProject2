using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    Dictionary<string, Queue<GameObject>> poolDict = new Dictionary<string, Queue<GameObject>>();

    public GameObject Get(string key, GameObject prefab)
    {

        if(!poolDict.ContainsKey(key))
            poolDict.Add(key, new Queue<GameObject>());

        if (poolDict[key].Count > 0)
        {
            GameObject obj = poolDict[key].Dequeue();
            obj.SetActive(true);
            return obj;
        }
        return Instantiate(prefab);

    }
}
