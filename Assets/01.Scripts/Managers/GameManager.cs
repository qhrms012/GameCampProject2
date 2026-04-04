using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{


    [SerializeField]
    float spawnRate = 1f;
    float timer;
    [SerializeField]
    int bodyCount;


    [SerializeField]
    Transform[] waypoints;

    private void Awake()
    {

    }
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

        Enemy head = PoolManager.Instance.Get(PoolType.EnemyHead).GetComponent<Enemy>();
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
}
