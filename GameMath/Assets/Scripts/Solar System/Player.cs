using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Vector2 move = new Vector2();
    public float movespeed = 5.0f;
    public bool isLeftParrying = false;
    public bool isRightParrying = false;
    public float rotateSpeed = 90.0f; 

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
       
        Vector3 movement = new Vector3(move.x, 0, move.y);

        transform.Translate(movement * movespeed * Time.deltaTime, Space.Self);

        if (isLeftParrying)
        {
            transform.Rotate(0, -rotateSpeed * Time.deltaTime, 0);
        }

        if (isRightParrying)
        {
            transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("End"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}