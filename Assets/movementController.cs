using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementController : MonoBehaviour
{
    UnitManager unitManager;

    GameObject target;

    Animator animator;

    bool canAttack = true;

    int status;

    float minimumAttackTime = 0.8f;
    float attackTime;
    float timePeriod = 0;

    Vector3 destination;

    Vector3 newPosition;

    private void Start()
    {
        unitManager = gameObject.GetComponentInParent<UnitManager>();
        animator = GetComponentInChildren<Animator>();
        attackTime = (float)(1.0 / unitManager.attackSpeed);
        if (attackTime < minimumAttackTime)
        {
            animator.SetFloat("attackSpeedMultiplier", (float)(minimumAttackTime / attackTime));
        }
        if (unitManager.autoAttack) status = 3;
        else status = 0;
        destination = transform.position;
        newPosition = transform.position;
        float ap = animator.GetFloat("attackSpeedMultiplier");
    }

    void Update()
    {
        if (!unitManager.IsAlive()) return;
        if (unitManager.IsSelected())
        {
            HandleMouseInput();
            HandleKeyInput();
        }
        CorrectRotation();
        Debug.Log(unitManager.GetGroup() + ": " + status);

        switch (status)
        {
            case 0:
                Idle();
                break;
            case 1:
                RotateToDest();
                ChargeToDest();
                break;
            case 2:
                RotateToTarget();
                ChargeToEnemy();
                Attacking();
                break;
            case 3:
                if (target == null && !unitManager.IsTargetListEmpty())
                {
                    target = unitManager.GetAnEnemy();
                }
                if(target != null)
                {
                    destination = target.transform.position;
                    RotateToTarget();
                    ChargeToEnemy();
                    Attacking();
                }
                else
                {
                    RotateToDest();
                    ChargeToDest();
                }
                break;
        }

                
    }

    void ChargeToDest()
    {
        Vector3 dir = destination - transform.position;
        dir.y = 0;
        if (dir.magnitude < 1f)
        {
            if (unitManager.autoAttack) Idle();
            else status = 0;
        }
        else
        {
            if (animator.GetInteger("states") != 1) animator.SetInteger("states", 1);
            transform.position += dir.normalized * unitManager.chargeSpeed * Time.deltaTime;
        }
    }

    void ChargeToEnemy()
    {
        Vector3 dir = target.transform.position - transform.position;
        dir.y = 0;
        if (dir.magnitude > unitManager.attackRange)
        {
            if (animator.GetInteger("states") != 1) animator.SetInteger("states", 1);
            transform.position += dir.normalized * unitManager.chargeSpeed * Time.deltaTime;
        }
    }

    void Attacking()
    {
        if (target != null && (transform.position - target.transform.position).magnitude < unitManager.attackRange)
        {
            if(!target.GetComponent<UnitManager>().IsAlive())
            {
                unitManager.RemoveTarget(target);
                target = null;
                destination = transform.position;
                if (status == 2) status = 0;
                return;
            }
            if (animator.GetInteger("states") != 2) animator.SetInteger("states", 2);
            timePeriod += Time.deltaTime;
            if (attackTime <= minimumAttackTime)
            {
                if (timePeriod > attackTime)
                {
                    timePeriod = 0;
                    target.GetComponent<UnitManager>().OnDamage(gameObject, unitManager.autoDamage);
                }
            }
            else
            {
                if (canAttack && timePeriod > 0.6 * minimumAttackTime)
                {
                    target.GetComponent<UnitManager>().OnDamage(gameObject, unitManager.autoDamage);
                    canAttack = false;
                }
                if (timePeriod > minimumAttackTime)
                {
                    animator.SetInteger("states", 0);
                }
                if (timePeriod > attackTime)
                {
                    timePeriod = 0;
                    canAttack = true;
                    animator.SetInteger("states", 2);
                }
            }
        }
    }

    void Idle()
    {
        if(animator.GetInteger("states") != 0)
            animator.SetInteger("states", 0);
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //destination = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo);
            if (hitInfo.transform.gameObject.tag == "Unit" && hitInfo.transform.GetComponent<UnitManager>().GetGroup() != 0)
            {
                if (unitManager.autoAttack) status = 3;
                else status = 2;
                target = hitInfo.transform.gameObject;
                unitManager.AddTarget(target);
                destination = target.transform.position;
            }
            else
            {
                if (unitManager.autoAttack) status = 3;
                else status = 1;
                target = null;
                destination = hitInfo.point;
                destination.y = 0;
            }
        }
    }

    void HandleKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            status = 0;
        }
    }

    // correct the x and z rotation.
    void CorrectRotation()
    {
        transform.rotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, 0.0f);
        //Debug.Log("rotation: " + transform.rotation.eulerAngles);
        //transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }

    void RotateToTarget()
    {
        //Debug.Log(unitManager.GetGroup() + ": " + "rotating to target");
        Vector3 movementDirection = target.transform.position - transform.position;
        //if (movementDirection.sqrMagnitude < unitManager.attackRange) return;
        float angle = Vector3.Angle(movementDirection, transform.forward);
        if (angle == 0) return;
        float dot = Vector3.Dot(movementDirection, transform.right);
        float minAngle = Mathf.Min(angle, unitManager.angularSpeed * Time.deltaTime);
        if (angle > 1f)
        {
            if (dot > 0)
            {
                transform.Rotate(new Vector3(0, minAngle, 0));
            }
            else
            {
                transform.Rotate(new Vector3(0, -minAngle, 0));
            }
        }
        else
        {
            transform.LookAt(destination);
        }
    }

    void RotateToDest()
    {
        //Debug.Log(unitManager.GetGroup() + ": " + "rotating to dest");
        Vector3 movementDirection = destination - transform.position;
        if (movementDirection.sqrMagnitude < 1f) return;
        float angle = Vector3.Angle(movementDirection, transform.forward);
        if (angle == 0) return;
        float dot = Vector3.Dot(movementDirection, transform.right);
        float minAngle = Mathf.Min(angle, unitManager.angularSpeed * Time.deltaTime);
        if (angle > 1f)
        {
            if (dot > 0)
            {
                transform.Rotate(new Vector3(0, minAngle, 0));
            }
            else
            {
                transform.Rotate(new Vector3(0, -minAngle, 0));
            }
        }
        else
        {
            transform.LookAt(destination);
        }
    }



    private void OnCollisionEnter(Collision collisionInfo)
    {
        if(collisionInfo.gameObject.name == "Terrain")
        {
            Debug.Log("hit terrain" + collisionInfo.transform.position.y);
        }
    }



}
