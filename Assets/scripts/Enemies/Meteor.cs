using UnityEngine;

/// <summary>
/// Classe que controla os meteoros:
/// MovimentaÃ§Ã£o contÃ­nua pela tela, rotaÃ§Ã£o e colisÃµes com tiros e nave.
/// </summary>
public class Meteor : MonoBehaviour
{
    [Header("ConfiguraÃ§Ãµes do Meteoro")]
    [SerializeField] private float minSpeed = 3f;
    [SerializeField] private float maxSpeed = 5.5f;
    [SerializeField] private int scoreValue = 1;

    private float speed;
    private float rotationSpeed;
    private Vector2 direction;
    private bool isDestroyed = false;

    private void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        rotationSpeed = Random.Range(-60f, 60f);

        // Movimento da direita para a esquerda com leve variaÃ§Ã£o vertical
        float randomY = Random.Range(-0.25f, 0.25f);
        direction = new Vector2(-1f, randomY).normalized;
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
            return;

        // Move o meteoro atravessando a tela
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // RotaÃ§Ã£o visual suave do asteroide
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

        // DestrÃ³i ao sair pela esquerda da tela
        if (transform.position.x < -16f)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Chamado quando o tiro atinge o meteoro.
    /// </summary>
    public virtual void OnHit()
    {
        if (isDestroyed) return;
        isDestroyed = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HandlePlayerCollision(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandlePlayerCollision(collision.gameObject);
    }

    private void HandlePlayerCollision(GameObject other)
    {
        if (isDestroyed) return;

        if (other.CompareTag("player") || other.CompareTag("Player") || other.GetComponent<PlayerController>() != null)
        {
            isDestroyed = true;

            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.OnHit();
            }
            else if (GameManager.Instance != null)
            {
                GameManager.Instance.PlayerHit();
            }

            Destroy(gameObject);
        }
    }
}
