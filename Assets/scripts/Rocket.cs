using UnityEngine;

public class Rocket : MonoBehaviour
{
    public GameObject explosion;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. 跳过玩家（保持不变）
        if (collision.CompareTag("Player")) return;

        // 2. ✅ 重点修复：从父物体层级查找 ShipController
        ShipController enemy = collision.GetComponentInParent<ShipController>();

        // 3. 仅当是敌人且存在ShipController时扣血
        if (collision.CompareTag("Enemy") && enemy != null)
        {
            enemy.ApplyDamage(1); // 安全调用扣血
            if (explosion != null)
            {
                Quaternion randomRot = Quaternion.Euler(0, 0, Random.Range(0f, 180f));
                Instantiate(explosion, transform.position, randomRot);
            }
            Destroy(gameObject);
        }
        // 4. ✅ 补充：输出更精准的错误日志（区分层级问题）
        else if (collision.CompareTag("Enemy") && enemy == null)
        {
            Debug.LogError(
                $"[Rocket] Enemy '{collision.gameObject.name}' is tagged 'Enemy' " +
                $"but missing ShipController on itself OR parent! " +
                $"Check hierarchy: {GetParentChain(collision.transform)}"
            );
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