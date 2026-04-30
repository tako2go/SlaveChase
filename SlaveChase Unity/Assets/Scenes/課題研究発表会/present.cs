using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using UnityEngine.Rendering.VirtualTexturing;
using Unity.VisualScripting;
using static UnityEngine.GraphicsBuffer;
using System;
public class present : Agent
{
    public Transform Player;
    Rigidbody rBody;

    private const int EnemySpeed = 500;
    private const int EnemyRotateSpeed = 100;
    // Start is called before the first frame update
    public override void Initialize()
    {
        this.rBody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        Player.transform.localPosition = new Vector3(UnityEngine.Random.Range(-17, 17), 1, UnityEngine.Random.Range(-17, 17));//学習マップ用
        this.transform.localPosition = new Vector3(UnityEngine.Random.Range(-17, 17), 1, UnityEngine.Random.Range(-17, 17));
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {

        float distanceToTarget = Vector3.Distance(Player.transform.localPosition, this.transform.localPosition);
        float MoveSignal;
        MoveSignal = actionBuffers.ContinuousActions[0];
        rBody.velocity = transform.forward * MoveSignal * EnemySpeed * Time.fixedDeltaTime;


        float RotateSignal;
        RotateSignal = actionBuffers.ContinuousActions[1];
        this.transform.Rotate(Vector3.up, RotateSignal * EnemyRotateSpeed * Time.fixedDeltaTime);//y軸に回転
        //AddReward(Mathf.Abs(RotateSignal) * (-0.015f));


        if (Player.transform.localPosition.y < 0)
        {
            EndEpisode();
        }
        if (this.transform.localPosition.y < 0)
        {
            EndEpisode();
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AddReward(5.0f);
            Debug.Log("プレイヤーに当たったよ");
            EndEpisode();
        }
        if (collision.gameObject.tag == "Wall")
        {
            AddReward(-1.0f);
            EndEpisode();
            Debug.Log("障害物に当たったよ");
        }


    }
}
