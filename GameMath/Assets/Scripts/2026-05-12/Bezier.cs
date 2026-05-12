using UnityEngine;
using System.Collections.Generic;

public class Bezier : MonoBehaviour
{
    private List<Vector3> points;
    private float time = 0f;
    private float duration = 2f; 
    private bool isInitialized = false;

    public void Setup(Vector3 start, Vector3 end, float p1Rad, float p2Rad, float h1, float h2)
    {

        Vector2 rand1 = Random.insideUnitCircle * p1Rad;
        Vector3 p1 = start + new Vector3(rand1.x, 0f, rand1.y);
        p1.y += h1;

        Vector2 rand2 = Random.insideUnitCircle * p2Rad;
        Vector3 p2 = end + new Vector3(rand2.x, 0f, rand2.y);
        p2.y += h2;

        points = new List<Vector3> { start, p1, p2, end };
        isInitialized = true;
    }

    void Update()
    {
        if (!isInitialized) return;

        time += Time.deltaTime / duration;
        transform.position = DeCasteljau(new List<Vector3>(points), time);

        // 도착하면 삭제
        if (time >= 1f)
        {
            Destroy(gameObject);
        }
    }
    Vector3 DeCasteljau(List<Vector3> p, float t)
    {
        while (p.Count > 1)
        {
            int last = p.Count - 1;
            var next = new List<Vector3>(last);
            for (int i = 0; i < last; i++)
            {
                next.Add(Vector3.Lerp(p[i], p[i + 1], t));
            }
            p = next;
        }
        return p[0];
    }
}