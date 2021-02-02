using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Bird : MonoBehaviour
{
    public enum BirdState { Idle, Thrown, HitSomething }
    public GameObject parent;
    public UnityAction OnBirdDestroy = delegate { };
    public UnityAction<Bird> OnBirdShot = delegate { };


    protected Rigidbody2D rb;
    private CircleCollider2D col;
    private BirdState state;
    private float minVelocity = 0.05f;
    private bool flagDestroy = false;

    private void Awake()
    {
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

        if (state == BirdState.Idle && rb.velocity.sqrMagnitude >= minVelocitySqr)
        {
            state = BirdState.Thrown;
        }

        if ((state == BirdState.Thrown || state == BirdState.HitSomething) && rb.velocity.sqrMagnitude < minVelocitySqr && !flagDestroy)
        {
            //Hancurkan gameobject setelah 2 detik
            //jika kecepatannya sudah kurang dari batas minimum

            //MENDING PAKE INVOKE
            flagDestroy = true;
            StartCoroutine(DestroyAfter(2));
        }
    }

    private IEnumerator DestroyAfter(float second)
    {
        yield return new WaitForSeconds(second);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        OnBirdDestroy();
    }

    public virtual void OnTap()
    {

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
        OnBirdShot(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        state = BirdState.HitSomething;
    }

    public BirdState State
    {
        get { return state; }
    }
}
