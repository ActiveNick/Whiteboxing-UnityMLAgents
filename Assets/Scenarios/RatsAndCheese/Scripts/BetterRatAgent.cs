using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterRatAgent : Agent
{
    public GameObject myAcademyObj;
    RatSpawningAcademy myAcademy;

    // Speed of agent movement.
    public float moveSpeed = 1;
    // Speed of agent rotation.
    public float turnSpeed = 300;

    // Internals
    Rigidbody agentRB;
    RayPerception rayPer;
    int cheese;

    public override void InitializeAgent()
    {
        base.InitializeAgent();
        agentRB = GetComponent<Rigidbody>();
        rayPer = GetComponent<RayPerception>();
        myAcademy = myAcademyObj.GetComponent<RatSpawningAcademy>();
    }

    public override void CollectObservations()
    {
        // Looking around with raycasts
        float rayDistance = 5f;
        float[] rayAngles = { 20f, 90f, 160f, 45f, 135f, 70f, 110f };
        string[] detectableObjects = { "cheese", "rat", "wall" };
        AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, -0.1f));

        // The current speed of the agent
        Vector3 localVelocity = transform.InverseTransformDirection(agentRB.velocity);
        AddVectorObs(localVelocity.x);
        AddVectorObs(localVelocity.z);
    }

    public void MoveAgent(float[] act)
    {
        Vector3 dirToGo = Vector3.zero;
        Vector3 rotateDir = Vector3.zero;

        if (brain.brainParameters.vectorActionSpaceType == SpaceType.continuous)
        {
            dirToGo = transform.forward * Mathf.Clamp(act[0], -1f, 1f);
            rotateDir = transform.up * Mathf.Clamp(act[1], -1f, 1f);
        }
        else
        {
            switch ((int)(act[0]))
            {
                case 1:
                    dirToGo = transform.forward;
                    break;
                case 2:
                    rotateDir = -transform.up;
                    break;
                case 3:
                    rotateDir = transform.up;
                    break;
            }
        }
        agentRB.AddForce(dirToGo * moveSpeed, ForceMode.VelocityChange);
        transform.Rotate(rotateDir, Time.fixedDeltaTime * turnSpeed);

        if (agentRB.velocity.sqrMagnitude > 25f) // slow it down
        {
            agentRB.velocity *= 0.95f;
        }
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        MoveAgent(vectorAction);

        // Time penalty
        AddReward(-0.005f);
    }

    public override void AgentReset()
    {
        cheese = 0;
        agentRB.velocity = Vector3.zero;
        //transform.position = new Vector3(0f, 0.2f, 0f);
        //transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));
        myAcademy.RespawnAgent(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("cheese"))
        {
            // Get rid of the cheese that the rat just ate
            Destroy(collision.gameObject);
            // Reward the rat for eating the cheese
            AddReward(1f);
            cheese += 1;
            myAcademy.totalScore += 1;
        }
        if (collision.gameObject.CompareTag("wall"))
        {
            //Done();

            // Penalty when hitting a wall
            AddReward(-0.2f);
        }
    }

    public override void AgentOnDone()
    {

    }
}
