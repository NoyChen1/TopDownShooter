using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TopDownShooter : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI killCountText;

    [Header("Enemy Settings")]
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private int maxKills = 10;

    /*
    [Header("Effects")]
    [SerializeField] private GameObject enemyDeathEffect;
    [SerializeField] private GameObject playerDeathEffect;
    [SerializeField] private GameObject hitEffect;
    */

    private int killCount = 0;

    private void Start()
    {
        healthBar.maxValue = playerController.MaxHealth;
        healthBar.value = playerController.CurrentHealth;
        UpdateKillCount();
    }

    private void Update()
    {
        healthBar.value = playerController.CurrentHealth;
        if (playerController.CurrentHealth <= 0)
        {
            EndGame("You lost!");
        }
        else if (killCount >= maxKills)
        {
            EndGame("You won!");
        }
    }

    public void OnEnemyKilled()
    {
        killCount++;
        UpdateKillCount();
    }

    private void UpdateKillCount()
    {
        killCountText.text = $"Kills: {killCount}";
    }

    private void EndGame(string message)
    {
        Debug.Log(message);
        Time.timeScale = 0;
    }
}
