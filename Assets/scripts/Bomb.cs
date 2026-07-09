using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
    public float bombRadius = 10f;      //伤害范围	
    public float bombForce = 100f;      //冲击力	
    public AudioClip boom;
    public AudioClip fuse;
    public float fuseTime = 1.5f;
    public GameObject explosion;


    private LayBombs layBombs;
    private PickupSpawner pickupSpawner;
    private ParticleSystem explosionFX;


    void Awake()
    {
        // 初始化.
        explosionFX = GameObject.FindGameObjectWithTag("ExplosionFX").GetComponent<ParticleSystem>();
        pickupSpawner = GameObject.Find("pickupManager").GetComponent<PickupSpawner>();
        if (GameObject.FindGameObjectWithTag("Player"))
            layBombs = GameObject.FindGameObjectWithTag("Player").GetComponent<LayBombs>();
    }

    void Start()
    {


        if (transform.root == transform)
            StartCoroutine(BombDetonation());
    }


    IEnumerator BombDetonation()
    {
        AudioSource.PlayClipAtPoint(fuse, transform.position);

        // 引信燃烧fuseTime秒.
        yield return new WaitForSeconds(fuseTime);

        // 爆炸
        Explode();
    }

    public void Explode()
    {
        // 爆炸后才能再次释放炸弹
        if (layBombs != null)
        {
            layBombs.bombLaid = false;
        }

        // 启动协程，产生下一个道具
        if (pickupSpawner != null)
        {
            pickupSpawner.StartCoroutine(pickupSpawner.DeliverPickup());
        }

        // 在炸弹的杀伤范围内查找敌人
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, bombRadius, 1 << LayerMask.NameToLayer("Enemy"));

        // 遍历杀伤的敌人
        foreach (Collider2D en in enemies)
        {
            // ✅ 核心修复：同时兼容 ShipController 和 ShipController2
            ShipController enemy1 = en.GetComponentInParent<ShipController>();
            ShipController2 enemy2 = en.GetComponent<ShipController2>();

            // 只要找到其中一个，就执行秒杀逻辑
            if (enemy1 != null)
            {
                //enemy1.currentHealth = 0;
                // 建议：既然血量归零了，最好主动调用 Die() 方法，否则它可能不会立刻触发死亡特效
                 enemy1.ApplyDamage(999); 
            }
            else if (enemy2 != null)
            {
                //enemy2.currentHealth = 0;
                 enemy2.ApplyDamage(999);
            }
            Debug.Log($"[bomb] 找到 ShipController: {enemy1 != null}");
            Debug.Log($"[bomb] 找到 ShipController2: {enemy2 != null}");
            // 设置爆炸受力向量并添加爆炸力
            Rigidbody2D rb = en.GetComponent<Rigidbody2D>();
            if (rb != null && en.CompareTag("Enemy"))
            {
                Vector3 deltaPos = rb.transform.position - transform.position;
                Vector3 force = deltaPos.normalized * bombForce;
                rb.AddForce(force);
            }
        }

        // 爆炸效果，粒子效果
        if (explosionFX != null)
        {
            explosionFX.transform.position = transform.position;
            explosionFX.Play();
        }

        if (explosion != null)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
        }

        if (boom != null)
        {
            AudioSource.PlayClipAtPoint(boom, transform.position);
        }

        Destroy(gameObject);
    }
}
