using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;


[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}

    [SerializeField] private Player Player;
    [SerializeField] private ParticleSystem Explosion;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private float RespawnTime = 3.0f;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private AsteroidSpawner asteroidSpawner;
    [SerializeField] private string menuSceneName;
    [SerializeField] private int scoreThreshold = 1000;
    [SerializeField] private int maxDifficultyLevel = 3;

    public int Score { get; private set; } = 0;
    public int Lives { get; private set; } = 3;
    private int difficultyLevel = 1;
    public int SpawnAmount = 1;

    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }
    }

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        if(Lives <= 0 && (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)))
        {
            NewGame();
        }
        else if(Lives <= 0 && (Input.GetKeyDown(KeyCode.Escape)))
        {
            AudioManager.Instance.musicSorce.clip = AudioManager.Instance.MenuBGM;
            AudioManager.Instance.musicSorce.Play();
            SceneManager.LoadScene(menuSceneName);
        }

        if (Score >= scoreThreshold * difficultyLevel && difficultyLevel <= maxDifficultyLevel)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.DificultyUp);
            difficultyLevel++;
            IncreaseDifficulty();
            if(difficultyLevel >= maxDifficultyLevel) {
                asteroidSpawner.AsteroidPrefab.speed -= 0.15f;
                Debug.Log(asteroidSpawner.AsteroidPrefab.speed);
            }
        }
    }

    private void NewGame()
    {
        AudioManager.Instance.musicSorce.Play();
        Asteroid[] asteroids = FindObjectsByType<Asteroid>(FindObjectsSortMode.None);
        Coins[] coins = FindObjectsByType<Coins>(FindObjectsSortMode.None);
    
        for (int i = 0; i < asteroids.Length; i++) 
        {
            Destroy(asteroids[i].gameObject);
        }

        for(int i = 0; i < coins.Length; i++)
        {
            Destroy(coins[i].gameObject);
        }

        gameOverUI.SetActive(false);
        gameUI.SetActive(true);

        SetScore(0);
        SetLives(3);
        difficultyLevel = 1;
        asteroidSpawner.SpawnAmount = SpawnAmount;
        asteroidSpawner.AsteroidPrefab.speed = 1.0f;
        Respawn();
    }

    private void OnDestroy()
    {
        if(Instance == this){
            Instance = null;
        }        
    }

    private void SetScore(int score){
        Score = score;
        scoreText.text = score.ToString();
    }

    private void SetLives(int lives){
        Lives = lives;
        livesText.text = "×" + lives.ToString();
    }

    public void AsteroidDestroy(Asteroid asteroid)
    {
        Explosion.transform.position = asteroid.transform.position;
        Explosion.Play();

        if (asteroid.Size < 0.75f) {
            SetScore(Score + 75);
        } else if (asteroid.Size < 1.2f) {
            SetScore(Score + 50);
        } else {
            SetScore(Score + 25);
        }
    }

    public void CoinCollected(Coins coins)
    {
        SetScore(Score + 150);
    }
    
    public void PlayerDied(Player player)
    {
        player.gameObject.SetActive(false);

        Explosion.transform.position = Player.transform.position;
        Explosion.Play();

        SetLives(Lives - 1);

        if(Lives <= 0) {
            highScoreText.text = Score.ToString();
            gameOverUI.SetActive(true);
            gameUI.SetActive(false);
            AudioManager.Instance.musicSorce.Stop();
        }
        else {
            Invoke(nameof(Respawn), RespawnTime);
        }   
    }

    private void Respawn()
    {
        Player.transform.position = Vector3.zero;
        Player.gameObject.SetActive(true);
    }

    private void IncreaseDifficulty()
    {
        Debug.Log("Current Difficulty: " + difficultyLevel);
        asteroidSpawner.SpawnAmount++;
        Debug.Log(asteroidSpawner.SpawnAmount);
    }
}
