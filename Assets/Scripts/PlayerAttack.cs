using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour, IPlayerAttack
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private List<Bullet> bullets;
    [SerializeField] private float attackCoolDown = 0.5f;
    [SerializeField] private float rotationSpeed = 8f;


    private float nextAttackTime = 0f;
    private Vector2 attackInput;

    private void Update()
    {
        HandleAttack();
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        attackInput = context.ReadValue<Vector2>();
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
        transform.rotation = Quaternion.Slerp(
            transform.rotation, 
            targetRotation, 
            Time.deltaTime * rotationSpeed);
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
}
