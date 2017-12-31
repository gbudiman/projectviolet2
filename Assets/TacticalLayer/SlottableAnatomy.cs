﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlottableAnatomy : MonoBehaviour {
  public enum Slot { body, slingback, quiver, belt, belt_toolkit,
                     gauntlet, arm, arm_dual, armbelt,
                     greaves,
                     helmet, hip, hipbag }
  Dictionary<string, EquipData> anatomy;
  Dictionary<string, List<EquipData>> multis;

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
      { "quiver", new List<EquipData>() },
      { "belt_toolkit", new List<EquipData>() },
      { "armholster_main", new List<EquipData>() },
      { "armholster_off", new List<EquipData>() },
      { "quiver_main", new List<EquipData>() },
      { "quiver_off", new List<EquipData>() },
      { "greavesholster_main", new List<EquipData>() },
      { "greavesholster_off", new List<EquipData>() },
    };
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
          for (int i = 0; i < amount; i++) {
            multis[target].Add(equip_data);
          }
        }
        break;
    }
  }

  public int get_capacity(string implement, string kind) {
    if (anatomy[implement] == null) return 0;
    switch (kind) {
      case "arrow_capacity": return anatomy[implement].arrow_capacity;
      default: throw new System.ArgumentException("Unknown multis " + implement);
    }
  }

  public void equip(EquipData equip_data, bool mainside = true) {
    string target = null;
    switch (equip_data.slot) {
      case Slot.arm_dual: unequip("arm_main"); unequip("arm_off"); target = "arm_dual"; break;
      case Slot.arm: unequip("arm_dual"); target = mainside ? "arm_main" : "arm_off"; break;
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
}
