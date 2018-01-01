using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorTest : MonoBehaviour {
  UnitActor actor;
  EquipsLoader equips_loader;
  SlottableAnatomy anatomy;

  Dictionary<string, bool> gt_actions;
	// Use this for initialization
	void Start () {
    actor = GetComponent<UnitActor>();
    equips_loader = GetComponent<EquipsLoader>();

    gt_actions = new Dictionary<string, bool>();
    begin_test();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  void begin_test() {
    actor.confer_tech("marksmanship_bow");
    actor.confer_tech("marksmanship_double_tap");

    actor.get_actions();
    int z = 0;
  }
}
