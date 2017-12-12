using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticalGameController : MonoBehaviour {
  TacticalMap tactical_map;
	// Use this for initialization
	void Start () {
    tactical_map = GameObject.FindObjectOfType<TacticalMap>();

    tactical_map.spawn_map(16);
    tactical_map.place_unit(1, 2, -3);
    tactical_map.place_unit(0, -1, 1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
