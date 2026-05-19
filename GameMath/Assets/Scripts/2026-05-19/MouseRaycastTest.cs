using UnityEngine;
using UnityEngine.InputSystem;

public class MouseRaycastTest : MonoBehaviour
{
    public float rayDistance = 100f;
    public float maxForce = 25f; 
    public CameraOrbit cam;

    private float moveInput;
    private Rigidbody targetRb;
    private Vector3 clickPoint;

    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        moveInput = input.x;
        if (cam != null) cam.moveInput = moveInput;
    }

    public void OnClick(InputValue value)
    {

        if (!BilliardsGameManager.Instance.CanHit()) return;
        if (value.isPressed)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
            {
                BilliardsBall ball = hit.collider.GetComponent<BilliardsBall>();

                if (ball != null)
                {
                    int currentTurn = BilliardsGameManager.Instance.currentTurn;
                    if (currentTurn == 1 && ball.ballType != BallType.Player1) return;
                    if (currentTurn == 2 && ball.ballType != BallType.Player2) return;

                    targetRb = ball.rb;
                    clickPoint = hit.point;
                }
            }
        }

        else
        {
            if (targetRb != null)
            {
                Vector3 forceDirection = (targetRb.transform.position - clickPoint);

                forceDirection.y = 0;

                float dragDistance = forceDirection.magnitude;
                forceDirection = forceDirection.normalized;

                float finalForce = Mathf.Clamp(dragDistance * 30f, 5f, maxForce);

                targetRb.AddForce(forceDirection * finalForce, ForceMode.Impulse);

                BilliardsGameManager.Instance.OnBallHitStart();

                targetRb = null; // 타격 완료 후 타겟 초기화
            }
        }
    }
}