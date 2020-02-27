using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class PlayerAnim
{
    public AnimationClip idle;
    public AnimationClip walkF;
    public AnimationClip shoot;
}
public class Player : MonoBehaviour
{
    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        DIE
    }
    //상태를 저장할 변수
    public State state = State.IDLE;
    //총알 프리팹
    public GameObject bullet;

    //총알 발사 좌표
    public Transform firePos;

    private Transform playerTr;
    private Transform enemyTr;

    public float attackDist = 5.0f;
    public float attackDamage = 6.0f;
    public bool isDie = false;

    private readonly float damping = 100.0f;

    private float nextFire;
    public float fireRate = 1.0f;

    public PlayerAnim playerAnim;
    public Animation anim;
    public EnemyAI enemyAI;

    void Start()
    { 
        playerTr = GetComponent<Transform>();
        var enemy = GameObject.FindGameObjectWithTag("ENEMY");
        enemyTr = enemy.GetComponent<Transform>();
        enemyAI = GameObject.FindGameObjectWithTag("ENEMY").GetComponent<EnemyAI>();

        anim = GetComponent<Animation>();
        anim.clip = playerAnim.idle;
        anim.Play();
    }

    void Update()
    {

        float dist = Vector3.Distance(playerTr.position, enemyTr.position);
        //Debug.Log(dist);

        if (dist <= attackDist)
        {
            Quaternion rot = Quaternion.LookRotation(enemyTr.position - playerTr.position);
            playerTr.rotation = Quaternion.Slerp(playerTr.rotation, rot, Time.deltaTime * damping);
        }

        if (dist <= attackDist)
        {
            if (Time.time > nextFire)
            {
                RaycastHit hit;

                if (Physics.Raycast(firePos.position, enemyTr.position - playerTr.position, out hit, 10.0f))
                {
                    if (hit.collider.tag == "ENEMY")
                    {
                        //Debug.Log("Attacking");
                        hit.collider.gameObject.SendMessage("OnDamage", attackDamage, SendMessageOptions.DontRequireReceiver);
                        anim.CrossFade(playerAnim.shoot.name, 0.3f);
                    }

                }
                nextFire = Time.time + fireRate;
            }
        }
    }
}

