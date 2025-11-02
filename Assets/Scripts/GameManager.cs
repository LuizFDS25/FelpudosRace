using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TMP_Text timerText;
    
    public GameObject gameOverPanel;
    public TMP_Text finalTimeTextBad;

    public GameObject gameWonPanel;
    public TMP_Text finalTimeTextGood;

    private float tempoSobrevivencia = 0f;
    private bool jogoAtivo = true;
    private Vector3 originalScale;

    private float proximoAviso = 30f;

    public float duracaoJogo = 90f;

    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        Time.timeScale = 1;
        jogoAtivo = true;

        gameOverPanel.SetActive(false);
        gameWonPanel.SetActive(false);

        if (timerText != null)
            originalScale = timerText.transform.localScale;
    }

    void Update()
    {
        if (!jogoAtivo) return;

        tempoSobrevivencia += Time.deltaTime;
        AtualizarTimerUI();

        if (tempoSobrevivencia >= proximoAviso)
        {
            StartCoroutine(AnimarTimer());
            proximoAviso += 10f;
        }

        if (tempoSobrevivencia >= duracaoJogo)
        {
            GameWon();
        }
    }

    
    void AtualizarTimerUI()
    {
        if (timerText != null)
            timerText.text = FormatTempo(tempoSobrevivencia);
    }

    
    string FormatTempo(float tempo)
    {
        int minutos = Mathf.FloorToInt(tempo / 60f);
        int segundos = Mathf.FloorToInt(tempo % 60f);
        int milissegundos = Mathf.FloorToInt((tempo * 100f) % 100f);

        // Coloca milissegundos em tamanho menor para destacar visualmente
        return $"{minutos:00}:{segundos:00}:<size=70%>{milissegundos:00}</size>";
    }


    IEnumerator AnimarTimer()
    {
        float t = 0f;
        float popScale = 1.4f;
        float speed = 6f;

        Color originalColor = timerText.color;
        Color destaque = new Color(1f, 0.6f, 0.2f);

        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            float scale = Mathf.Lerp(1f, popScale, Mathf.Sin(t * Mathf.PI));
            timerText.transform.localScale = originalScale * scale;
            timerText.color = Color.Lerp(originalColor, destaque, Mathf.Sin(t * Mathf.PI));
            yield return null;
        }

        timerText.transform.localScale = originalScale;
        timerText.color = originalColor;
    }

    public void GameOver()
    {
        jogoAtivo = false;
        Time.timeScale = 0;

        if (timerText != null)
            Destroy(timerText.gameObject);

        gameOverPanel.SetActive(true);

        if (finalTimeTextBad != null)
            finalTimeTextBad.text = "Tempo Final: " + FormatTempo(tempoSobrevivencia).Replace(" ", "");

    }

    public void GameWon()
    {
        jogoAtivo = false;
        Time.timeScale = 0;

        if (timerText != null)
            Destroy(timerText.gameObject);

        gameWonPanel.SetActive(true);

        if (finalTimeTextGood != null)
            finalTimeTextGood.text = "Parabens! Seu Tempo Final Foi: " + FormatTempo(tempoSobrevivencia).Replace(" ", "");

    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public float GetTempoSobrevivencia()
    {
        return tempoSobrevivencia;
    }

    public void VoltarMenu ()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
}
