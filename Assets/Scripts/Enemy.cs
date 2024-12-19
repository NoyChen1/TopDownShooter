using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private float damageCooldown = 1f;

    private Transform player;
    private float lastDamageTime;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time > lastDamageTime + damageCooldown)
        {
            collision.gameObject.GetComponent<PlayerController>().CurrentHealth -= damage;
            lastDamageTime = Time.time;
            Debug.Log("Player Damaged!");
        }
    }

    public void TakeDamage(int damage)
    {
        Destroy(gameObject); // Replace with damage logic if needed
    }

}
