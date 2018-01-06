using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour {
  UnitActor actor;
  const float DEX_CA = 250;
  const float DEX_CB = 250;
  const float RANGE_MOD_CLOSE = 1.5f;
  const float RANGE_MOD_FAR = 2f;
  const bool IS_MELEE = true;
  const bool IS_RANGED = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void set_parent_actor(UnitActor _p) {
    actor = _p;
  }

  public void deliver_to(UnitActor other, string action) {
    switch(action) {
      case "marksmanship_double_tap":
        EquipData eq = actor.check_has_equipment_with_attributes("is_bow");

        if (eq != null) {
          List<EquipData> ammo = actor.use_ammo("arrow_iron");
          if (ammo.Count >= 1) {
            if (is_strictly_hit(other, IS_RANGED, get_delta_range(eq, other))) {
              float damage = eq.base_damage + ammo[0].base_damage;

              if (actor.has_tech("marksmanship_bow")) {
                damage = Mathf.Pow(damage, (DEX_CA + actor.stats.dexterity) / DEX_CB);
              }

              Debug.Log("Damage = " + damage);
            } else {
              Debug.Log("Missed!");
            }
          } else {
            throw new System.InvalidOperationException("Actor does not have available arrows");
          }
        } else {
          throw new System.InvalidOperationException("Actor does not have bow equipment");
        }
        break;
    }
  }

  float get_delta_range(EquipData eq, UnitActor other) {
    // Negative value = less than weapon's max range = bonus accuracy
    return get_range_from(other) - eq.range;
  }

  float get_range_from(UnitActor other) {
    return Vector3.Distance(actor.transform.position, other.transform.position);
  }

  bool is_strictly_hit(UnitActor other, bool is_melee = true, float range = 0) {
    return get_hit_miss_info(other, is_melee, range).is_hit;
  }

  HitMissInfo get_hit_miss_info(UnitActor other, bool is_melee = true, float range = 0) {
    
    Stats actor_stats = actor.stats;
    Stats other_stats = other.stats;
    float attacker_accuracy = actor_stats.dexterity + actor_stats.dexterity_plus + actor_stats.level;
    float other_dodge = other_stats.agility + other_stats.agility_plus + other_stats.level;
    float copy_of_attacker_accuracy = attacker_accuracy;
    

    RangePenaltyInfo range_penalty_info = new RangePenaltyInfo(attacker_accuracy);

    if (!is_melee) {
      range_penalty_info = accuracy_range_modifier(attacker_accuracy, range);
      attacker_accuracy = range_penalty_info.accuracy; //accuracy_range_modifier(attacker_accuracy, range);
      
    }

    HitMissInfo roll = roll_hit_dice(attacker_accuracy, other_dodge);
    roll.attacker_accuracy = copy_of_attacker_accuracy;
    roll.other_dodge = other_dodge;
    roll.range_penalty_info = range_penalty_info;

    Debug.Log("Accuracy = " + roll.attacker_accuracy + " | Dodge = " + roll.other_dodge);
    Debug.Log("Range Bonus: " + (-roll.range_penalty_info.penalty * 100) + "%");
    Debug.Log("Hit Chance: " + roll.reported_hit_chance * 100 + "%");
    return roll;
  }

  RangePenaltyInfo accuracy_range_modifier(float accuracy, float range) {
    if (range > 0) {
      // penalize because too far
      //return accuracy + Mathf.Pow(range, RANGE_MOD_FAR);
      float penalty = Mathf.Pow(range, RANGE_MOD_FAR) / 100;
      return new RangePenaltyInfo(accuracy - accuracy * penalty, penalty);
    } else {
      float penalty = Mathf.Pow(Mathf.Abs(range), RANGE_MOD_CLOSE) / 100;
      return new RangePenaltyInfo(accuracy + accuracy * penalty, -penalty);
    }
  }

  HitMissInfo roll_hit_dice(float accuracy, float dodge) {
    float rand = Random.value;
    float hit_threshold = Mathf.Clamp(accuracy / dodge, 0, 999);

    if (hit_threshold > rand) {
      // strike lands on target;
      return new HitMissInfo(true, hit_threshold, 1f);
    } else {
      // miss
      float suppressed = hit_threshold / 2;
      if (suppressed < 0.1f) return new HitMissInfo(false, hit_threshold, 0f);

      //return suppressed;
      return new HitMissInfo(false, hit_threshold, suppressed);
    }

  }
}

public class RangePenaltyInfo {
  public float accuracy;
  public float penalty;

  public RangePenaltyInfo(float _a) {
    accuracy = _a;
    penalty = 0;
  }

  public RangePenaltyInfo(float _a, float _p) {
    accuracy = _a;
    penalty = _p;
  }
}

public class HitMissInfo {
  public bool is_hit;
  float raw_hit_chance;
  public float reported_hit_chance;
  public float damage_factor;
  public float attacker_accuracy;
  public float other_dodge;
  public RangePenaltyInfo range_penalty_info;

  public HitMissInfo(bool _is_hit, float hit_chance, float _factor) {
    is_hit = _is_hit;
    raw_hit_chance = hit_chance;
    damage_factor = _factor;

    reported_hit_chance = raw_hit_chance;
  }
}