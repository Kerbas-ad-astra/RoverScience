using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RoverScience
{

	#pragma warning disable 0108

	public class RoverScience : ModuleScienceContainer
	{
		/*
		 * CHECK THIS OUT
		 * 
		 * CelBody.GetRelblahblah
		 * ALSO CHECK OUT GETWORLDPOS PROPERLY
		 * 
		 * */

		//public List<Part> vesselParts = new List<Part>();
		public static RoverScience Instance = null;

		public System.Random rand = new System.Random();

		public double distCounter;

		public Rover rover = new Rover ();
		RoverScienceGUI RoverScienceGUIM = new RoverScienceGUI ();

		double oldMET = (double)0;
		double delMET = (double)0;

		Vessel vessel
		{
			get {
				if (HighLogic.LoadedSceneIsFlight) {
					return FlightGlobals.ActiveVessel;
				} else {
					return null;
				}
			}

		}

		//[KSPField(guiActive = true, guiName = "Distance Travelled", isPersistant = true, guiUnits = "m")]
		//double disp_distanceTravelled;

		[KSPEvent(guiActive = true, guiName = "Open Rover Terminal")]
		private void showGUI()
		{
			RoverScienceGUIM.consoleGUI.toggle ();
		}

		//ActionGroupThing
		[KSPAction("Toggle showGUI", actionGroup = KSPActionGroup.None)]
		private void showGUIAction(KSPActionParam param)
		{
			Debug.Log ("showGUIAction");
			if (IsPrimary) showGUI ();
		}

		[KSPEvent(guiActive = true, guiName = "Reset Distance")]
		private void resetDistanceButton()
		{
			if (IsPrimary) rover.distanceTravelled = 0;
		}


		void OnDestroy()
		{
			Debug.Log ("RoverScience OnDestroy()");
		}

		public override void OnLoad(ConfigNode vesselNode)
		{
			// GUI position still not saved - could be IsPrimary causing problems
			if (IsPrimary) {
				Debug.Log ("OnLoad()");
				// Load distance travelled by rover
				rover.distanceTravelled = Convert.ToDouble (vesselNode.GetValue ("disp_distanceTravelled"));

				// Load GUI window position for vessel
				//RoverScienceGUIM.pos_x = (float)Convert.ToDouble (vesselNode.GetValue ("RoverScienceGUI_x"));
				//RoverScienceGUIM.pos_y = (float)Convert.ToDouble (vesselNode.GetValue ("RoverScienceGUI_y"));
			}
		}

		public override void OnStart(PartModule.StartState state)
		{
			if (!HighLogic.LoadedSceneIsFlight) {
				return;
			}
			//if (HighLogic.LoadedSceneIsFlight)
			if (IsPrimary) {
				Debug.Log ("RoverScience 2 initiated!");

				Instance = this;
				//setFieldDisplays ();

				delMET = (double)0;
				oldMET = vessel.missionTime;



				RenderingManager.AddToPostDrawQueue (0, RoverScienceGUIM.drawGUI);
			} else {
				// For when a duplicate PartModule is found
				Debug.Log ("Not primary");
			}

		}
		
		public override void OnUpdate()
		{
			if (IsPrimary) {
				// check if the rover fits conditions
				// set delMET for distance calculations with horizontal surface speed.
				if (checkRoverValidStatus()) timeKeeper ();

				// rover stuff

				rover.setLandingSpot ();
				if (rover.landingSite.established) rover.setRoverLocation ();
				if ((!rover.scienceSpot.established) && (!rover.scienceSpotReached)) checkAndSetScienceSpot ();

			}

			DebugKey ();
		}



		private void timeKeeper()
		{
				delMET = (vessel.missionTime - oldMET);
				rover.calculateDistanceTravelled (delMET);
				oldMET = vessel.missionTime;
		}


		public void analyzeScienceSample()
		{
			if (rover.scienceSpotReached) {
				_ScienceSpot sciValues = new _ScienceSpot ();
				sciValues = rover.scienceSpot.getValues ();
				rover.scienceSpot.reset ();


				Debug.Log ("Science retrieved! - " + rover.scienceSpot.potentialScience);
			} else {
				Debug.Log ("Tried to analyze while not at spot?");
			}
		}



		// This handles what happens after the distance travelled passes the distance roll
		// If the roll is successful establish a science spot
		public void checkAndSetScienceSpot()
		{

			// Once distance travelled passes the random check distance
			if (rover.distanceTravelled >= rover.distanceCheck) {
				rover.totalDistanceTravelled += rover.distanceTravelled;
				rover.resetDistanceTravelled ();

				RoverScienceGUIM.addRandomConsoleJunk ();

				Debug.Log ("" + rover.distanceCheck + " meter mark reached");

				// Reroll distanceCheck value
				rover.distanceCheck = rand.Next (20, 70);

				// farther you are from established site the higher the chance of striking science!
				int rNum = rand.Next (0, 100);
				double dist = rover.distanceFromSite;
				double chanceAlgorithm = dist < 100 ?
					(0.75) * dist : 
					(0.5) * dist + 60;

				double chance = (chanceAlgorithm < 100) ? chanceAlgorithm : 100;

				// rNum is a random number between 0 and 100
				// chance is the percentage number we check for to determine a successful roll
				// higher chance == higher success roll chance
				if ((double)rNum <= chance) {
					rover.setScienceSpotLocation ();
					rover.scienceSpot.generateScience ();

					RoverScienceGUIM.clearConsole ();

					Debug.Log ("Distance from spot is: " + rover.distanceFromScienceSpot);
					Debug.Log ("Bearing is: " + rover.bearingToScienceSpot);
					Debug.Log ("Something found");
				} else {
					// Science hotspot not found
					Debug.Log ("Nothing found!");
				}

			}

		}



		public void DebugKey()
		{
			if (Input.GetKeyDown (KeyCode.Equals)) {
				RoverScienceGUIM.debugGUI.show ();
			}
		}


		public bool checkRoverValidStatus()
		{
			// Checks if rover is landed with at least one wheel on no time-warp.
			return ((TimeWarp.CurrentRate == 1) && (vessel.horizontalSrfSpeed > (double) 0.01) && (rover.numberWheelsLanded > 0));
		}




		// TAKEN FROM KERBAL ENGINEERING REDUX SOURCE by cybutek
		// This is to hopefully prevent multiple instances of this PartModule from running simultaneously
		public bool IsPrimary
		{
			get
			{
				if (this.vessel != null)
				{
					foreach (Part part in this.vessel.parts)
					{
						if (part.Modules.Contains(this.ClassID))
						{
							if (this.part == part)
							{
								return true;
							} else {
								break;
							}
						}
					}
				}
				return false;
			}
		}


	}

}

