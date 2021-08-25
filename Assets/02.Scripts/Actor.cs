using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] 
    protected int maxHP;
    public int MaxHP
    {
        get { return maxHP; }
    }

    [SerializeField]
    protected int currentHP;

    public int CurrentHP
    {
        get { return currentHP; }
    }

    [SerializeField]
    protected int damage;
    
    void Start()
    {
        currentHP = maxHP;
    }

    public virtual void AttackOn()
    {

    }

    public virtual void HitOn(int damage, Transform transform = null)
    {

    }


}
