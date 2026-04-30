using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{
    public Transform target;
    Rigidbody rBody;
    private int EnemySpeed = 500;
    private int EnemyRotateSpeed = 200;
    private int EnemyHP = 200;
    float beforeDistance = 0;
    public GameObject Player;

    public GameObject EnemyWeapon;

    public GameObject EnemyAni;

    private bool AttackAnimeFlag = false;
    private float attackTime = 0;
    private bool AttackEnableFlag = false;
    private float AttackEnableTiemr = 0;
    private Animator EnemyAnimation;

    bool CollisionFirstFlag = false;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        EnemyAnimation = this.GetComponent<Animator>();

        rBody.angularVelocity = Vector3.zero;
        rBody.velocity = Vector3.zero;
        this.transform.position = new Vector3(0, 0.5f, 0);
        //target.transform.position = new Vector3(Random.Range(-90, 90), 15, Random.Range(-90, 100));
        Player.GetComponent<charactor>().UpDownSpeed = 0;

        float dx = this.transform.position.x - target.transform.position.x;
        float dz = this.transform.position.z - target.transform.position.z;
        float distanceToTarget = Mathf.Sqrt(dx * dx + dz * dz);

        this.rBody = GetComponent<Rigidbody>();
        EnemyAnimation = EnemyAni.GetComponent<Animator>();
    }


    int count = 0;
    void Update()
    {


        float dx = this.transform.localPosition.x - target.transform.localPosition.x;
        float dz = this.transform.localPosition.z - target.transform.localPosition.z;

        Vector3 toTarget = (target.position - transform.position).normalized;
        Vector3 forward = transform.forward;



        float distanceToTarget = Mathf.Sqrt(dx * dx + dz * dz);


        if (AttackAnimeFlag == false)
        {
            bool MoveSignal;
            MoveSignal = Input.GetKey(KeyCode.I);

            if (Input.GetKey(KeyCode.I))
            {
                rBody.velocity = transform.forward * EnemySpeed * Time.fixedDeltaTime;//ëOï˚å¸Ç÷à⁄ìÆ
                EnemyAnimation.SetBool("Walk", false);
                EnemyAnimation.SetBool("Run", true);
            }
            else if (Input.GetKey(KeyCode.K))
            {
                rBody.velocity = transform.forward * (EnemySpeed / 2) * Time.fixedDeltaTime;
                EnemyAnimation.SetBool("Walk", true);
                EnemyAnimation.SetBool("Run", false);
            }
            else
            {
                rBody.velocity = Vector3.zero;
                EnemyAnimation.SetBool("Walk", false);
                EnemyAnimation.SetBool("Run", false);
            }
        }

            //float RotateSignal;
            //RotateSignal = action.ContinuousActions[1];
            //this.transform.Rotate(Vector3.up, RotateSignal * EnemyRotateSpeed * Time.fixedDeltaTime);//yé≤Ç…âÒì]

            Debug.Log(AttackAnimeFlag);

            if (AttackAnimeFlag == true)//çUåÇÉÇÅ[ÉVÉáÉìíÜÇ…
            {
               
                if (EnemyWeapon.GetComponent<EnemyWeapon>().EWCollisionFlag == true)//åïÇ™çUåÇÇ™ìñÇΩÇ¡ÇΩÇÁ
                {
                    if (CollisionFirstFlag == false)//ç≈èâÇÃàÍâÒÇæÇØ
                    {
                        //Player.GetComponent<charactor>().DamagePlayer(5);//enemyAgent.DamageEnemy(2);//ìGÇÃëÃóÕÇ™å∏ÇÈä÷êîé¿çs
                        CollisionFirstFlag = true;
                    }
                }
        }



        //AddReward(-0.0001f * (Vector3.Distance(target.localPosition, transform.localPosition)));


        //if (Vector3.Distance(target.localPosition, transform.localPosition) < 3.5f)
        //{
        //    AddReward(0.01f);
        //}


        bool AttackFlag = Input.GetKey(KeyCode.P);
        if (AttackAnimeFlag == false)
        {
            if (AttackFlag == true && AttackEnableFlag)
            {
                Attack(distanceToTarget);
                AttackAnimeFlag = true;
            }
            CollisionFirstFlag = false;
        }


        if (AttackAnimeFlag == true)
        {
            attackTime += Time.deltaTime;
            EnemyAnimation.SetBool("Run", false);
        }

        if (attackTime > 1.5f)
        {
            AttackAnimeFlag = false;
            EnemyAnimation.SetBool("Attack", false);
        }
        AttackEnableTiemr += Time.deltaTime;
        if (AttackEnableTiemr >= 4.0f && distanceToTarget <= 10.0f)
        {
            AttackEnableFlag = true;
        }
        else
        {
            AttackEnableFlag = false;
        }
        //Debug.Log(AttackEnableFlag);
        //AddReward(-0.001f);

       
    }
    public void DamageEnemy(int DamageNum)
    {
        EnemyHP -= DamageNum;
    }


    void Attack(float dis)
    {
        EnemyAnimation.SetBool("Attack", true);
        AttackEnableTiemr = 0;
        attackTime = 0;
    }
}