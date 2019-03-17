using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAgents;

public class CivAcademy : Academy {

    [HideInInspector]
    public GameObject[] agents;
    [HideInInspector]
    public LandSpawnArea[] listArea;
    public LandSpawnArea mainBoard;

    public Text resourceWoodText;

    public override void AcademyReset()
    {
        ClearObjects(GameObject.FindGameObjectsWithTag("resource"));
        ClearObjects(GameObject.FindGameObjectsWithTag("farm"));

        agents = GameObject.FindGameObjectsWithTag("civilian");
        listArea = FindObjectsOfType<LandSpawnArea>();
        foreach (LandSpawnArea sa in listArea)
        {
            sa.ResetSpawnArea(agents);
        }
    }

    void ClearObjects(GameObject[] objects)
    {
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
    }

    public void RespawnAgent(GameObject agent)
    {
        LandSpawnArea sa = agent.GetComponentInParent<LandSpawnArea>();
        if (sa != null)
        {
            sa.RespawnNPC(agent);
        }
    }

    public override void AcademyStep()
    {
        // We only display the resources for the first environment for debugging purposes
        resourceWoodText.text = string.Format(@"Wood: {0:#,###.#}", mainBoard.totalWood);
    }

}
