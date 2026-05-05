using UnityEngine;

public class SpawnerTriggerZone : MonoBehaviour
{
    [SerializeField] private ZombieSpawner     zombieSpawner;
    [SerializeField] private EazyZombieSpawner easySpawner;
    [SerializeField] private TankZombieSpawner tankSpawner;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (zombieSpawner != null)
            zombieSpawner.SpawnWave();

        if (easySpawner != null && !(easySpawner.spawnTimer.IsRunning() && easySpawner.spawnTimer.GetTimeRemaining() > 0))
            easySpawner.SpawnWave();

        if (tankSpawner != null && !(tankSpawner.spawnTimer.IsRunning() && tankSpawner.spawnTimer.GetTimeRemaining() > 0))
            tankSpawner.SpawnWave();
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        easySpawner?.spawnTimer.Start();
        tankSpawner?.spawnTimer.Start();
    }
}