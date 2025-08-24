using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("UI")]
    public Text scoreText;
    public Text highscoreText;

    [Header("Save Key")]
    [SerializeField] private string highscoreKey = "Highscore";

    public int Score { get; private set; }
    public int Highscore { get; private set; }

    private void Awake()
    {
        // Simple singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Highscore = PlayerPrefs.GetInt(highscoreKey, 0);
    }

    private void Start()
    {
        UpdateUI();
    }

    public void AddPoints(int amount)
    {
        if (amount <= 0) return;

        Score += amount;

        if (Score > Highscore)
        {
            Highscore = Score;
            PlayerPrefs.SetInt(highscoreKey, Highscore);
            PlayerPrefs.Save();
        }

        UpdateUI();
    }

    public void ResetScore()
    {
        Score = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreText) scoreText.text = Score + " POINTS";
        if (highscoreText) highscoreText.text = "HIGHSCORE: " + Highscore;
    }
}

