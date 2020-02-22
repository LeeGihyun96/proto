using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float speed = 10.0f;
    private Transform playerTr;
    private Transform enemyTr;
    private Transform bulletTr;

    // Start is called before the first frame update
    void Start()
    {
        var player = GameObject.FindWithTag("PLAYER");
        playerTr = player.GetComponent<Transform>();
        var enemy = GameObject.FindGameObjectWithTag("ENEMY");
        enemyTr = enemy.GetComponent<Transform>();

        bulletTr = GetComponent<Transform>();
        Quaternion rot = Quaternion.LookRotation(enemyTr.position - bulletTr.position);
        bulletTr.rotation = Quaternion.Slerp(bulletTr.rotation, rot, 0.0f);

        Vector3 attackWay = enemyTr.position - playerTr.position;
        GetComponent<Rigidbody>().AddForce(attackWay * speed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
