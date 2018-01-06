using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour {
  public enum UnitStat {dodge, acc,
                        patk, ratk, matk,
                        pdef, mdef,
                        aspd,
                        hp_max };
  UnitActor actor;

  public int level;
  public int strength;
  public int strength_plus;
  public int vitality;
  public int vitality_plus;
  public int agility;
  public int agility_plus;
  public int dexterity;
  public int dexterity_plus;
  public int intellect;
  public int intellect_plus;
  public int faith;
  public int faith_plus;

  public float dodge;
  public float accuraccy;
  public float physical_attack;
  public float magical_attack;
  public float physical_defense;
  public float magical_defense;
  public float ap_recharge_rate; public float ap_max;
  public float weight;

  public float hp; public float hp_max;
  public float mp; public float mp_max; // Mana - Elementalist
  public int sp; public int sp_max;     // Soul - Necromancer
  public int pp; public int pp_max;     // Prayer - Priest
  public float gp;                      // Gestalt - Geomancer
  public int ip; public int ip_max;     // Impulse - Acrobat
  public int summoned_orb;              // Summoned Orb - this Actor's own orb
  public int summoned_orb_max;
  public int charged_orb;               // Charged Orb - enemies putting orb to this Actor
  public int limit_break; public int limit_break_max; // Limit Break - Sentry
  public float trance; public float trance_max;       // Trance - Reaver

  public bool is_concealed;

  const float STR_EXP = 2.2f;
  const float STR_CONST = 50f;
  const float STR_MULT = 1.2f;

	// Use this for initialization
	void Start () {
    level = 1;
    ap_max = 100f;
    strength = 1; strength_plus = 0;
    vitality = 1; vitality_plus = 0;
    agility = 1;  agility_plus = 0;
    dexterity = 1; dexterity_plus = 0;
    intellect = 1; intellect_plus = 0;
    faith = 1;    faith_plus = 0;
    weight = 0;
    hp = 0;

    
  }

  public void recompute_all() {
    //compute(UnitStat.acc);
    //compute(UnitStat.dodge);
    compute(UnitStat.patk);
    compute(UnitStat.ratk);
    compute(UnitStat.matk);
    compute(UnitStat.pdef);
    compute(UnitStat.mdef);
    compute(UnitStat.aspd);
    compute(UnitStat.hp_max);
    fill_hp();
  }

  public void set_parent_actor(UnitActor _p) {
    actor = _p;
  }

  void fill_hp(float pct=1f) {
    hp = pct * hp_max;
  }

  void compute(UnitStat stat) {
    switch(stat) {
      case UnitStat.patk: break;
      case UnitStat.ratk: break;
      case UnitStat.matk: break;
      case UnitStat.pdef: break;
      case UnitStat.mdef: break;
      case UnitStat.aspd: break;
      case UnitStat.hp_max:
        hp_max = level * STR_CONST * STR_MULT + Mathf.Pow(vitality, STR_EXP);
        break;
      default: throw new System.ArgumentException("Unknown stat to compute: " + stat);
    }
  }
	
	// Update is called once per frame
	void Update () {
		
	}
}