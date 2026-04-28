using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class PredictionLineRender : MonoBehaviour
{

    public Slerp cameraScript; 
    [Range(1f, 5f)] public float extend = 1.5f;

    private LineRenderer lr;
    private Transform currentTarget;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.enabled = false; 
    }

    void Update()
    {
        if (currentTarget == null) return;

        Vector3 a = transform.position;
        Vector3 b = currentTarget.position;
        Vector3 pred = Vector3.LerpUnclamped(a, b, extend);

        lr.SetPosition(0, a);
        lr.SetPosition(1, pred);
    }

    public void OnRightClick(InputValue value)
    {
        if (!value.isPressed) return;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                currentTarget = hit.transform;
                cameraScript.target = currentTarget;
                lr.enabled = true;
                Debug.Log("dkdk");
            }
            else { ResetTarget(); }
        }
        else { ResetTarget(); }
    }

    void ResetTarget()
    {
        currentTarget = null;
        cameraScript.target = null;
        lr.enabled = false;
    }
}
