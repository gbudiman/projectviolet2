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

  public enum Action { attack };
  public Action action;
  public Dictionary<Action, string> tooltip_dict;
  public Dictionary<Action, string> confirmation_dict;
	// Use this for initialization
	void Start () {
    hex_tile = GetComponentInParent<HexCoord>();
    tactical_map = GameObject.FindObjectOfType<TacticalMap>();

    apc_attack = 50;
    turn_ending_attack = true;
    initialize_tooltip_dictionary();
    initialize_confirmation_dictionary();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  void initialize_tooltip_dictionary() {
    tooltip_dict = new Dictionary<Action, string>() {
      {Action.attack, "Select adjacent target to commence melee attack" }
    };
  }

  void initialize_confirmation_dictionary() {
    confirmation_dict = new Dictionary<Action, string>() {
      {Action.attack, "Click to attack this target" }
    };
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

  public void OnMouseUp() {
    hex_tile.mouse_up_action();
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
      hex_tile.highlight_as_active(false);
    }

    return current_ap;
  }

  public float time_to_full() {
    return (100f - current_ap) / ap_fill_rate;
  }

  public void set_default_action() {
    action = Action.attack;
  }

  public void set_action(Action a) {
    action = a;
  }

  public string get_tooltip() {
    return tooltip_dict[action];
  }

  public string get_confirmation() {
    return confirmation_dict[action];
  }

  public void execute_action() {
    switch (action) {
      case Action.attack: expend_ap(apc_attack, turn_ending_attack); break;
    }
  }

  public void highlight_as_active() {
    hex_tile.highlight_as_active();
  }

  public HexCoord get_tile() {
    return hex_tile;
  }

}
