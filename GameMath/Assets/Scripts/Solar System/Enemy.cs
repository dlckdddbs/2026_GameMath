using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;       
    public float viewDistance = 5f; 
    public float viewAngle = 60f;    

    private Vector3 originScale;   

    void Start()
    {
        originScale = transform.localScale;
    }

    void Update()
    {
        if (player == null) return;

        if (CheckPlayerInFOV())
        {
            
            transform.localScale = Vector3.one * 2f;
        }
        else
        {
            transform.localScale = originScale;
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
}