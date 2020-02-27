using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class Test : MonoBehaviour
{
    public enum State
    {
        IDLE,
        ATTACK,
        TRACE,
        DIE
    }
    //상태를 저장할 변수
    public State state = State.IDLE;

    private Transform playerTr;
    private Transform enemyTr;
    private Camera mainCamera; // 메인 카메라
    private Vector3 targetPos; // 캐릭터의 이동 타겟 위치
   
    //Animator 컴포넌트를 저장할 변수
    private Animator animator;
    private NavMeshAgent agent;

    //공격 사정거리
    public float attackDist = 1.0f;
    //추적 사정거리
    public float traceDist = 5.0f;
    public bool isDie = false;

    private readonly float damping = 100.0f;

    private float nextFire;
    public float fireRate = 1.0f;

    private EnemyAI enemyAI;
    private WaitForSeconds ws;
    private bool isMove = false;
    //애니메이터 컨트롤러에 정의한 파라미터의 해시값을 미리 추출
    private readonly int hashMove = Animator.StringToHash("IsMove");

    public float speed = 1.0f;
    private bool IsRotating;
    private float rotationSpeed = 100.0f;
    void Start()
    {
        playerTr = GetComponent<Transform>();
        var enemy = GameObject.FindGameObjectWithTag("ENEMY");
        if (enemy != null)
            enemyTr = enemy.GetComponent<Transform>();
        else
            Debug.Log("No Enemy");
        enemyAI = GameObject.FindGameObjectWithTag("ENEMY").GetComponent<EnemyAI>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        agent = GetComponent<NavMeshAgent>();
        //Animator 컴포넌트 추출
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // 마우스로 찍은 위치의 좌표 값을 가져온다
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            animator.SetBool(hashMove, true);
            isMove = true;

            if (Physics.Raycast(ray, out hit) && isMove)
            {
                animator.SetBool(hashMove, true);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(hit.point - transform.position), rotationSpeed * Time.deltaTime);
                agent.SetDestination(hit.point);
                agent.isStopped = false;  
            }
            
        }
        if (agent.remainingDistance <= 0.02f && agent.velocity.magnitude >= 0.05f)
        {
            animator.SetBool(hashMove, false);
        }


    }
}

