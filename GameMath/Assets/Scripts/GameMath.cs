using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameMath : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 mouseScreenPostion;
    private Vector3 targetPostion;
    private bool Moving = false;
    private bool Sprinting = false;
    float currentSpeed;
    //public void OnMove(InputValue value)
    //{
    //    moveinput = value.Get <Vector2>();
    //}

    public void OnPoint(InputValue value)
    {
        mouseScreenPostion = value.Get<Vector2>();
    }

    public void OnClick(InputValue value)
    {
        if (value.isPressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPostion);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject != gameObject)
                {

                    targetPostion = hit.point;
                    targetPostion.y = transform.position.y;
                    Moving = true;

                    break;
                }
            }
        }


        
    }
    public void OnSprint(InputValue value)
    {
        Sprinting = value.isPressed;
    }

    void Update()
    {
        if (Moving)
        {

            Vector3 direction = targetPostion - transform.position;
            float sqrMagnitude = direction.x * direction.x + direction.y * direction.y + direction.z * direction.z;
            float magnitude = Mathf.Sqrt(sqrMagnitude);


            if (magnitude > 0.01f)
            {
                Vector3 normalizedVector = direction / magnitude;
                transform.Translate(normalizedVector * moveSpeed * Time.deltaTime);
                if (Sprinting)
                {
                    currentSpeed = moveSpeed * 10;
                }
                else
                {
                    currentSpeed = moveSpeed;
                }
               

              
                transform.Translate(normalizedVector * currentSpeed * Time.deltaTime);
            }
            else
            {
                Moving = false;
            }
        }


    }
}
