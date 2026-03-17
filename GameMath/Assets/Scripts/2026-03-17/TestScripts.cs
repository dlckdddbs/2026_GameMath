using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestScripts : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //float degress = 45f;
        //float radins = degress * Mathf.Deg2Rad;
        //Debug.Log("45도 -> 라디안 : " + radins);

        //float radinaValue = MathF.PI / 3;
        //float degreeValue = radinaValue * Mathf.Rad2Deg;
        //Debug.Log("파이/3라디안 -> 도 변환 : " + degreeValue);

    }



    // Update is called once per frame
    void Update()
    {
       
        //float speed = 5f;
        //float angle1 = 30f;
        //float radiaus = angle1 * Mathf.Deg2Rad;

        //Vector3 direction = new Vector3(Mathf.Cos(radiaus), 0, MathF.Sin(radiaus));
        //transform.position += direction * speed * Time.deltaTime;

    }
}
