using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IPlayerMovement
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float rotationSpeed = 8f;

    [SerializeField] private Vector3 groundMinBounds; 
    [SerializeField] private Vector3 groundMaxBounds; 

    [SerializeField] private Transform attackPoint;
    [SerializeField] private List<Bullet> bullets;
    public float attackCoolDown = 0.5f;
    private float nextAttackTime = 0f;

    public event Action<int> OnHealthChanged;
    public event Action OnPlayerDied;
    [SerializeField] private int currentHealth;

    public int CurrentHealth { 
        get => currentHealth;
        set
        {
            if (currentHealth != value)
            {
                currentHealth = value;
                OnHealthChanged?.Invoke(currentHealth);
                if(currentHealth <= 0)
                {
                    OnPlayerDied?.Invoke();
                }
            }
        }
    }
    public int MaxHealth => maxHealth;

    private Vector2 moveInput;
    private Vector2 attackInput;

    private void Awake()
    {
        groundMinBounds = new Vector3(-5f, 0f, -5f);
        groundMaxBounds = new Vector3(5f, 0f, 5f);
    }

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

            Vector3 targetPosition = transform.position + (movement * speed * Time.deltaTime);
            targetPosition.x = Mathf.Clamp(targetPosition.x, groundMinBounds.x, groundMaxBounds.x);
            targetPosition.z = Mathf.Clamp(targetPosition.z, groundMinBounds.z, groundMaxBounds.z);

            transform.position = targetPosition;
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
                SmoothRotation(attackDirection);
                transform.rotation = Quaternion.LookRotation(attackDirection);
                FireProjectile(attackDirection);
                nextAttackTime = Time.time + attackCoolDown;
            }
        }
    }

    private void SmoothRotation(Vector3 direction)
    {
        
        Vector3 flatDirection = new Vector3(direction.x, 0f, direction.z).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(flatDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
    private void FireProjectile(Vector3 direction)
    {
        Bullet bullet = bullets.Find(b => !b.gameObject.activeInHierarchy);

        if (bullet != null)
        {
            bullet.gameObject.SetActive(true);
            bullet.Initialize(direction, attackPoint);
        }
        else
        {
            Debug.LogWarning("No bullets available in the pool!");
        }
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
    }
}
