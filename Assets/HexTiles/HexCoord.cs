using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCoord : MonoBehaviour {
  public int a, b, c;
  bool is_active;
  SpriteRenderer sprite;
  UnitActor occupant;
  ActionUIController action_ui_controller;
  float strobe_alpha;
  int strobe_alpha_multiplier;
  // Use this for initialization
  void Start () {
    sprite = GetComponent<SpriteRenderer>();
    occupant = GetComponentInChildren<UnitActor>();
    action_ui_controller = GameObject.FindObjectOfType<ActionUIController>();
    is_active = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (is_active) {
      strobe();
    }
	}

  public void set_abc(int _a, int _b, int _c) {
    a = _a; b = _b; c = _c;
    transform_abc();
  }

  void transform_abc() {
    float x = b + 0.5f * c;
    float y = 0.866f * a + 0.866f * b;

    transform.position = new Vector3(x, y, 0);
  }

  public void OnMouseEnter() {
    highlight(true);
  }

  public void OnMouseExit() {
    highlight(false);
  }

  public void OnMouseUp() {
    mouse_up_action();
  }

  public void mouse_up_action() {
    if (this.is_adjacent_to(action_ui_controller.get_active_actor_position()) && tile_has_valid_target()) {
      action_ui_controller.execute_selected_action();
    }
  }

  public bool is_adjacent_to(HexCoord other) {
    int d_a = Mathf.Abs(a - other.a);
    int d_b = Mathf.Abs(b - other.b);
    int d_c = Mathf.Abs(c - other.c);

    return (d_a + d_b + d_c == 2);
  }

  public bool tile_has_valid_target() {
    return GetComponentsInChildren<UnitActor>().Length > 0;
  }

  public void highlight(bool val = true) {
    //if (!is_active) {

    //  if (action_ui_controller.get_active_actor() != null
    //    && action_ui_controller.get_active_actor().action == UnitActor.Action.attack) {
    //    HexCoord active_actor_position = action_ui_controller.get_active_actor_position();

    //    if (this.is_adjacent_to(active_actor_position) && tile_has_valid_target()) {
    //      sprite.color = val ? ColorController.tile_valid_target : ColorController.tile_neutral;
    //      action_ui_controller.paint_target();
    //    } else {
    //      sprite.color = val ? ColorController.tile_highlighted : ColorController.tile_neutral;
    //      action_ui_controller.paint_target(false);
    //    }
    //  } else {
    //    sprite.color = val ? ColorController.tile_highlighted : ColorController.tile_neutral;
    //    action_ui_controller.paint_target(false);
    //  }
    //}
  }

  public void highlight_as_active(bool val = true) {
    sprite.color = val ? ColorController.tile_active : ColorController.tile_neutral;
    is_active = val;
    strobe_alpha = 1f;
    strobe_alpha_multiplier = -1;

    if (!val) {
      Color c = sprite.color;
      sprite.color = new Color(c.r, c.g, c.b, 1f);
    }
  }

  void strobe() {
    strobe_alpha += Time.fixedDeltaTime * strobe_alpha_multiplier * 1.5f;

    float strobe_alpha_clamped = Mathf.Clamp(strobe_alpha, 0f, 1f);
    Color c = sprite.color;
    sprite.color = new Color(c.r, c.g, c.b, strobe_alpha_clamped);
    if (strobe_alpha < 0f) {
      strobe_alpha_multiplier = 1;
    } else if (strobe_alpha > 1f) {
      strobe_alpha_multiplier = -1;
    }

    
  }
}
