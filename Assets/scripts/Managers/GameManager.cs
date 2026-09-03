using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gerencia score, vidas e estado do jogo.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int startingLives = 3;
    private int lives;
    private int score;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        lives = startingLives;
        score = 0;
        Debug.Log("GameManager iniciado. Vidas: " + lives + " | Score: " + score);
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);
        // Aqui você pode chamar UIManager.Instance.UpdateScore(score);
    }

    public void PlayerHit()
    {
        lives--;
        Debug.Log("Player atingido! Vidas restantes: " + lives);

        if (lives <= 0)
        {
            Debug.Log("Game Over!");
            // Aqui você pode chamar UIManager.Instance.ShowGameOver();
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
