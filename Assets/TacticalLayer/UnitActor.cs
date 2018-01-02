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

  Dictionary<string, SkillData> techs;
  Dictionary<string, EquipData> equips;
  public ActorTechs actor_techs;
  public SlottableAnatomy anatomy;
  public ActionController action_controller;

	// Use this for initialization
	void Start () {
    hex_tile = GetComponentInParent<HexCoord>();
    tactical_map = GameObject.FindObjectOfType<TacticalMap>();

    apc_attack = 50;
    turn_ending_attack = true;
    initialize_tooltip_dictionary();
    initialize_confirmation_dictionary();

    techs = GetComponent<TechsLoader>().get_techs();
    equips = GetComponent<EquipsLoader>().equips;

    anatomy = GetComponent<SlottableAnatomy>();
    actor_techs = GetComponent<ActorTechs>();
    action_controller = GetComponent<ActionController>();

    anatomy.set_parent_actor(this);
    action_controller.set_parent_actor(this);
    action_controller.set_tech_dict(techs);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public Dictionary<string, bool> get_actions() {
    return action_controller.get_actions();
  }

#region Techs
  void check_tech_or_raise_exception(string tech_id) {
    if (!techs.ContainsKey(tech_id)) throw new System.ArgumentException("Invalid Tech ID " + tech_id);
  }

  public bool has_tech(string tech_id) {
    check_tech_or_raise_exception(tech_id);
    return actor_techs.has_tech(tech_id);
  }

  public void confer_tech(string tech_id) {
    check_tech_or_raise_exception(tech_id);
    actor_techs.assign(tech_id);
    action_controller.apply_techs();
  }

  public void hard_remove_tech(string tech_id) {
    check_tech_or_raise_exception(tech_id);
    actor_techs.hard_remove(tech_id);
  }

  public void enable_tech(string tech_id, bool val = true) {
    check_tech_or_raise_exception(tech_id);
    actor_techs.enable(tech_id, val);
  }

  #endregion

#region Equips
  void check_item_or_raise_exception(string item) {
    if (!equips.ContainsKey(item)) throw new System.ArgumentException("Invalid Item ID " + item);
  }

  public void equip(string s) {
    anatomy.equip(equips[s]);
  }

  public void swap_arm_equips() {
    anatomy.swap_arms();
  }

  public bool check_has_equipment_with_attributes(string attb) {
    return anatomy.check_has_equipment_with_attributes(attb);
  }

  public bool check_has_equipment_with_attributes(List<string> attb) {
    return anatomy.check_has_equipment_with_attributes(attb);
  }

#endregion

  void initialize_tooltip_dictionary() {
    //tooltip_dict = new Dictionary<Action, string>() {
    //  {Action.attack, "Select adjacent target to commence melee attack" }
    //};
  }

  void initialize_confirmation_dictionary() {
    //confirmation_dict = new Dictionary<Action, string>() {
    //  {Action.attack, "Click to attack this target" }
    //};
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

  //public void set_default_action() {
  //  action = Action.attack;
  //}

  //public void set_action(Action a) {
  //  action = a;
  //}

  //public string get_tooltip() {
  //  return tooltip_dict[action];
  //}

  //public string get_confirmation() {
  //  return confirmation_dict[action];
  //}

  //public void execute_action() {
  //  switch (action) {
  //    case Action.attack: expend_ap(apc_attack, turn_ending_attack); break;
  //  }
  //}

  public void highlight_as_active() {
    hex_tile.highlight_as_active();
  }

  public HexCoord get_tile() {
    return hex_tile;
  }

}
