using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class VictoryZone : MonoBehaviour {

	void OnTriggerEnter(Collider other)
    {
        if(Player.instance != null && other == Player.instance.headCollider)
        {
            Debug.Log("Victory !!!");
        }
    }
}
