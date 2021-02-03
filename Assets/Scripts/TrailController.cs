using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : MonoBehaviour
{
    public GameObject trail;
    public Bird targetBird;

    private List<GameObject> trails;
    private int counter = 0;
    private Vector3 lastPosition;

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
        while (Vector2.Distance(targetBird.transform.position, lastPosition) < .8)
        {
            yield return null;
        }

        GameObject spawned = Instantiate(trail, targetBird.transform.position, Quaternion.identity);
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
