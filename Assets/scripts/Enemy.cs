using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public float health = 2f;
    public Sprite deadSprite;
    public Sprite damageSprite;
    public float spinMin = -100f;
    public float spinMax = 100f;
    public Animator animator;

    SpriteRenderer ren;
    Rigidbody2D enemyBody; // 现在指向父物体的刚体
    Transform front_Check;
    bool dead = false;

    public void Hurt() 
    {
        health--;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        // 【修改1】：请确保 Rigidbody2D 挂在敌人父物体上，而不是子物体上！
        enemyBody = GetComponent<Rigidbody2D>();

        // 子物体只负责获取渲染器
        ren = transform.Find("char_enemy_alienShip").GetComponent<SpriteRenderer>();
        front_Check = transform.Find("front_Check");

        // 初始速度
        enemyBody.velocity = new Vector2(speed, 0);
    }
    private void Update()
    {
        if(health == 1 && damageSprite != null)
        {
            ren.sprite = damageSprite;
        }
        if (health == 0 && !dead) 
        {
            Death();
        }
    }

    // 【修改2】：物理移动必须放在 FixedUpdate

    void FixedUpdate()
    {
        if (dead) return; // 死亡后停止移动

        // 1. 根据当前朝向设置速度
        float direction = Mathf.Sign(transform.localScale.x);
        enemyBody.velocity = new Vector2(speed * direction, enemyBody.velocity.y);

        // 2. 【修改3】：使用射线检测代替 OverlapPoint，防止卡墙！
        // 从 front_Check 的位置，向当前朝向发射一条长度为 0.2 的射线
        Vector2 rayDirection = new Vector2(direction, 0);
        RaycastHit2D hit = Physics2D.Raycast(front_Check.position, rayDirection, 0.2f, 1 << LayerMask.NameToLayer("Obstacle"));

        // 如果射线打中了东西，且标签是 Tower
        if (hit.collider != null && hit.collider.CompareTag("Tower"))
        {
            Flip();
        }
    }

    void Flip()
    {
        // 翻转父物体的缩放
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        // 射线检测已经处理了方向，这里不需要再手动改速度了
    }

    // 可以在 Scene 窗口画出射线，方便你调试检测距离
    void OnDrawGizmosSelected()
    {
        if (front_Check == null) front_Check = transform.Find("front_Check");
        if (front_Check == null) return;

        float direction = Mathf.Sign(transform.localScale.x);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(front_Check.position, front_Check.position + new Vector3(direction * 0.2f, 0, 0));
    }

    void Death()
    {
        dead = true;
        ren.sprite = deadSprite;
        Collider2D[] cols = GetComponents<Collider2D>();
        foreach (Collider2D col in cols)
        {
            col.isTrigger = true;
        }

        enemyBody.freezeRotation = false;
        enemyBody.AddTorque(Random.Range(spinMin, spinMax));

        SpriteRenderer[] sprs = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer spr in sprs)
        {
            spr.sortingLayerName = "UI";
        }
        animator.SetTrigger("Dead");//播放死亡动画
        
        Destroy(gameObject, 5f); // 5秒后销毁敌人
    }
}