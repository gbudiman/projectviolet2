using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorTest : MonoBehaviour {
  EquipsLoader equips_loader;
  TechsLoader techs_loader;
  SlottableAnatomy anatomy;

  ActionComparator validator;

  UnitActor actor_a;
  UnitActor actor_b;
  int test_count = 0;
  int pass_count = 0;
	// Use this for initialization
	void Start () {
    equips_loader = GetComponent<EquipsLoader>();
    techs_loader = GetComponent<TechsLoader>();

    actor_a = GameObject.Find("actor_a").GetComponent<UnitActor>();
    actor_b = GameObject.Find("actor_b").GetComponent<UnitActor>();

    actor_a.assign_item_loader(equips_loader.get_equips());
    actor_a.assign_tech_loader(techs_loader.get_techs());
    actor_b.assign_item_loader(equips_loader.get_equips());
    actor_b.assign_tech_loader(techs_loader.get_techs());

    begin_test();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  

  void begin_test() {
    validator = new ActionComparator();
    actor_a.set_level(30);
    actor_a.set_six_stats(1, 1, 10, 50, 1, 1);
    actor_a.stats_recompute_all();
    actor_a.equip("bow_long");
    actor_a.equip("quiver_slingback");
    actor_a.load_multis("arrow_iron", 5);

    actor_a.confer_tech("marksmanship_bow");
    actor_a.confer_tech("marksmanship_double_tap");

    actor_b.set_level(1);
    actor_b.stats.agility = 99;
    actor_b.stats_recompute_all();

    validator.add("marksmanship_double_tap");
    validator.enable("attack_melee", false);
    validator.enable("attack_firearm", false);
    validator.enable("throw", false);

    actor_a.deliver_to(actor_b, "marksmanship_double_tap");
    test();

    end_test();
  }

  void test() {
    if (validator.equals(actor_a.get_actions())) {
      pass_count++;
    }

    test_count++;
  }

  void end_test() {
    Debug.Log(pass_count + "/" + test_count + " tests passed");
  }
}

public class ActionComparator {
  Dictionary<string, bool> actions;

  public ActionComparator() {
    actions = new Dictionary<string, bool>();
    preinit_expectation();
  }

  void preinit_expectation() {
    actions = new Dictionary<string, bool>();
    List<string> gts = new List<string>() { { "move"}, { "wait"},
                                            { "attack_melee"}, { "attack_bow"}, {"attack_firearm" }, {"throw" } };

    foreach (string gt in gts) {
      actions[gt] = true;
    }
  }

  public void add(string s) {
    actions.Add(s, true);
  }

  public void enable(string s, bool val = true) {
    actions[s] = val;
  }

  public bool equals(Dictionary<string, bool> other) {
    bool is_equal = true;

    if (actions.Count != other.Count) {
      List<string> missing_in_others = new List<string>();
      List<string> missing_in_self = new List<string>();

      foreach (string s in actions.Keys) {
        if (!other.ContainsKey(s)) {
          missing_in_others.Add(s);
        }
      }

      foreach (string s in other.Keys) {
        if (!actions.ContainsKey(s)) {
          missing_in_self.Add(s);
        }
      }

      if (missing_in_others.Count > 0) {
        is_equal = false;
        Debug.Log("Missing in Test: \n" + string.Join("\n", missing_in_others.ToArray()));
      }

      if (missing_in_self.Count > 0) {
        is_equal = false;
        Debug.Log("Extra entry in Test: \n" + string.Join("\n", missing_in_self.ToArray()));
      }

      return is_equal;
    } else {
      List<string> mismatches = new List<string>();
      foreach (KeyValuePair<string, bool> k in actions) {
        if (k.Value != other[k.Key]) {
          List<bool> tuple = new List<bool>() { { other[k.Key] }, { k.Value } };
          mismatches.Add(k.Key + " : " + (other[k.Key] ? "T" : "F") + " | " +  (k.Value ? "T" : "F"));
        }

        
      }

      if (mismatches.Count > 0) {
        is_equal = false;
        Debug.Log("Mismatch entries (Test | Expected):\n" + string.Join("\n", mismatches.ToArray()));
      }

      return is_equal;
    }
  }
}