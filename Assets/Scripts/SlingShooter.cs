using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class SlingShooter : MonoBehaviour
{
    public LineRenderer trajectory;
    public int trajectorySegmentCount = 5;
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
        Vector2 direction = (startPos - (Vector2)transform.position).normalized;
        float distance = Vector2.Distance(startPos, transform.position);

        bird.Shoot(direction, distance, throwSpeed);

        gameObject.transform.position = startPos;
        trajectory.enabled = false;
    }

    void OnMouseDrag()
    {
        if (bird == null)
        {
            return;
        }

        Vector2 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = p - startPos;

        if (dir.sqrMagnitude > radius * radius)
        {
            dir = dir.normalized * radius;
        }

        transform.position = startPos + dir;
        float distance = Vector2.Distance(startPos, transform.position);

        if (!trajectory.enabled)
        {
            trajectory.enabled = true;
        }

        DisplayTrajectory(distance);
    }

    void DisplayTrajectory(float distance)
    {
        Vector2 direction = (startPos - (Vector2)transform.position).normalized;
        Vector2[] segments = new Vector2[trajectorySegmentCount];
        Vector2 shootVelocity = direction * throwSpeed * distance;

        segments[0] = transform.position;
        for (int i = 1; i < trajectorySegmentCount; i++)
        {
            float elapsedTime = i * Time.fixedDeltaTime * 20 / shootVelocity.magnitude; //make elapsed time inversely proportional with speed
            segments[i] = segments[0] + shootVelocity * elapsedTime + 0.5f * Physics2D.gravity * Mathf.Pow(elapsedTime, 2);
        }

        trajectory.positionCount = trajectorySegmentCount;
        for (int i = 0; i < trajectorySegmentCount; i++)
        {
            trajectory.SetPosition(i, segments[i]);
        }
    }
}