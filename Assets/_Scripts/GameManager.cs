using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] float generateNextPlatformDistance;
    [SerializeField] ObjectGenerator generator;
    [SerializeField] List<Platform> platforms;
    [SerializeField] GameObject player;

    [Header("Death Panel")]
    [SerializeField] GameObject deathPanel;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text bestScoreText;

    PlayerController playerController;
    PlayerStats playerStats;
    public List<Platform> Platforms { get => platforms; set => platforms = value; }
    public ObjectGenerator Generator { get => generator; set => generator = value; }

    private void Awake()
    {
        playerController = player.GetComponent<PlayerController>();
        playerStats = player.GetComponent<PlayerStats>();

        playerController.OnPlayerDeath.AddListener(OpenDeathMenu);

        Generator.GeneratePlatform();
        Generator.GeneratePlatform();
    }
    private void Update()
    {
        if(Vector3.Distance(Generator.LastPlatform.transform.position, player.transform.position) < generateNextPlatformDistance)
        {
            Generator.GeneratePlatform();
        }  
    }
    private void OpenDeathMenu()
    {
        Time.timeScale = 0f;

        deathPanel.SetActive(true);

        scoreText.text = "Score : " + playerStats.DistanceTraveled.ToString("0");

        if (playerStats.DistanceTraveled > playerStats.BestScore)
        {
            playerStats.BestScore = playerStats.DistanceTraveled;
            bestScoreText.text = "Best Score : " + playerStats.BestScore.ToString("0");
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
        playerController.StopAllCoroutines();
    }
}
