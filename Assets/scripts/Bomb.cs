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
        layBombs.bombLaid = false;

        // 启动协程，产生下一个道具.
        pickupSpawner.StartCoroutine(pickupSpawner.DeliverPickup());

        // 在炸弹的杀伤范围内查找敌人
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, bombRadius, 1 << LayerMask.NameToLayer("Enemy"));

        // 遍历杀伤的敌人
        foreach (Collider2D en in enemies)
        {
            Rigidbody2D rb = en.GetComponent<Rigidbody2D>();
            if (rb != null && rb.tag == "Enemy")
            {
                rb.gameObject.GetComponentInParent<ShipController>().currentHealth = 0;

                // 设置爆炸受力向量.
                Vector3 deltaPos = rb.transform.position - transform.position;

                // 受力向量方向添加爆炸力
                Vector3 force = deltaPos.normalized * bombForce;
                rb.AddForce(force);
            }
        }

        // 爆炸效果，粒子效果
        explosionFX.transform.position = transform.position;
        explosionFX.Play();

        Instantiate(explosion, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(boom, transform.position);
        Destroy(gameObject);
    }
}
