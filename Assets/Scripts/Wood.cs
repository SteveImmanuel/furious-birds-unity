using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : Enemy
{
    public Sprite brokenSprite;
    private float fullHealth;

    protected override void InitializeComponent()
    {
        base.InitializeComponent();
        fullHealth = health;
    }
    protected override void OnDestroy()
    {
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (health <= fullHealth / 2)
        {
            spriteRenderer.sprite = brokenSprite;
        }
    }
}
