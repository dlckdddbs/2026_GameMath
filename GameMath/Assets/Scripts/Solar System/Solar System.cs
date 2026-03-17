using UnityEngine;

public class SolarSystem: MonoBehaviour
{
    public float viewAngle = 60f;
    public float viewDistance = 5f;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        // 시야 거리만큼 앞쪽으로 뻗은 벡터
        Vector3 forward = transform.forward * viewDistance;

        // 왼쪽 시야 경계
        // 정면 벡터를 y축 기준으로 -viewAngle / 2 만큼 회전
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * forward;

        // 오른쪽 시야 경계
        // 정면 벡터를 y축 기준으로 viewAngle / 2 만큼 회전
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * forward;

        // 씬 뷰에 광선(Ray) 그리기
        Gizmos.DrawRay(transform.position, leftBoundary);
        Gizmos.DrawRay(transform.position, rightBoundary);

        // 캐릭터 앞쪽 방향 (빨간색)
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, forward);
    }
}