using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : MonoBehaviour
{
    public GameObject trail;

    private List<GameObject> trails;
    private int counter = 0;
    private Vector3 lastPosition;
    private Bird targetBird;

    void Start()
    {
        trails = new List<GameObject>();
    }

    public void ShowTrail(Bird bird)
    {
        targetBird = bird;

        for (int i = 0; i < trails.Count; i++)
        {
            Destroy(trails[i].gameObject);
        }

        trails.Clear();
        StartCoroutine(SpawnTrail());
    }

    public IEnumerator SpawnTrail()
    {
        Vector2 currentPos = targetBird.transform.position;
        while (Vector2.Distance(currentPos, lastPosition) < .8)
        {
            if (targetBird != null)
            {
                currentPos = targetBird.transform.position;
            }
            yield return null;
        }

        GameObject spawned = Instantiate(trail, currentPos, Quaternion.identity);
        lastPosition = targetBird.transform.position;
        trails.Add(spawned);
        if (counter % 5 == 0)
        {
            spawned.transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
        }
        counter++;

        if (targetBird != null && targetBird.State != Bird.BirdState.HitSomething)
        {
            StartCoroutine(SpawnTrail());
        }
    }
}
