using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    //오디오 없음
    //Animator 컴포넌트를 저장할 변수
    private Animator animator;
    //player의 Transform 컴포턴트
    private Transform playerTr;
    //enemy의 Transform 컴포넌트
    private Transform enemyTr;

    //애니메이터 컨트롤러에 정의한 파라미터의 해시값을 미리 추출
    private readonly int hashAttack = Animator.StringToHash("Attack");

    //다음 공격할 시간 계산용 변수
    private float nextAttack = 0.0f;
    //공격 간격
    private readonly float attackRate = 0.7f;
    //player를 향해 회전할 속도 계수
    private readonly float damping = 11.0f;

    //공격 여부를 판단할 변수
    public bool isAttack = false;
    //사운드 없음
    // Start is called before the first frame update
    void Start()
    {
        //컴포넌트 추출 및 변수 저장
        playerTr = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<Transform>();
        enemyTr = GetComponent<Transform>();
        animator = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttack)
        {
            //현재 시간이 다음 공격 시간보다 큰지를 확인
            if (Time.time >= nextAttack)
            {
                Attack();
                //다음 공격 시간 계산
                nextAttack = Time.time + attackRate;
            }

            //player가 있는 위치까지의 회전 각도 계산
            Quaternion rot = Quaternion.LookRotation(playerTr.position - enemyTr.position);
            //보간함수를 사용해 점진적으로 회전시킴
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
        }
    }

    void Attack()
    {
        animator.SetTrigger(hashAttack);
        //오디오 없음
    }
}
