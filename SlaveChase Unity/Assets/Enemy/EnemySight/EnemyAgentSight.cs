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

    private float Timer;
    private Vector3 Pos;

    private float RotateNum;

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
        //Player.transform.position = new Vector3(Random.Range(-90, 90), 15, Random.Range(-90, 100));�S�}�b�v
        // Player.transform.position = new Vector3(Random.Range(-30, 30), 15, Random.Range(-30, 30));//�}�b�v�̒����t��
        // Player.transform.localPosition = new Vector3(Random.Range(-17, 17), 1, Random.Range(-17, 17));//�w�K�}�b�v�p
        //for (int i = 0; i < 3; i++)
        //{
        //    Obstacle[i].transform.localPosition = new Vector3(Random.Range(-17, 17), 0.8f, Random.Range(-17, 17));
        //    Obstacle[i].transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        //}

        //this.transform.localPosition = new Vector3(Random.Range(-17, 17), 1, Random.Range(-17, 17));
        this.transform.localPosition = new Vector3(Random.Range(-15, 22), 1, Random.Range(-4, 30));
        this.transform.localRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

        //Player.transform.localPosition = this.transform.position + this.transform.forward * (Random.Range(20, 35)) + this.transform.right * Random.Range(-10, 10);//�w�K�}�b�v�p
        //Player.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        Player.GetComponent<charactor>().UpDownSpeed = 0;
        Timer = 0;
        Pos = this.transform.position;
        RotateNum = 0;
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
        //    Debug.Log("�v���C���[�ɓ���������");
        //    EndEpisode();
        //}

        //if(!(this.GetComponent<EnemyAttack>().AttackFlag) && !StanFlag)//�U���A�X�^�������Ă��Ȃ��Ƃ�
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
        RotateNum += RotateSignal;
        this.transform.Rotate(Vector3.up, RotateSignal * EnemyRotateSpeed * Time.fixedDeltaTime);//y���ɉ�]

        Timer += Time.deltaTime;
        if (distanceToTarget <= 8.0f && (Timer >= 10 && Vector3.Distance(Pos, this.transform.position) < 5.0f))
        {
            AddReward(-2.0f);
            Timer = 0;
            Pos = this.transform.position;
            AddReward(Mathf.Abs(RotateNum) * (-0.01f));
        }

        //}


        Vector3 distance = (Player.transform.localPosition - this.transform.localPosition).normalized;
        float cos = Vector3.Dot(distance, this.transform.forward);

        if (distanceToTarget <= 8.0f)
        {
            AddReward(cos * (0.15f));
        }

        //if (distanceToTarget <= 1.0f)
        //{
        //    AddReward(5.0f);
        //    Debug.Log("�v���C���[�ɓ���������");
        //    EndEpisode();
        //}

        if (!(Enemy.AttackFlag | StanFlag | Enemy.CoolFlag))
        {
            action = actionBuffers.DiscreteActions[0];//0:�Î~ 1:�ʏ�@2:�˂� 3:��]
        }
        else
        {
            action = 0;
        }

        //Debug.Log(action);
        //�ʏ�:3�ȉ�cos0.7�ȏ�@�@�˂�:3.7�ȉ�cos0.9�ȏ�   ��]:2.5�ȉ�360�x
        if (action == 1)
        {
            if (distanceToTarget <= 3.0f && cos >= 0.5f)
            {
                AddReward(5.0f - count1);
                // Debug.Log("通常当");
                // EndEpisode();
            }
            else
            {
                // Debug.Log("通常外");
                AddReward(-0.5f);
            }
            count++;
            count1++;
        }
        if (action == 2)
        {
            if (distanceToTarget <= 3.5f && cos >= 0.9f)
            {
                AddReward(5.0f - count2);
                // Debug.Log("突き当");
                // EndEpisode();
            }
            else
            {
                AddReward(-0.5f);
                // Debug.Log("突き外");
            }
            count++;
            count2++;
        }
        if (action == 3)
        {
            if (distanceToTarget <= 2.8f)
            {
                AddReward(5.0f - count3);
                // Debug.Log("回転当");
                // EndEpisode();
            }
            else
            {
                AddReward(-0.5f);
                // Debug.Log("回転外");
            }
            //count1--;
            //count2--;
            count++;
            count3++;
        }

        if (count >= 10)
        {
            count1 = 0;
            count2 = 0;
            count3 = 0;
            count = 0;
        }

        AddReward(-0.001f);

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
            // EndEpisode();
        }
    }
    //public void DamageEnemy(int DamageNum)
    // {
    //     EnemyHP -= DamageNum;
    // }
}