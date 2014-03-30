using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RoverScience
{
	public partial class RoverScienceGUI
	{
		private void drawDebugGUI (int windowID)
		{

			GUILayout.BeginVertical ();

			GUILayout.Label (roverScience.RSVersion);
			GUILayout.Label ("# Data Stored: " + roverScience.container.GetStoredDataCount ());
			GUILayout.Label ("distCheck: " + Math.Round(rover.distanceCheck, 2));
			GUILayout.Label ("distTrav: " + Math.Round(rover.distanceTraveled));
			GUILayout.Label ("distTravTotal: " + Math.Round(rover.distanceTraveledTotal));
			GUIBreakline ();
			GUILayout.Label ("currentScalarDecay: " + roverScience.scienceDecayScalar);
			//GUILayout.Label ("scienceDistanceScalarBoost: " + roverScience.scienceDistanceScalarBoost);

			GUILayout.Label ("ScienceSpot potential: " + rover.scienceSpot.potentialGenerated);

			GUILayout.Label ("generatedScience: " + rover.scienceSpot.potentialScience);
			GUILayout.Label ("with decay: " + rover.scienceSpot.potentialScience * roverScience.scienceDecayScalar);
			//GUILayout.Label ("with distanceScalarBoost & decay & bodyScalar: " + rover.scienceSpot.potentialScience * 
			//roverScience.scienceDecayScalar * roverScience.scienceDistanceScalarBoost * roverScience.bodyScienceScalar);




			if (GUILayout.Button ("Find Science Spot")) {
				rover.scienceSpot.setLocation (rover.minRadius, rover.maxRadius);
			}


			if (GUILayout.Button ("Cheat Spot Here")) {
				if ((!rover.scienceSpot.established) && (consoleGUI.isOpen)) {
					rover.scienceSpot.setLocation (0, 1);
				} else if (rover.scienceSpot.established){
					rover.scienceSpot.reset ();
				}
			}

			if (GUILayout.Button ("CLEAR CONSOLE")) {
				consolePrintOut.Clear ();
			}

			GUIBreakline ();

			GUILayout.Label("Times Analyzed: " + roverScience.amountOfTimesAnalyzed);

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("-")) {
				if (roverScience.amountOfTimesAnalyzed > 0)
					roverScience.amountOfTimesAnalyzed--;
			}

			if (GUILayout.Button ("+")) {
				roverScience.amountOfTimesAnalyzed++;
			}

			if (GUILayout.Button("0")){
				roverScience.amountOfTimesAnalyzed = 0;
			}
			GUILayout.EndHorizontal ();

			GUIBreakline ();
			GUIBreakline ();


			GUILayout.Label("Dist. Upgraded Level: " + roverScience.levelMaxDistance);

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("-")) {
				if (roverScience.levelMaxDistance > 1)
					roverScience.levelMaxDistance--;
			}

			if (GUILayout.Button ("+")) {
				roverScience.levelMaxDistance++;
			}

			if (GUILayout.Button("0")){
				roverScience.levelMaxDistance = 0;
			}
			GUILayout.EndHorizontal ();



			GUILayout.Label("Acc. Upgraded Level: " + roverScience.levelPredictionAccuracy);

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("-")) {
				if (roverScience.levelPredictionAccuracy > 1)
					roverScience.levelPredictionAccuracy--;
			}

			if (GUILayout.Button ("+")) {
				roverScience.levelPredictionAccuracy++;
			}

			if (GUILayout.Button("0")){
				roverScience.levelPredictionAccuracy = 0;
			}
			GUILayout.EndHorizontal ();

			GUIBreakline ();
			if (GUILayout.Button ("Close Window")) {
				debugGUI.hide ();
			}

			GUILayout.EndVertical ();

			GUI.DragWindow ();
		}

		private void GUIBreakline ()
		{
			GUILayout.BeginHorizontal ();
			GUILayout.EndHorizontal ();
		}
	}
}