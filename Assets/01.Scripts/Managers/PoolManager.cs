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

    public GameObject[] preFabs;

    List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[preFabs.Length];

        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(PoolType type)
    {
        int index = (int)type;
        GameObject select = null;

        foreach (GameObject item in pools[index])
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
            select = Instantiate(preFabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }

    public void Return(GameObject obj)
    {
        obj.SetActive(false);
    }
}

