using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Gerencia vidas, pontuaÃ§Ã£o, estado do jogo (jogando, vitÃ³ria, game over) e interface visual.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("ConfiguraÃ§Ãµes de Jogo")]
    [SerializeField] private int startingLives = 3;
    [SerializeField] private int targetScoreToWin = 10;

    [Header("ReferÃªncias da UI")]
    [SerializeField] private Text livesTextLegacy;
    [SerializeField] private TextMeshProUGUI scoreTextTMP;
    [SerializeField] private Text scoreTextLegacy;

    [Header("PainÃ©is de Fim de Jogo")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Text gameOverTextLegacy;
    [SerializeField] private TextMeshProUGUI gameOverTextTMP;

    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private Text victoryTextLegacy;
    [SerializeField] private TextMeshProUGUI victoryTextTMP;

    [SerializeField] private Button restartButton;

    private int lives;
    private int score;
    public bool IsGameOver { get; private set; }
    public bool IsVictory { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1f;
        IsGameOver = false;
        IsVictory = false;
        lives = startingLives;
        score = 0;

        InitializeUI();
        UpdateUI();
    }

    private void Update()
    {
        // Atalho para reiniciar com a tecla R se o jogo estiver terminado
        if (IsGameOver && Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    /// <summary>
    /// Localiza e configura referÃªncias da UI automaticamente caso nÃ£o estejam atribuÃ­das.
    /// </summary>
    private void InitializeUI()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();

        // Busca texto de vidas ("LivresText" ou "LivesText" ou "VidasText")
        if (livesTextLegacy == null)
        {
            GameObject livesObj = GameObject.Find("LivresText") ?? GameObject.Find("LivesText") ?? GameObject.Find("VidasText");
            if (livesObj != null)
            {
                livesTextLegacy = livesObj.GetComponent<Text>();
                // Ajusta Ã¢ncora para canto superior esquerdo
                RectTransform rt = livesObj.GetComponent<RectTransform>();
                if (rt != null)
                {
                    rt.anchorMin = new Vector2(0f, 1f);
                    rt.anchorMax = new Vector2(0f, 1f);
                    rt.pivot = new Vector2(0f, 1f);
                    rt.anchoredPosition = new Vector2(20f, -20f);
                    rt.sizeDelta = new Vector2(250f, 40f);
                }
                if (livesTextLegacy != null)
                {
                    livesTextLegacy.fontSize = 26;
                    livesTextLegacy.fontStyle = FontStyle.Bold;
                    livesTextLegacy.color = new Color(1f, 0.3f, 0.3f, 1f); // Vermelho vibrante contrastante
                }
            }
        }

        // Busca texto de pontuaÃ§Ã£o ("ScoreText")
        if (scoreTextTMP == null && scoreTextLegacy == null)
        {
            GameObject scoreObj = GameObject.Find("ScoreText");
            if (scoreObj != null)
            {
                scoreTextTMP = scoreObj.GetComponent<TextMeshProUGUI>();
                scoreTextLegacy = scoreObj.GetComponent<Text>();

                // Ajusta Ã¢ncora para canto superior direito
                RectTransform rt = scoreObj.GetComponent<RectTransform>();
                if (rt != null)
                {
                    rt.anchorMin = new Vector2(1f, 1f);
                    rt.anchorMax = new Vector2(1f, 1f);
                    rt.pivot = new Vector2(1f, 1f);
                    rt.anchoredPosition = new Vector2(-20f, -20f);
                    rt.sizeDelta = new Vector2(250f, 40f);
                }
                if (scoreTextTMP != null)
                {
                    scoreTextTMP.alignment = TextAlignmentOptions.TopRight;
                    scoreTextTMP.fontSize = 26;
                    scoreTextTMP.color = new Color(1f, 0.9f, 0.2f, 1f); // Amarelo vibrante contrastante
                }
                if (scoreTextLegacy != null)
                {
                    scoreTextLegacy.alignment = TextAnchor.UpperRight;
                    scoreTextLegacy.fontSize = 26;
                    scoreTextLegacy.fontStyle = FontStyle.Bold;
                    scoreTextLegacy.color = new Color(1f, 0.9f, 0.2f, 1f);
                }
            }
        }

        // Busca GameOverPanel
        if (gameOverPanel == null)
        {
            gameOverPanel = GameObject.Find("GameOverPanel");
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);

            // Localiza texto dentro do GameOverPanel
            GameObject goTextObj = GameObject.Find("GameOverText");
            if (goTextObj != null)
            {
                gameOverTextLegacy = goTextObj.GetComponent<Text>();
                gameOverTextTMP = goTextObj.GetComponent<TextMeshProUGUI>();

                if (gameOverTextLegacy != null)
                {
                    gameOverTextLegacy.text = "Fim de jogo, tente novamente";
                    gameOverTextLegacy.fontSize = 32;
                    gameOverTextLegacy.fontStyle = FontStyle.Bold;
                    gameOverTextLegacy.color = Color.white;
                    gameOverTextLegacy.alignment = TextAnchor.MiddleCenter;

                    RectTransform rt = goTextObj.GetComponent<RectTransform>();
                    if (rt != null)
                    {
                        rt.sizeDelta = new Vector2(500f, 80f);
                        rt.anchoredPosition = new Vector2(0f, 50f);
                    }
                }
            }

            // Localiza botÃ£o de reiniciar
            if (restartButton == null)
            {
                GameObject btnObj = GameObject.Find("RestartButtom") ?? GameObject.Find("RestartButton");
                if (btnObj != null)
                {
                    restartButton = btnObj.GetComponent<Button>();
                }
            }

            if (restartButton != null)
            {
                restartButton.onClick.RemoveAllListeners();
                restartButton.onClick.AddListener(Restart);

                RectTransform rt = restartButton.GetComponent<RectTransform>();
                if (rt != null)
                {
                    rt.anchoredPosition = new Vector2(0f, -50f);
                    rt.sizeDelta = new Vector2(200f, 50f);
                }
            }
        }

        // Configura ou cria VictoryPanel
        if (victoryPanel == null && canvas != null)
        {
            Transform existingVictory = canvas.transform.Find("VictoryPanel");
            if (existingVictory != null)
            {
                victoryPanel = existingVictory.gameObject;
            }
            else
            {
                CreateVictoryPanel(canvas);
            }
        }

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Cria dinamicamente a tela de vitÃ³ria no Canvas se ainda nÃ£o existir.
    /// </summary>
    private void CreateVictoryPanel(Canvas canvas)
    {
        GameObject vPanel = new GameObject("VictoryPanel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        vPanel.transform.SetParent(canvas.transform, false);

        RectTransform panelRT = vPanel.GetComponent<RectTransform>();
        panelRT.anchorMin = Vector2.zero;
        panelRT.anchorMax = Vector2.one;
        panelRT.offsetMin = Vector2.zero;
        panelRT.offsetMax = Vector2.zero;

        Image panelImg = vPanel.GetComponent<Image>();
        panelImg.color = new Color(0f, 0f, 0f, 0.75f); // Fundo escuro semitransparente

        // Texto principal de vitÃ³ria
        GameObject textObj = new GameObject("VictoryText", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
        textObj.transform.SetParent(vPanel.transform, false);

        RectTransform textRT = textObj.GetComponent<RectTransform>();
        textRT.anchorMin = new Vector2(0.5f, 0.5f);
        textRT.anchorMax = new Vector2(0.5f, 0.5f);
        textRT.pivot = new Vector2(0.5f, 0.5f);
        textRT.anchoredPosition = new Vector2(0f, 30f);
        textRT.sizeDelta = new Vector2(600f, 100f);

        Text victoryText = textObj.GetComponent<Text>();
        victoryText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf") ?? Resources.GetBuiltinResource<Font>("Arial.ttf");
        victoryText.text = "Boa, você ganhou";
        victoryText.fontSize = 42;
        victoryText.fontStyle = FontStyle.Bold;
        victoryText.color = new Color(0.2f, 1f, 0.4f, 1f); // Verde vibrante
        victoryText.alignment = TextAnchor.MiddleCenter;

        // Subtexto indicando reinÃ­cio
        GameObject subTextObj = new GameObject("VictorySubText", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
        subTextObj.transform.SetParent(vPanel.transform, false);

        RectTransform subRT = subTextObj.GetComponent<RectTransform>();
        subRT.anchorMin = new Vector2(0.5f, 0.5f);
        subRT.anchorMax = new Vector2(0.5f, 0.5f);
        subRT.pivot = new Vector2(0.5f, 0.5f);
        subRT.anchoredPosition = new Vector2(0f, -40f);
        subRT.sizeDelta = new Vector2(500f, 50f);

        Text subText = subTextObj.GetComponent<Text>();
        subText.font = victoryText.font;
        subText.text = "Reiniciando em instantes...";
        subText.fontSize = 22;
        subText.color = Color.white;
        subText.alignment = TextAnchor.MiddleCenter;

        victoryPanel = vPanel;
        victoryTextLegacy = victoryText;
    }

    private void UpdateUI()
    {
        // Atualiza Vidas no HUD (canto superior esquerdo)
        if (livesTextLegacy != null)
        {
            livesTextLegacy.text = "Vidas: " + lives;
        }

        // Atualiza PontuaÃ§Ã£o no HUD (canto superior direito)
        string scoreStr = "Pontuação: " + score + " / " + targetScoreToWin;
        if (scoreTextTMP != null)
        {
            scoreTextTMP.text = scoreStr;
        }
        if (scoreTextLegacy != null)
        {
            scoreTextLegacy.text = scoreStr;
        }
    }

    /// <summary>
    /// Adiciona pontos Ã  pontuaÃ§Ã£o ao destruir um meteoro.
    /// </summary>
    public void AddScore(int amount = 1)
    {
        if (IsGameOver) return;

        score += amount;
        UpdateUI();
        Debug.Log("Score atual: " + score);

        if (score >= targetScoreToWin)
        {
            TriggerVictory();
        }
    }

    /// <summary>
    /// Chamado quando a nave colide com um meteoro.
    /// </summary>
    public void PlayerHit()
    {
        if (IsGameOver) return;

        lives--;
        UpdateUI();
        Debug.Log("Nave atingida! Vidas restantes: " + lives);

        if (lives <= 0)
        {
            TriggerGameOver();
        }
    }

    /// <summary>
    /// Tela de VitÃ³ria quando atinge 10 pontos.
    /// </summary>
    private void TriggerVictory()
    {
        IsGameOver = true;
        IsVictory = true;
        Debug.Log("vitoria, pontuação maxima atingida.");

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        // Pausa novos meteoros e tiros
        StartCoroutine(VictoryRestartRoutine());
    }

    private IEnumerator VictoryRestartRoutine()
    {
        // Aguarda 3 segundos em tempo real
        yield return new WaitForSeconds(3.0f);

        Restart();
    }

    /// <summary>
    /// Tela de Game Over quando perde todas as 3 vidas.
    /// </summary>
    private void TriggerGameOver()
    {
        IsGameOver = true;
        Debug.Log("Game Over! Vidas esgotadas.");

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            if (gameOverTextLegacy != null)
                gameOverTextLegacy.text = "Fim de jogo, tente novamente";
            if (gameOverTextTMP != null)
                gameOverTextTMP.text = "Fim de jogo, tente novamente";
        }

        // Para o jogo completamente
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Reinicia a partida.
    /// </summary>
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
