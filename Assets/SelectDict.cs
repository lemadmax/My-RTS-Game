using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectDict : MonoBehaviour
{
    public Dictionary<int, GameObject> selectDic = new Dictionary<int, GameObject>();

    public void select(GameObject ob)
    {
        selectDic.Add(ob.GetInstanceID(), ob);
    }

    public void deselect(GameObject ob)
    {
        selectDic.Remove(ob.GetInstanceID());
    }

    public void deselectAll()
    {
        foreach(KeyValuePair<int, GameObject> pair in selectDic) {
            if(pair.Value != null)
            {

            }
        }
        selectDic.Clear();
    }

    public bool isSelected(int id)
    {
        if (selectDic.ContainsKey(id)) return true;
        else return false;
    }
}
