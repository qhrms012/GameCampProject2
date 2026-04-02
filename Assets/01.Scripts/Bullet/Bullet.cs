using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    int damage;

    private void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //collision.GetComponent<Enemy>().TakeDamage(damage);
            PoolManager.Instance.Return(gameObject);
        }
    }
}
