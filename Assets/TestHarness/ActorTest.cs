using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorTest : MonoBehaviour {
  UnitActor actor;
  EquipsLoader equips_loader;
  SlottableAnatomy anatomy;

  ActionComparator validator;
  int test_count = 0;
  int pass_count = 0;
	// Use this for initialization
	void Start () {
    actor = GetComponent<UnitActor>();
    equips_loader = GetComponent<EquipsLoader>();
    begin_test();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  

  void begin_test() {
    validator = new ActionComparator();
    actor.equip("bow_long");
    actor.confer_tech("marksmanship_bow");
    actor.confer_tech("marksmanship_double_tap");
    validator.add("marksmanship_double_tap");
    validator.enable("attack_melee", false);
    validator.enable("attack_firearm", false);
    validator.enable("throw", false);
    test();

    end_test();
  }

  void test() {
    if (validator.equals(actor.get_actions())) {
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