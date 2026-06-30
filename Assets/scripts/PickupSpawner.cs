using UnityEngine;
using System.Collections;

public class PickupSpawner : MonoBehaviour
{
    public GameObject[] pickups;
    public float pickupDeliveryTime = 5f;
    public float dropRangeLeft;
    public float dropRangeRight;
    public float highHealthThreshold = 75f;
    public float lowHealthThreshold = 25f;


    private PlayerHealth playerHealth;


    void Awake()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }


    void Start()
    {
        StartCoroutine(DeliverPickup());
    }

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
