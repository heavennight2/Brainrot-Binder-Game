using UnityEngine;

public class StickerSpawner : MonoBehaviour
{
    public GameObject stickerPrefab;
    public Transform[] spawnPoints;

    public float minimumSpawnTime = 6f;
    public float maximumSpawnTime = 10f;

    private float timeUntilSpawn;
    private GameObject currentSticker;

    private void Start()
    {
        SetTimeUntilSpawn();
    }

    private void Update()
    {
        if (currentSticker != null)
        {
            return;
        }

        timeUntilSpawn -= Time.deltaTime;

        if (timeUntilSpawn <= 0)
        {
            SpawnSticker();
            SetTimeUntilSpawn();
        }
    }

    private void SpawnSticker()
    {
        if (spawnPoints.Length == 0)
        {
            return;
        }

        int randomIndex =
            Random.Range(0, spawnPoints.Length);

        currentSticker = Instantiate(
            stickerPrefab,
            spawnPoints[randomIndex].position,
            Quaternion.identity
        );
    }

    private void SetTimeUntilSpawn()
    {
        timeUntilSpawn = Random.Range(
            minimumSpawnTime,
            maximumSpawnTime
        );
    }
}