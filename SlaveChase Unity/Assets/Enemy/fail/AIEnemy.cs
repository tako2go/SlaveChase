using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using UnityEngine.Rendering.VirtualTexturing;
using Unity.VisualScripting;
public class AIEnemy : Agent
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
        this.transform.position = new Vector3(0, 0.5f, 0);
        target.transform.position = new Vector3(Random.Range(-90, 90), 15, Random.Range(-90, 100));

        float dx = this.transform.position.x - target.transform.position.x;
        float dz = this.transform.position.z - target.transform.position.z;
        float distanceToTarget = Mathf.Sqrt(dx * dx + dz * dz);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //sensor.AddObservation(target.localPosition.x);
        //sensor.AddObservation(target.localPosition.z);

        sensor.AddObservation(this.transform.localPosition.x);
        sensor.AddObservation(this.transform.localPosition.z);

        sensor.AddObservation(Vector3.Distance(target.localPosition, transform.localPosition));

        Vector3 dir = (target.localPosition - transform.localPosition).normalized;
        sensor.AddObservation(dir.x);
        sensor.AddObservation(dir.z);

        sensor.AddObservation(rBody.velocity.z);
        sensor.AddObservation(this.transform.forward);
    }


    int count = 0;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        float MoveSignal;
        MoveSignal = Mathf.Abs(actionBuffers.ContinuousActions[0]);//ïâÇÃêîÇä∑éZÇµÇ»Ç¢0~2Å@Å@ê‚ëŒílÇÊÇËå¯ó¶Ç™ÇÊÇ≥ÇªÇ§(Ç†Ç≠Ç‹Ç≈ó\ëz)
        rBody.velocity = transform.forward * MoveSignal * EnemySpeed * Time.fixedDeltaTime;//ëOï˚å¸Ç÷à⁄ìÆ

        float RotateSignal;
        RotateSignal = actionBuffers.ContinuousActions[1];
        this.transform.Rotate(Vector3.up, RotateSignal * EnemyRotateSpeed * Time.fixedDeltaTime);//yé≤Ç…âÒì]


        //AddReward(-0.0001f * (Vector3.Distance(target.localPosition, transform.localPosition)));


        if (Vector3.Distance(target.localPosition, transform.localPosition) < 2.5f)
        {
            AddReward(1.0f);
            EndEpisode();
        }

        float dx = this.transform.localPosition.x - target.transform.localPosition.x;
        float dz = this.transform.localPosition.z - target.transform.localPosition.z;

        float distanceToTarget = Mathf.Sqrt(dx * dx + dz * dz);

        count++;

        float reward = (beforeDistance - distanceToTarget) * 0.1f;
        AddReward(reward);
        beforeDistance = distanceToTarget;
        if (count >= 100)
        {

            if (distanceToTarget < beforeDistance)
            {
                AddReward(0.1f);
            }
            if (distanceToTarget > beforeDistance)
            {
                AddReward(-0.1f);
            }

            count = 0;
            Debug.Log(distanceToTarget);
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.tag == "Player")
        //{
        //    AddReward(1);
        //    EndEpisode();
        //}
        if (collision.gameObject.tag == "MapObject")
        {
            Debug.Log(collision.gameObject.tag);
            AddReward(-1);
            EndEpisode();
        }

    }
}