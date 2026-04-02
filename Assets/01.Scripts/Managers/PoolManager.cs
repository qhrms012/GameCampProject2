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

    private void Awake()
    {
        foreach (var data in poolDatas)
            pools[data.type] = new List<GameObject>();
    }

    public GameObject Get(PoolType type)
    {
        List<GameObject> pool = pools[type];
        GameObject select = null;

        foreach (GameObject item in pool)
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }
        if (select == null)
        {
            GameObject prefab = GetPrefab(type);
            select = Instantiate(prefab, transform);
            pool.Add(select);
        }
        return select;
    }
    public void Return(GameObject obj)
    {
        obj.SetActive(false);
    }

    GameObject GetPrefab(PoolType type)
    {
        foreach (var data in poolDatas)
        {
            if (data.type == type)
                return data.prefab;
        }

        Debug.LogError($"프리팹 타입이 없음: {type}");
        return null;
    }
}

