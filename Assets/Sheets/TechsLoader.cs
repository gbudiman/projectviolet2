using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechsLoader : MonoBehaviour {
  Dictionary<string, SkillData> techs;

	// Use this for initialization
	void Start () {
    string raw = System.IO.File.ReadAllText("Assets/Sheets/Techs.tsv");
    techs = process(raw);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public Dictionary<string, SkillData> get_techs() {
    return techs;
  }

  Dictionary<string, SkillData> process(string raw) {
    string[] lines = raw.Split("\n"[0]);
    Dictionary<string, SkillData> skills = new Dictionary<string, SkillData>(); 

    foreach (string line in lines) {
      string[] cell = (line.Trim()).Split("\t"[0]);
      string code_s = cell[0];
      float code = -1f;

      if (!float.TryParse(code_s, out code)) continue;
      if (code < 0f) continue;
      string skill_key = cell[1];
      string skill_name = cell[2];
      string activation = cell[3];
      string target = cell[4];
      string requirements = cell[5];
      string description = cell[6];

      SkillData skill_data = new SkillData(skill_key, skill_name, activation, target, requirements, description);
      skills.Add(skill_key, skill_data);
    }

    return skills;
  }
}

public class SkillData {
  public string skill_key;
  public string skill_name;
  public string activation;
  public string target;
  public string requirements;
  public string description;

  public SkillData(string _skill_key, 
                   string _skill_name, 
                   string _activation, 
                   string _target, 
                   string _requirements, 
                   string _description) {
    skill_key = _skill_key;
    skill_name = _skill_name;
    activation = _activation;
    target = _target;
    requirements = _requirements;
    description = _description;
  }
}