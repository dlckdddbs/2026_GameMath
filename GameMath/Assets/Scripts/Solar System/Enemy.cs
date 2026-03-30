using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public enum EnemyType { White, Yellow, Red }
    public EnemyType type;

    public Player playerScript;
    public float moveSpeed = 3.0f;
    public float parryDistance = 2.5f; 

    private float viewDistance;
    private float viewAngle;
    private float rotationSpeed;
    private bool isChasing = false;

    void Start()
    {
        if (playerScript == null) playerScript = FindFirstObjectByType<Player>();

        SetupStats();
    }

    void SetupStats()
    {
        switch (type)
        {
            case EnemyType.White:
                viewAngle = 60f; rotationSpeed = 30f; viewDistance = 5f;
                break;
            case EnemyType.Yellow:
                viewAngle = 90f; rotationSpeed = 45f; viewDistance = 8f;
                break;
            case EnemyType.Red:
                viewAngle = 180f; rotationSpeed = 60f; viewDistance = 15f;
                break;
        }
    }

    void Update()
    {
        if (playerScript == null) return;

        float distance = Vector3.Distance(transform.position, playerScript.transform.position);

        if (!isChasing && CheckPlayerInFOV(distance))
        {
            isChasing = true;
        }

        if (isChasing)
        {
            Vector3 dir = (playerScript.transform.position - transform.position).normalized;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 5f);
            transform.position += transform.forward * moveSpeed * Time.deltaTime;

            if (distance <= parryDistance)
            {
                CheckParryMechanism();
            }
        }
        else
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
    }

    bool CheckPlayerInFOV(float dist)
    {
        if (dist > viewDistance) return false;

        Vector3 dirToPlayer = (playerScript.transform.position - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, dirToPlayer);
        float cosThreshold = Mathf.Cos((viewAngle * 0.5f) * Mathf.Deg2Rad);

        return dot >= cosThreshold;
    }

    void CheckParryMechanism()
    {
        Vector3 playerForward = playerScript.transform.forward;
        Vector3 dirToEnemy = (transform.position - playerScript.transform.position).normalized;
        Vector3 cross = Vector3.Cross(playerForward, dirToEnemy);
        bool enemyOnRight = cross.y > 0;
        if (enemyOnRight && playerScript.isRightParrying)
        {
            Destroy(gameObject);
        }
        else if (!enemyOnRight && playerScript.isLeftParrying)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}