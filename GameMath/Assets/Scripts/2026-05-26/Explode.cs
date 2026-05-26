using UnityEngine;

public class Explode : MonoBehaviour
{
    public float radius = 10f;
    public int count = 0;
    void Start()
    {
        Invoke("RunExplode", 1f);
    }
    private void Update()
    {
        if (count == 3)
        {
            RunExplode();
            count = 0;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            count++;
        }
        else if (col.gameObject.CompareTag("Enemy"))
        {
            RunExplode();
        }
    }

    void RunExplode()
    {
        Vector3 explosionPos = transform.position;

        Collider[] hitColliders = Physics.OverlapSphere(explosionPos, radius);

        foreach (var col in hitColliders)
        {

            if (col.CompareTag("Enemy"))
            {
                Debug.Log("적이 폭발 범위 안에 있습니다. (데미지 처리 가능)");
                
            }
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}