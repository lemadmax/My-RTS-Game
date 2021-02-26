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
        bar.transform.localScale = new Vector3((float)unitManager.health / (float)unitManager.maximumHealth, 1.0f, 1.0f);
    }

    void SetOrientation()
    {
        transform.right = -Camera.main.transform.right;
        transform.forward = -Camera.main.transform.forward;
    }
}
