using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public float health;
    public float maximumHealth;
    public float chargeSpeed;
    public float walkSpeed;
    public float angularSpeed;
    public float attackSpeed;
    public float attackRange;
    public float autoDamage;

    public bool  autoAttack;

    public GameObject body;
    public GameObject deadBody;
    public GameObject healthBar;

    int group;
    bool selected = false;
    bool isUnderAttack = false;
    bool isAlive = true;
    List<GameObject> targets = new List<GameObject>();

    LineRenderer line;
    int segments = 50;
    float radius = 2.5f;

    public void init(int _group)
    {
        group = _group;
    }

    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        line.positionCount = segments + 1;
        line.useWorldSpace = false;
        line.startColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        line.endColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        line.startWidth = 0.3f;
        line.endWidth = 0.3f;
        line.transform.localPosition.Set(0.0f, 0.2f, 0.0f);
        CreatePoints();
        line.enabled = false;
        group = GetComponentInParent<PlayerManager>().group;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsUnderAttack()
    {
        return isUnderAttack;
    }

    public bool IsTargetListEmpty()
    {
        return targets.Count == 0;
    }

    public void OnDamage(GameObject ob, float damage)
    {
        isUnderAttack = true;
        health = Mathf.Max(0.0f, health - damage);
        if (health == 0.0f)
        {
            OnDeath();
        }
        targets.Add(ob);
    }

    public void AddTarget(GameObject ob)
    {
        targets.Add(ob);
    }

    public void RemoveTarget(GameObject ob)
    {
        targets.Remove(ob);
    }

    public GameObject GetAnEnemy()
    {
        return targets[0];
    }

    public int GetGroup()
    {
        return group;
    }

    public bool IsSelected()
    {
        return selected;
    }

    public void Select()
    {
        selected = true;
        line.enabled = true;
    }

    public void UnSelect()
    {
        selected = false;
        line.enabled = false;
    }

    public bool IsAlive()
    {
        return isAlive;
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

    void OnDeath()
    {
        isAlive = false;
        if(selected)
        {
            GameManager.GM_instance.GetComponentInChildren<SelectDict>().Deselect(gameObject);
        }
        Destroy(body);
        Destroy(healthBar);
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.x = -90f;
        Quaternion rot = Quaternion.Euler(rotation);
        Instantiate(deadBody, transform.position, rot);
    }

    private void OnDestroy()
    {

    }
}
