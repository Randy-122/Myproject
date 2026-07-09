using UnityEngine;

public class Rocket : MonoBehaviour
{
    public GameObject explosion;
    private PickupSpawner pickupSpawner;

    private void Start()
    {
        pickupSpawner = GameObject.Find("pickupManager").GetComponent<PickupSpawner>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"[Rocket] 碰到了: {collision.gameObject.name}，Tag是: {collision.tag}");
        // 1. 跳过玩家（保持不变）
        if (collision.CompareTag("Player")) return;

        // 2. ✅ 重点修复：从父物体层级查找，将两个 Controller 的查找放在同一层级
        ShipController enemy = collision.GetComponentInParent<ShipController>();
        ShipController2 enemy2 = collision.GetComponent<ShipController2>(); // 修正拼写并提升作用域

        Debug.Log($"[Rocket] 找到 ShipController: {enemy != null}");
        Debug.Log($"[Rocket] 找到 ShipController2: {enemy2 != null}");

        // 3. ✅ 修改：增加对 ShipController2 的扣血判断
        if (collision.CompareTag("Enemy") && (enemy != null || enemy2 != null))
        {
            if (enemy != null)
            {
                enemy.ApplyDamage(1);
            }
            else if (enemy2 != null)
            {
                // 假设 ShipController2 也有 ApplyDamage 方法，请根据实际情况调整
                enemy2.ApplyDamage(1); 
            }

            if (explosion != null)
            {
                Quaternion randomRot = Quaternion.Euler(0, 0, Random.Range(0f, 180f));
                Instantiate(explosion, transform.position, randomRot);
            }
            Destroy(gameObject);
        }
        // 4. ✅ 补充：输出更精准的错误日志（区分层级问题）
        else if (collision.CompareTag("Enemy") && enemy == null && enemy2 == null)
        {
            Debug.LogError(
                $"[Rocket] Enemy '{collision.gameObject.name}' is tagged 'Enemy' " +
                $"but missing ShipController OR ShipController2 on itself OR parent! " +
                $"Check hierarchy: {GetParentChain(collision.transform)}"
            );
        }
        else if (collision.CompareTag("BombCrate"))
        {
            if (explosion != null)
            {
                Quaternion randomRot = Quaternion.Euler(0, 0, Random.Range(0f, 180f));
                Instantiate(explosion, transform.position, randomRot);
            }
            Destroy(gameObject);
            Destroy(collision.gameObject);
            pickupSpawner.StartCoroutine(pickupSpawner.DeliverPickup());
        }

        // 5. 爆炸和销毁逻辑（保持不变）
        if (explosion != null)
        {
            Quaternion randomRot = Quaternion.Euler(0, 0, Random.Range(0f, 180f));
            Instantiate(explosion, transform.position, randomRot);
        }
        Destroy(gameObject);
    }

    // ✅ 辅助函数：打印完整的父物体链（用于调试）
    private string GetParentChain(Transform t)
    {
        string chain = t.name;
        while (t.parent != null)
        {
            t = t.parent;
            chain = $"{t.name} > {chain}";
        }
        return chain;
    }
}