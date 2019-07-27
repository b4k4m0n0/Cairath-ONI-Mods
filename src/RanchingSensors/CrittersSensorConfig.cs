﻿using STRINGS;
using TUNING;
using UnityEngine;
using BUILDINGS = TUNING.BUILDINGS;

namespace RanchingSensors
{
	public class CrittersSensorConfig : IBuildingConfig
	{
		public static string Id = "CrittersSensor";
		public const string DisplayName = "Live Critters Sensor";
		public const string Description = "Counts up the number of critters in the room.";
		public static string Effect = $"Sends {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} " +
		                              $"or on {UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby)} depending on the number of live critters in a room.";

		public override BuildingDef CreateBuildingDef()
		{
			var buildingDef = BuildingTemplates.CreateBuildingDef(
				id: Id,
				width: 1,
				height: 1,
				anim: "critter_live_sensor_kanim",
				hitpoints: BUILDINGS.HITPOINTS.TIER1,
				construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
				construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER1,
				construction_materials: MATERIALS.REFINED_METALS,
				melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				build_location_rule: BuildLocationRule.Anywhere,
				decor: DECOR.NONE,
				noise: NOISE_POLLUTION.NONE);

			buildingDef.Overheatable = false;
			buildingDef.Floodable = false;
			buildingDef.Entombable = false;
			buildingDef.ViewMode = OverlayModes.Logic.ID;
			buildingDef.AudioCategory = "Metal";
			buildingDef.SceneLayer = Grid.SceneLayer.Building;
			SoundEventVolumeCache.instance.AddVolume("switchgaspressure_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
			SoundEventVolumeCache.instance.AddVolume("switchgaspressure_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
			GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, Id);

			return buildingDef;
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, LogicSwitchConfig.OUTPUT_PORT);
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, LogicSwitchConfig.OUTPUT_PORT);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			GeneratedBuildings.RegisterLogicPorts(go, LogicSwitchConfig.OUTPUT_PORT);

			var critterSensor = go.AddOrGet<CrittersSensor>();
			critterSensor.Threshold = 0.0f;
			critterSensor.ActivateAboveThreshold = true;
			critterSensor.manuallyControlled = false;
		}
	}
}
