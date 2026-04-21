using System.Collections.Generic;
using UnityEngine;

public class TankZombieSpawner : MonoBehaviour
{
    public TankZombie tankZombiePrefab;
    public int spawnAmount;
    public int spawnRadius;
    public int spawnerCooldown = 15;
    public Timer spawnTimer;
    public List<TankZombie> zombieLists = new List<TankZombie>();
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

        TankZombie tankZombie = Instantiate(tankZombiePrefab, position, Quaternion.identity, this.transform);
        zombieLists.Add(tankZombie);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[EazyZombieSpawner] OnTriggerEnter fired! Tag: {other.tag}");

        if (other.CompareTag("Player") == true)
        {

            Debug.Log($"[EazyZombieSpawner] Player detected! Timer running: {spawnTimer.IsRunning()}, spawnAmount: {spawnAmount}");
            
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
