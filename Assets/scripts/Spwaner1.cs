using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] Enemies;
    void Start()
    {
        InvokeRepeating("SpwanEnemy", 1, 0.5f);
    }
    void SpwanEnemy()
    {
        int index = Random.Range(0, Enemies.Length);
        Instantiate(Enemies[index], transform.position, Quaternion.identity);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
