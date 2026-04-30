using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using UnityEngine.Rendering.VirtualTexturing;
using Unity.VisualScripting;
using static UnityEngine.GraphicsBuffer;
public class EnemyAgentSight : Agent
{
    public GameObject Player;
    public GameObject[] Obstacle = new GameObject[3];
    Rigidbody rBody;

    Animator EnemyAnimation;
    private EnemyAttack Enemy;

    private const int EnemySpeed = 500;
    private const int EnemyRotateSpeed = 100;

    public int HP = 100;
    public bool StanFlag = false;
    public float StanTimer;
    private const float StanTime = 0.2f;
    
    public int action;

    private int count1 = 0;
    private int count2 = 0;
    private int count3 = 0;

    private int count = 0;
    private int ContinuousCount = 0;

    private int ColliderCount = 0;
    public override void Initialize()
    {
        this.rBody = GetComponent<Rigidbody>();
        EnemyAnimation = GetComponent<Animator>();
        Enemy = GetComponent<EnemyAttack>();
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(count1);
        sensor.AddObservation(count2);
        sensor.AddObservation(count3);
    }

    public override void OnEpisodeBegin()
    {

        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        //Player.transform.position = new Vector3(Random.Range(-90, 90), 15, Random.Range(-90, 100));全マップ
        //Player.transform.position = new Vector3(Random.Range(-30, 30), 15, Random.Range(-30, 30));マップの中央付近
        //Player.transform.localPosition = new Vector3(Random.Range(-17, 17), 1, Random.Range(-17, 17));//学習マップ用
        //for (int i = 0; i < 3; i++)
        //{
        //    Obstacle[i].transform.localPosition = new Vector3(Random.Range(-17, 17), 0.8f, Random.Range(-17, 17));
        //    Obstacle[i].transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        //}

        //this.transform.localPosition = new Vector3(Random.Range(-17, 17), 1, Random.Range(-17, 17));
        this.transform.localPosition = new Vector3(Random.Range(-15, 22), 1, Random.Range(-4, 30));
        this.transform.localRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

        //Player.transform.localPosition = this.transform.position + this.transform.forward * (Random.Range(20, 35)) + this.transform.right * Random.Range(-10, 10);//学習マップ用
        //Player.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        //Player.GetComponent<charactor>().UpDownSpeed = 0;
        ColliderCount = 0;
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {

        if (StanFlag)
        {
            StanTimer += Time.deltaTime;
        }
        if (StanTimer >= StanTime)
        {
            EnemyAnimation.SetBool("StanFlag", false);
            StanFlag = false;
        }

        float distanceToTarget = Vector3.Distance(Player.transform.localPosition, this.transform.localPosition);
        //if (distanceToTarget <= 2.5f)
        //{
        //    AddReward(5.0f);
        //    Debug.Log("プレイヤーに当たったよ");
        //    EndEpisode();
        //}

        //if(!(this.GetComponent<EnemyAttack>().AttackFlag) && !StanFlag)//攻撃、スタンをしていないとき
        //{

        float MoveSignal;
        if (!(Enemy.AttackFlag | StanFlag))
        {
            MoveSignal = actionBuffers.ContinuousActions[0];
    }
        else
        {
            MoveSignal = 0;
        }

        rBody.velocity = transform.forward * MoveSignal * EnemySpeed * Time.fixedDeltaTime;

        if (MoveSignal < 0)
        {
            AddReward(MoveSignal * (0.1f));
        }





        float RotateSignal;
        if (!(Enemy.AttackFlag | StanFlag))
        {
            RotateSignal = actionBuffers.ContinuousActions[1];
    }
        else
        {
            RotateSignal = 0;

        }

        this.transform.Rotate(Vector3.up, RotateSignal * EnemyRotateSpeed * Time.fixedDeltaTime);//y軸に回転
        AddReward(Mathf.Abs(RotateSignal) * (-0.15f));
        //}



       
  
        Vector3 distance = (Player.transform.localPosition - this.transform.localPosition).normalized;
        float cos = Vector3.Dot(distance, this.transform.forward);
      
        if (distanceToTarget <= 30.0f)
        {
            AddReward(cos * (0.15f));
        }

        //if (distanceToTarget <= 1.0f)
        //{
        //    AddReward(5.0f);
        //    Debug.Log("プレイヤーに当たったよ");
        //    EndEpisode();
        //}

        if (!(Enemy.AttackFlag | StanFlag | Enemy.CoolFlag))
        {
            action = actionBuffers.DiscreteActions[0];//0:静止 1:通常　2:突き 3:回転
    }
        else
        {
            action = 0;
        }

//Debug.Log(action);
//通常:3以下cos0.7以上　　突き:3.7以下cos0.9以上   回転:2.5以下360度
        if (action == 1)
        {
            if (distanceToTarget <= 3.0f && cos >= 0.5f)
            {
                count1 += 2;
                AddReward(5.0f - count1);
            }
            else
            {
                AddReward(-0.5f);
            }
            count++;
        }
        if (action == 2)
        {
            if (distanceToTarget <= 3.5f && cos >= 0.9f)
            {
                count2 += 2;
                AddReward(5.0f - count2);
            }
            else
            {
                AddReward(-0.5f);
            }
            count++;
        }
        if (action == 3)
        {
            if (distanceToTarget <= 2.8f)
            {
                count3 += 2;
                AddReward(5.0f - count3);
                Debug.Log("回転が当たったよ");
                //EndEpisode();

            }
            else
            {
                AddReward(-0.5f);
                Debug.Log("回転を外しちゃった...");
            }
            //count1--;
            //count2--;
            count++;
        }

        if (count >= 6)
        {
            count1 = 0;
            count2 = 0;
            count3 = 0;
            count = 0;
        }

        AddReward(-0.0001f);

        if (Player.transform.localPosition.y < 0)
        {
            EndEpisode();
        }
        if (this.transform.localPosition.y < 0)
        {
            EndEpisode();
        }
        //Debug.Log("distanceToTarget" + ":  " + distanceToTarget + "      " + "cos" + ":  " + cos);
        //Debug.Log(distanceToTarget <= 3.0f && cos >= 0.6f);
    }
    void OnCollisionStay(Collision collision)
    {
        //if (collision.gameObject.tag == "Player")
        //{
           
        //}
        if (collision.gameObject.tag == "MapObject")
        {
            AddReward(-4.0f);
            //AddReward(-0.01f);
            //EndEpisode();
            Debug.Log("障害物に当たったよ");
            ColliderCount++;
        }

        if(ColliderCount >= 100)
        {
            //EndEpisode();
        }

    }
    //public void DamageEnemy(int DamageNum)
    // {
    //     EnemyHP -= DamageNum;
    // }
}