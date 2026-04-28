using System.Collections.Generic;
using UnityEngine;

public class CameraObstructDetect : MonoBehaviour
{
    private Player player;
    public LayerMask obstacleMask;

    private HashSet<FadableObject> currentFaded = new HashSet<FadableObject>();

    void Awake()
    {
        player = FindFirstObjectByType<Player>();
    }

    void LateUpdate()
    {
        Vector3 direction = player.transform.position - transform.position;// camera -> player
        float distance = direction.magnitude;

        Ray ray = new Ray(transform.position, direction);
        RaycastHit[] hits = Physics.RaycastAll(ray, distance, obstacleMask);

        if(hits.Length != 0)
        {
            // Debug.Log("Wall detected amount = " + hits.Length);
        }

        HashSet<FadableObject> newFaded = new HashSet<FadableObject>();

        foreach (var hitObstable in hits)
        {
            FadableObject fadeObject = hitObstable.collider.GetComponentInParent<FadableObject>();

            if(fadeObject != null)
            {
                fadeObject.FadeOut();
                newFaded.Add(fadeObject);
            }

        }

        foreach (var fadedObstable in currentFaded)
        {
            if(!newFaded.Contains(fadedObstable))
                fadedObstable.FadeIn();
        }

        currentFaded = newFaded;
    }
}
