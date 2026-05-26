using UnityEngine;

public class ManualExplode : MonoBehaviour
{
    public float delay = 1.5f;
    public float radius = 5f;
    public float force = 300f;
    public float upwardsModifier = 1f;
    public int count = 0;
    public bool affectPlayer = false;

    void Start()
    {
        Invoke("Explode", delay);
    }

    private void Update()
    {
        if (count == 3)
        {
            Explode();
            count = 0; 
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            Explode();
        }
        else if (col.gameObject.CompareTag("Ground"))
        {
            count++;
        }
    }

    void Explode()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (var col in colliders)
        {
            if (!affectPlayer && col.CompareTag("Player"))
                continue;

            Rigidbody rb = col.attachedRigidbody;
            if (rb == null) continue;

            Vector3 toTarget = rb.position - explosionPos;
            float distance = toTarget.magnitude;
            Vector3 dir = toTarget.normalized;

            float attenuation = 1f - Mathf.Clamp01(distance / radius);

            dir += Vector3.up * upwardsModifier;
            dir = dir.normalized;

            Vector3 impulse = dir * force * attenuation;
            rb.AddForce(impulse, ForceMode.Impulse);
        }
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}