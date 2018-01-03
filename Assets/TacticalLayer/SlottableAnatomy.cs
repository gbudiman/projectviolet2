using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlottableAnatomy : MonoBehaviour {
  public enum Slot { body, slingback, quiver, belt, belt_toolkit,
                     gauntlet, arm, arm_dual, armbelt,
                     greaves,
                     helmet, hipbelt }
  public Dictionary<string, EquipData> anatomy;
  public Dictionary<string, List<EquipData>> multis;
  Dictionary<string, string> holsterables;
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
      { "holster_slingback_main", null },
      { "holster_slingback_off", null },
      { "helmet", null },
      { "hipbelt_main", null },
      { "hipbelt_off", null },
      { "hipholster_main", null },
      { "hipholster_off", null },
    };

    // These are slots that can take multiple items
    multis = new Dictionary<string, List<EquipData>>() {
      { "quiver_main", new List<EquipData>() },
      { "quiver_off", new List<EquipData>() },
      { "belt_toolkit", new List<EquipData>() },
      { "armmunition_main", new List<EquipData>() },
      { "armmunition_off", new List<EquipData>() },
    };

    holsterables = new Dictionary<string, string>() {
      {"slingback_main", "holster_slingback_main" },
      {"slingback_off", "holster_slingback_off"},
      {"armbelt_main", "armholster_main" },
      {"armbelt_off", "armholster_off"},
      {"greaves_main", "greavesholster_main" },
      {"greaves_off", "greavesholster_off"},
      {"hipbelt_main", "hipholster_main" },
      {"hipbelt_off", "hipholster_off"}
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
        string source_alt = mainside ? "slingback_off" : "slingback_main";
        string target = mainside ? "quiver_main" : "quiver_off";
        string target_alt = mainside ? "quiver_off" : "quiver_main";
        
        int maxes = get_capacity(source, "arrow_capacity");
        int alternate_maxes = get_capacity(source_alt, "arrow_capacity");
        

        if (maxes > 0) {
          bool overflow = false;
          int remainder = 0;
          for (int i = 0; i < amount; i++) {
            if (multis[target].Count >= maxes) {
              remainder = amount - i;
              Debug.Log("Overflow " + remainder + " " + equip_data.name);
              overflow = true;
              break;
            }
            multis[target].Add(equip_data);
          }

          if (overflow && alternate_maxes > 0) {
            Debug.Log("Found alternate quiver for overflow");
            for (int i = 0; i < remainder; i++) {
              if (multis[target_alt].Count >= alternate_maxes) {
                Debug.Log("Overflow " + (remainder - i).ToString() + " " + equip_data.name);
                break;
              }
              multis[target_alt].Add(equip_data);
            }
          } else {
            Debug.Log("Loaded " + (amount - remainder).ToString() + " " + equip_data.name + " to " + target);
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

  public List<EquipData> take_from_multis(string equip_id, int amount = 1, bool test_only = false) {
    bool has_enough = false;
    int count = 0;
    List<string> records = new List<string>();
    List<EquipData> items = new List<EquipData>();

    foreach (KeyValuePair<string, List<EquipData>> p in multis) {
      string multi = p.Key;
      foreach (EquipData eq in p.Value) {
        if (eq.key == equip_id) {
          count++;
          records.Add(multi);
          items.Add(eq);
        }
      }

      has_enough = count > amount;
      if (has_enough) break;
    }

    if (has_enough && !test_only) {
      int removed = 0;
      foreach (string record in records) {
        remove_one_from_multi(equip_id, record);
        removed++;

        if (removed == amount) break;
      }
    }

    return items;
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

  public EquipData check_has_equipment_with_attributes(string attb) {
    bool is_true = false;

    foreach (EquipData eq in anatomy.Values) {
      if (eq == null) continue;
      switch (attb) {
        case "is_sharp": is_true |= eq.is_sharp;  break;
        case "is_blunt": is_true |= eq.is_blunt; break;
        case "is_gunpowder": is_true |= eq.is_gunpowder; break;
        case "is_mounted": is_true |= eq.is_mounted; break;
        case "is_baggable": is_true |= eq.is_baggable; break;
        case "is_melee": is_true |= eq.is_melee; break;
        case "is_axe": is_true |= eq.is_axe; break;
        case "is_sword": is_true |= eq.is_sword; break;
        case "is_oversize": is_true |= eq.is_oversize; break;
        case "is_ranged": is_true |= eq.is_ranged; break;
        case "is_bow": is_true |= eq.is_bow; break;
        case "is_crossbow": is_true |= eq.is_crossbow; break;
        case "is_rifled": is_true |= eq.is_rifled; break;
        case "is_spear": is_true |= eq.is_spear; break;
        case "is_handcannon": is_true |= eq.is_handcannon; break;
        case "is_maul": is_true |= eq.is_maul; break;
        case "is_whip": is_true |= eq.is_whip; break;
        case "is_holy_book": is_true |= eq.is_holy_book; break;
        case "is_scepter": is_true |= eq.is_scepter; break;
        case "is_mace": is_true |= eq.is_mace; break;
        case "is_claw": is_true |= eq.is_claw; break;
        case "is_dagger": is_true |= eq.is_dagger; break;
        case "is_throwable": is_true |= eq.is_throwable; break;
        case "is_dedicated_throwable": is_true |= eq.is_dedicated_throwable; break;
        default: throw new System.ArgumentException("Unknown equipment attribute " + attb);
      }

      if (is_true) {
        return eq;
      }
    }

    return null;
  }

  public bool check_has_equipment_with_attributes(List<string> attributes) {
    bool is_true = false;

    foreach (string attb in attributes) {
      is_true |= (check_has_equipment_with_attributes(attb) != null);
    }

    return is_true;
  }

  public void swap_arms() {
    EquipData temp = anatomy["arm_main"];
    anatomy["arm_main"] = anatomy["arm_off"];
    anatomy["arm_off"] = temp;
  }

  public void equip_holstered(EquipData equip_data) {
    bool success = false;
    foreach (KeyValuePair<string, string> k in holsterables) {
      string body_slot = k.Key;
      string attachment = k.Value;

      if (anatomy[body_slot] == null) continue;
      EquipData slot_eq = anatomy[body_slot];

      if (can_holster(slot_eq, equip_data) && anatomy[attachment] == null) {
        Debug.Log("Holster slot found at " + body_slot + ": " + slot_eq.name);
        anatomy[attachment] = equip_data;
        success = true;
        break;
      }
    }

    if (!success) {
      Debug.Log("Unable to find holsterable slot for " + equip_data.name);
    }
  }

  bool can_holster(EquipData slot, EquipData item) {
    if (slot.holster_any_weapons && item.type == "Weapon") return true;
    if (slot.holster_not_oversize && item.is_oversize) return false;

    if (item.is_axe) return slot.holster_axe;
    if (item.is_bow) return slot.holster_bow;
    if (item.is_crossbow) return slot.holster_crossbow;
    if (item.is_dagger) return slot.holster_dagger;
    if (item.is_gunpowder) return slot.holster_gunpowder;
    if (item.is_holy_book) return slot.holster_holy_book;
    if (item.is_mace) return slot.holster_mace;
    if (item.is_maul) return slot.holster_maul;
    if (item.is_mounted) return slot.holster_mounted;
    if (item.is_scepter) return slot.holster_scepter;
    if (item.is_spear) return slot.holster_spear;
    if (item.is_sword) return slot.holster_sword;
    if (item.is_whip) return slot.holster_whip;

    return false;
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
      case Slot.hipbelt: target = mainside ? "hipbelt_main" : "hipbelt_off"; break;
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
    if (holsterables.ContainsKey(implement)) {
      string cascade = holsterables[implement];
      anatomy[cascade] = null;
    }
    return uneq;
  }

  void unequip_if_weapon(string implement) {
    EquipData uneq = anatomy[implement];

    if (uneq != null && uneq.type == "Weapon") {
      anatomy[implement] = null;
    }
  }
}
