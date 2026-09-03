using UnityEngine;

/// <summary>
/// Spawna inimigos em intervalos regulares em posições Y aleatórias.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnInterval = 1.2f;
    [SerializeField] private float spawnX = 12f; // posição X onde inimigos aparecem
    [SerializeField] private float minY = -3.5f;
    [SerializeField] private float maxY = 3.5f;

    private float timer = 0f;

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SpawnEnemy();
            timer = spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        float y = Random.Range(minY, maxY);
        Vector3 pos = new Vector3(spawnX, y, 0f);
        Instantiate(enemyPrefab, pos, Quaternion.identity, GameObject.Find("Enemies").transform);
    }
}
