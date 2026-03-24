using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float viewDistance = 5f;
    public float viewAngle = 90f;
    public float moveSpeed = 2f;     // 이동 속도
    public float rotationSpeed = 5f; // 회전 속도

    void Update()
    {
       
        if (player == null) return;

        if (CheckPlayerInFOV())
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        else
        {
            transform.Rotate(0, 45f * rotationSpeed * Time.deltaTime, 0);
        }

    }

    bool CheckPlayerInFOV()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > viewDistance)
            return false;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        float dotProduct = Vector3.Dot(transform.forward, directionToPlayer);
        float cosThreshold = Mathf.Cos((viewAngle * 0.5f) * Mathf.Deg2Rad);

        return dotProduct >= cosThreshold;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("충돌");
            Destroy(collision.gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}