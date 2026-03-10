using UnityEngine;
using UnityEngine.InputSystem;

public class GameMath : MonoBehaviour
{
    public float moveSpeed = 5f;
    //private Vector2 moveinput;
    //Vector3 normalizedVector;
    private Vector2 mouseScreenPostion;
    private Vector3 targetPostion;
    private bool isMoving = false;
    
    //public void OnMove(InputValue value)
    //{
    //    moveinput = value.Get <Vector2>();
    //}

    public void OnPoint(InputValue value)
    {
        mouseScreenPostion = value.Get<Vector2>();
    }

    public void OnClick(InputValue value)
    {
        if (value.isPressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPostion);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject != gameObject)
                {

                    targetPostion = hit.point;
                    targetPostion.y = transform.position.y;
                    isMoving = true;

                    break;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (isMoving)
            {
                Vector3 ab = targetPostion - transform.position;

                float a = (ab.x * ab.x) + (ab.y * ab.y) + (ab.z * ab.z);
                float b = Mathf.Sqrt(a);
                //targetPostion = ab / b;

                if (b<0.05f)
                {
                   isMoving = false;
                    return;
                }

                Vector3 cc = ab / b;
                transform.position += cc * 5 * Time.deltaTime;
                
                //transform.Translate(ab * 5 * Time.deltaTime);

            }

            //Vector3 direction = new Vector3(moveinput.x,moveinput.y,0);
            //float sqrMagnitude = direction.x * direction.x + direction.y * direction.y+ direction.z* direction.z;
            //float magnitude = Mathf.Sqrt(sqrMagnitude);

            //if (magnitude > 0)
            //{
            //    normalizedVector = direction / magnitude;
            //}
            //else
            //    normalizedVector = Vector3.zero;
            //    transform.Translate(direction * moveSpeed * Time.deltaTime);



        }
    }
}
