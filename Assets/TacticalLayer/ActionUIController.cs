using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionUIController : MonoBehaviour {
  Canvas action_canvas;
  UnitActor actor;
	// Use this for initialization
	void Start () {
    action_canvas = GameObject.Find("ActionCanvas").GetComponent<Canvas>();
    actor = null;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void show_actions_ui(bool val = true) {
    action_canvas.enabled = val;

    if (actor != null) {
      Button end_turn_button = action_canvas.transform.Find("EndTurnButton").GetComponent<Button>();

      end_turn_button.interactable = actor.has_taken_actions;
    }
  }

  public void action_skip() {
    actor.expend_ap(100, true);
  }

  public void action_end_turn() {
    actor.expend_ap(0, true);
  }

  public void action_attack() {
    actor.expend_ap(actor.apc_attack, actor.turn_ending_attack);
  }

  public void set_actor(UnitActor _actor) {
    actor = _actor;
  }
}
