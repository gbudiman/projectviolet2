using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorActionController : MonoBehaviour {
  public enum Action {
    move,
    free_move,
    melee_main_hand,
    melee_off_hand,
    ranged_mounted_main_hand,
    ranged_mounted_off_hand,
    ranged_attack,
    ranged_attack_main_hand,
    ranged_attack_off_hand,
    swap_equipment,
    pickup_equipment,
    throw_equipment,
    brawl_main_hand,
    brawl_off_hand,
    reload_ranged_mounted,
    reload_ranged_weapon,
    // Defensive: Stances
    stance_vigilance,
    stance_bulwark,
    stance_colossus,
    stance_aggression,
    stance_phalanx,
    // Defensive: Limit Break
    limit_break_skewer_charge,
    limit_break_exploding_stomp,
    limit_break_zip_fend,
    // Defensive: Shield Mastery
    shield_bash,
    shield_charge,
    shield_devotion,
    shield_hunker,
    // Offensive: Dual Wield
    dw_flurry_pommel_strike,
    dw_flurry_cross_break,
    dw_cyclone,
    dw_pulverize,
    // Offensive: Polearm
    polearm_dance,
    polearm_earthshatter,
    polearm_split_to_dw,
    polearm_combine_from_dw,
    // Offensive: Blood Mastery
    blood_overcharge,
    blood_trance_roar,
    blood_trance_ravage,
    blood_trance_berserk,
    blood_trance_sonic_wave,
    blood_trance_arc_wave,
    blood_trance_rage_strike,
    // Martial Arts: Fistfight
    brawl_left_foot,
    brawl_right_foot,
    // Martial Arts: Momentum
    momentum_summon_orb,
    momentum_finisher_3,
    momentum_finisher_4,
    momentum_finisher_5,
    momentum_finisher_6,
    momentum_finisher_7,
    // Battlefield: Stealth
    stealth_conceal,
    stealth_preemptive_strike,
    // Battlefield: Acrobatics
    acrobatics_dash,
    acrobatics_leap,
    acrobatics_blink_strike,
    acrobatics_blink_return,
    acrobatics_backstab,
    acrobatics_fleche,
    // Battlefield: Mounted Warfare
    mount_rider_ride,
    mount_rider_melee_off_hand,
    mount_rider_ranged_mounted_off_hand,
    mount_rider_parry,
    mount_rider_throw_equipment,
    mount_passenger_ride,
    mount_passenger_melee_main_hand,
    mount_passenger_melee_off_hand,
    mount_passenger_ranged_attack,
    mount_passenger_parry,
    mount_passenger_throw_equipment,
    mount_rider_dismount,
    mount_passenger_dismount,
    // Marksmandship: Dexterous
    dexterous_concentration_activate,
    dexterous_concentration_deactivate,
    dexterous_overdraw,
    dexterous_aim,
    dexterous_overwatch,
    // Marksmanship: Skills
    marksmanship_double_tap,
    marksmanship_strafe,
    marksmanship_knockback,
    marksmanship_hail_of_arrows,
    marksmanship_scatter_shot,
    marksmanship_sniped_shot,
    marksmanship_walking_target,
    // Saboteur: Sabotage
    sabotage_disarm,
    sabotage_shatter,
    sabotage_disorient,
    sabotage_maim,
    sabotage_blind,
    sabotage_shock,
    sabotage_rupture,
    sabotage_pierce,
    // Saboteur: Anti-Magic
    antimagic_disrupt,
    antimagic_dispel,
    antimagic_mana_drain,
    antimagic_magic_shield,
    antimagic_silence,
    antimagic_purify,
    // Saboteur: Trap
    trap_snare,
    trap_claymore,
    trap_flak,
    trap_install_slot,
    trap_install_trigger,
    // Elementalist: Elemental
    elemental_set_fire,
    elemental_set_wind,
    elemental_set_ice,
    incantation,
    // Elementalist: Payload
    payload_bolt,
    payload_cone,
    payload_wall,
    // Elementalist: Utilities
    utilities_channeling,
    utilities_barrier,
    utilities_meditation,
    utilities_enchant,
    // Dark Magic: Necromancy
    necromancy_soul_harvest,
    necromancy_animate_dead,
    necromancy_corpse_explosion,
    necromancy_soul_restoration,
    necromancy_walking_bomb,
    // Dark Magic: Hexes
    hexes_life_drain,
    hexes_life_leech_activate,
    hexes_life_leech_deactivate,
    hexes_weakness,
    hexes_horror,
    hexes_curse,
    hexes_sleep,
    // Dark Magic: Mastery
    dark_mirror_image_summon,
    dark_mirror_image_absolve,
    dark_enchant,
    dark_blood_sacrifice,
    dark_soul_engrave,
    dark_soul_slot,
    // Geomancy: Gestalt
    gestalt_summon_owl,
    gestalt_summon_pigeon,
    gestalt_summon_hawk,
    gestalt_summon_wolf,
    gestalt_summon_bear,
    gestalt_summon_spider,
    gestalt_summon_carrion_swarm,
    gestalt_summon_unicorn,
    gestalt_summon_dragonling,
    gestalt_owl_sight,
    gestalt_pigeon_heal,
    gestalt_pigeon_cure,
    gestalt_hawk_blitz,
    gestalt_hawk_sweeping_dart,
    gestalt_wolf_kinetic_lunge,
    gestalt_bear_lacerate,
    gestalt_bear_maul,
    gestalt_spider_web,
    gestalt_spider_toxic_spit,
    gestalt_carrion_swarm_assult,
    gestalt_dragonling_breath,
    gestalt_dragonling_throw,
    // Geomancy: Geomancy
    geomancy_stonefist,
    geomancy_deluge,
    geomancy_fissure,
    geomancy_gust,
    geomancy_boulder,
    geomancy_entangling_roots,
    geomancy_quicksand,
    geomancy_starfall,
    geomancy_earthspike,
    // Geomancy: Nature Attunement
    nature_glyph_teleport_entrance,
    nature_glyph_teleport_exit,
    nature_glyph_repulsion,
    nature_glyph_imprisonment,
    nature_glyph_tranquility,
    nature_glyph_levitation,
    nature_petrify,
    nature_rock_armor,
    // Priesthood: Psionics
    psionics_statis,
    psionics_pillar,
    psionics_safety_wall,
    psionics_strike,
    psionics_lance,
    psionics_vortex,
    psionics_luminaire,
    // Priesthood: holy
    holy_zeal,
    holy_fortress,
    holy_battle_hymn,
    holy_rapid_casting,
    holy_bloomfield,
    holy_ablative_shield,
    holy_blessed_aim,
    holy_sanctus_maledicta,
    holy_benedicta_sacramonia,
    holy_praesidium,
    holy_cast_to_ward,
    holy_exorcise,
    // Priesthood: Genesis
    genesis_heal,
    genesis_resurrect,
    genesis_guardian_angel,
    genesis_pray,
    genesis_aura,
    genesis_ward
  }

  Dictionary<Action, bool> available_actions;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void initialize() {
    available_actions = new Dictionary<Action, bool>();
    foreach (Action action in System.Enum.GetValues(typeof(Action))) {
      available_actions.Add(action, false);
    }
  }
}
