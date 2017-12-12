using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCoord : MonoBehaviour {
  public int a, b, c;
  private Color neutral = new Color(1f, 1f, 1f);
  private Color highlighted = new Color((float)0xff / 255, (float)0xd8 / 255, (float)0x52 / 255);//#ffd852
  SpriteRenderer sprite;
	// Use this for initialization
	void Start () {
    sprite = GetComponent<SpriteRenderer>();
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
    sprite.color = highlighted;
  }

  public void OnMouseExit() {
    sprite.color = neutral;
  }
}
