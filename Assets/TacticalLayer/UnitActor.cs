using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActor : MonoBehaviour {
  HexCoord hex_tile;
	// Use this for initialization
	void Start () {
    hex_tile = GetComponentInParent<HexCoord>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void highlight(bool val = true) {
  }

  public void OnMouseEnter() {
    hex_tile.highlight(true);
  }

  public void OnMouseExit() {
    hex_tile.highlight(false);
  }
}
