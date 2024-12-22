using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private int health = 10;
    [SerializeField] private float damageCooldown = 1f;

    private Transform player;
    private float lastDamageTime;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip hurtSound; 
    [SerializeField] private AudioClip dieSound;  

    private AudioSource audioSource;

    public static event Action<Enemy> OnEnemyKilledWithObject; 
    public static event Action OnEnemyKilledSimple; 
    

    private void Start()
    {
        player = FindObjectOfType<PlayerHealth>().transform;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        MoveTowardsPlayer();
    } 

    private void MoveTowardsPlayer()
    {

        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && Time.time > lastDamageTime + damageCooldown)
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            lastDamageTime = Time.time;
        }
    }
  
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
        else
        {
            PlaySound(hurtSound);
        }
    }

    private void Die()
    {
        PlaySound(dieSound);
        OnEnemyKilledWithObject?.Invoke(this);
        OnEnemyKilledSimple?.Invoke();
        Destroy(gameObject, dieSound.length);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
