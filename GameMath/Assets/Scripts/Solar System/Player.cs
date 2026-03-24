using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class Player : MonoBehaviour
{
    public Vector2 move = new Vector2();
    public float movespeed = 5.0f;
    public bool isLeftParrying = false;
    public bool isRightParrying = false;
    public float rotateSpeed = 45.0f;

    void OnMove(InputValue value)
    {
   
        move = value.Get<Vector2>();
    }

    public void OnLeftParry(InputValue value)
    {
        isLeftParrying = value.isPressed;
    }

    public void OnRightParry(InputValue value)
    {
        isRightParrying = value.isPressed;
    }


    void Update()
    {   
        Vector3 ab = new Vector3(move.x, 0, move.y);
        transform.Translate(ab * movespeed * Time.deltaTime,Space.World);

        if (isLeftParrying)
        {
            transform.Rotate(0, -rotateSpeed  * Time.deltaTime, 0);
        }

        // E를 누르고 있으면 오른쪽으로 회전
        if (isRightParrying)
        {
            transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
        }
    }

   
}
