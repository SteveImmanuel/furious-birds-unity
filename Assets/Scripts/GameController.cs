using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GameController : MonoBehaviour
{
    public SlingShooter slingShooter;
    public TrailController TrailController;
    public List<Bird> birds;
    public List<Enemy> enemies;

    private bool isGameEnded = false;
    private Bird shotBird;
    private BoxCollider2D tapArea;

    private void Awake()
    {
        tapArea = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        for(int i = 0; i < birds.Count; i++)
        {
            birds[i].OnBirdDestroy += ChangeBird;
            birds[i].OnBirdShot += AssignTrail;
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].OnEnemyDestroyed += CheckGameEnd;
        }
        slingShooter.InitiateBird(birds[0]);
        tapArea.enabled = false;
    }

    void ChangeBird()
    {
        tapArea.enabled = false;
        shotBird = null;

        if (isGameEnded)
        {
            return;
        }

        birds.RemoveAt(0);
        
        if (birds.Count > 0)
        {
            slingShooter.InitiateBird(birds[0]);
        }
    }

    public void AssignTrail(Bird bird)
    {
        shotBird = bird;
        TrailController.SetBird(bird);
        StartCoroutine(TrailController.SpawnTrail());
        tapArea.enabled = true;
    }

    public void CheckGameEnd(GameObject destroyedEnemy)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].gameObject == destroyedEnemy)
            {
                enemies.RemoveAt(i);
                break;
            }
        }

        if (enemies.Count == 0)
        {
            isGameEnded = true;
        }
    }

    private void OnMouseDown()
    {
        if (shotBird != null)
        {
            tapArea.enabled = false;
            shotBird.OnTap();
        }
    }
}
