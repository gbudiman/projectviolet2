using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipsTest : MonoBehaviour {
  EquipsLoader equips_loader;
  SlottableAnatomy anatomy;
  SlottableAnatomy validator;
  const bool MAIN = true;
  const bool OFF = false;

  int test_count;
  int test_passed;
	// Use this for initialization
	void Start () {
    equips_loader = GetComponent<EquipsLoader>();
    anatomy = GetComponent<SlottableAnatomy>();
    validator = GameObject.Find("Validator").GetComponent<SlottableAnatomy>();

    begin_test();
    print_test_result();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  void print_test_result() {
    Debug.Log(test_passed + "/" + test_count + " tests passed");
  }

  void begin_test() {
    test_count = 0; test_passed = 0;

    set_test("bow_long", "arm_dual");  test("bow_long");
    set_test(null, "arm_dual"); set_test("whip", "arm_off"); test("whip", OFF);
    set_test("scepter", "arm_main"); set_test("scepter", "arm_main"); test("scepter", MAIN);
    //equip("bow_long"); list();
    //equip("whip", false); list();
    //equip("scepter", true); list();
    //equip("quiver_slingback", true); list();
    //add("arrow_iron", 7); list();
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

  void test(string equip_id, bool mainside = true) {
    equip(equip_id, mainside);
    validate();
    test_count++;
  }

  void set_test(string equip_id, string implement) {
    if (equip_id == null) {
      validator.anatomy[implement] = null;
    } else {
      EquipData equip_data = equips_loader.equips[equip_id];
      validator.anatomy[implement] = equip_data;
    }
  }

  void set_clear() {
    validator.unequip_all();
  }

  void validate() {
    List<string> invalids = anatomy.equals(validator);

    foreach (string invalid_implement in invalids) {
      Debug.Log("Checking implement " + invalid_implement);
      string test = anatomy.anatomy[invalid_implement] == null ? "Empty" : anatomy.anatomy[invalid_implement].name;
      string expectation = validator.anatomy[invalid_implement] == null ? "Empty" : validator.anatomy[invalid_implement].name;

      Debug.Log("Anatomy: " + invalid_implement + " - Expected " + expectation + " | Got " + test);
    }

    if (invalids.Count > 0) {
      list();
    } else {
      test_passed++;
    }
  }
}
