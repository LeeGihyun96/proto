using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //적 캐릭터가 출현할 위치
    public Transform[] points;
    //적 캐릭터 프리팹을 저장할 변수
    public GameObject enemy;
    // 적 캐릭터 생성 주기
    public float createTime = 2.0f;
    //적 캐릭터의 최대 생성 개수
    public int maxEnemy = 3;
    //게임 종료 판단
    public bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        //모든 SpawnPointGroup 하위 Transform 컴포넌트를 찾아옴
        points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();

        if (points.Length > 0)
        {
            StartCoroutine(this.CreateEnemy());
        }
    }

    //적 캐릭터 생성 코루틴 함수
    IEnumerator CreateEnemy()
    {
        //게임 종료시까지 루프
        while (!isGameOver)
        {
            //현재 생성된 캐릭터 수
            int enemyCount = (int)GameObject.FindGameObjectsWithTag("ENEMY").Length;

            //적 캐릭터의 최대 생성 개수보다 적을 때만 생성
            if (enemyCount < maxEnemy)
            {
                //적 캐릭터의 생성 주기 시간만큼 대기
                yield return new WaitForSeconds(createTime);

                //불규칙적인 위치 산출
                int idx = Random.Range(1, points.Length);
                //적 캐릭터의 동적 생성
                Instantiate(enemy, points[idx].position, points[idx].rotation);
            }
            else
            {
                yield return null;
            }
        }
    }

}
