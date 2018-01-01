using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipsTest : MonoBehaviour {
  UnitActor actor;
  EquipsLoader equips_loader;
  SlottableAnatomy anatomy;
  SlottableAnatomy validator;
  const bool MAIN = true;
  const bool OFF = false;
  const bool APPEND = true;

  int test_count;
  int test_passed;
	// Use this for initialization
	void Start () {
    actor = GetComponent<UnitActor>();
    equips_loader = GetComponent<EquipsLoader>();
    anatomy = actor.anatomy; //GetComponent<SlottableAnatomy>();
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
    set_test("scepter", "arm_main"); set_test(null, "arm_off"); test("scepter", MAIN);
    actor.confer_tech("dual_wield");
    set_test("whip", "arm_off"); test("whip", OFF);
    set_test("whip", "arm_main"); set_test("scepter", "arm_off"); test_swap();
    set_test(null, "arm_main"); test_unequip("arm_main");
    set_test("quiver_slingback", "slingback_main"); test("quiver_slingback", MAIN);

    set_multitest("arrow_iron", "quiver_main", 7); test_multi("arrow_iron", 7, MAIN);
    set_multitest("arrow_steel", "quiver_main", 18, APPEND); test_multi("arrow_steel", 18, MAIN);
    test_multi("arrow_iron", 1, MAIN);
    set_multiremove("arrow_steel", "quiver_main", 3); test_multiremove("arrow_steel", 3);
    set_multitest(null, "quiver_main"); test_empty_multi("quiver_main");
    set_test(null, "slingback_main"); test_unequip("slingback_main");
    set_multitest(null, "quiver_main"); test_multi("arrow_iron", 5, MAIN);

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

  void test_swap() {
    actor.swap_arm_equips();
    validate();
    test_count++;
  }

  void test(string equip_id, bool mainside = true) {
    equip(equip_id, mainside);
    validate();
    test_count++;
  }

  void test_multi(string equip_id, int amount, bool mainside = true) {
    add(equip_id, amount, mainside);
    validate();
    test_count++;
  }

  void test_multiremove(string equip_id, int amount) {
    anatomy.take_from_multis(equip_id, amount);
    validate();
    test_count++;
  }

  void test_empty_multi(string implement) {
    anatomy.empty_multis(implement);
    validate();
    test_count++;
  }

  void test_unequip(string implement) {
    anatomy.unequip(implement);
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

  void set_multitest(string equip_id, string implement, int amount = 1, bool append = false) {
    if (!append || equip_id == null) {
      validator.empty_multis(implement);

      if (equip_id == null) return;
    }

    List<EquipData> m = validator.multis[implement];

    for (int i = 0; i < amount; i++) {
      m.Add(equips_loader.equips[equip_id]);
    }
  }

  void set_multiremove(string equip_id, string implement, int amount) {
    int removed = 0;
    for (int i = validator.multis[implement].Count - 1; i >= 0 && removed < amount; i--) {
      if (validator.multis[implement][i].key == equip_id) {
        validator.multis[implement].RemoveAt(i);
        removed++;
      }
    }
  }

  void set_clear() {
    validator.unequip_all();
  }

  void validate() {
    List<string> invalids = anatomy.equals(validator);

    foreach (string invalid_implement in invalids) {
      string test = anatomy.anatomy[invalid_implement] == null ? "Empty" : anatomy.anatomy[invalid_implement].name;
      string expectation = validator.anatomy[invalid_implement] == null ? "Empty" : validator.anatomy[invalid_implement].name;

      if (!anatomy.multis.ContainsKey(invalid_implement)) {
        Debug.Log("Anatomy: " + invalid_implement + " - Expected " + expectation + " | Got " + test);
      } else {
        string exp_s = "";
        string got_s = "";
        foreach (EquipData e in validator.multis[invalid_implement]) {
          exp_s += e.name + "\n";
        }

        foreach (EquipData e in anatomy.multis[invalid_implement]) {
          got_s += e.name + "\n";
        }

        Debug.Log("Anatomy: " + invalid_implement + "\n" + 
                  "Expected (" + validator.multis[invalid_implement].Count.ToString() + "): \n" + exp_s + "\n" +
                  "Got (" + anatomy.multis[invalid_implement].Count.ToString() + "): \n" + got_s);
      }
    }

    if (invalids.Count > 0) {
      list();
    } else {
      test_passed++;
    }
  }
}
