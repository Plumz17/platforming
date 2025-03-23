using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    private int score = 0;
    public int highScore;
    public int deathCount;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI deathCountText;
    private Vector3 respawnPosition = new Vector3(45, -7, 0);
    public static PlayerData instance = null;
    private SaveDataJSON saveDataJSON;

    public static PlayerData Instance //Singleton
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<PlayerData>();
            }
            return instance;
        }
    }

    void Awake()
    {
        saveDataJSON = FindFirstObjectByType<SaveDataJSON>();
        saveDataJSON.LoadData();
        SetPlayerData();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal") {
            Debug.Log("You scored!");
            score++;
            if (score > highScore)
            {
                highScore = score;
            }
            UpdateUI();
            TeleportToStart();
        }

        if (collision.gameObject.tag == "Void") {
            Debug.Log("You died!");
            deathCount++;
            score = 0;
            UpdateUI();
            TeleportToStart();
        }
        saveDataJSON.SaveData();
    }

    private void TeleportToStart()
    {
        transform.position = respawnPosition;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }

    public void SetPlayerData() {
        highScore = saveDataJSON.playerData.highScore;
        deathCount = saveDataJSON.playerData.deathCount;
        score = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        scoreText.text = score.ToString();
        highScoreText.text = highScore.ToString();
        deathCountText.text = deathCount.ToString();
    }
}
