using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipsLoader : MonoBehaviour {
  public Dictionary<string, EquipData> equips;

	// Use this for initialization
	void Start () {
    string raw = System.IO.File.ReadAllText("Assets/Sheets/Equipments.tsv");
    process(raw);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public Dictionary<string, EquipData> get_equips() {
    return equips;
  }

  Dictionary<string, EquipData> process(string raw) {
    string[] lines = raw.Split("\n"[0]);
    equips = new Dictionary<string, EquipData>();

    //foreach (string line in lines) {
    for (int i = 1; i < lines.Length; i++) {
      string line = lines[i];
      string[] cell = (line.Trim()).Split("\t"[0]);
      string key = cell[0];
      string type = cell[1];
      string e_class = cell[2];
      string name = cell[3];
      float weight;

      if (!float.TryParse(cell[4], out weight)) weight = 0f;
      string slot = cell[5];
      string[] attributes = (cell[6].Trim()).Split(',');

      EquipData equip_data = process_attributes(attributes);
      equip_data.set_slot(slot);
      equip_data.key = key;
      equip_data.type = type;
      equip_data.e_class = e_class;
      equip_data.name = name;
      equip_data.weight = weight;

      equips.Add(key, equip_data);
    }

    return equips;
  }

  EquipData process_attributes(string[] atbs) {
    EquipData equip_data = new EquipData();
    foreach (string atb in atbs) {
      string[] tuple = (atb.Trim()).Split(':');
      try {
        equip_data.set_property(tuple[0].Trim());
      } catch (System.ArgumentException) {
        float f;
        if (float.TryParse(tuple[1].Trim(), out f)) {
          equip_data.set_property(tuple[0].Trim(), f);
        } else {
          throw new System.ArgumentException("Failed to process EquipData argument " + atb);
        }
      }
    }

    return equip_data;
  }
}

public class EquipData {
  public float base_damage = 0;
  public float armor_point = 0;
  public float durability = 0;
  public float block_chance = 0;

  public int arrow_capacity = 0;
  public int bullet_capacity = 0;
  public int flask_capacity = 0;
  public int ammo_chamber = 0;

  public float belt_capacity = 0;

  public float armor_pierce_chance = 0;
  public float armor_bypass_chance = 0;

  public float range = 1;

  public float draw_weight = 0;

  public bool is_sharp = false;
  public bool is_blunt = false;
  public bool is_ranged = false;
  public bool is_melee = false;
  public bool is_gunpowder = false;
  public bool is_mounted = false;
  public bool is_baggable = false;
  public bool is_axe = false;
  public bool is_maul = false;
  public bool is_spear = false;
  public bool is_sword = false;
  public bool is_oversize = false;
  public bool is_claw = false;
  public bool is_bow = false;
  public bool is_crossbow = false;
  public bool is_rifled = false;
  public bool is_handcannon = false;
  public bool is_whip = false;
  public bool is_holy_book = false;
  public bool is_mace = false;
  public bool is_scepter = false;
  public bool is_dagger = false;
  public bool is_throwable = false;
  public bool is_dedicated_throwable = false;

  public bool holster_any_weapons = false;
  public bool holster_not_oversize = false;

  public bool holster_axe = false;
  public bool holster_bow = false;
  public bool holster_claw = false;
  public bool holster_crossbow = false;
  public bool holster_dagger = false;
  public bool holster_gunpowder = false;
  public bool holster_holy_book = false;
  public bool holster_mace = false;
  public bool holster_maul = false;
  public bool holster_mounted = false;
  public bool holster_scepter = false;
  public bool holster_spear = false;
  public bool holster_sword = false;
  public bool holster_whip = false;

  public string key;
  public string name;
  public string type;
  public string e_class;
  public float weight;
  public SlottableAnatomy.Slot slot;

  public EquipData() { }
  public EquipData(string _key, string _name, string _type, string _e_class, float _weight) {
    key = _key;
    name = _name;
    type = _type;
    e_class = _e_class;
    weight = _weight;
  }

  public void set_slot(string _slot) {
    switch (_slot) {
      case "body": slot = SlottableAnatomy.Slot.body; break;
      case "slingback": slot = SlottableAnatomy.Slot.slingback; break;
      case "quiver": slot = SlottableAnatomy.Slot.quiver; break;
      case "belt": slot = SlottableAnatomy.Slot.belt; break;
      case "belt_toolkit": slot = SlottableAnatomy.Slot.belt_toolkit; break;
      case "gauntlet": slot = SlottableAnatomy.Slot.gauntlet; break;
      case "arm": slot = SlottableAnatomy.Slot.arm; break;
      case "arm_dual": slot = SlottableAnatomy.Slot.arm_dual; break;
      case "armbelt": slot = SlottableAnatomy.Slot.armbelt; break;
      case "greaves": slot = SlottableAnatomy.Slot.greaves; break;
      case "helmet": slot = SlottableAnatomy.Slot.helmet; break;
      case "hipbelt": slot = SlottableAnatomy.Slot.hipbelt; break;
      default: throw new System.ArgumentException("Unknown Anatomy Slot " + _slot);
    }
  }

  public void set_property(string p, float v) {
    switch (p) {
      case "armor_point": armor_point = v; break;
      case "durability": durability = v; break;
      case "block_rate": block_chance = v; break;
      case "arrow_capacity": arrow_capacity = (int) v; break;
      case "belt_capacity": belt_capacity = v; break;
      case "bullet_capacity": bullet_capacity = (int) v; break;
      case "flask_capacity": flask_capacity = (int)v; break;
      case "armor_pierce": armor_pierce_chance = v; break;
      case "armor_bypass": armor_bypass_chance = v; break;
      case "range": range = (int)v; break;
      case "draw_weight": draw_weight = v; break;
      case "ammo_chamber": ammo_chamber = (int)v; break;
      case "base_damage": base_damage = v; break;
      default: throw new System.ArgumentException("Unknown property " + p);
    }
  }

  public void set_property(string p) {
    switch (p) {
      case "is_sharp": is_sharp = true; break;
      case "is_blunt": is_blunt = true; break;
      case "is_gunpowder": is_gunpowder = true; break;
      case "is_mounted": is_mounted = true; break;
      case "is_baggable": is_baggable = true; break;
      case "is_melee": is_melee = true; break;
      case "is_axe": is_axe = true; break;
      case "is_sword": is_sword = true; break;
      case "is_oversize": is_oversize = true; break;
      case "is_ranged": is_ranged = true; break;
      case "is_bow": is_bow = true; break;
      case "is_crossbow": is_crossbow = true; break;
      case "is_rifled": is_rifled = true; break;
      case "is_spear": is_spear = true; break;
      case "is_handcannon": is_handcannon = true; break;
      case "is_maul": is_maul = true; break;
      case "is_whip": is_whip = true; break;
      case "is_holy_book": is_holy_book = true; break;
      case "is_scepter": is_scepter = true; break;
      case "is_mace": is_mace = true; break;
      case "is_claw": is_claw = true; break;
      case "is_dagger": is_dagger = true; break;
      case "is_throwable": is_throwable = true; break;
      case "is_dedicated_throwable": is_dedicated_throwable = true; break;
      case "holster_any_weapons": holster_any_weapons = true; break;
      case "holster_dagger": holster_dagger = true; break;
      case "holster_sword": holster_sword = true; break;
      case "holster_axe": holster_axe = true; break;
      case "holster_crossbow": holster_crossbow = true; break;
      case "holster_bow": holster_bow = true; break;
      case "holster_mace": holster_mace = true; break;
      case "holster_gunpowder": holster_gunpowder = true; break;
      case "holster_holy_book": holster_holy_book = true; break;
      case "holster_scepter": holster_scepter = true; break;
      case "holster_not_oversize": holster_not_oversize = true; break;
      case "holster_whip": holster_whip = true; break;
      default: throw new System.ArgumentException("Unknown toggle property " + p);
    }
  }
}