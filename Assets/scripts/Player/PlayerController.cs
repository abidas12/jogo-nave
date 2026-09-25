using UnityEngine;

/// <summary>
/// Controla o movimento da nave (WASD e setas) com limites de tela,
/// invulnerabilidade temporÃ¡ria e detecÃ§Ã£o de colisÃµes com meteoros.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6.5f;
    [SerializeField] private Vector2 minBounds = new Vector2(-8f, -4f);
    [SerializeField] private Vector2 maxBounds = new Vector2(8f, 4f);

    [Header("Invulnerability")]
    [SerializeField] private float invulnerabilityDuration = 1.0f;
    private float invulnerabilityTimer = 0f;

    private Rigidbody2D rb;
    private Vector2 input;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Se o jogo acabou ou venceu, bloqueia o controle
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
        {
            input = Vector2.zero;
            return;
        }

        // Timer e efeito visual de piscar na invulnerabilidade
        if (invulnerabilityTimer > 0f)
        {
            invulnerabilityTimer -= Time.deltaTime;
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = (Mathf.FloorToInt(invulnerabilityTimer * 10f) % 2 == 0);
            }
        }
        else if (spriteRenderer != null && !spriteRenderer.enabled)
        {
            spriteRenderer.enabled = true;
        }

        // Entrada do jogador (WASD e setas)
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        input = new Vector2(h, v).normalized;
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
            return;

        Vector2 newPos = rb.position + input * moveSpeed * Time.fixedDeltaTime;

        // Limita a nave dentro da tela visÃ­vel
        newPos.x = Mathf.Clamp(newPos.x, minBounds.x, maxBounds.x);
        newPos.y = Mathf.Clamp(newPos.y, minBounds.y, maxBounds.y);

        rb.MovePosition(newPos);
    }

    /// <summary>
    /// Chamado ao colidir com um meteoro.
    /// </summary>
    public void OnHit()
    {
        // Se jÃ¡ estiver invulnerÃ¡vel, nÃ£o perde vida novamente de imediato
        if (invulnerabilityTimer > 0f) return;

        invulnerabilityTimer = invulnerabilityDuration;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.PlayerHit();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckCollision(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckCollision(collision.gameObject);
    }

    private void CheckCollision(GameObject other)
    {
        if (other.CompareTag("Enemy") || other.GetComponent<Meteor>() != null)
        {
            OnHit();
        }
    }
}
