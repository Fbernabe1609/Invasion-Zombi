using UnityEngine;

public class SpawnPointController : MonoBehaviour
{
    [SerializeField] private GameObject[] humanPrefabs;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private int maxHumans = 5;
    [SerializeField] private float spawnRadius = 20f;

    private float nextSpawnTime = 0f;
    private int currentHumans = 0;
    private GameObject[] instantiatedHumans;

    private void Start()
    {
        instantiatedHumans = new GameObject[maxHumans];
    }

    private void Update()
    {
        CheckIfHumanDied();

        if (currentHumans < maxHumans && Time.time >= nextSpawnTime)
        {
            SpawnRandomHuman();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void SpawnRandomHuman()
    {
        int randomIndex = Random.Range(0, humanPrefabs.Length);
        Vector3 spawnPosition = GenerateRandomPosition();
        GameObject human = Instantiate(humanPrefabs[randomIndex], spawnPosition, Quaternion.identity);
        instantiatedHumans[currentHumans] = human;
        currentHumans++;
    }

    private Vector3 GenerateRandomPosition()
    {
        Vector3 randomPosition = Random.insideUnitSphere * spawnRadius;
        randomPosition += transform.position;
        randomPosition.y = transform.position.y;
        return randomPosition;
    }

    private void CheckIfHumanDied()
    {
        for (int i = 0; i < currentHumans; i++)
        {
            if (!instantiatedHumans[i])
            {
                currentHumans--;
                break;
            }
        }
    }
}