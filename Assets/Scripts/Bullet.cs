using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float maxLifeTime = 3f;

    private Vector3 direction;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Initialize(Vector3 direction, Transform attackPoint)
    {
        this.direction = direction.normalized;
        transform.position = attackPoint.position;
        transform.rotation = Quaternion.LookRotation(direction);

        // Reset velocity and apply new direction
        if (rb != null)
        {
            rb.velocity = this.direction * speed;
        }

        // Schedule deactivation after lifetime
        CancelInvoke(nameof(Deactivate));
        Invoke(nameof(Deactivate), maxLifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            Deactivate();
        }
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);

        // Reset Rigidbody velocity to avoid lingering motion
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
        }
    }
}
