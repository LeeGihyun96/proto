using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAttack : MonoBehaviour
{
    private Transform playerTr;
    private Transform enemyTr;
    public EnemyAI enemyAI;
    private NavMeshAgent agent;

    public float attackDist = 10.0f;
    public float attackDamage = 6.0f;
    public bool isDie = false;

    private readonly float damping = 100.0f;
    private float nextFire;
    public float fireRate = 1.0f;
    public Transform firePos;
    private Animator animator;
    public bool isMove;

    private readonly int hashAttack = Animator.StringToHash("Attack");

    void Start()
    {
        playerTr = GetComponent<Transform>();
        var enemy = GameObject.FindGameObjectWithTag("ENEMY");
        if (enemy != null)
            enemyTr = enemy.GetComponent<Transform>();
        else
            Debug.Log("No Enemy");
        enemyTr = enemy.GetComponent<Transform>();
        enemyAI = GameObject.FindGameObjectWithTag("ENEMY").GetComponent<EnemyAI>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float dist = Vector3.Distance(playerTr.position, enemyTr.position);

        if (dist <= attackDist && !isMove)
        {
            Quaternion rot = Quaternion.LookRotation(enemyTr.position - playerTr.position);
            playerTr.rotation = Quaternion.Slerp(playerTr.rotation, rot, Time.deltaTime * damping);

            if (Time.time > nextFire)
            {
                RaycastHit hit;

                if (Physics.Raycast(firePos.position, enemyTr.position - playerTr.position, out hit, 10.0f))
                {
                    if (hit.collider.tag == "ENEMY")
                    {
                        Debug.Log("Attacking");
                        hit.collider.gameObject.SendMessage("OnDamage", attackDamage, SendMessageOptions.DontRequireReceiver);
                        animator.SetTrigger(hashAttack);
                    }

                }
                nextFire = Time.time + fireRate;
            }
        }
    }
}
