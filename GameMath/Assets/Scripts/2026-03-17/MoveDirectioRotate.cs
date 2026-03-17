using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveDirectioRotate : MonoBehaviour
{
    private Vector2 moveInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log("dkdkdk");
    }

    // Update is called once per frame
    void Update()
    {
        if (moveInput.sqrMagnitude > 0.01f)
        {
            float angle = MathF.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
