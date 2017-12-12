using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticalGameController : MonoBehaviour {
  TacticalMap tactical_map;
  ColorController color_controller;
  TurnMeterController turn_meter_controller;
	// Use this for initialization
	void Start () {
    tactical_map = GameObject.FindObjectOfType<TacticalMap>();

    tactical_map.spawn_map(16);
    //tactical_map.place_unit(1, 2, -3);
    //tactical_map.place_unit(0, -1, 1);

    turn_meter_controller = GetComponent<TurnMeterController>();

    
    spawn_unit_actor("B", 40f, 0, -1, 1);
    spawn_unit_actor("C", 18f, 0, 0, 0);
    spawn_unit_actor("A", 33f, 1, 2, -3);
    turn_meter_controller.run_turn_meter();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  void spawn_unit_actor(string name, float ap_fill_rate, int a, int b, int c) {
    GameObject unit = tactical_map.place_unit(a, b, c);
    UnitActor actor = unit.GetComponent<UnitActor>();

    actor.initialize(ap_fill_rate, turn_meter_controller);
    turn_meter_controller.register_actor(name, actor);
  }
}
