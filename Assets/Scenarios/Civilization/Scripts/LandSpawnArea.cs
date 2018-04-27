using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class acts as an environment manager
/// </summary>
public class LandSpawnArea : Area {

    public enum BuildingType { Farm, Barracks }

    public GameObject tree;
    public GameObject farm;
    public GameObject barracks;
    public int numTrees;
    public float range;
    public float HeightTree = 0.1f;
    public float HeightNPC = 0.2f;
    public float totalWood = 0f;

    void CreateTree(int numTrees)
    {
        for (int i = 0; i < numTrees; i++)
        {
            GameObject objTree = Instantiate(tree, new Vector3(Random.Range(-range, range), HeightTree,
                                                              Random.Range(-range, range)) + transform.position,
                                          Quaternion.Euler(new Vector3(0f, 0f, 0f)));
        }
    }

    public void PlaceBuilding(BuildingType buildType, float posX, float posZ)
    {
        GameObject building = null;

        switch (buildType)
        {
            case BuildingType.Farm:
                building = farm;
                break;
            case BuildingType.Barracks:
                building = barracks;
                break;
        }

        if (building != null)
        {
            Instantiate(building, new Vector3(posX, gameObject.transform.position.y, posZ), Quaternion.Euler(new Vector3(0f, 0f, 0f)));
        }    
    }

    public void RespawnNPC(GameObject agent)
    {
        agent.transform.position = new Vector3(Random.Range(-range, range), HeightNPC,
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
                RespawnNPC(agent);
            }
        }

        totalWood = 0f;
        CreateTree(numTrees);
    }

    private void Update()
    {
        
    }
}