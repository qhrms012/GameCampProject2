using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D bc;
    Animator animator;

    public Enemy next;
    public Enemy prev;

    [SerializeField]
    Sprite headSprite;
    [SerializeField]
    Sprite bodySprite;

    SpriteRenderer sr;
    Color originalColor;

    Vector3 originalScale;

    [SerializeField]
    float speed;
    [SerializeField]
    float normalSpeed;
    [SerializeField]
    float boostSpeed;
    [SerializeField]
    float boostDuration;

    Coroutine boostCoroutine;


    [SerializeField]
    int curHp;
    [SerializeField]
    int maxHp;

    [SerializeField]
    bool isBody = true;
    Transform target;

    [SerializeField]
    Transform[] waypoints;
    int currentIndex;

    [SerializeField]
    float followDistance = 1.5f;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        originalScale = transform.localScale;

        originalColor = sr.color;
    }

    private void OnEnable()
    {
        curHp = maxHp;
        currentIndex = 0;

        UpdateSprite();

    }

    private void Update()
    {
        Move();
        AnimateScale();
    }

    private void AnimateScale()
    {
        float speed = isBody ? 0.1f : 0.5f;
        float amount = isBody ? 0.05f : 0.1f;


        float scale = 1 + Mathf.PingPong(Time.time * speed, amount);
        transform.localScale = originalScale * scale;
    }

    void Move()
    {
        // Body면 Head 따라가기
        if (isBody && target != null)
        {
            float dist = Vector2.Distance(transform.position, target.position);

            if (dist > followDistance)
            {
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    target.position,
                    speed * Time.deltaTime
                );
            }

            return;
        }

        // Head면 Waypoint 이동
        if (waypoints == null || waypoints.Length == 0)
            return;

        Transform waypointTarget = waypoints[currentIndex];

        transform.position = Vector2.MoveTowards(
            transform.position,
            waypointTarget.position,
            speed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, waypointTarget.position) < 0.1f)
        {
            currentIndex++;

            StartSpeedBoost();

            if (currentIndex >= waypoints.Length)
            {
                PoolManager.Instance.Return(gameObject);
            }
        }
    }

    private void StartSpeedBoost()
    {
        if (SpeedBoost() != null)
        {
            StopCoroutine(SpeedBoost());
        }

        StartCoroutine(SpeedBoost());
    }

    IEnumerator SpeedBoost()
    {
        speed = boostSpeed;

        transform.localScale *= 1.2f;
        sr.color = Color.hotPink;

        yield return new WaitForSeconds(boostDuration);

        speed = normalSpeed;

        transform.localScale /= 1.2f;
        sr.color = originalColor;


    }

    private void UpdateSprite()
    {
        if (isBody)
            sr.sprite = bodySprite;
        else
            sr.sprite = headSprite;
    }
    public void SetPath(Transform[] newPath)
    {
        waypoints = newPath;
        float minDist = Mathf.Infinity;
        int closestIndex = 0;

        for (int i = 0; i < waypoints.Length; i++)
        {
            float dist = Vector2.Distance(transform.position, waypoints[i].position);
            if (dist < minDist)
            {
                minDist = dist;
                closestIndex = i;
            }
        }

        currentIndex = closestIndex;
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }


    private void Die()
    {
        if (isBody)
        {
            // 앞뒤 연결 복구
            if (prev != null)
                prev.next = next;

            if (next != null)
            {
                next.prev = prev;

                //타겟 다시 연결
                if (prev != null)
                    next.SetTarget(prev.transform);
                else
                    next.BecomeHead(); // 앞이 없으면 head 승격
            }

            PoolManager.Instance.Return(gameObject);
            return;
        }

        // head 죽을 때
        if (!isBody)
        {
            if (next != null)
            {
                next.BecomeHead();
                next.SetPath(waypoints);
                
            }

            PoolManager.Instance.Return(gameObject);
        }
    }
    public void BecomeHead()
    {
        isBody = false;
        
        target = null; // 이제 따라가지 않음

        UpdateSprite();
    }

    IEnumerator HitEffect()
    {
        sr.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        sr.color = originalColor;
    }
    public void TakeDamage(int damage)
    {
        curHp -= damage;

        StartCoroutine(HitEffect());

        if(curHp <= 0)
            Die();
    }
}
