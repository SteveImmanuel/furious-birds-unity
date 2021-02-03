using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(TrailController))]
public class GameController : MonoBehaviour
{
    public SlingShooter slingShooter;
    public List<Bird> birds;
    public int enemyCount;

    private TrailController trailController;
    private bool isGameEnded = false;
    private Bird shotBird;
    private BoxCollider2D tapArea;
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
        trailController = GetComponent<TrailController>();
    }

    void Start()
    {
        for (int i = 0; i < birds.Count; i++)
        {
            birds[i].OnBirdShot += AssignTrail;
        }

        slingShooter.InitiateBird(birds[0]);
        tapArea.enabled = false;
    }

    private void Update()
    {
        if (isGameEnded)
        {
            LevelLoader.instance.LoadNextLevel();
        }

        if (birds.Count == 0)
        {
            LevelLoader.instance.FadeToLevel(0);
        }
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
        ChangeBird();
        CameraController.instance.SetFollowTarget(bird.transform);
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
        shotBird.OnTap();
        tapArea.enabled = false;
    }
}
