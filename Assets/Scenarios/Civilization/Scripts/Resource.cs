using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour {

    public enum resourceType { wood, stone, gold }

    public resourceType resource;
    public float quantity = 20f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // If the resource is depleted
        if (quantity == 0f)
        {
            Destroy(gameObject);
        }
    }

    public float Gather(float qtty)
    {
        if (qtty > quantity)
        {
            qtty = quantity;
            quantity = 0f;
        } else
        {
            quantity -= qtty;
        }
        return qtty;
    }
}
