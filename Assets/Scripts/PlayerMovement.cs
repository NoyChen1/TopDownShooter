using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, IPlayerMovement
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private Vector3 groundMinBounds = new Vector3(-5f, 0f, -5f);
    [SerializeField] private Vector3 groundMaxBounds = new Vector3(5f, 0f, 5f);

    private Vector2 moveInput;

    private void Update()
    {
        MovePlayer();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
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
}
