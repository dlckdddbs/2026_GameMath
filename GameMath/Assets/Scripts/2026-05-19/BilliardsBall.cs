using UnityEngine;

public enum BallType { Player1, Player2, Red }

public class BilliardsBall : MonoBehaviour
{
    public BallType ballType;
    [HideInInspector] public Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 상대 오브젝트가 당구공인지 확인
        BilliardsBall otherBall = collision.gameObject.GetComponent<BilliardsBall>();
        if (otherBall != null)
        {
           
            BilliardsGameManager.Instance.RecordCollision(this.ballType, otherBall.ballType, collision.gameObject);
        }
    }
}