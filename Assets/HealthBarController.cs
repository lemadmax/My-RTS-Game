using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 temp = Camera.main.transform.position - transform.position;
        temp.z = 0.0f;
        transform.right = -Camera.main.transform.right;
        transform.LookAt(temp);
    }
}
