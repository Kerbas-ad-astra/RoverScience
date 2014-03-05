﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RoverScience
{

	public partial class RoverScienceGUI
	{

        private bool analyzeButtonPressedOnce = false;

		private void drawRoverConsoleGUI (int windowID)
		{
			GUILayout.BeginVertical (GUIStyles.consoleArea);
			scrollPosition = GUILayout.BeginScrollView (scrollPosition, new GUILayoutOption[]{GUILayout.Width(240), GUILayout.Height(340)});

			if (!roverScience.allowAnalyze) {
				GUILayout.Label ("Must wait until next analysis can be made");
				GUILayout.Label ("Time Remaining (d): " + Math.Round(TimeSpan.FromSeconds(roverScience.timeRemainingDelay).TotalDays, 1) + " days");
				GUILayout.Label ("_____________________________");
			}


			if (!rover.scienceSpot.established) {
				// PRINT OUT CONSOLE CONTENTS

					GUILayout.Label ("Searching for science spot . . .");
					GUILayout.Label ("Total dist. travelled searching: " + Math.Round(rover.distanceTravelledTotal, 2));
					foreach (string line in consolePrintOut) {
						GUILayout.Label (line);
					}

			} else {
				if (!rover.scienceSpotReached) {
					double relativeBearing = rover.heading - rover.bearingToScienceSpot;
					GUILayout.Label ("[POTENTIAL SCIENCE SPOT]");
					GUILayout.Label ("Distance to: " + rover.distanceFromScienceSpot);
					GUILayout.Label ("Bearing of Site: " + rover.bearingToScienceSpot);
					GUILayout.Label ("Rover Bearing: " + rover.heading);
					GUILayout.Label ("Rel. Bearing: " + relativeBearing);

					if (relativeBearing < 0) {
						GUILayout.Label ("TURN RIGHT");
					} else {
						GUILayout.Label ("TURN LEFT");
					}

				} else {
					GUILayout.Label ("[SCIENCE SPOT REACHED]");
					GUILayout.Label ("Total dist. travelled for this spot: " + rover.distanceTravelledTotal);
					GUILayout.Label ("Distance from landing site: " +
                        rover.getDistanceBetweenTwoPoints(rover.scienceSpot.location, rover.landingSpot.location));
					GUILayout.Label ("Potential: " + rover.scienceSpot.potentialString);

					GUILayout.Label ("");

					GUILayout.Label ("WARNING: BY ANALYZING THIS SITE THE ROVER CANNOT ANALYZE FOR ANOTHER 30 KERBAL DAYS");
				}

			}

			GUILayout.EndScrollView ();
			GUILayout.EndVertical ();
			
			// ACTIVATE ROVER BUTTON
			if (!analyzeButtonPressedOnce) {
				if (GUILayout.Button ("Analyze Science")) {
					if ((roverScience.allowAnalyze) && (rover.scienceSpotReached)) {
						analyzeButtonPressedOnce = true;
						consolePrintOut.Clear ();

					}
				}
			} else {
				GUILayout.BeginHorizontal ();
				if (GUILayout.Button ("Cancel")) {
					analyzeButtonPressedOnce = false;
				}

				if (GUILayout.Button ("Confirm")) {
					analyzeButtonPressedOnce = false;
					roverScience.analyzeScienceSample ();
				}
				GUILayout.EndHorizontal ();
			}

			if (GUILayout.Button ("Reset Science Site")) {
				rover.scienceSpot.established = false;
				rover.resetDistanceTravelled ();
				consolePrintOut.Clear ();

			}
			
			if (GUILayout.Button ("Reorient from Part")) {
				roverScience.command.MakeReference ();
			}
			
			GUILayout.BeginHorizontal();
			GUILayout.EndHorizontal();

			if (GUILayout.Button ("Close and Shutdown")) {
				rover.scienceSpot.established = false;
				rover.resetDistanceTravelled ();
				consolePrintOut.Clear ();

				consoleGUI.hide ();
			}


			GUI.DragWindow ();
		}
	}


}

