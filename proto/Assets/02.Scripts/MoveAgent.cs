using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//내비게이션 기능을 사용하기 위해 추가해야 하는 네임스페이스
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class MoveAgent : MonoBehaviour
{
    //추적속도
    private readonly float traceSpeed = 2.0f;
    //회전할때의 속도를 조절하는 계수
    private float damping = 1.0f;

    //NavMeshAgent 컴포넌트를 저장할 변수
    private NavMeshAgent agent;
    //enemy의 Transform 컴포넌트를 저장할 변수
    private Transform enemyTr;

    //대기 여부를 판단하는 변수
    private bool _idling;
    //idleing 프로퍼티 정의(getter, setter)
    public bool idling
    {
        get { return _idling; }
        set
        {
            _idling = value;
        }
    }

    //추적대상의 위치를 저장하는 변수
    private Vector3 _traceTarget;
    //traceTarget 프로퍼티 정의(getter,setter)
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            //추적 상태의 회전계수
            damping = 7.0f;
            agent.speed = traceSpeed;
            TraceTarget(_traceTarget);
        }
    }

    
    void Start()
    {
        //enemy의 Transform 컴포넌트 추출 후 변수에 저장
        enemyTr = GetComponent<Transform>();
        //NavMeshAgent 컴포넌트를 추출한 후 변수에 저장
        agent = GetComponent<NavMeshAgent>();
        //목적지에 가까워질수록 속도를 줄이는 옵션을 비활성화
        agent.autoBraking = false;

        
    }

    //player를 추적할때 이동시키는 함수
    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;

        agent.destination = pos;
        agent.isStopped = false;
    }

    //추적을 정지시키는 함수
    public void Stop()
    {
        agent.isStopped = true;
        //바로 정지하기 위해 속도를 0으로 설정
        agent.velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        //enemy가 이동중일 때만 회전
        if (agent.isStopped == false)
        {
            //NavMeshAgent가 가야 할 방향 벡터를 쿼터니언 타입의 각도로 변환
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            //보간 함수를 사용해 점진적으로 회전시킴
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
        }
    }
}
