using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActor : MonoBehaviour {
  HexCoord hex_tile;
  float ap_fill_rate;
  float current_ap;
  public float apc_attack;
  public bool turn_ending_attack;
  public bool has_taken_actions;
  TacticalMap tactical_map;
  TurnMeterController turn_meter_controller;
	// Use this for initialization
	void Start () {
    hex_tile = GetComponentInParent<HexCoord>();
    tactical_map = GameObject.FindObjectOfType<TacticalMap>();

    apc_attack = 50;
    turn_ending_attack = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void initialize(float _ap_fill_rate, TurnMeterController _tmc) {
    current_ap = 0;
    ap_fill_rate = _ap_fill_rate;
    turn_meter_controller = _tmc;
    has_taken_actions = false;
  }

  public void highlight(bool val = true) {
  }

  public void OnMouseEnter() {
    hex_tile.highlight(true);
  }

  public void OnMouseExit() {
    hex_tile.highlight(false);
  }

  public float tick() {
    current_ap += ap_fill_rate * Time.fixedDeltaTime;
    return current_ap;
  }

  public float expend_ap(float amount = 100f, bool is_turn_ending=false) {
    has_taken_actions = true;
    current_ap -= amount;
    turn_meter_controller.update_current_meter(time_to_full(), is_turn_ending);

    if (is_turn_ending) {
      has_taken_actions = false;
    }

    return current_ap;
  }

  public float time_to_full() {
    return (100f - current_ap) / ap_fill_rate;
  }


}
