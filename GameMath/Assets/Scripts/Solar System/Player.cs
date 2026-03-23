using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;

public class Player : MonoBehaviour
{
    public Vector2 move = new Vector2();
    public float movespeed = 5.0f;
    void OnMove(InputValue value)
    {
   
        move = value.Get<Vector2>();
    }


    void Update()
    {   
        Vector3 ab = new Vector3(move.x, 0, move.y);
        transform.Translate(ab * movespeed * Time.deltaTime,Space.World);
    }
}
