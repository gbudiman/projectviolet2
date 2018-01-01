using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlottableAnatomy : MonoBehaviour {
  public enum Slot { body, slingback, quiver, belt, belt_toolkit,
                     gauntlet, arm, arm_dual, armbelt,
                     greaves,
                     helmet, hip, hipbag }
  public Dictionary<string, EquipData> anatomy;
  public Dictionary<string, List<EquipData>> multis;
  UnitActor parent_actor;
  
	// Use this for initialization
	void Start () {
    anatomy = new Dictionary<string, EquipData>() {
      { "body", null },
      { "slingback_main", null },
      { "slingback_off", null },
      { "quiver_main", null },
      { "quiver_off", null },
      { "belt", null },
      { "belt_toolkit", null },
      { "gauntlet_main", null },
      { "gauntlet_off", null },
      { "arm_main", null },
      { "arm_off", null },
      { "arm_dual", null },
      { "armbelt_main", null },
      { "armbelt_off", null },
      { "armholster_main", null },
      { "armholster_off", null },
      { "greaves_main", null },
      { "greaves_off", null },
      { "greavesholster_main", null },
      { "greavesholster_off", null },
      { "helmet", null },
      { "hip", null },
      { "hipbag", null },
    };

    multis = new Dictionary<string, List<EquipData>>() {
      { "quiver_main", new List<EquipData>() },
      { "quiver_off", new List<EquipData>() },
      { "belt_toolkit", new List<EquipData>() },
      { "armholster_main", new List<EquipData>() },
      { "armholster_off", new List<EquipData>() },
      { "greavesholster_main", new List<EquipData>() },
      { "greavesholster_off", new List<EquipData>() },
    };
	}

  public void set_parent_actor(UnitActor parent) {
    parent_actor = parent;
  }

  public void unequip_all() {
    foreach (string k in anatomy.Keys) {
      anatomy[k] = null;
    }

    foreach (string k in multis.Keys) {
      multis[k] = new List<EquipData>();
    }
  }

  public List<string> equals(SlottableAnatomy other_anatomy) {
    List<string> mismatch = new List<string>();

    foreach (KeyValuePair<string, EquipData> a in anatomy) {
      EquipData mine = a.Value;
      EquipData other = other_anatomy.anatomy[a.Key];
      if (mine == null && other == null) continue;
      if (mine == null && other != null || mine != null && other == null || mine.key != other.key) {
        mismatch.Add(a.Key);
      }
    }

    foreach (KeyValuePair<string, List<EquipData>> multi in multis) {
      List<EquipData> mine = multi.Value;
      List<EquipData> other = other_anatomy.multis[multi.Key];

      if (mine.Count != other.Count) {
        mismatch.Add(multi.Key);
        break;
      }

      List<string> my_names = new List<string>();
      List<string> other_names = new List<string>();

      foreach (EquipData x in mine) {
        my_names.Add(x.key);
      }

      foreach (EquipData x in other) {
        other_names.Add(x.key);
      }

      my_names.Sort();
      other_names.Sort();
      for (int i = 0; i < mine.Count; i++) {
        if (my_names[i] != other_names[i]) {
          mismatch.Add(multi.Key);
          break;
        }
      }
      
    }

    return mismatch;
  }

  // Update is called once per frame
  void Update() {
 
  }

  public void load_multis(EquipData equip_data, int amount, bool mainside = true) {
    switch (equip_data.slot) {
      case Slot.quiver:
        string source = mainside ? "slingback_main" : "slingback_off";
        string target = mainside ? "quiver_main" : "quiver_off";
        int maxes = get_capacity(source, "arrow_capacity");

        

        if (maxes > 0) {
          bool overflow = false;
          for (int i = 0; i < amount; i++) {
            if (multis[target].Count >= maxes) {
              Debug.Log("Overflow " + (amount - i).ToString() + " " + equip_data.name);
              overflow = true;
              break;
            }
            multis[target].Add(equip_data);
          }

          if (overflow) {
          } else {
            Debug.Log("Loaded " + amount + " " + equip_data.name + " to " + target);
          }
        } else {
          Debug.Log("No available Quiver to store " + amount + " " + equip_data.name);
        }
        break;
    }
  }

  public void empty_multis(string implement) {
    multis[implement] = new List<EquipData>();
  }

  public bool take_from_multis(string equip_id, int amount) {
    bool has_enough = false;
    int count = 0;
    List<string> records = new List<string>();

    foreach (KeyValuePair<string, List<EquipData>> p in multis) {
      string multi = p.Key;
      foreach (EquipData eq in p.Value) {
        if (eq.key == equip_id) {
          count++;
          records.Add(multi);
        }
      }

      has_enough = count > amount;
      if (has_enough) break;
    }

    if (has_enough) {
      int removed = 0;
      foreach (string record in records) {
        remove_one_from_multi(equip_id, record);
        removed++;

        if (removed == amount) break;
      }
    }

    return has_enough;
  }

  void remove_one_from_multi(string equip_id, string implement) {
    for (int i = 0; i < multis[implement].Count; i++) { 
      if (equip_id == multis[implement][i].key) {
        multis[implement].RemoveAt(i);
        break;
      }
    }
  }

  public int get_capacity(string implement, string kind) {
    if (anatomy[implement] == null) return 0;
    switch (kind) {
      case "arrow_capacity": return anatomy[implement].arrow_capacity;
      default: throw new System.ArgumentException("Unknown multis " + implement);
    }
  }

  public void swap_arms() {
    EquipData temp = anatomy["arm_main"];
    anatomy["arm_main"] = anatomy["arm_off"];
    anatomy["arm_off"] = temp;
  }

  public void equip(EquipData equip_data, bool mainside = true) {
    string target = null;
    switch (equip_data.slot) {
      case Slot.arm_dual: unequip("arm_main"); unequip("arm_off"); target = "arm_dual"; break;
      case Slot.arm:
        unequip("arm_dual");
        if (!parent_actor.has_tech("dual_wield")) {
          // If Actor cannot dual wield weapon
          // Unequip any weapons from both arms
          unequip_if_weapon("arm_main");
          unequip_if_weapon("arm_off");
        }
        target = mainside ? "arm_main" : "arm_off";

        break;
      case Slot.body: target = "body"; break;
      case Slot.slingback: target = mainside ? "slingback_main" : "slingback_off"; break;
      case Slot.quiver: target = mainside ? "quiver_main" : "quiver_off"; break;
      case Slot.belt: target = "belt"; break;
      case Slot.belt_toolkit: target = "belt_toolkit"; break;
      case Slot.gauntlet: target = mainside ? "gauntlet_main" : "gauntlet_off"; break;
      case Slot.armbelt: target = mainside ? "armbelt_main" : "armbelt_off"; break;
      case Slot.greaves: target = mainside ? "greaves_main" : "greaves_off"; break;
      case Slot.helmet: target = "helmet"; break;
      case Slot.hip: target = "hip"; break;
      case Slot.hipbag: target = "hipbag"; break;
      default: throw new System.ArgumentException("Don't know how to equip " + equip_data.name + " to " + equip_data.slot);
    }

    unequip(target); _equip(equip_data, target);
  }

  void _equip(EquipData equip_data, string implement) {
    anatomy[implement] = equip_data;
    Debug.Log("Equipped " + equip_data.name + " to " + implement);
  }

  public void list_equipments() {
    string s = "";
    foreach (KeyValuePair<string, EquipData> eq in anatomy) {
      s += eq.Key + ": " + (eq.Value == null ? "Empty" : eq.Value.name) + "\n";
    }

    foreach (KeyValuePair<string, List<EquipData>> ms in multis) {
      s += ms.Key + "\n";
      foreach (EquipData a in ms.Value) {
        s += "  " + a.name + "\n";
      }
    }

    Debug.Log(s);
  }

  public EquipData unequip(string implement) {
    EquipData uneq = anatomy[implement];
    anatomy[implement] = null;

    if (uneq != null) Debug.Log("Unequipped " + uneq.name + " from " + implement);
    return uneq;
  }

  void unequip_if_weapon(string implement) {
    EquipData uneq = anatomy[implement];

    if (uneq != null && uneq.type == "Weapon") {
      anatomy[implement] = null;
    }
  }
}
