using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Bird : MonoBehaviour
{
    public enum BirdState { Idle, Thrown, HitSomething }
    public UnityAction<Bird> OnBirdShot = delegate { };

    protected Rigidbody2D rb;
    private CircleCollider2D col;
    private BirdState state;
    private float minVelocity = 0.05f;
    private bool flagDestroy = false;
    private GameObject furParticle;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        InitializeComponent();
    }

    protected virtual void InitializeComponent()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        furParticle = transform.GetChild(0).gameObject;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
    }

    void Start()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        col.enabled = false;
        state = BirdState.Idle;
    }

    void FixedUpdate()
    {
        float minVelocitySqr = minVelocity * minVelocity;

        if ((state == BirdState.Thrown || state == BirdState.HitSomething) && rb.velocity.sqrMagnitude < minVelocitySqr && !flagDestroy)
        {
            flagDestroy = true;
            StartCoroutine(DestroyAfter(2));
        }
    }

    protected IEnumerator DestroyAfter(float second)
    {
        furParticle.SetActive(true);
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(1);
        yield return new WaitForSeconds(1 - second);
        Destroy(gameObject);
    }

    public virtual void OnTap()
    {
    }

    private void OnDestroy()
    {
        GameController.instance.DecreaseBird();
    }

    public void MoveTo(Vector2 target, GameObject parent)
    {
        gameObject.transform.SetParent(parent.transform);
        gameObject.transform.position = target;
    }

    public void Shoot(Vector2 direction, float distance, float speed)
    {
        col.enabled = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = direction * speed * distance;
        state = BirdState.Thrown;
        OnBirdShot(this);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (state == BirdState.Thrown)
        {
            state = BirdState.HitSomething;
        }
    }

    public BirdState State
    {
        get { return state; }
    }
}
