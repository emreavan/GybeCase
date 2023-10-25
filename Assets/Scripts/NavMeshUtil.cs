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
    
    public static Vector3? FindRandomPosition(Vector3 center, float range, int layerMask)
    {
        for (int i = 0; i < 30; i++)  // tries for 30 times (this number can be adjusted)
        {
            Vector3 randomDirection = Random.insideUnitSphere * range;
            randomDirection += center;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, range, layerMask))
            {
                return hit.position;
            }
        }
        return null;  // return null if a valid position is not found
    }
}