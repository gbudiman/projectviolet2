﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour {
  public static Color tile_neutral;
  public static Color tile_highlighted;
  public static Color tile_active;
  public static Color tile_valid_target;
  public static Color actor_neutral;
  public static Color actor_highlighted;
  // Use this for initialization
  void Start() {
    initialize();
  }

  public void initialize() { 
    tile_neutral = hex_to_float(0x58, 0x58, 0x58);
    tile_highlighted = hex_to_float(0xff, 0xd8, 0x52);
    tile_active = hex_to_float(0x40, 0xeb, 0x85);
    tile_valid_target = hex_to_float(0xff, 0x69, 0x45);
    actor_neutral = tile_neutral;
    actor_highlighted = tile_highlighted;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public static Color hex_to_float(int r, int g, int b) {
    return new Color(((float)r)/255, ((float)g)/255, ((float)b)/255);
  }
}
