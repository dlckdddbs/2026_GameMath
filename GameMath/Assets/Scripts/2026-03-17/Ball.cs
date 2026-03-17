using TMPro;
using Unity.VisualScripting;
using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public TMP_InputField angleInputField;
    public GameObject spherePrefabs;
    public GameObject ShootPoint;
    public bool second = true;
    public float force = 15f;
    
    void Start()
    {
        
    }
    public void Launch()
    {
        float angle = float.Parse(angleInputField.text);
        float red = angle * Mathf.Deg2Rad;

        Vector3 dir = new Vector3(Mathf.Cos(red), 0f, Mathf.Sin(red));
        if (second)
        {
            StartCoroutine("printAfterwait");
            GameObject sphere = Instantiate(spherePrefabs, ShootPoint.transform.position, Quaternion.identity);
            Rigidbody rb = sphere.GetComponent<Rigidbody>();
            rb.AddForce((dir + Vector3.up * .3f).normalized * force, ForceMode.Impulse);
        }

        //Debug.Log("น฿ป็");
        
            
    }

    IEnumerator printAfterwait()
    {
        second = false;      
        yield return new WaitForSeconds(1.0f);
        second = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
