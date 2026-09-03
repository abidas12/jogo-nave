using UnityEngine;

/// <summary>
/// Controla a taxa de tiro do jogador e instancia projéteis.
/// </summary>
public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint; // posição de saída do tiro
    [SerializeField] private float fireRate = 0.26f; // segundos entre tiros

    private float fireCooldown = 0f;

    private void Update()
    {
        fireCooldown -= Time.deltaTime;

        // Disparo por tecla Espaço ou botão esquerdo do mouse
        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && fireCooldown <= 0f)
        {
            Shoot();
            fireCooldown = fireRate;
        }
    }

    private void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject b = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity, GameObject.Find("Projectiles").transform);
        Bullet bullet = b.GetComponent<Bullet>();
        if (bullet != null)
        {
            // Para side-scroller, atira para a direita; ajuste se for top-down
            Vector2 dir = Vector2.right;
            bullet.Initialize(dir);
        }
    }
}
