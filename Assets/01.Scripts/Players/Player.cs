using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    private Rigidbody2D rb;
    private BoxCollider2D bx;
    Animator animator;

    [SerializeField]
    float speed;
    [SerializeField]
    Transform firePoint;
    [SerializeField]
    float fireRate = 0.3f;
    float timer;

    private Vector2 playerVector;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bx = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= fireRate)
        {
            AutoFire();
            timer = 0;
        }
    }
    private void AutoFire()
    {
        GameObject bullet = PoolManager.Instance.Get(PoolType.PlayerBullet);
        AudioManager.Instance.PlaySfx(Sfx.Shot);
        bullet.transform.position = firePoint.position;
    }
    private void FixedUpdate()
    {
        Vector2 vector2 = playerVector.normalized * speed * Time.deltaTime;

        rb.MovePosition(rb.position + vector2);
    }
    private void OnMove(InputValue value)
    {
        playerVector = value.Get<Vector2>();
    }
    
}
