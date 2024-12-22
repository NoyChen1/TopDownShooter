using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IPlayerHealth
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip damageSound; 
    [SerializeField] private AudioClip deathSound;  

    private AudioSource audioSource;

    public event Action<int> OnHealthChanged;
    public event Action OnPlayerDied;

    public int MaxHealth => maxHealth;

    public int CurrentHealth
    {
        get => currentHealth;
        set
        {
            if (currentHealth != value)
            {
                currentHealth = value;
                OnHealthChanged?.Invoke(currentHealth);

                if (currentHealth <= 0)
                {
                    Die();
                }
            }
        }
    }

    private void Start()
    {
        CurrentHealth = MaxHealth;
        audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(int damage)
    {
        PlaySound(damageSound);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
    }
    private void Die()
    {
        PlaySound(deathSound);
        OnPlayerDied?.Invoke();
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
