using UnityEngine;
using UnityEngine.InputSystem; 

public class BombLauncher : MonoBehaviour
{
    public GameObject bombPrefab; 
    public Transform firePoint;   
    public Transform targetPoint; 

    [Header("Curve Settings")]
    public float p1Radius = 3f;
    public float p2Radius = 3f;
    public float pHeight = 5f;


    public void OnAttack()
    {
        for (int i = 0; i < 10; i++)
        {
            LaunchBomb();
        }
    }

    void LaunchBomb()
    {
        if (bombPrefab == null || firePoint == null || targetPoint == null) return;

        GameObject go = Instantiate(bombPrefab, firePoint.position, Quaternion.identity);

        Bezier projectile = go.GetComponent<Bezier>();
        if (projectile != null)
        {
            projectile.Setup(
                firePoint.position,
                targetPoint.position,
                p1Radius,
                p2Radius,
                Random.Range(2f, pHeight), 
                Random.Range(2f, pHeight)
            );
        }
    }
}