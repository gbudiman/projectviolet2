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
  List<UnitActor> queued_actors = new List<UnitActor>();
  bool is_running;

	// Use this for initialization
	void Start () {
    actors = new Dictionary<string, UnitActor>();
    markers = new Dictionary<string, GameObject>();
    is_running = false;
	}
	
	// Update is called once per frame
	void Update () {
    if (is_running) tick_all_actors();
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

  public void run_turn_meter(bool val = true) {
    is_running = val;
  }

  void tick_all_actors() {

    if (queued_actors.Count > 0) {

    } else {
      foreach (KeyValuePair<string, UnitActor> pair in actors) {
        UnitActor actor = pair.Value;
        float ap = actor.tick();
        set_marker_position(pair.Key, actor.time_to_full());

        if (ap > 100f) {
          queued_actors.Add(actor);
        }
      }

      is_running = queued_actors.Count == 0;
    }

    
  }
}
