using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShooter : MonoBehaviour
{

    [SerializeField]
    private float radius = 0.75f;
    [SerializeField]
    private float throwSpeed = 30f;
    private Vector2 startPos;
    private CircleCollider2D col;
    private Bird bird;

    private void Awake()
    {
        col = GetComponent<CircleCollider2D>();
    }

    void Start()
    {
        startPos = transform.position;
    }

    public void InitiateBird(Bird newBird)
    {
        bird = newBird;
        bird.MoveTo(gameObject.transform.position, gameObject);
        col.enabled = true;
    }

    void OnMouseUp()
    {
        col.enabled = false;
        Vector2 direction = (startPos - (Vector2) transform.position).normalized;
        float distance = Vector2.Distance(startPos, transform.position);

        bird.Shoot(direction, distance, throwSpeed);

        gameObject.transform.position = startPos;
    }

    void OnMouseDrag()
    {
        Vector2 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = p - startPos;

        if (dir.sqrMagnitude > radius * radius) //KESALAHAN DI TUTORIAL TIDAK DIKUADRATKAN
        {
            dir = dir.normalized * radius;
        }

        transform.position = startPos + dir;
    }
}