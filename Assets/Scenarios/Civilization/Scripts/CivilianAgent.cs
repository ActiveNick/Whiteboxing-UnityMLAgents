using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianAgent : Agent {

    public GameObject myAcademyObj;
    CivAcademy myAcademy;

    // Speed of agent movement.
    public float moveSpeed = 1;
    // Speed of agent rotation.
    public float turnSpeed = 300;
    // Gathering speed 
    public float gatheringRate = 0.1f;
    // Range of vision
    public float visionRange = 5f;

    public enum AgentMode { exploring, gathering, building };
    public AgentMode currentMode;

    // Internals
    Rigidbody agentRB;
    RayPerception rayPer;
    float wood;
    GameObject gatherTarget;

    public override void InitializeAgent()
    {
        base.InitializeAgent();
        currentMode = AgentMode.exploring;
        agentRB = GetComponent<Rigidbody>();
        rayPer = GetComponent<RayPerception>();
        myAcademy = myAcademyObj.GetComponent<CivAcademy>();
    }

    public override void CollectObservations()
    {
        // Looking around with raycasts
        float rayDistance = visionRange;
        float[] rayAngles = { 20f, 90f, 160f, 45f, 135f, 70f, 110f };
        string[] detectableObjects = { "resource", "civilian", "wall" };
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

        // Time penalty if the villager is just exploring
        if (currentMode == AgentMode.exploring)
        {
            AddReward(-0.05f);
            //Debug.Log("Negative time reward logged.");
        }
    }

    public override void AgentReset()
    {
        wood = 0;
        currentMode = AgentMode.exploring;
        agentRB.velocity = Vector3.zero;
        // Agent position reset is managed by the Academy
        myAcademy.RespawnAgent(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        float qtty = 0f;
        if (other.gameObject.CompareTag("resource"))
        {
            gatherTarget = other.gameObject;
            currentMode = AgentMode.gathering;
            Resource res = gatherTarget.GetComponent<Resource>();
            if (res != null)
            {
                qtty = res.Gather(gatheringRate);
                myAcademy.totalWood += qtty;
                wood += qtty;

                if (qtty > 0f)
                {
                    // Reward the villager for gathering a resource
                    AddReward(0.2f);
                } else
                {
                    currentMode = AgentMode.exploring;
                }
            }
        }
    }

    private void Update()
    {
        if ((currentMode == AgentMode.gathering) && (gatherTarget == null))
        {
            currentMode = AgentMode.exploring;
            Debug.Log("Resource we were gathering was destroyed.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        currentMode = AgentMode.exploring;
        Debug.Log("Exited trigger area.");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            // Penalty when hitting a wall
            AddReward(-0.2f);
        }
    }

    public override void AgentOnDone()
    {

    }

}
