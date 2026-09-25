using UnityEngine;

/// <summary>
/// Comportamento do projÃ©til:
/// Movimento em linha reta, rotaÃ§Ã£o alinhada com o disparo e destruiÃ§Ã£o ao colidir com meteoros.
/// </summary>
public class Bullet : MonoBehaviour
{
    [Header("ConfiguraÃ§Ãµes do ProjÃ©til")]
    [SerializeField] private float speed = 18f;
    [SerializeField] private float lifeTime = 3.5f;

    private Vector2 direction = Vector2.right;
    private bool hasHit = false;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void Initialize(Vector2 dir)
    {
        direction = dir.normalized;

        // Ajusta a rotaÃ§Ã£o visual do projÃ©til para apontar na direÃ§Ã£o do tiro
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // Se sair da tela, destrÃ³i
        if (transform.position.x > 16f || transform.position.x < -16f ||
            transform.position.y > 10f || transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckHit(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckHit(collision.gameObject);
    }

    private void CheckHit(GameObject hitObj)
    {
        if (hasHit) return;

        // Procura componente Meteor ou Enemy
        Meteor meteor = hitObj.GetComponent<Meteor>();
        if (meteor == null)
        {
            meteor = hitObj.GetComponentInParent<Meteor>();
        }

        if (meteor != null || hitObj.CompareTag("Enemy"))
        {
            hasHit = true;
            if (meteor != null)
            {
                meteor.OnHit();
            }
            else
            {
                Enemy enemy = hitObj.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.OnHit();
                }
                else
                {
                    if (GameManager.Instance != null)
                        GameManager.Instance.AddScore(1);
                    Destroy(hitObj);
                }
            }

            Destroy(gameObject);
        }
    }
}
