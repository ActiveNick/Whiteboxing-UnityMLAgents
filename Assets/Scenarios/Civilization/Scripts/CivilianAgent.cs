using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianAgent : Agent {

    //public GameObject myAcademyObj;
    //CivAcademy myAcademy;
    LandSpawnArea myArea;

    // Speed of agent movement.
    public float moveSpeed = 1;
    // Speed of agent rotation.
    public float turnSpeed = 300;
    // Gathering speed 
    public float gatheringRate = 0.1f;
    // Range of vision
    public float visionRange = 5f;

    float currentGatherReward;
    float startGatherReward = 0.025f;

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
        currentGatherReward = startGatherReward;
        agentRB = GetComponent<Rigidbody>();
        rayPer = GetComponent<RayPerception>();
        //myAcademy = myAcademyObj.GetComponent<CivAcademy>();
        myArea = GetComponentInParent<LandSpawnArea>();
    }

    public override void CollectObservations()
    {
        // Looking around with raycasts
        float rayDistance = visionRange;
        float[] rayAngles = { 20f, 90f, 160f, 45f, 135f, 70f, 110f };
        string[] detectableObjects = { "resource", "farm", "wall" };
        AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, -0.1f));

        // The current speed of the agent
        Vector3 localVelocity = transform.InverseTransformDirection(agentRB.velocity);
        AddVectorObs(localVelocity.x);
        AddVectorObs(localVelocity.z);

        // The current amount of wood we hold
        AddVectorObs(myArea.totalWood);

        // The current mode of the Agent (gathering, exploring, etc.)
        AddVectorObs((int)currentMode);
    }

    public void MoveAgent(float[] act)
    {
        float absDir = 0f;
        Vector3 dirToGo = Vector3.zero;
        Vector3 rotateDir = Vector3.zero;

        if (brain.brainParameters.vectorActionSpaceType == SpaceType.continuous)
        {
            absDir = Mathf.Clamp(act[0], -1f, 1f);
            dirToGo = transform.forward * absDir;
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
        Vector3 localVel = transform.InverseTransformDirection(agentRB.velocity);
        if (localVel.z < 0)
        {
            // Penalize the agent for moving backwards, forcing it to move forward and use raycasts
            //AddReward(-0.001f);
#if (UNITY_EDITOR)
            Debug.Log("Penalized for moving backwards.");
#endif
        }
        transform.Rotate(rotateDir, Time.fixedDeltaTime * turnSpeed);

        if (agentRB.velocity.sqrMagnitude > 25f) // slow it down
        {
            agentRB.velocity *= 0.95f;
        }
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        MoveAgent(vectorAction);

        bool buildCommand = Mathf.Clamp(vectorAction[2], 0f, 1f) > 0.5f;
        if (buildCommand)
        {
            if (myArea.totalWood >= 100)
            {
                myArea.totalWood -= 100;
                myArea.PlaceBuilding(LandSpawnArea.BuildingType.Farm, gameObject.transform.position.x, gameObject.transform.position.z);
                AddReward(5f);
                Debug.Log("Farm built!");
            } else
            {
                // The penalty size is proportional to how much wood we're missing
#if (UNITY_EDITOR)
                Debug.Log("Not allowed to build a farm yet.");
#endif
                //AddReward((100 - myArea.totalWood) / 100 * -1);
            }
        }

        // Time penalty if the villager is just exploring
        if (currentMode == AgentMode.exploring)
        {
            AddReward(-0.0002f);
#if (UNITY_EDITOR)
            Debug.Log("Negative time reward logged.");
#endif
        }
    }

    public override void AgentReset()
    {
        wood = 0;
        currentMode = AgentMode.exploring;
        currentGatherReward = startGatherReward;
        agentRB.velocity = Vector3.zero;
        // Agent position reset was managed by the Academy, now moved to LandSpawnArea
        //myAcademy.RespawnAgent(gameObject);
        myArea.RespawnNPC(gameObject);
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
                //myAcademy.totalWood += qtty;
                myArea.totalWood += qtty;
                wood += qtty;

                if (qtty > 0f)
                {
                    // Reward the villager for gathering a resource
                    AddReward(currentGatherReward);
                    // Incentivize the villager to not interrupt the gathering process
                    currentGatherReward += 0.01f;
#if (UNITY_EDITOR)
                    Debug.Log("Current Gather Reward: " + currentGatherReward.ToString());
#endif
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
            currentGatherReward = startGatherReward;
            Debug.Log("Resource we were gathering was destroyed.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        currentMode = AgentMode.exploring;
        currentGatherReward = startGatherReward;
        Debug.Log("Exited trigger area.");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            // Penalty when hitting a wall
            AddReward(-1f);
            //Done();
        }
    }

    public override void AgentOnDone()
    {

    }
}
