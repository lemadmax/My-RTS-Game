using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementController : MonoBehaviour
{
    public float chargeSpeed;
    public float walkSpeed;
    public float attackSpeed;
    public float attackRange;


    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
