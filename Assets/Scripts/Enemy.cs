using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    public float health = 50f;

    private GameController gameController;
    private GameObject smokeParticle;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private CircleCollider2D col;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        smokeParticle = transform.GetChild(0).gameObject;
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameController = GameController.instance;
    }
    private void OnDestroy()
    {
        gameController.DecreaseEnemy();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float damage = collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 10;
            health -= damage;

            if (health <= 0)
            {
                StartCoroutine(DestroyAfter());
            }
        }
    }

    protected IEnumerator DestroyAfter()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = Vector3.zero;
        col.enabled = false;
        smokeParticle.SetActive(true);
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
