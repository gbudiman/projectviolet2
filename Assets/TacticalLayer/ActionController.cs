using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour {
  Dictionary<string, bool> actions;
  Dictionary<string, SkillData> tech_dict;
  UnitActor actor;
	// Use this for initialization
	void Start () {
    actions = new Dictionary<string, bool>() {
      { "move", true },
      { "wait", true },
      { "attack_melee", true },
      { "attack_bow", true },
      { "attack_firearm", true },
      { "throw", true },
    };
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void set_parent_actor(UnitActor unit_actor) {
    actor = unit_actor;
  }

  public void set_tech_dict(Dictionary<string, SkillData> td) {
    tech_dict = td;
  }

  public void apply_techs() {
    foreach (string tech in actor.actor_techs.techs.Keys) {
      SkillData skill = tech_dict[tech];
      if (skill.activation == "Active") {
        actions.Add(tech, true);
      }
    }
  }

  public Dictionary<string, bool> get_actions() {
    return actions;
  }
}
