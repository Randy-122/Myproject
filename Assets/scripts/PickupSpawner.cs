using UnityEngine;
using System.Collections;

public class PickupSpawner : MonoBehaviour
{
    [Header("原有掉落物设置")]
    public GameObject[] pickups;
    public float pickupDeliveryTime = 5f;
    public float dropRangeLeft;
    public float dropRangeRight;
    public float highHealthThreshold = 75f;
    public float lowHealthThreshold = 25f;

    [Header("新增：天鹅(Swan)设置")]
    public GameObject swanPrefab;
    public float swanSpawnInterval = 3f;
    public float swanSpeed = 5f;
    public float swanDestroyTime = 10f;

    [Header("新增：载具(Cab & Bus)设置")]
    public GameObject cabPrefab;          // 小轿车预制体
    public GameObject busPrefab;          // 公交车预制体
    public float vehicleSpawnInterval = 4f; // 载具生成间隔（秒）
    public float vehicleSpeed = 6f;       // 载具行驶速度
    public float vehicleDestroyTime = 15f; // 载具自动销毁时间（秒）

    private PlayerHealth playerHealth;

    void Awake()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    void Start()
    {
        // 启动原有的掉落物逻辑
        StartCoroutine(DeliverPickup());

        // 启动天鹅循环生成逻辑
        StartCoroutine(SpawnSwanLoop());

        // ✅ 启动载具循环生成逻辑
        StartCoroutine(SpawnVehicleLoop());
    }

    // ✅ 新增：载具循环生成协程
    private IEnumerator SpawnVehicleLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(vehicleSpawnInterval);

            // 随机决定这次生成 Cab 还是 Bus (0 = Cab, 1 = Bus)
            int randomIndex = Random.Range(0, 2);
            GameObject vehiclePrefab = (randomIndex == 0) ? cabPrefab : busPrefab;

            if (vehiclePrefab != null)
            {
                
                Vector3 spawnPos = new Vector3(dropRangeRight + 2f, -8f, 1f);
                GameObject vehicle = Instantiate(vehiclePrefab, spawnPos, Quaternion.identity);

                // 2. 赋予从右往左飞行的速度
                Rigidbody2D rb = vehicle.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = Vector2.left * vehicleSpeed;
                }

                // 3. 一定时间后自动销毁
                Destroy(vehicle, vehicleDestroyTime);
            }
        }
    }

    // 原有的天鹅生成逻辑保持不变
    private IEnumerator SpawnSwanLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(swanSpawnInterval);

            if (swanPrefab != null)
            {
                Vector3 spawnPos = new Vector3(dropRangeRight + 2f, 10f, 1f);
                GameObject swan = Instantiate(swanPrefab, spawnPos, Quaternion.identity);

                Rigidbody2D rb = swan.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = Vector2.left * swanSpeed;
                }

                Destroy(swan, swanDestroyTime);
            }
        }
    }

    // 原有的掉落物逻辑保持不变
    public IEnumerator DeliverPickup()
    {
        yield return new WaitForSeconds(pickupDeliveryTime);

        float dropPosX = Random.Range(dropRangeLeft, dropRangeRight);
        Vector3 dropPos = new Vector3(dropPosX, 15f, 1f);

        if (playerHealth.health >= highHealthThreshold)
            Instantiate(pickups[0], dropPos, Quaternion.identity);
        else if (playerHealth.health <= lowHealthThreshold)
            Instantiate(pickups[1], dropPos, Quaternion.identity);
        else
        {
            int pickupIndex = Random.Range(0, pickups.Length);
            Instantiate(pickups[pickupIndex], dropPos, Quaternion.identity);
        }
    }
}