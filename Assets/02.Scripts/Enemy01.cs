using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy01 : Enemy
{

    [Header("Attack")]
    [SerializeField]
    Transform pos;
    [SerializeField]
    Vector2 boxSize;
    [SerializeField]
    float fieldOfVision;
    [SerializeField]
    float attackOfVision;

    GameObject target;

    Rigidbody2D rigid2D;
    Animator animator;

    bool usedTurn = false;
    bool firstTurn = true;
    bool moveOn = false;
    bool isAttack = false;
    
    float attackTime = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigid2D = GetComponent<Rigidbody2D>();

        attackTime -= attackDelay;

        target = GameManager.Instance.ChangeManager.CurrentSkull;
    }

    void Update()
    {
        target = GameManager.Instance.ChangeManager.CurrentSkull;

        float distance = Vector3.Distance(target.transform.position , transform.position);
        if (distance <= fieldOfVision)
        {
            Turn(distance);

            if (distance <= attackOfVision)
            {
                moveOn = false;
                animator.SetBool("enemy01Move", false);
                rigid2D.velocity = new Vector2(0f, rigid2D.velocity.y);

                if (Time.time - attackTime > attackDelay)
                {
                    attackTime = Time.time;

                    isAttack = true;
                    
                    animator.SetTrigger("enemy01Attack");
                }
            }

            else if (usedTurn && !isAttack)
            {
                moveOn = true;
            }
        }

        else
        {
            moveOn = false;
            animator.SetBool("enemy01Move", false);
        }
    }

    void FixedUpdate()
    {

        if (moveOn)
        {
            MoveToTarget();
        }
    }

    public override void AttackOn()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0f);
        for (int i = 0; i < collider2Ds.Length; i++)
        {
            if (collider2Ds[i].tag == "Player")
            {
                collider2Ds[i].GetComponent<Actor>().HitOn(damage, transform);
            }
        }
    }

    void AttackOff()
    {
        isAttack = false;
    }

    public override void HitOn(int damage, Transform transform = null)
    {
        base.HitOn(damage);
        usedTurn = true;
        isAttack = false;
        animator.SetTrigger("enemy01Hit");
    }

    void Turn(float distance)
    {
        if (!usedTurn && firstTurn && (distance >= fieldOfVision - 0.75f) && (((target.transform.position.x > transform.position.x) && transform.localScale.x > 0)
                        || ((target.transform.position.x < transform.position.x) && transform.localScale.x < 0)))
        {
            firstTurn = false;
            animator.SetTrigger("enemy01Turn");
        }
        else if (isAttack == false && usedTurn && (target.transform.position.x >= transform.position.x + 0.36f) && transform.localScale.x > 0)
        {
            transform.position = new Vector2(transform.position.x + 0.72f, transform.position.y);
            transform.localScale = new Vector3(-3f, 3f, 1f);
            Debug.Log(isAttack);
        }

        else if (isAttack == false && usedTurn && (target.transform.position.x < transform.position.x - 0.36f) && transform.localScale.x < 0)
        {
            transform.position = new Vector2(transform.position.x - 0.72f, transform.position.y);
            transform.localScale = new Vector3(3f, 3f, 1f);
            Debug.Log(isAttack);
        }
    }

    void TurnOff()
    {
        if (transform.localScale.x > 0)
            transform.position = new Vector2(transform.position.x + 0.72f, transform.position.y);
        else
            transform.position = new Vector2(transform.position.x - 0.72f, transform.position.y);

        transform.localScale = new Vector3(transform.localScale.x * -1, 3f, 1f);

        usedTurn = true;
    }

    void MoveToTarget()
    {
        float dir = transform.position.x > target.transform.position.x ? -1f : 1f;

        rigid2D.velocity = new Vector2(dir * moveSpeed, rigid2D.velocity.y);
        animator.SetBool("enemy01Move", true);
    }

    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, fieldOfVision);
        Gizmos.DrawWireSphere(transform.position, fieldOfVision - 0.75f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackOfVision);
        Gizmos.DrawWireSphere(transform.position - new Vector3(0.36f, 0f, 0f), 0.2f);
    }
    */
}
