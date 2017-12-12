using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCoord : MonoBehaviour {
  public int a, b, c;
  bool is_active;
  SpriteRenderer sprite;
  UnitActor occupant;
	// Use this for initialization
	void Start () {
    sprite = GetComponent<SpriteRenderer>();
    occupant = GetComponentInChildren<UnitActor>();
    is_active = false;
	}
	
	// Update is called once per frame
	void Update () {
		
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

  public void highlight(bool val = true) {
    if (!is_active) {
      sprite.color = val ? ColorController.tile_highlighted : ColorController.tile_neutral;
    }
  }

  public void highlight_as_active(bool val = true) {
    sprite.color = val ? ColorController.tile_active : ColorController.tile_neutral;
    is_active = val;
  }
}
