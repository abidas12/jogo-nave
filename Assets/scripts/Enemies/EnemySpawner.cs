using UnityEngine;

/// <summary>
/// Spawna meteoros continuamente na borda direita da tela em alturas aleatÃ³rias.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnInterval = 1.1f;
    [SerializeField] private float spawnX = 11.5f;
    [SerializeField] private float minY = -3.8f;
    [SerializeField] private float maxY = 3.8f;

    private float timer = 0f;
    private Transform enemiesContainer;

    private void Start()
    {
        GameObject enemiesObj = GameObject.Find("Enemies");
        if (enemiesObj == null)
        {
            enemiesObj = new GameObject("Enemies");
        }
        enemiesContainer = enemiesObj.transform;

        // Se o enemyPrefab nÃ£o estiver atribuÃ­do no inspector, busca no Resources ou Prefabs
        if (enemyPrefab == null)
        {
            enemyPrefab = Resources.Load<GameObject>("Enemy");
        }
    }

    private void Update()
    {
        // Se o jogo acabou ou venceu, nÃ£o gera novos meteoros
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
            return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SpawnEnemy();
            timer = spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null) return;

        float y = Random.Range(minY, maxY);
        Vector3 pos = new Vector3(spawnX, y, 0f);

        Instantiate(enemyPrefab, pos, Quaternion.identity, enemiesContainer);
    }
}
