using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticalMap : MonoBehaviour {
  Dictionary<int, Dictionary<int, Dictionary<int, HexCoord>>> hex_dict;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void spawn_map(int radius = 8) {
    /// <summary>
    /// Spawns empty hex tiles on the tactical map. Size is determined by the <paramref name="radius"/> parameter
    /// <param name="radius">Radius of the tactical map</param>
    /// </summary>

    hex_dict = new Dictionary<int, Dictionary<int, Dictionary<int, HexCoord>>>();

    for (int a = -radius + 1; a < radius; a++) {
      hex_dict.Add(a, new Dictionary<int, Dictionary<int, HexCoord>>());
      for (int b = -radius + 1; b < radius; b++) {
        hex_dict[a].Add(b, new Dictionary<int, HexCoord>());
        int c = -(a + b);

        if (Mathf.Abs(c) > radius) continue;

        GameObject tile = Instantiate(Resources.Load("HexTile")) as GameObject;
        HexCoord hxc = tile.GetComponent<HexCoord>();
        hxc.set_abc(a, b, c);

        hex_dict[a][b].Add(c, hxc);
      }
    }
  }

  public GameObject place_unit(int a, int b, int c) {
    /// <summary>
    /// Place a unit on specified hex tile. Supply ABC coordinates
    /// </summary>
    /// 

    if (a + b + c != 0) {
      throw new System.ArithmeticException(string.Format("ABC coordinate must sum up to 0. Received ({0}, {1}, {2})", a, b, c));
    }

    HexCoord tile = hex_dict[a][b][c];
    GameObject unit = Instantiate(Resources.Load("UnitRep")) as GameObject;

    unit.transform.SetParent(tile.transform);
    //unit.transform.position = new Vector3(0, 0, -1);
    unit.transform.localPosition = new Vector3(0, 0, -1);

    return unit;
  }
}
