using UnityEngine;
using UnityEngine.AI;

public class NavMeshUtils
{
    public static Vector3? FindRandomPosition(float maxRange, int layerMask)
    {
        for (int i = 0; i < 30; i++) 
        {
            Vector3 randomDirection = new Vector3(Random.Range(-maxRange, maxRange), Random.Range(-maxRange, maxRange), Random.Range(-maxRange, maxRange)) + Random.insideUnitSphere * maxRange;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, maxRange, layerMask))
            {
                return hit.position;
            }
        }
        return null;
    }
}