using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IPlayerController
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float speed = 8f;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private List<Bullet> bullets;
    public float attackCoolDown = 0.5f;
    private float nextAttackTime = 0f;

    public int CurrentHealth { get; set; }
    public int MaxHealth => maxHealth;

    private Vector2 moveInput;
    private Vector2 attackInput;


    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    void Update()
    {
        MovePlayer();
        HandleAttack();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        attackInput = context.ReadValue<Vector2>();
    }
    public void MovePlayer()
    {
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);

        if (movement.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
            transform.Translate(movement * speed * Time.deltaTime, Space.World);
        }
    }

    public void HandleAttack()
    {
        if (attackInput.magnitude > 0.1f &&
            Time.time >= nextAttackTime)
        {
            Vector3 attackDirection = new Vector3(attackInput.x, 0, attackInput.y).normalized;
            if (attackDirection.magnitude > 0.1f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(attackDirection), 0.15f);
                attackPoint.rotation = Quaternion.LookRotation(attackDirection);
                FireProjectile(attackDirection);
                nextAttackTime = Time.time + attackCoolDown;
            }
        }
    }

    private void FireProjectile(Vector3 direction)
    {
        Bullet bullet = bullets.Find(b => !b.gameObject.activeInHierarchy);

        if (bullet != null)
        {
            // Activate and initialize the bullet
            bullet.gameObject.SetActive(true);
            bullet.Initialize(direction, attackPoint);
        }
        else
        {
            Debug.LogWarning("No bullets available in the pool!");
        }
    }
}
