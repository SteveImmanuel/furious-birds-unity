using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrownBird : Bird
{
    public float explosionRadius;
    public float explosionForce;
    public LayerMask mask;

    private GameObject explosionParticle;
    private bool hasExploded = false;

    protected override void InitializeComponent()
    {
        base.InitializeComponent();
        explosionParticle = transform.GetChild(1).gameObject;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        Explode();
    }

    void Explode()
    {
        if (!hasExploded)
        {
            hasExploded = true;
            explosionParticle.SetActive(true);
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = Vector2.zero;

            Vector2 currentPos = transform.position;
            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, explosionRadius, mask);

            foreach (Collider2D collider in colliderArray)
            {
                Rigidbody2D rb = collider.attachedRigidbody;
                Vector2 force = rb.position - currentPos;
                rb.AddForce(force * explosionForce, ForceMode2D.Impulse);
                Debug.Log(collider.tag + collider.name);
            }

            StartCoroutine(DestroyAfter(2));
        }
    }

    public override void OnTap()
    {
        Explode();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
