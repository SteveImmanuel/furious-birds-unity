using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GameController : MonoBehaviour
{
    public SlingShooter slingShooter;
    public TrailController trailController;
    public List<Bird> birds;
    public List<Enemy> enemies;

    private bool isGameEnded = false;
    private Bird shotBird;
    private BoxCollider2D tapArea;
    private int enemyCount;
    [HideInInspector]
    public static GameController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        tapArea = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        for (int i = 0; i < birds.Count; i++)
        {
            birds[i].OnBirdDestroy += ChangeBird;
            birds[i].OnBirdShot += AssignTrail;
        }

        slingShooter.InitiateBird(birds[0]);
        enemyCount = enemies.Count;
        tapArea.enabled = false;
    }

    void ChangeBird()
    {
        tapArea.enabled = false;

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
        trailController.ShowTrail(bird);
        tapArea.enabled = true;
    }


    public void DecreaseEnemy()
    {
        enemyCount--;
        if (enemyCount == 0)
        {
            isGameEnded = true;
        }
    }

    private void OnMouseDown()
    {
        tapArea.enabled = false;
        shotBird.OnTap();
    }
}
