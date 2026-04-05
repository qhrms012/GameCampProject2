using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    float spawnRate = 1f;
    [SerializeField]
    int bodyCount;

    [SerializeField]
    int currentBodyCount;

    [SerializeField]
    Transform[] waypoints;

    Enemy currentHead;


    private void Start()
    {
        SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        StartCoroutine(SpawnChain());
    }

    IEnumerator SpawnChain()
    {
        currentBodyCount = bodyCount + 1;
        Enemy head = PoolManager.Instance.Get(PoolType.EnemyHead).GetComponent<Enemy>();
        currentHead = head;

        head.transform.position = waypoints[0].position;
        head.SetPath(waypoints);

        Enemy prev = head;

        for (int i = 0; i < bodyCount; i++)
        {
            yield return new WaitForSeconds(0.2f); //µô·¹ÀÌ

            Enemy body = PoolManager.Instance.Get(PoolType.Enemy).GetComponent<Enemy>();
            body.transform.position = prev.transform.position - Vector3.down * 2f; // À§Ä¡ ¸ÂÃçÁÖ±â
            body.SetTarget(prev.transform);

            body.prev = prev;
            prev.next = body;
            prev = body;
        }
    }

    public void OnBodyDead()
    {
        currentBodyCount--;

        if (currentBodyCount <= 0)
        {
            UIManager.Instance.GameClear();
        }
    }

    public void TestLose()
    {
        currentHead.ForceReachEnd();
    }

}

