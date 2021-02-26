using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GM_instance;

    public Texture2D cursorArrow;
    public Texture2D cursorAttack;
    public Texture2D cursorSelect;

    GlobalSelection globalSelection;

    void Start()
    {
        GM_instance = this;
        globalSelection = GetComponentInChildren<GlobalSelection>();
    }
    void Update()
    {
        UpdateCursor();
    }

    private void UpdateCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo, 50000.0f))
        {
            if(hitInfo.transform.gameObject.tag == "Unit")
            {
                if(hitInfo.transform.GetComponent<UnitManager>().GetGroup() == 0)
                {
                    Cursor.SetCursor(cursorSelect, Vector2.zero, CursorMode.ForceSoftware);
                }
                else
                {
                    if (globalSelection.IsUnitSelected())
                    {
                        Cursor.SetCursor(cursorAttack, Vector2.zero, CursorMode.ForceSoftware);
                    }
                }
            }
            else
            {
                Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.ForceSoftware);
            }
        }

    }


}
