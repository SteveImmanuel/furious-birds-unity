using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : MonoBehaviour
{
    public GameObject trail;
    public Bird targetBird;

    private List<GameObject> trails;

    void Start()
    {
        trails = new List<GameObject>();
    }

    public void SetBird(Bird bird)
    {
        targetBird = bird;

        for (int i = 0; i < trails.Count; i++)
        {
            Destroy(trails[i].gameObject);
        }

        trails.Clear();
    }

    public IEnumerator SpawnTrail()
    {
        trails.Add(Instantiate(trail, targetBird.transform.position, Quaternion.identity));

        yield return new WaitForSeconds(0.1f);

        if (targetBird != null && targetBird.State != Bird.BirdState.HitSomething)
        {
            StartCoroutine(SpawnTrail());
        }
    }
}
