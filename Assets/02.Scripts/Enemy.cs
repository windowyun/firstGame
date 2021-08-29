using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    [SerializeField]
    protected float moveSpeed = 3f;
    [SerializeField]
    protected float attackDelay = 3f;
    [SerializeField]
    protected int superAromour = 3;
    [SerializeField]
    protected float superAromourTime = 5f;

    protected int hitNumber = 0;
    protected bool isSuperArmour = false;

    void Start()
    {
        currentHP = maxHP;
    }

    public override void HitOn(int damage, Transform transform = null)
    {
        currentHP = currentHP - damage;
    }
}
    
