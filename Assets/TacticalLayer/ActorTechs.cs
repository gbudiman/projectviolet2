using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorTechs : MonoBehaviour {
  public Dictionary<string, bool> techs;
	// Use this for initialization
	void Start () {
    techs = new Dictionary<string, bool>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void assign(string skill_id) {
    techs[skill_id] = true;
  }

  public void hard_remove(string skill_id) {
    techs.Remove(skill_id);
  }

  public void enable(string skill_id, bool val = true) {
    techs[skill_id] = val;
  }

  public bool has_tech(string skill_id) {
    return techs.ContainsKey(skill_id) && techs[skill_id];
  }
}
