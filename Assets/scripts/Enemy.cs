using UnityEngine;

public class ShipController : MonoBehaviour
{
    [Header("基础设置")]
    [SerializeField] private Transform frontCheck;      // 前方检测点
    [SerializeField] private float raycastDistance = 0.5f; // 射线检测距离
    [SerializeField] private LayerMask detectLayer;    // 障碍物层
    [SerializeField] private float moveSpeed = 3.5f;    // 移动速度

    [Header("生命系统")]
    [SerializeField] private float maxHealth = 3f;         // 默认2(bug3)滴血
    [SerializeField] private Sprite fullHealthSprite;   // 满血素材
    [SerializeField] private Sprite lowHealthSprite;    // 残血素材
    [SerializeField] private Sprite deathSprite;        // 死亡素材

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public float currentHealth;
    private bool isDead;
    private bool isFacingRight = true; // 明确声明朝向（默认向右）

    private UIManager uIManager;
    public GameObject addScore; 
    void Awake()
    {
        // 初始化组件（避免Start中空引用）
        uIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    void Start()
    {
        UpdateHealthVisual();
    }

    void FixedUpdate()
    {
        if (currentHealth <= 0) 
        {
            Die();
        }
        if (isDead) return; // 死亡后停止逻辑

        // 移动逻辑
        float direction = isFacingRight ? 1f : -1f;
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);

        // 障碍检测
        Vector2 rayDirection = isFacingRight ? Vector2.right : Vector2.left;
        if (Physics2D.Raycast(frontCheck.position, rayDirection, raycastDistance, detectLayer))
        {
            Flip();
        }
    }

    // ✅ 关键修改：公开安全的扣血接口（其他脚本调用）
    public void ApplyDamage(int damage)
    {
        if (isDead) return;
        TakeDamage(damage);
    }

    private void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        UpdateHealthVisual();

        if (currentHealth <= 0)
            Die();
    }

    private void UpdateHealthVisual()
    {
        if (isDead) return;

        spriteRenderer.sprite = currentHealth >= 2 ?
            fullHealthSprite :
            lowHealthSprite;
    }

    // ✅ 核心需求实现：死亡逻辑
    private void Die()
    {
        if (!isDead) 
        {
            // 1. 获取主摄像机（确保你的相机标签是 MainCamera）
            Camera mainCam = Camera.main;

            // 2. 核心步骤：将敌人的【世界坐标】转换为【屏幕坐标】
            Vector3 screenPos = mainCam.WorldToScreenPoint(transform.position);

            // 3. 找到 Canvas
            Transform canvasTransform = GameObject.Find("Canvas").transform;

            if (canvasTransform != null && addScore != null)
            {
                // 4. 生成时传入转换后的 screenPos
                // 注意：最后一个参数 false 表示不锁定局部坐标，直接使用传入的世界/屏幕坐标
                Instantiate(addScore, screenPos, Quaternion.identity, canvasTransform);
            }


            uIManager.AddScore(100); // 死亡加分
        }

        
        isDead = true;
        
        spriteRenderer.sprite = deathSprite;

        // 1. 所有碰撞体设为Trigger（停止碰撞检测）
        Collider2D[] cols = GetComponents<Collider2D>();
        foreach (Collider2D col in cols)
        {
            col.isTrigger = true;
        }

        // 2. ✅ 设置一次性随机旋转角度（关键！）
        transform.eulerAngles = new Vector3(0, 0, Random.Range(0f, 360f));

        // 3. ✅ 5秒后自动销毁（原生实现）
        Destroy(gameObject, 5f);
    }

    private void Flip()
    {
        if (isDead) return;

        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(
            -transform.localScale.x,
            transform.localScale.y,
            transform.localScale.z
        );
    }

    // ✅ 可视化调试（编辑器中显示检测射线）
    private void OnDrawGizmosSelected()
    {
        if (frontCheck == null) return;

        Gizmos.color = Color.red;
        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
        Gizmos.DrawRay(frontCheck.position, direction * raycastDistance);
    }
}