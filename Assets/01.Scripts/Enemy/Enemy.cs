using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D bc;
    Animator animator;


    [SerializeField]
    float speed;
    [SerializeField]
    int curHp;
    [SerializeField]
    int maxHp;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        curHp = maxHp;
    }

    private void Die()
    {
        PoolManager.Instance.Return(gameObject);
    }
    public void TakeDamage(int damage)
    {
        curHp -= damage;

        if(curHp < 0)
            Die();
    }
}
