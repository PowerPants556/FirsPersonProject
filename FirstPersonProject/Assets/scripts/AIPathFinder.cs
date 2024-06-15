using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPathFinder : MonoBehaviour
{
    [SerializeField] private float moveSpeed, turnSpeed;
    private int currentPoint = 0;
    private bool isAttack = false;

    [SerializeField] private Transform[] patrolPoints;
    private Collider[] destColliders;
    [SerializeField] private Rigidbody rb;
    [SerializeField] Transform entityT, targetT;


    private void Awake()
    {

    }

    void Start()
    {
        InvokeRepeating("FindTarget", 0f,2f);
    }

    void Update()
    {
        if (isAttack)
        {
            Move(targetT);
        }
        else
        {
            Move(patrolPoints[currentPoint]);
        }
    }

    private void Move(Transform target)
    {
        Vector3 targetDir = target.position - entityT.position;
        targetDir = new Vector3(targetDir.x, 0f, targetDir.z);
        float singleStep = turnSpeed * Time.deltaTime;
        Vector3 look = Vector3.RotateTowards(entityT.forward, targetDir, singleStep, 0f);
        entityT.rotation = Quaternion.LookRotation(look);
    }

    private void FindTarget()
    {
        if (Vector3.Distance(entityT.position, targetT.position) < 10)
        {
            isAttack = true;
        }
        else
        {
            isAttack = false;
            if (Vector3.Distance(entityT.position, patrolPoints[currentPoint].position) < 2)
            {
                currentPoint++;
                if (currentPoint >= patrolPoints.Length)
                {
                    currentPoint = 0;
                }
            }
        }
    }
    public void Jump()
    {
        rb.velocity = new Vector3(0f, 5f);
    }
    public void Explodion()
    {
        destColliders = Physics.OverlapSphere(entityT.position, 2f);
        foreach (Collider col in destColliders)
        {
            if (!col.CompareTag("Player"))
            {
                Destroy(col.gameObject);
            }
        }
        Destroy(gameObject);
    }
   
}
