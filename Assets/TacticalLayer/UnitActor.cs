using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActor : MonoBehaviour {
  HexCoord hex_tile;
  float ap_fill_rate;
  float current_ap;
  TacticalMap tactical_map;
	// Use this for initialization
	void Start () {
    hex_tile = GetComponentInParent<HexCoord>();
    tactical_map = GameObject.FindObjectOfType<TacticalMap>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void initialize(float _ap_fill_rate) {
    current_ap = 0;
    ap_fill_rate = _ap_fill_rate;
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

  public float expend_ap(float amount = 100f) {
    current_ap -= amount;
    return current_ap;
  }

  public float time_to_full() {
    return (100f - current_ap) / ap_fill_rate;
  }
}
