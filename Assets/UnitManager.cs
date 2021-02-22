using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    int group;
    bool selected;

    LineRenderer line;
    int segments = 50;
    float radius = 2.5f;
    void Start()
    {
        selected = false;

        line = gameObject.GetComponent<LineRenderer>();
        line.positionCount = segments + 1;
        line.useWorldSpace = false;
        line.startColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        line.endColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        line.startWidth = 0.3f;
        line.endWidth = 0.3f;
        line.transform.localPosition.Set(0.0f, 0.2f, 0.0f);
        CreatePoints();
        line.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void select()
    {
        selected = true;
        line.enabled = true;
    }

    void unSelect()
    {
        selected = false;
        line.enabled = false;
    }

    void CreatePoints()
    {
        float x;
        float z;
        float angle = 20f;
        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            line.SetPosition(i, new Vector3(x, 0.1f, z));
            angle += (360f / segments);
        }
    }
}
