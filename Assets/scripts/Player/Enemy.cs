using UnityEngine;

/// <summary>
/// Comportamento simples do inimigo: move-se para a esquerda e é destruído ao ser atingido.
/// </summary>
public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private int scoreValue = 10;

    private void Update()
    {
        // Move para a esquerda
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // Se sair da tela à esquerda, destrói
        if (transform.position.x < -20f) // ajuste conforme tamanho da cena
            Destroy(gameObject);
    }

    // Chamado pelo Bullet quando atinge
    public void OnHit()
    {
        // Notifica GameManager para adicionar pontos
        GameManager.Instance.AddScore(scoreValue);

        // Aqui você pode instanciar partículas/efeitos
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Se colidir com o Player, notifica GameManager e destrói
        if (collision.collider.CompareTag("Player"))
        {
            GameManager.Instance.PlayerHit();
            Destroy(gameObject);
        }
    }
}
