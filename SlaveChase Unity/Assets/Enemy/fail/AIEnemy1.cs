using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using UnityEngine.Rendering.VirtualTexturing;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
public class AIEnemy1 : Agent
{
    public Transform target;
    Rigidbody rBody;
    private int EnemySpeed = 250;
    private int EnemyRotateSpeed = 80;
    float beforeDistance = 0;
    public override void Initialize()
    {
      
        this.rBody = GetComponent<Rigidbody>();
 
    }

    public override void OnEpisodeBegin()
    {
        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        this.transform.localPosition = new Vector3(-22.0f, -23.0f, -17.0f);
        target.transform.localPosition = new Vector3(Random.Range(-36.5f, -10.0f), -20.0f, Random.Range(-30.0f, -4.0f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //sensor.AddObservation(target.localPosition.x);
        //sensor.AddObservation(target.localPosition.z);

        sensor.AddObservation(this.transform.localPosition.x);
        sensor.AddObservation(this.transform.localPosition.z);

        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(this.transform.rotation.y);

        var Direction = (target.localPosition - transform.localPosition).normalized;
        sensor.AddObservation(Direction);
    }


    int count = 0;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        //float MoveSignal;
        //MoveSignal = Mathf.Abs(actionBuffers.ContinuousActions[0]);//ê‚ëŒílÇÇ¬ÇØÇÈ0~1
        rBody.velocity = transform.forward *  EnemySpeed * Time.fixedDeltaTime;//ëOï˚å¸Ç÷à⁄ìÆ

        Vector3 RotateSignal;
        RotateSignal = new (actionBuffers.ContinuousActions[0],0, actionBuffers.ContinuousActions[1]);
        transform.rotation = Quaternion.Lerp(
                                                 transform.localRotation,
                                                 Quaternion.LookRotation(RotateSignal.normalized, Vector3.up),
                                                 6f * Time.deltaTime
                                                );

        AddReward(-0.01f * (Vector3.Distance(target.localPosition, transform.localPosition)));


        if (Vector3.Distance(target.localPosition, transform.localPosition) < 2.5f)
        {
            AddReward(1.0f);
            EndEpisode();
        }

        //float dx = this.transform.position.x - target.transform.position.x;
        //float dz = this.transform.position.z - target.transform.position.z;

        //float distanceToTarget = Mathf.Sqrt(dx * dx + dz * dz);

        //count++;

        //float reward = (beforeDistance - distanceToTarget) * 0.1f;
        //AddReward(reward);
        //beforeDistance = distanceToTarget;
        //if (count >= 100)
        //{

        //    if (distanceToTarget < beforeDistance)
        //    {
        //        AddReward(0.5f);
        //    }
        //    if (distanceToTarget > beforeDistance)
        //    {
        //        AddReward(-0.5f);
        //    }

        //count = 0;
        //Debug.Log(distanceToTarget);
        //}
        //AddReward(-1.0f / MaxStep);
    }

    void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.tag == "Player")
        //{
        //    AddReward(1.0f);
        //    EndEpisode();
        //}
        if (collision.gameObject.tag == "MapObject")
        {
            AddReward(-0.5f);
            EndEpisode();
        }

    }
}