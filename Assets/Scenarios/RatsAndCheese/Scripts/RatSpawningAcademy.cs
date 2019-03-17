using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAgents;

public class RatSpawningAcademy : Academy {

    [HideInInspector]
    public GameObject[] agents;
    [HideInInspector]
    public SpawnArea[] listArea;

    public int totalScore;
    public Text scoreText;

    public override void AcademyReset()
    {
        ClearObjects(GameObject.FindGameObjectsWithTag("cheese"));

        agents = GameObject.FindGameObjectsWithTag("rat");
        listArea = FindObjectsOfType<SpawnArea>();
        foreach (SpawnArea sa in listArea)
        {
            sa.ResetSpawnArea(agents);
        }

        totalScore = 0;
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
        SpawnArea sa = agent.GetComponentInParent<SpawnArea>();
        if (sa != null)
        {
            sa.RespawnRat(agent);
        }
    }

    public override void AcademyStep()
    {
        scoreText.text = string.Format(@"Score: {0}", totalScore);
    }
}
