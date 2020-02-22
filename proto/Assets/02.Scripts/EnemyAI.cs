﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    //적 캐릭터의 상태를 표현하기 위한 열거형 변수 정의
    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        DIE
    }
    //상태를 저장할 변수
    public State state = State.IDLE;
    //주인공의 위치를 저장할 변수
    private Transform playerTr;
    //적 캐릭터의 위치를 저장할 변수
    private Transform enemyTr;
    //Animator 컴포넌트를 저장할 변수
    private Animator animator;

    //공격 사정거리
    public float attackDist = 0.4f;
    //추적 사정거리
    public float traceDist = 100.0f;
    //사망 여부를 판단할 변수
    public bool isDie = false;
    //코루틴에서 사용할 지연시간 변수
    private WaitForSeconds ws;

    //이동을 제어하는 MoveAgent 클래스를 저장할 변수
    private MoveAgent moveAgent;
    //총 발사를 제어하는 EnemyFire 클래스를 저장할 변수
    private EnemyAttack enemyFire;

    //애니메이터 컨트롤러에 정의한 파라미터의 해시값을 미리 추출
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDie = Animator.StringToHash("Die");

    public float EnemyHp = 35.0f;
    void Awake()
    {
        //주인공 게임오브젝트 추출
        var player = GameObject.FindGameObjectWithTag("PLAYER");
        //주인공의 Transform 컴포넌트 추출
        if (player != null)
            playerTr = player.GetComponent<Transform>();
        //적 캐릭터의 Tranform 컴포넌트 추출
        enemyTr = GetComponent<Transform>();
        //Animator 컴포넌트 추출
        animator = GetComponent<Animator>();
        //이동을 제어하는 MoveAgent 클래스를 추출
        moveAgent = GetComponent<MoveAgent>();
        //총 발사를 제어하는 EnemyFire 클래스를 추출
        enemyFire = GetComponent<EnemyAttack>();

        //코루틴의 지연시간 생성
        ws = new WaitForSeconds(0.3f);
    }

    void OnEnable()
    {
        //CheckState 코루틴 함수 실행
        StartCoroutine(CheckState());
        //Action 코루틴 함수 실행
        StartCoroutine(Action());
    }

    //적 캐릭터의 상태를 검사하는 코루틴 함수
    IEnumerator CheckState()
    {
        //적 캐릭터가 사망하기 전까지 도는 무한루프
        while (!isDie)
        {
            //상태가 사망이면 코루틴 함수를 종료시킴
            if (state == State.DIE) yield break;
            //주인공과 적 캐릭터 간의 거리를 계산
            float dist = Vector3.Distance(playerTr.position, enemyTr.position);
            //공격 사정거리 이내의 경우
            if (dist <= attackDist)
            {
                state = State.ATTACK;
            }//추적 사정거리 이내의 경우
            else if (dist <= traceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.IDLE;
            }
            //0.3초 동안 대기하는 동안 제어권을 양보
            yield return ws;
        }
    }

    //상태에 따라 적 캐릭터의 행동을 처리하는 코루틴 함수
    IEnumerator Action()
    {
        while (!isDie)
        {
            yield return ws;
            //상태에 따라 분기 처리
            switch (state)
            {
                case State.IDLE:
                    //공격 정지
                    enemyFire.isAttack = false;
                    //대기 모드를 활성화
                    moveAgent.idling = true;
                    animator.SetBool(hashMove, true);
                    break;
                case State.TRACE:
                    //공격 정지
                    enemyFire.isAttack = false;
                    //주인공의 위치를 넘겨 추적 모드로 변경
                    moveAgent.traceTarget = playerTr.position;
                    animator.SetBool(hashMove, true);
                    break;
                case State.ATTACK:
                    //추적을 정지
                    moveAgent.Stop();
                    animator.SetBool(hashMove, false);

                    //공격 시작
                    if (enemyFire.isAttack == false)
                        enemyFire.isAttack = true;
                    break;
                case State.DIE:
                    isDie = true;
                    enemyFire.isAttack = false;
                    //추적을 정지
                    moveAgent.Stop();
                    //사망 애니메이션 실행
                    animator.SetTrigger(hashDie);
                    //Capsule Collider 컴포넌트를 비활성화
                    GetComponent<CapsuleCollider>().enabled = false;
                    //Destroy(this.gameObject);
                    break;
            }
        }
    }
    
    void OnDamage(float damage)
    {
        EnemyHp -= damage;
        if (EnemyHp > 0)
        {
            Debug.Log("Enemy HP : " + EnemyHp);
        }
        else
        {
            Debug.Log("Enemy Died!");
            state = State.DIE;
        }
    }
}