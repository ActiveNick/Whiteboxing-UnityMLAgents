using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CivAcademy : Academy {

    [HideInInspector]
    public GameObject[] agents;
    [HideInInspector]
    public LandSpawnArea[] listArea;

    public float totalWood;
    public Text resourceWoodText;

    public override void AcademyReset()
    {
        ClearObjects(GameObject.FindGameObjectsWithTag("resource"));

        agents = GameObject.FindGameObjectsWithTag("civilian");
        listArea = FindObjectsOfType<LandSpawnArea>();
        foreach (LandSpawnArea sa in listArea)
        {
            sa.ResetSpawnArea(agents);
        }

        totalWood = 0f;
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
        resourceWoodText.text = string.Format(@"Wood: {0}", totalWood);
    }

}
