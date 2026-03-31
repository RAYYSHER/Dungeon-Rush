using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject prefabChar;
    public int spawnAmount;
    public int spawnRadius;
    public int maxZombies = 100;

    void Start()
    {
        SpawnWave();
    }

    public void SpawnWave()
    {
        for (int i = 0; i < spawnAmount ; i++)
        {
            // if (Zombie.zombieLists.Count >= maxZombies)
            // {
            //     Debug.Log($"[ZombieSpawner] Max zombies reached ({maxZombies}), skipping spawn.");
            //     break;
            // }
            SpawnRandomPosition(spawnRadius);
        }
    }

    void SpawnRandomPosition(int radius)
    {
        int x = Random.Range(-radius, radius);
        int z = Random.Range(-radius, radius);

        Vector3 position = transform.position + new Vector3(x, 0, z);

        Instantiate(prefabChar, position, Quaternion.identity, this.transform);
    }
}
