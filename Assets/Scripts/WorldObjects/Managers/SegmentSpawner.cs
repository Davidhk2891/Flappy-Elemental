using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using System.Collections.Generic;

public class SegmentSpawner : MonoBehaviour
{
    [Header("Pipe set Settings")]
    public PipePool pipePool;
    public Transform pipeSetEnvironment;
    public float pipeSpawnX = 10f;
    public float pipeHeightOffset = 4f;

    [Header("Obstacle segment Settings")]
    public BouncerPool bouncerPool;
    public PulsarPool pulsarPool;
    public Transform obstacleEnvironment;
    public int obstaclesPerSegment = 5;
    public float delayBeforeObstacles = 2.5f;
    public float intervalBetweenObstacles = 3f;
    public float delayAfterObstacles = 1f;
    public float obstacleSpawnX = 10f;
    public float obstacleMinY = -3f;
    public float obstacleMaxY = 3f;

    [Header("Checkpoint")]
    public float checkpointPipeGap = 3f;

    private void Start()
    {
        StartCoroutine(SpawnSegments());
    }

    private IEnumerator SpawnSegments()
    {
        while (true)
        {
            // Entry pipe
            SpawnPipeSet();

            // Wait before spawning obstacles
            yield return new WaitForSeconds(delayBeforeObstacles);

            // Obstacles segment
            for (int i = 0; i < obstaclesPerSegment; i++)
            {
                SpawnObstacle();
                yield return new WaitForSeconds(intervalBetweenObstacles);
            }

            // Exit pipe
            yield return new WaitForSeconds(delayAfterObstacles);
            SpawnPipeSet();

            // Delay before next segment (this will change)
            yield return new WaitForSeconds(checkpointPipeGap);
        }
    }

    private void SpawnPipeSet()
    {
        // Pipe set
        float lowestPoint = transform.position.y - pipeHeightOffset;
        float highestPoint = transform.position.y + pipeHeightOffset;

        float randomY = Random.Range(lowestPoint, highestPoint);

        GameObject pipeSet = pipePool.GetPipe();
        pipeSet.transform.SetParent(pipeSetEnvironment);
        pipeSet.transform.SetPositionAndRotation
        (
            new Vector3(pipeSpawnX, randomY, 0f),
            transform.rotation
        );
        pipeSet.SetActive(true);
    }
    
    private void SpawnObstacle()
    {
        // Bouncer
        // GameObject bouncer = bouncerPool.GetBouncer();
        // bouncer.transform.SetParent(obstacleEnvironment);
        // bouncer.transform.position = new Vector3(obstacleSpawnX, 0f, 0f);
        // bouncer.SetActive(true);

        // Pulsar
        GameObject pulsar = pulsarPool.GetPulsar();
        pulsar.transform.SetParent(obstacleEnvironment);
        pulsar.transform.position = new Vector3(obstacleSpawnX, 0f, 0f);
        pulsar.SetActive(true);

        
    }
}