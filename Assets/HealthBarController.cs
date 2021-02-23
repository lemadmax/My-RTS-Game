using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    UnitManager unitManager;
    Transform bar;

    void Start()
    {
        unitManager = GetComponentInParent<UnitManager>();
        bar = transform.Find("Bar");
    }

    void Update()
    {
        SetOrientation();
        bar.localScale.Set((float)unitManager.health / (float)unitManager.maximumHealth, 1.0f, 1.0f);
    }

    void SetOrientation()
    {
        Vector3 temp = (Camera.main.transform.position - transform.position);
        temp.z = 0.0f;
        transform.right = -Camera.main.transform.right;
        transform.LookAt(temp);
    }
}
