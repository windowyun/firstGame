using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    [SerializeField]
    protected float moveSpeed = 3f;
    [SerializeField]
    protected float attackDelay = 3f;

    void Start()
    {
        currentHP = maxHP;
    }

    void Update()
    {

    }

    public override void HitOn(int damage, Transform transform = null)
    {
        currentHP = currentHP - damage;
    }
}
    
