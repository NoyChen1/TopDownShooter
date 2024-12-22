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

        if (rb != null)
        {
            rb.velocity = this.direction * speed;
        }

        CancelInvoke(nameof(Deactivate));
        Invoke(nameof(Deactivate), maxLifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            Deactivate();
        }
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
        }
    }
}
