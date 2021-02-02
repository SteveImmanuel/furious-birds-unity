using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bird : MonoBehaviour
{
    public enum BirdState { Idle, Thrown }
    public GameObject parent;
    public UnityAction OnBirdDestroy;

    private Rigidbody2D rb;
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

        if (state == BirdState.Thrown && rb.velocity.sqrMagnitude < minVelocitySqr && !flagDestroy)
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
        if (OnBirdDestroy != null)
        {
            OnBirdDestroy();
        }
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

    }
}
