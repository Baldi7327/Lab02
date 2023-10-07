using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    [SerializeField]
    public PathManager pathManager;

    List<Waypoint> thePath;
    Waypoint target;

    public float MoveSpeed;
    public float RotateSpeed;

    public Animator animator;
    bool isWalking;

    private void Start()
    {
        isWalking = false;
        animator.SetBool("isWalking", isWalking);
        thePath = pathManager.GetPath();
        if (thePath != null && thePath.Count > 0)
        {
            //set starting target to first waypoint
            target = thePath[0];
        }
    }

    void rotateTowardsTarget()
    {
        float stepSize = RotateSpeed * Time.deltaTime;

        Vector3 targetDir = target.pos - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, stepSize, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }
    void moveForwards()
    {
        float stepSize = Time.deltaTime * MoveSpeed;
        float distanceToTarget = Vector3.Distance(transform.position, target.pos);
        if (distanceToTarget < stepSize)
        {
            //we will overshoot the target with this as is, think of something better
            return;
        }
        //take a step forward
        Vector3 moveDir = Vector3.forward;
        transform.Translate(moveDir * stepSize);
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            //toggle if any key is pressed
            isWalking = !isWalking;
            animator.SetBool("isWalking", isWalking);
        }
        if (isWalking)
        {
            rotateTowardsTarget();
            moveForwards();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            isWalking = !isWalking;
            animator.SetBool("isWalking", isWalking);
        }
        else
        {
            //switch to next target
            target = pathManager.GetNextTarget();
        }
        


    }

    
}
