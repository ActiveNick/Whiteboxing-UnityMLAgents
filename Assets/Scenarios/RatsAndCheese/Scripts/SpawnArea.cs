using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : Area {

    public GameObject cheese;
    public int numCheese;
    //public bool respawnCheese;
    public float range;
    public float HeightCheese = 0.1f;
    public float HeightRat = 0.2f;

    void CreateCheese(int numCheese)
    {
        for (int i = 0; i < numCheese; i++)
        {
            GameObject chz = Instantiate(cheese, new Vector3(Random.Range(-range, range), HeightCheese,
                                                              Random.Range(-range, range)) + transform.position,
                                          Quaternion.Euler(new Vector3(0f, Random.Range(0f, 360f), 90f)));
        }
    }

    public void RespawnRat(GameObject agent)
    {
        agent.transform.position = new Vector3(Random.Range(-range, range), HeightRat,
                                                       Random.Range(-range, range))
                    + transform.position;
        agent.transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));
    }

    public void ResetSpawnArea(GameObject[] agents)
    {
        foreach (GameObject agent in agents)
        {
            if (agent.transform.parent == gameObject.transform)
            {
                RespawnRat(agent);
            }
        }

        CreateCheese(numCheese);
    }
}
