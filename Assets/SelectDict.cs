using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectDict : MonoBehaviour
{
    public Dictionary<int, GameObject> selectDic = new Dictionary<int, GameObject>();

    public bool IsEmpty()
    {
        return selectDic.Count == 0;
    }
    public void Select(GameObject ob)
    {
        selectDic.Add(ob.GetInstanceID(), ob);
        ob.GetComponent<UnitManager>().Select();
    }

    public void Deselect(GameObject ob)
    {
        selectDic.Remove(ob.GetInstanceID());
    }

    public void DeselectAll()
    {
        foreach(KeyValuePair<int, GameObject> pair in selectDic) {
            if(pair.Value != null)
            {
                pair.Value.GetComponent<UnitManager>().UnSelect();
            }
        }
        selectDic.Clear();
    }

    public bool IsSelected(int id)
    {
        if (selectDic.ContainsKey(id)) return true;
        else return false;
    }
}
