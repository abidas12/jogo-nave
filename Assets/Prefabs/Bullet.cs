using UnityEngine;

/// <summary>
/// Comportamento do projétil: movimento em linha reta e destruição ao colidir.
/// </summary>
public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 19f;
    [SerializeField] private float lifeTime = 4f;

    private Vector2 direction;

    private void Start()
    {
        // Destrói automaticamente após lifeTime segundos
        Destroy(gameObject, lifeTime);
    }

    // Chamado pelo PlayerShooting ao instanciar
    public void Initialize(Vector2 dir)
    {
        direction = dir.normalized;
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Se colidir com inimigo, notifica e destrói
        if (other.CompareTag("Enemy"))
        {
            // Chama método no inimigo para ser destruído
            Enemy Enemy = other.GetComponent<Enemy>();
            if (Enemy != null) Enemy.OnHit();

            Destroy(gameObject);
        }
    }
}
