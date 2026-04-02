using System.Collections.Generic;
using UnityEngine;


public enum PoolType
{
    PlayerBullet,
    Enemy,
    EnemyHead
}

public class PoolManager : Singleton<PoolManager>
{
    public PoolData[] poolDatas;

    Dictionary<PoolType, List<GameObject>> pools = new Dictionary<PoolType, List<GameObject>>();
    Dictionary<PoolType, GameObject> prefabDict = new Dictionary<PoolType, GameObject>();

    private void Awake()
    {
        base.Awake();

        foreach (var data in poolDatas)
        {

            if (pools.ContainsKey(data.type))
            {
                Debug.LogError($"중복된 PoolType: {data.type}");
                continue;
            }

            pools[data.type] = new List<GameObject>();
            prefabDict[data.type] = data.prefab;
        }
    }

    public GameObject Get(PoolType type)
    {

        if (!pools.ContainsKey(type))
        {
            Debug.LogError($"Pool 타입 없음: {type}");
            return null;
        }

        List<GameObject> pool = pools[type];


        foreach (GameObject item in pool)
        {
            if (!item.activeSelf)
            {
                item.SetActive(true);
                return item;
            }
        }
        GameObject prefab = GetPrefab(type);
        if (prefab == null) return null;

        GameObject obj = Instantiate(prefab, transform);


        PoolObject poolObj = obj.GetComponent<PoolObject>();
        if (poolObj != null && poolObj.poolType != type)
        {
            Debug.LogError($"타입 불일치! 요청:{type}, 프리팹:{poolObj.poolType}");
        }

        pool.Add(obj);
        return obj;
    }

    public void Return(GameObject obj)
    {
        if (obj == null) return;

        obj.SetActive(false);
    }

    GameObject GetPrefab(PoolType type)
    {
        if (prefabDict.TryGetValue(type, out GameObject prefab))
        {
            return prefab;
        }

        Debug.LogError($"프리팹 타입이 없음: {type}");
        return null;
    }
}

