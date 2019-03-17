using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class RatAgent : Agent {

    public float ArenaDimensions = 20.0f;
    public float speed = 10;
    // What the agent is chasing
    public Transform Target;

    Rigidbody rBody;

    private float previousDistance = float.MaxValue;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }
    
    public override void AgentReset()
    {
        float allowedArea = ArenaDimensions * 0.9f;

        // Move the rat to a new spot
        this.rBody.position = new Vector3((Random.value * allowedArea) - (allowedArea / 2),
                                        0.16f,
                                        (Random.value * allowedArea) - (allowedArea / 2));
        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;

        // Move the target to a new spot
        Target.position = new Vector3((Random.value * allowedArea) - (allowedArea / 2),
                                        0.1f,
                                        (Random.value * allowedArea) - (allowedArea / 2));
    }

    public override void CollectObservations()
    {
        float arenaEdgefromCenter = ArenaDimensions / 2;

        // Calculate relative position
        Vector3 relativePosition = Target.position - this.transform.position;

        // Relative position
        AddVectorObs(relativePosition.x / arenaEdgefromCenter);
        AddVectorObs(relativePosition.z / arenaEdgefromCenter);

        // Distance to edges of platform
        AddVectorObs((this.transform.position.x + arenaEdgefromCenter) / arenaEdgefromCenter);
        AddVectorObs((this.transform.position.x - arenaEdgefromCenter) / arenaEdgefromCenter);
        AddVectorObs((this.transform.position.z + arenaEdgefromCenter) / arenaEdgefromCenter);
        AddVectorObs((this.transform.position.z - arenaEdgefromCenter) / arenaEdgefromCenter);

        // Agent velocity
        AddVectorObs(rBody.velocity.x / arenaEdgefromCenter);
        AddVectorObs(rBody.velocity.z / arenaEdgefromCenter);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.position,
                                                  Target.position);

        // Reached target
        if (distanceToTarget < 0.5f)
        {
            AddReward(1.0f);
            Done();
        }

        // Getting closer
        if (distanceToTarget < previousDistance)
        {
            AddReward(0.1f);
        }

        // Time penalty
        AddReward(-0.05f);

        // Fell off platform
        //if (this.transform.position.y < -1.0)
        //{
        //    Done();
        //    AddReward(-1.0f);
        //}
        previousDistance = distanceToTarget;

        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = Mathf.Clamp(vectorAction[0], -1, 1);
        controlSignal.z = Mathf.Clamp(vectorAction[1], -1, 1);
#if UNITY_EDITOR
        Debug.Log($"Action X:{controlSignal.x}, Z:{controlSignal.z}");
#endif
        rBody.AddForce(controlSignal * speed);
    }
}
