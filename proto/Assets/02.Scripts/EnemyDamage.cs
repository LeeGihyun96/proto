using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyDamage : MonoBehaviour
{
    private const string bulletTag = "BULLET";
    //생명 게이지
    private float hp = 100.0f;

    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.tag == bulletTag)
        {
            //총알 삭제
            Destroy(coll.gameObject);
            //생명 게이지 차감 일단 주석처리 해놓음
           // hp -= coll.gameObject.GetComponent<BulletCtrl>().damage;

            if (hp <= 0.0f)
            {
                //적 캐릭터의 상태를 DIE로 변경
                GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
            }
        }
    }
}