using UnityEngine;

/// <summary>
/// Controla o disparo de tiros da nave.
/// Dispara projÃ©teis para a direita (em direÃ§Ã£o aos meteoros).
/// </summary>
public class PlayerShooting : MonoBehaviour
{
    [Header("ConfiguraÃ§Ã£o de Disparo")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.22f;

    private float fireCooldown = 0f;
    private Transform projectilesContainer;

    private void Start()
    {
        if (firePoint == null)
        {
            Transform gun = transform.Find("Playergun");
            firePoint = (gun != null) ? gun : transform;
        }

        GameObject projObj = GameObject.Find("Projectiles");
        if (projObj == null)
        {
            projObj = new GameObject("Projectiles");
        }
        projectilesContainer = projObj.transform;
    }

    private void Update()
    {
        // NÃ£o permite atirar se o jogo estiver encerrado ou pausado por vitÃ³ria/derrota
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
            return;

        fireCooldown -= Time.deltaTime;

        // Disparo por EspaÃ§o ou Clique esquerdo
        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && fireCooldown <= 0f)
        {
            Shoot();
            fireCooldown = fireRate;
        }
    }

    private void Shoot()
    {
        if (bulletPrefab == null) return;

        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;

        // Side-scroller: tiros vÃ£o para a direita
        Vector2 dir = Vector2.right;

        GameObject b = Instantiate(bulletPrefab, spawnPos, Quaternion.identity, projectilesContainer);
        Bullet bullet = b.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.Initialize(dir);
        }
    }
}
