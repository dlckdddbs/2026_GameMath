using UnityEngine;
using UnityEngine.InputSystem;

public class Player2 : MonoBehaviour
{
    public float movespeed = 5f;

    public GameObject skill1Prefab; 
    public GameObject skill2Prefab; 

    public Transform spawnPoint;
    public float mineDistance = 3f; 

    public bool isLeftParrying = false;
    public bool isRightParrying = false;
    public float rotateSpeed = 90.0f;
    public Vector2 move = new Vector2();

    void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    public void OnLeftParry(InputValue value) { isLeftParrying = value.isPressed; }
    public void OnRightParry(InputValue value) { isRightParrying = value.isPressed; }

    void Update()
    {
        Vector3 movement = new Vector3(move.x, 0, move.y);

        transform.Translate(movement * movespeed * Time.deltaTime, Space.World);


        if (movement.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(movement);
        }

        if (isLeftParrying) transform.Rotate(0, -rotateSpeed * Time.deltaTime, 0);
        if (isRightParrying) transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }

    public void OnJump()
    {
        if (skill1Prefab != null)
        {

            Vector3 pos = (spawnPoint != null) ? spawnPoint.position : transform.position;

            Quaternion rot = (spawnPoint != null) ? spawnPoint.rotation : transform.rotation;

            GameObject bomb = Instantiate(skill1Prefab, pos, rot);

            Collider playerCollider = GetComponent<Collider>();
            Collider bombCollider = bomb.GetComponent<Collider>();
            if (playerCollider != null && bombCollider != null)
            {
                Physics.IgnoreCollision(playerCollider, bombCollider);
            }

            ReflectAuto movement = bomb.GetComponent<ReflectAuto>();
            if (movement != null)
            {
                Vector3 shootDir = (spawnPoint != null) ? spawnPoint.forward : transform.forward;
                movement.velocity = shootDir * 10f + Vector3.up * 2f;
            }
        }
    }

    public void OnJump1()
    {
        if (skill2Prefab != null)
        {
            Vector3 pos = spawnPoint.position;
            Instantiate(skill2Prefab, pos, transform.rotation);
        }
    }
}