using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public float health = 50f;

    private GameController gameController;

    private void Start()
    {
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
                Destroy(gameObject);
            }
        }
    }
}
