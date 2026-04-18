using System.Collections.Generic;
using UnityEngine;

public class EazyZombieSpawner : MonoBehaviour
{
    public EazyZombie eazyZombiePrefab;
    public int spawnAmount;
    public int spawnRadius;
    public int spawnerCooldown = 15;
    public Timer spawnTimer;
    public List<EazyZombie> zombieLists = new List<EazyZombie>();
    public int maxAmount = 30; 

    
    void Start()
    {
        spawnTimer = new Timer(spawnerCooldown);
    }

    void Update()
    {
        spawnTimer.Update(Time.deltaTime);
    }

    public void SpawnWave()
    {
        for (int i = 0; i < spawnAmount ; i++)
        {
            if (zombieLists.Count < maxAmount)
            {
                SpawnRandomPosition(spawnRadius);
            }
        }
    }

    void SpawnRandomPosition(int radius)
    {
        int x = Random.Range(-radius, radius);
        int z = Random.Range(-radius, radius);

        Vector3 position = transform.position + new Vector3(x, 0, z);

        EazyZombie eazyZombie = Instantiate(eazyZombiePrefab, position, Quaternion.identity, this.transform);
        zombieLists.Add(eazyZombie);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            if ( !(spawnTimer.IsRunning() && spawnTimer.GetTimeRemaining() > 0) )
            {
                SpawnWave();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            spawnTimer.Start();
        }
    }
}
