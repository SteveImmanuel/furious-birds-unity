using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    public float health = 50f;

    private GameController gameController;
    private GameObject smokeParticle;
    private Rigidbody2D rb;
    private Collider2D col;
    protected SpriteRenderer spriteRenderer;

    private void Start()
    {
        InitializeComponent();
    }

    protected virtual void InitializeComponent()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        smokeParticle = transform.GetChild(0).gameObject;
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameController = GameController.instance;
    }

    protected virtual void OnDestroy()
    {
        gameController.DecreaseEnemy();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);

        float damage = contact.relativeVelocity.magnitude * 5;
        health -= damage;

        if (health <= 0)
        {
            StartCoroutine(DestroyAfter());
        }
    }

    protected IEnumerator DestroyAfter()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0;
        col.enabled = false;
        smokeParticle.SetActive(true);
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
