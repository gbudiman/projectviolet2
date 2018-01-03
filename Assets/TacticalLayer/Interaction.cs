using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour {
  UnitActor actor;
  const float DEX_CA = 250;
  const float DEX_CB = 250;
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
            float damage = eq.base_damage + ammo[0].base_damage;

            if (actor.has_tech("marksmanship_bow")) {
              damage = Mathf.Pow(damage, (DEX_CA + actor.stats.dexterity) / DEX_CB);
            }

            Debug.Log("Damage = " + damage);
          } else {
            throw new System.InvalidOperationException("Actor does not have available arrows");
          }
        } else {
          throw new System.InvalidOperationException("Actor does not have bow equipment");
        }
        break;
    }
  }
}
