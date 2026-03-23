using UnityEngine;

public class Motion : MonoBehaviour
{
    public Transform centerTarget;
    public float orbitSpeed = 2f;

    public float orbitRadius;
    private float currentAngle;

    void Start()
    {
        if (centerTarget == null) return;

        orbitRadius = Vector3.Distance(transform.position, centerTarget.position);

        Vector3 direction = transform.position - centerTarget.position;

       
        currentAngle = Mathf.Atan2(direction.z, direction.x);
    }

    void Update()
    {
        if (centerTarget == null) return;

        currentAngle += Time.deltaTime * orbitSpeed;

        float x = centerTarget.position.x + orbitRadius * Mathf.Cos(currentAngle);
        float z = centerTarget.position.z + orbitRadius * Mathf.Sin(currentAngle);

        transform.position = new Vector3(x, transform.position.y, z);
    }
}