using UnityEngine;

public class EnemyDespawnBody : MonoBehaviour
{
    public Timer removeBody;
    public int bodyTimeRemaining = 5;

    void Start()
    {
        removeBody = new Timer(bodyTimeRemaining);
    }

    void Update()
    {
        removeBody.Update(Time.deltaTime);
        if (removeBody.GetTimeRemaining() <= 0)
        {
           Destroy(this.gameObject);
        }
    }

    public void Despawn()
    {
        removeBody.Start();
    }
}
