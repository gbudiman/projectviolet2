using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipsTest : MonoBehaviour {
  EquipsLoader equips_loader;
  SlottableAnatomy anatomy;
	// Use this for initialization
	void Start () {
    equips_loader = GetComponent<EquipsLoader>();
    anatomy = GetComponent<SlottableAnatomy>();
    begin_test();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  void begin_test() {
    
    equip("bow_long"); list();
    equip("whip", false); list();
    equip("scepter", true); list();
    equip("quiver_slingback", true); list();
    add("arrow_iron", 7); list();
  }

  void equip(string s, bool mainside = true) {
    var equips = equips_loader.equips;
    anatomy.equip(equips[s], mainside);
  }

  void add(string s, int amount, bool mainside = true) {
    var equips = equips_loader.equips;
    anatomy.load_multis(equips[s], amount, mainside);
  }

  void list() {
    anatomy.list_equipments();
  }
}
