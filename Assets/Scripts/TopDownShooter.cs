using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TopDownShooter : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI killCountText;

    [Header("Enemy Settings")]
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private int maxKills = 10;

    [Header("EndGame Settings")]
    [SerializeField] private TextMeshProUGUI endGameText;
    [SerializeField] private GameObject endGamePanel;


    private int killCount = 0;

    private void OnEnable()
    {
        Enemy.OnEnemyKilledSimple += HandleOnEnemyKilled;
        playerHealth.OnHealthChanged += UpdateHealthBar;
        playerHealth.OnPlayerDied += () => EndGame("You lost!");

    }

    private void OnDisable()
    {
        Enemy.OnEnemyKilledSimple -= HandleOnEnemyKilled;
        playerHealth.OnHealthChanged -= UpdateHealthBar;
        playerHealth.OnPlayerDied -= () => EndGame("You lost!");
    }


    private void Start()
    {
        healthBar.maxValue = playerHealth.MaxHealth;
        healthBar.value = playerHealth.CurrentHealth;
        UpdateKillCount();
    }

    private void UpdateHealthBar(int newHealth)
    {
        healthBar.value = newHealth;
    }

    public void HandleOnEnemyKilled()
    {
        killCount++;
        UpdateKillCount();

        if (killCount >= maxKills)
        {
            EndGame("You won!");
        }
    }

    private void UpdateKillCount()
    {
        killCountText.text = $"Kills: {killCount}";
    }

    private void EndGame(string message)
    {
        endGamePanel.gameObject.SetActive(true);
        endGameText.gameObject.SetActive(true);
        endGameText.text = message;
        Debug.Log(message);
        Time.timeScale = 0;
    }
}
