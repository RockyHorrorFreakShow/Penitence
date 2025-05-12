using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class RoomEnemySpawner : MonoBehaviour
{

    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private int minEnemiesPerRoom = 1;
    [SerializeField] private int maxEnemiesPerRoom = 5;

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    public void SpawnEnemies(RoomInstance room)
    {
        int enemyCount = Random.Range(minEnemiesPerRoom, maxEnemiesPerRoom + 1);
        Debug.Log($"Spawning {enemyCount} enemies in {room.gameObject.name}");

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Vector3 spawnPosition = GetRandomSpawnPoint(room);

            if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                spawnPosition = hit.position;

                // Check if the spawn position is buried under a platform
                //Vector3 spawnPosition = GetRandomSpawnPoint(room);

                // Step 1: Raycast downward from above the room
                Vector3 rayOrigin = spawnPosition + Vector3.up * 10f;
                float rayDistance = 20f;

                if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hitInfo, rayDistance, LayerMask.GetMask("IsGround")))
                {
                    Vector3 topSurface = hitInfo.point;

                    // Step 2: Validate position with NavMesh
                    if (NavMesh.SamplePosition(topSurface, out NavMeshHit navHit, 1f, NavMesh.AllAreas))
                    {
                        topSurface = navHit.position;

                        // Step 3: Final ground clearance check to avoid props/obstacles
                        if (!Physics.CheckSphere(topSurface + Vector3.up * 0.5f, 0.5f, LayerMask.GetMask("IsGround")))
                        {
                            GameObject enemy = Instantiate(enemyPrefab, topSurface, Quaternion.identity);
                            spawnedEnemies.Add(enemy);

                            Enemy enemyScript = enemy.GetComponent<Enemy>();
                            if (enemyScript != null)
                            {
                                enemyScript.AssignRoom(room);
                                room.RegisterEnemy();
                            }
                        }
                        else
                        {
                            Debug.LogWarning($"Blocked spawn on IsGround collision at {topSurface}");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("No valid NavMesh found near top surface.");
                    }
                }
                else
                {
                    Debug.LogWarning("Raycast failed to find ground below intended spawn point.");
                }

            }
            else
            {
                Debug.LogWarning("Failed to find NavMesh near: " + spawnPosition);
            }
        }
    }



    private Vector3 GetRandomSpawnPoint(RoomInstance room)
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-room.roomSize / 3, room.roomSize / 3),
            0,
            Random.Range(-room.roomSize / 3, room.roomSize / 3)
        );

        return room.transform.position + randomOffset;
    }


    public void ClearEnemies()
    {
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null)
                Destroy(enemy);
        }
        spawnedEnemies.Clear();
    }
}
