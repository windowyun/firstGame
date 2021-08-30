using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csPlayer : Actor
{
    [SerializeField] float scale = 3f;
    [SerializeField] Vector3 effectPoint;

    [Header("Move & Jump & Roll")]
    [SerializeField] float speed = 3.0f;
    [SerializeField] float rollSpeed = 4.0f;
    [SerializeField] float jumpPower = 4.0f;
    /*
    [Header("BoxCast")]
    [SerializeField] Vector2 boxCastSize = new Vector2(0.28f, 0.01f);
    [SerializeField] float boxCastDistance = 0.03f;
    [SerializeField] Vector3 boxCastStart = new Vector3(0.0f, -1.1f, 0.0f);
    */
    //[Header("OverlapCircle")]
    //[SerializeField] Vector3 overlapCircleStart = new Vector3(0.0f, -1.1f, 0.0f);
    //[SerializeField] float overlapCircleRadius = 0.03f;

    [Header("CoolTime")]
    [SerializeField] float rollCoolTime = 3.0f;
    [SerializeField] float rollTime = 0f;
    [SerializeField] float invincibilityTime = 0.8f;
    //[SerializeField] float attackCoolTime = 0f;
    //[SerializeField] float attackTime = 0f;

    [Header("Attack")]
    [SerializeField] Transform pos;
    [SerializeField] Vector2 boxSize;
    
    bool isJump = true;
    public bool IsJump
    {
        get { return isJump; }
    }

    bool isRoll = false;
    bool invincibilityOn = false;

    bool stopAct = false;
    public bool StopAct
    {
        get { return stopAct; }
    }

    //GameManager gameManager = new GameManager();

    Rigidbody2D rigid = new Rigidbody2D();
    Transform trans;
    SpriteRenderer render;
    Animator anim;
    CapsuleCollider2D coli;
    Collider2D coli2;
    GameManager gameManager;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
        render = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coli = GetComponent<CapsuleCollider2D>();

    }

    void Start()
    {
        rollTime -= rollCoolTime;    
    }

    void Update()
    {
        if (!stopAct)
        {
            FilpX();

            MoveAnim();

            Jump();

            Laying();

            Attack();

            Roll();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            HitOn(damage, transform);
        }

    }

    void FixedUpdate()
    {
        //rigidBody2D.velocity 이동 velocity : 리지드 바디의 현재 속도
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        //float hf = Input.GetAxisRaw("Horzontal");

        if (isRoll)
        {
            if (transform.localScale.x > 0)
                rigid.velocity = new Vector2(1f * rollSpeed, rigid.velocity.y);
            else
                rigid.velocity = new Vector2(-1f * rollSpeed, rigid.velocity.y);
        }


        else if (!stopAct)
        {
            rigid.velocity = new Vector2(h * speed, rigid.velocity.y);
            //Debug.Log(rigid.velocity.x);
        }
    }

    

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //attackTime = 0f; //쿨타임 on
            anim.SetTrigger("Attack");

            //coli2.GetComponent<Enemy01>().enemyHit(Damage);
        }
    }

    void AttackTrue()
    {

        //float distance = render.flipX == true ? 0.44f : 0;

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0f);
        //Debug.Log(new Vector2(pos.position.x- distance, pos.position.y));
        //Collider2D[] collider2Ds = Physics2D.OverlapBoxAll( new Vector2(pos.position.x - distance, pos.position.y), boxSize, 0f);

        for (int i = 0;  i < collider2Ds.Length; i++)
        {
            if(collider2Ds[i].tag == "Enemy")
                collider2Ds[i].GetComponent<Enemy>().HitOn(damage);
        }
    }

    void Laying()//눕기
    {
        if (Input.GetKey(KeyCode.DownArrow) && rigid.velocity == Vector2.zero)
        {
            anim.SetBool("laying", true);
            coli.offset = new Vector2(0, -0.25f);
            coli.size = new Vector2(0.13f, 0.25f);
        }
        else
        {
            anim.SetBool("laying", false);
            coli.offset = new Vector2(0, -0.21f);
            coli.size = new Vector2(0.13f, 0.33f);
        }
    }

    void Move()//움직임
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Move 모션 -> AnimationManager에서 사용
        //if (Input.GetButton("Horizontal"))
        //    anim.SetBool("moving", true);
        //else
        //    anim.SetBool("moving", false);

        //Postion 직접 변경
        //postion.x += h * speed * Time.deltaTime;
        //trans.position = postion;

        //TransLate
        //trans.Translate(new Vector3(h * speed * Time.deltaTime, 0, 0));

        //rigidBody2D.velocity 이동 velocity : 리지드 바디의 현재 속도
        rigid.velocity = new Vector2(h * speed, rigid.velocity.y);
    }

    void MoveAnim()
    {
        if (Input.GetButton("Horizontal"))
            anim.SetBool("moving", true);
        else
            anim.SetBool("moving", false);
    }

    void Roll()//구르기
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && IsJump && Time.time - rollTime > rollCoolTime)
        {
            rollTime = Time.time;

            anim.SetTrigger("Roll");

            isRoll = true;
            stopAct = true;
            invincibilityOn = true;
        }
        /*
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Skull2Roll"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
            {
                isRoll = false;
            }

            else if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f)
            {
                isRoll = true;
            }
        }
        */
    }

    void RollOff()//구르기 종료
    {
        isRoll = false;
        invincibilityOn = false;
        stopAct = false;
    }

    void Jump()//점프
    {
        anim.SetFloat("velocityY", rigid.velocity.y);

        // OverlapCircle로 땅 확인
        //isJump = Physics2D.OverlapCircle(trans.position + overlapCircleStart, overlapCircleRadius, LayerMask.GetMask("Ground"));

        /*
        // boxcast로 땅 확인
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(trans.position + boxCastStart, boxCastSize, 0f, Vector2.down, boxCastDistance, LayerMask.GetMask("Ground"));
        if (raycastHit2D.collider != null)
        {
            isJump = false;
            anim.SetBool("jumping", false);
        }
        */

        if (Input.GetButtonDown("Jump") && IsJump)// && isGround == true)
        {
            isJump = false;

            anim.SetTrigger("Jump");

            anim.SetBool("jumping", true);

            //velocity 점프
            //rigid.velocity = Vector2.up * jumpPower;
            //최고 속도 제한
            /*
            if (Mathf.Abs(rigid.velocity.x) > 3)
            {
                rigid.velocity = new Vector2(3.0f * h, rigid.velocity.y);
            }
            */
            //anim.SetBool("jumping", true);
            //AddForce 점프
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }

        
    }

    void FilpX() //방향 전환
    {

        /*
        if (Input.GetButton("Horizontal"))
        {
            render.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }
        */

        if (Input.GetAxis("Horizontal") > 0)
        {
            //render.flipX = false;
            transform.localScale = new Vector3(scale, scale, 1f);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            //render.flipX = true;
            transform.localScale = new Vector3(-scale, scale, 1f);
        }
    }

    //void coolTime()//쿨타임
    //{
    //    rollTime += Time.deltaTime;
    //    //attackTime += Time.deltaTime;

    //    if (rollTime > 100)
    //        rollTime = rollCoolTime;
    //    /*
    //    if (attackTime > 100)
    //        attackTime = attackCoolTime;
    //    */
    //}

    
    // OncpllisionEnter 으로 땅 확인
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJump = UpOrDown(collision);
        }
    }

    bool UpOrDown(Collision2D _col)
    {
        Vector3 distVec = transform.position - _col.transform.position;
        if (Vector3.Cross(_col.transform.right, distVec).z > 0)
        {
            //Debug.Log("Up");
            //Debug.Log(Vector3.Cross(_col.transform.right, distVec).z);
            //점프 모션 종료 : 착지 모션 시작
            anim.SetBool("jumping", false);

            return true;
        }
        Debug.Log("Down");
        return false;
    }

   
    public override void HitOn(int damage, Transform transform)
    {
        
        if (!invincibilityOn)
        {
            currentHP = currentHP - damage;

            StopCoroutine(StunnedWait());
            anim.SetBool("stunning", false);

            anim.SetTrigger("Hit");

            GameManager.Instance.EffectManager.SpawnHitEffect(this.transform, effectPoint);

            StartCoroutine(HitWait(transform));

            if (currentHP <= 0)
            {

            }
        }
    }

    IEnumerator HitWait(Transform transform)
    {
        invincibilityOn = true;
        stopAct = true;

        float direction = trans.position.x > transform.position.x ? 1 : -1;
        if (direction == 1)
            trans.localScale = new Vector3(-scale, scale, 1f);//render.flipX = true;
        else
            trans.localScale = new Vector3(scale, scale, 1f); //render.flipX = false;

        rigid.velocity = new Vector2(0f, rigid.velocity.y);
        rigid.AddForce(Vector2.right * direction * 2, ForceMode2D.Impulse);
        yield return new WaitForSeconds(invincibilityTime);
        stopAct = false;
        yield return new WaitForSeconds(0.2f);
        invincibilityOn = false;
        yield return null;
    }

    public void Stunned()
    {
        anim.SetTrigger("Stun");
        anim.SetBool("stunning", true);
        stopAct = true;
        StartCoroutine(StunnedWait());
    }

    IEnumerator StunnedWait()
    {
        yield return new WaitForSeconds(1f);
        anim.SetBool("stunning", false);
        stopAct = false;
    }    

    public void Knockback(Transform transform)
    {
        float direction = trans.position.x > transform.position.x ? 1 : -1;
        rigid.velocity = new Vector2(0f, rigid.velocity.y);
        rigid.AddForce(Vector2.right * direction * 2, ForceMode2D.Impulse);
    }


    /*
    void OnDrawGizmos()
    {
        //Attack Box 기즈모
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);


        //ovelapCircle 방식
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position + overlapCircleStart, overlapCircleRadius);
        
        
        // 레이케스트 방식
        //RaycastHit2D raycastHit = Physics2D.BoxCast(transform.position + boxCastStart, boxCastSize, 0f, Vector2.down, boxCastDistance, LayerMask.GetMask("Ground"));

        //Gizmos.color = Color.red;
        //if (raycastHit.collider != null)
        //{
        //    Gizmos.DrawRay(transform.position + boxCastStart, Vector2.down * raycastHit.distance);
        //    Gizmos.DrawWireCube(transform.position + boxCastStart + Vector3.down * raycastHit.distance , boxCastSize);
        //}
        //else
        //{
        //    Gizmos.DrawRay(transform.position + boxCastStart, Vector2.down * boxCastDistance);
        //}
        
    }
    */

}