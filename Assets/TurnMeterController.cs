using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnMeterController : MonoBehaviour {
  const float METER_LEFT = -15.5f;
  const float METER_RIGHT = 4.8f;
  const float TIME_TO_FULL_CAP = 5.0f;
  const float METER_LENGTH = METER_RIGHT - METER_LEFT;

  Dictionary<string, UnitActor> actors;
  Dictionary<string, GameObject> markers;
  List<string> queued_actors = new List<string>();
  ActionUIController action_ui;
  bool is_running;
  string current_actor_id;

  bool is_lerping;
  GameObject lerping_marker;
  float lerp_origin_position;
  float lerp_target_position;
  float lerp_timing;
  bool queue_is_turn_ending;

	// Use this for initialization
	void Start () {
    actors = new Dictionary<string, UnitActor>();
    markers = new Dictionary<string, GameObject>();
    action_ui = GetComponent<ActionUIController>();
    run_turn_meter(false);
    current_actor_id = null;

    is_lerping = false;
    lerping_marker = null;
    queue_is_turn_ending = false;
	}
	
	// Update is called once per frame
	void Update () {
    if (is_running) {
      tick_all_actors();
    }

    if (is_lerping) {
      lerp_marker();
    }
	}

  public void register_actor(string identifier, UnitActor actor) {
    GameObject marker = Instantiate(Resources.Load("TurnMeterMarker")) as GameObject;
    actors.Add(identifier, actor);
    markers.Add(identifier, marker);

    set_marker_position(identifier);
  }

  void set_marker_position(string identifier, float _time_to_full=999) {
    GameObject marker = markers[identifier];
    float y_position = -7.8f;
    float time_to_full = Mathf.Clamp(_time_to_full, 0f, 5f);
    float x_position = METER_RIGHT - METER_LENGTH * time_to_full / TIME_TO_FULL_CAP;

    marker.transform.position = new Vector3(x_position, y_position, 0f);
  }

  void set_marker_position_lerp(string identifier, float _time_to_full=999) {
    lerping_marker = markers[identifier];
    float time_to_full = Mathf.Clamp(_time_to_full, 0f, 5f);
    lerp_target_position = METER_RIGHT - METER_LENGTH * time_to_full / TIME_TO_FULL_CAP;
    lerp_origin_position = lerping_marker.transform.position.x;
    is_lerping = true;
    lerp_timing = 0f;
  }

  void lerp_marker() {
    lerp_timing += Time.fixedDeltaTime * 3f;
    float clamped_lerp_timing = Mathf.Clamp(lerp_timing, 0f, 1f);
    float x_pos = Mathf.Lerp(lerp_origin_position, lerp_target_position, clamped_lerp_timing);
    Vector3 pos = lerping_marker.transform.position;
    lerping_marker.transform.position = new Vector3(x_pos, pos.y, pos.z);

    if (lerp_timing > 1f) {
      is_lerping = false;

      if (queue_is_turn_ending) {
        end_current_actor_turn();
      }
    }
  }

  public void run_turn_meter(bool val = true) {
    is_running = val;
    action_ui.show_actions_ui(!val);
  }

  void tick_all_actors() {
    foreach (KeyValuePair<string, UnitActor> pair in actors) {
      UnitActor actor = pair.Value;
      float ap = actor.tick();
      set_marker_position(pair.Key, actor.time_to_full());

      if (ap > 100f) {
        queued_actors.Add(pair.Key);
      }
    }

    if (queued_actors.Count > 0) {
      set_actor();
    } else {
      run_turn_meter(true);
    }
  }

  void set_actor() {
    ActionUIController action_ui_controller = GameObject.FindObjectOfType<ActionUIController>();
    current_actor_id = queued_actors[0];
    action_ui_controller.set_actor(actors[current_actor_id]);

    run_turn_meter(false);
  }

  public void update_current_meter(float ttf, bool is_turn_ending) {
    queue_is_turn_ending = is_turn_ending;
    set_marker_position_lerp(current_actor_id, ttf);
  }

  public void end_current_actor_turn() {
    queued_actors.RemoveAt(0);

    if (queued_actors.Count == 0) {
      run_turn_meter();
    }
  }
}
