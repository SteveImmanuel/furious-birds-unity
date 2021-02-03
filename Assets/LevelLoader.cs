using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class LevelLoader : MonoBehaviour
{
    private Animator animator;
    private int levelBuildIndex;

    [HideInInspector]
    public static LevelLoader instance;

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
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void LoadNextLevel()
    {
        FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void FadeToLevel(int index)
    {
        animator.SetTrigger("FadeOut");
        levelBuildIndex = index;
    }

    public void OnFadeOutComplete()
    {
        if (levelBuildIndex < 4)
        {
            SceneManager.LoadScene(levelBuildIndex);
        }
        else
        {

        }
    }
}
