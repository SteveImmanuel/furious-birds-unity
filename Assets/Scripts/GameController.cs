using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public SlingShooter SlingShooter;
    public List<Bird> Birds;

    void Start()
    {
        for(int i = 0; i < Birds.Count; i++)
        {
            Birds[i].OnBirdDestroy += ChangeBird;
        }
        SlingShooter.InitiateBird(Birds[0]);
    }

    void ChangeBird()
    {
        Birds.RemoveAt(0);
        
        if (Birds.Count > 0)
        {
            SlingShooter.InitiateBird(Birds[0]);
        }
    }
}
