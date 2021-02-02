using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowBird : Bird
{
    public float boostForce = 100f;
    
    private bool hasBoost = false;

    public void Boost()
    {
        if (State == BirdState.Thrown && !hasBoost)
        {
            rb.AddForce(rb.velocity * boostForce, ForceMode2D.Impulse);
            hasBoost = true;
        }
    }

    public override void OnTap()
    {
        Boost();
    }
}
