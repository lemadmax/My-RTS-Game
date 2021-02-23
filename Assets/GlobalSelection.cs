using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSelection : MonoBehaviour
{
    SelectDict selectTable;

    bool dragSelect = false;

    MeshCollider selectionBox;
    Mesh selectionMesh;

    Vector3 mouseStartPos;
    Vector3 mouseEndPos;

    Vector2[] corners;
    Vector3[] verts;
    Vector3[] vecs;
    void Start()
    {
        selectTable = GetComponent<SelectDict>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            mouseStartPos = Input.mousePosition;
        }

        if(Input.GetMouseButton(0))
        {
            if((mouseStartPos - Input.mousePosition).magnitude > 40)
            {
                dragSelect = true;
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            if(!dragSelect)
            {
                Ray ray = Camera.main.ScreenPointToRay(mouseStartPos);
                RaycastHit hitInfo;
                
                if(Physics.Raycast(ray, out hitInfo, 50000.0f) && 
                    hitInfo.transform.gameObject.tag == "Unit" && 
                    hitInfo.transform.gameObject.GetComponent<UnitManager>().GetGroup() == 0)
                {
                    if(Input.GetKey(KeyCode.LeftShift))
                    {
                        selectTable.Select(hitInfo.transform.gameObject);
                    }
                    else
                    {
                        selectTable.DeselectAll();
                        selectTable.Select(hitInfo.transform.gameObject);
                    }
                }
                else
                {
                    selectTable.DeselectAll();
                }
            }
            else
            {
                verts = new Vector3[4];
                vecs = new Vector3[4];
                int i = 0;
                mouseEndPos = Input.mousePosition;
                corners = getBoundingBox(mouseStartPos, mouseEndPos);
                foreach(Vector2 corner in corners)
                {
                    Ray ray = Camera.main.ScreenPointToRay(corner);
                    RaycastHit hitInfo;
                    if(Physics.Raycast(ray, out hitInfo, 50000.0f, (1 << 8)))
                    {
                        verts[i] = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
                        vecs[i] = ray.origin - hitInfo.point;
                        Debug.DrawLine(Camera.main.ScreenToWorldPoint(corner), hitInfo.point, Color.red, 1.0f);
                    }
                    i++;
                }
                selectionMesh = generateSelectionMesh(verts, vecs);

                selectionBox = gameObject.AddComponent<MeshCollider>();
                selectionBox.sharedMesh = selectionMesh;
                selectionBox.convex = true;
                selectionBox.isTrigger = true;

                if(!Input.GetKey(KeyCode.LeftShift))
                {
                    selectTable.DeselectAll();
                }
                Destroy(selectionBox, 0.02f);
            }
            dragSelect = false;
        }
    }

    private void OnGUI()
    {
        if(dragSelect == true)
        {
            var rect = Utils.GetScreenRect(mouseStartPos, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }

    Vector2[] getBoundingBox(Vector2 p1, Vector2 p2)
    {
        Vector2 newP1;
        Vector2 newP2;
        Vector2 newP3;
        Vector2 newP4;

        if (p1.x < p2.x) //if p1 is to the left of p2
        {
            if (p1.y > p2.y) // if p1 is above p2
            {
                newP1 = p1;
                newP2 = new Vector2(p2.x, p1.y);
                newP3 = new Vector2(p1.x, p2.y);
                newP4 = p2;
            }
            else //if p1 is below p2
            {
                newP1 = new Vector2(p1.x, p2.y);
                newP2 = p2;
                newP3 = p1;
                newP4 = new Vector2(p2.x, p1.y);
            }
        }
        else //if p1 is to the right of p2
        {
            if (p1.y > p2.y) // if p1 is above p2
            {
                newP1 = new Vector2(p2.x, p1.y);
                newP2 = p1;
                newP3 = p2;
                newP4 = new Vector2(p1.x, p2.y);
            }
            else //if p1 is below p2
            {
                newP1 = p2;
                newP2 = new Vector2(p1.x, p2.y);
                newP3 = new Vector2(p2.x, p1.y);
                newP4 = p1;
            }

        }

        Vector2[] corners = { newP1, newP2, newP3, newP4 };
        return corners;
    }

    Mesh generateSelectionMesh(Vector3[] corners, Vector3[] vecs)
    {
        Vector3[] verts = new Vector3[8];
        int[] tris = { 0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 0, 1, 1, 4, 0, 4, 5, 6, 6, 5, 7 };

        for (int i = 0; i < 4; i++)
        {
            verts[i] = corners[i];
        }

        for (int j = 4; j < 8; j++)
        {
            verts[j] = corners[j - 4] + vecs[j - 4] * 100.0f;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = verts;
        mesh.triangles = tris;
        return mesh;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Unit" && other.gameObject.GetComponent<UnitManager>().GetGroup() == 0)
        {
            selectTable.Select(other.gameObject);
        }
    }
}
