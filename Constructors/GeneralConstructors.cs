public class GeneralConstructors
{
    // Private alanlar
    private int health;
    private int score;
    private Label healthLabel;
    private Label scoreLabel;
    private bool isRunning;

    // Parametreli constructor
    public GeneralConstructors(int health, int score, Label healthLabel, Label scoreLabel, bool isRunning = false)
    {
        this.health = health;
        this.score = score;
        this.healthLabel = healthLabel;
        this.scoreLabel = scoreLabel;
        this.isRunning = isRunning;  // isRunning durumunu başlat
        UpdateHealthLabel();  // Sağlık etiketini başlat
        UpdateScoreLabel();   // Skor etiketini başlat
    }

    // Getter ve Setter (Property'ler)
    public int Health
    {
        get { return health; }
        set
        {
            health = value;
            UpdateHealthLabel();  // Sağlık değiştiğinde etiket güncellenir
        }
    }

    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            UpdateScoreLabel();   // Skor değiştiğinde etiket güncellenir
        }
    }

    public Label HealthLabel
    {
        get { return healthLabel; }
        set
        {
            healthLabel = value;
            UpdateHealthLabel();  // Sağlık etiketini yeniden ayarla
        }
    }

    public Label ScoreLabel
    {
        get { return scoreLabel; }
        set
        {
            scoreLabel = value;
            UpdateScoreLabel();   // Skor etiketini yeniden ayarla
        }
    }

    public bool IsRunning
    {
        get { return isRunning; }
        set
        {
            isRunning = value;
            // isRunning değiştiğinde etiketleri kontrol et ve güncelle
            if (isRunning)
            {
                UpdateHealthLabel();
                UpdateScoreLabel();
            }
        }
    }

    // Sağlık etiketini güncelleyen yardımcı metot
    private void UpdateHealthLabel()
    {
        if (isRunning && healthLabel != null)  // Eğer oyun çalışıyorsa, etiketi güncelle
        {
            healthLabel.Text = "Health: " + health.ToString();
        }
    }

    // Skor etiketini güncelleyen yardımcı metot
    private void UpdateScoreLabel()
    {
        if (isRunning && scoreLabel != null)  // Eğer oyun çalışıyorsa, etiketi güncelle
        {
            scoreLabel.Text = "Score: " + score.ToString();
        }
    }
}
